﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public enum PlayerState
{
    Idle,
    WalkingAndShooting,
    GettingRevived,
}

public class PlayerController : MonoBehaviour 
{

    //Public Variables
    [Header("Player Variables")]
    [Range(0, 10)] public float moveSpeed;
    public PlayerState playerState = PlayerState.Idle;
    public Animator anim;

    [Header("Weapon Variables")]
    [Range(0, 50)] public float bulletSpeed;
    [Range(0, 1)] public float timeBetweenShots;
    [Range(0, 10)] public float bulletSpread;
    public Bullet bullet;
    public Transform fireFrom;
    public GameObject newWeapon;
    public Transform attachmentOne;

    //Private Variables
    private Rigidbody myRB;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Camera mainCamera;
    private float shotCounter;
    private float bulletSpreadWidth;
    private Vector3 playerDirection;
    private Vector2 playerLookDirection;

    public InputDevice Device
    {
        get;
        set;
    }

    void Start () 
    {
        myRB = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
        
        playerLookDirection.x = 0f;
        playerLookDirection.y = 1f;
	}
	
	void Update () 
	{
        switch (playerState)
        {
            case PlayerState.Idle:
            //Code for the idle state here
            if (Device.AnyButtonWasPressed)
            {
                Idle();
            }
            else if (Device.LeftStickX || Device.LeftStickY || Device.RightStickX || Device.RightStickY)
            {
                playerState = PlayerState.WalkingAndShooting;
            }
            break;

            case PlayerState.WalkingAndShooting:
            //Code for the player walking and shooting
            PlayerMovement();
            PlayerRotation();
            PlayerShooting();
            break;

            case PlayerState.GettingRevived:
            //Code for the player getting revived
            break;
        }
	}

    void FixedUpdate()
    {
        myRB.velocity = moveVelocity;
    }

    void Idle()
    {
        //Having No Input?
    }

    void PlayerMovement()
    {
        //This is for the AnimMove() animator
        var xLeft = Device.LeftStickX;
        var yLeft = Device.LeftStickY;
        var xRight = Device.RightStickX;
        var yRight = Device.RightStickY;
        AnimMove(xLeft, yLeft, xRight, yRight);
        
        //Adding velocity to the player with moveSpeed to make him move
        moveInput = new Vector3(Device.LeftStickX, 0f, Device.LeftStickY);
        moveVelocity = moveInput * moveSpeed;
    }

    void PlayerRotation()
    {
        //The normal direction is where the player is set to being able to move around and rotate
        //whilst shooting
        playerDirection = Vector3.right * Device.RightStickX + Vector3.forward * Device.RightStickY;
        //Checking if the vector3 has got a value inputed
        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
            Vector3 tempRotationValue = transform.rotation.eulerAngles;
            tempRotationValue.y = tempRotationValue.y + 17;
            transform.rotation = Quaternion.Euler(tempRotationValue);
            playerLookDirection.x = playerDirection.x;
            playerLookDirection.y = playerDirection.z;
        }
        else
        {
            //The alt direction is where the player is set to being able to move around and rotate
            //whilst not having to shoot
            Vector3 playerDirectionAlt = Vector3.right * Device.LeftStickX + Vector3.forward * Device.LeftStickY;
            if (playerDirectionAlt.sqrMagnitude > 0.0f)
            {
                transform.rotation = Quaternion.LookRotation(playerDirectionAlt, Vector3.up);
                Vector3 tempRotationValue = transform.rotation.eulerAngles;
                tempRotationValue.y = tempRotationValue.y + 17;
                transform.rotation = Quaternion.Euler(tempRotationValue);
                playerLookDirection.x = playerDirectionAlt.x;
                playerLookDirection.y = playerDirectionAlt.z;
            }
        }      
    }

    bool IsFiring()
    {
        //If the inputs are pressed then bool set to true
        //Otherwise if there are no inputs pressed, bool is set to false
        return Device.RightStickX || Device.RightStickY;
    }

    void PlayerShooting()
    {
        //Add bullet spread to the weapon
        bulletSpreadWidth = Random.Range(-bulletSpread, bulletSpread);
        
        //If firing
        if (IsFiring())
        {
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                Bullet newBullet = Instantiate(bullet, fireFrom.position, fireFrom.rotation) as Bullet;
                newBullet.bulletSpeed = bulletSpeed;
                newBullet.transform.Rotate(0f, bulletSpreadWidth, 0f);
            }

            return;
        }
        
        //If not firing
        if (!IsFiring())
        {
            shotCounter = 0;
            return;
        }
    }

    void OnTriggerEnter(Collider theCol)
    {
        //Checking whether the player is standing over a weapon and then automatically 
        //picking it up and setting it active
        if (theCol.gameObject.CompareTag("Weapon"))
        {
            theCol.gameObject.transform.position = attachmentOne.position;
            theCol.gameObject.transform.rotation = attachmentOne.rotation;
            SetWeaponActive();
        }
    }

    bool SetWeaponActive()
    {
        //Simply setting the picked up weapon true
        return true;
    }

    void AnimMove(float xLeft, float yLeft, float xRight, float yRight)
    {
        //Setting the float of the animation in the beginning 
        anim.SetFloat("velX", xLeft);
        anim.SetFloat("velZ", yLeft);

        //Creating two new vectors, a move and look vector
        Vector2 moveVector = new Vector2(xLeft, yLeft);
        Vector2 lookVector = new Vector2(playerDirection.x, playerDirection.z);

        //Creating a value that is based between two vectors
        float dotMovement = Vector2.Dot(moveVector, lookVector);
	    
        float lookVelocity = Mathf.Sqrt(Vector2.SqrMagnitude(lookVector));
        float moveVelocity = Mathf.Sqrt(Vector2.SqrMagnitude(moveVector));

        if (lookVelocity > 0 && moveVelocity > 0)
        {
            //Both sticks are moving
            anim.SetFloat("dotMovement", dotMovement);
            anim.SetFloat("lookVelocity", lookVelocity);
        } else if (lookVelocity > 0f)
        {
            //Idle but shooting
            anim.SetFloat("dotMovement", 0f);
            anim.SetFloat("lookVelocity", -1f);
        } else if (moveVelocity > 0f)
        {
            //moving but not shooting
            lookVector = playerLookDirection;
            lookVector.Normalize();
            dotMovement = Vector2.Dot(moveVector, lookVector);
            anim.SetFloat("dotMovement", dotMovement);
            anim.SetFloat("lookVelocity", 1f);
            anim.SetInteger("whatStateAmI", 1);
        }
        else
        {
            anim.SetFloat("dotMovement", 0f);
            anim.SetFloat("lookVelocity", -1f);
            anim.SetInteger("whatStateAmI", 0);
        }
    }


}
