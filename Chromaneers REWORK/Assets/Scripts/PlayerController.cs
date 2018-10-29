using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public enum PlayerState
{
    Idle,
    WalkingAndShooting,
    GettingRevived,
}

public class PlayerController : MonoBehaviour {

    //Public Variables
    [Header("Player Variables")]
    [Range(0, 10)] public float moveSpeed;
    public PlayerState playerState = PlayerState.Idle;

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

    public InputDevice Device
    {
        get;
        set;
    }

    void Start () {
        myRB = GetComponent<Rigidbody>();
        mainCamera = FindObjectOfType<Camera>();
	}
	
	void Update () {
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
        
    }

    void PlayerMovement()
    {
        moveInput = new Vector3(Device.LeftStickX, 0f, Device.LeftStickY);
        moveVelocity = moveInput * moveSpeed;
    }

    void PlayerRotation()
    {
        Vector3 playerDirection = Vector3.right * Device.RightStickX + Vector3.forward * Device.RightStickY;
        if (playerDirection.sqrMagnitude > 0.0f)
        {
            transform.rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        }
    }

    bool IsFiring()
    {
        if (Device.RightStickX || Device.RightStickY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void PlayerShooting()
    {
        //Add bullet spread to the weapon
        bulletSpreadWidth = Random.Range(-bulletSpread, bulletSpread);

        if (IsFiring())
        {
            //Makes sure we can shoot, and the speed of which bullet come out
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                Bullet newBullet = Instantiate(bullet, fireFrom.position, fireFrom.rotation) as Bullet;
                newBullet.bulletSpeed = bulletSpeed;
                newBullet.transform.Rotate(0f, bulletSpreadWidth, 0f);
            }
        }
        else
        {
            shotCounter = 0;
        }
    }

    void OnTriggerStay(Collider theCol)
    {
        if (theCol.gameObject.CompareTag("Weapon"))
        {
            theCol.gameObject.transform.position = attachmentOne.position;
            theCol.gameObject.transform.rotation = attachmentOne.rotation;
            SetWeaponActive();
        }
    }

    bool SetWeaponActive()
    {
        return true;
    }


}
