using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public enum WeaponState
{
    Pistol,
    Shotgun,
    SMG,
}

public class PlayerGunControllerOne : MonoBehaviour {

    //Public Variables
    [Header("Weapon Variables")]
    public float bulletSpeed;
    public float timeBetweenShots;
    public WeaponState weaponState = WeaponState.Pistol;
    public Bullet bullet;
    public Transform fireFrom;

    //Private Variables
    private float shotCounter;

    InputDevice Device
    {
        get;
        set;
    }
	
	public void Update () {
        switch (weaponState)
        {
            case WeaponState.Pistol:
                Pistol();
                break;
        }
	}

    bool IsFiring()
    {
        return true;
    }

    public void Pistol()
    {
        
    }
}
