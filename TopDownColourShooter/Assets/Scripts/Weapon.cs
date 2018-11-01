using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName ="Scriptable Objects/Weapon Stats")]
public class Weapon : ScriptableObject {

    public string wepName;

    public int damage;
    public int fireRate;
    public int bulletSpread;
    public int range;

    public ParticleSystem trailParticle;
    public ParticleSystem explosionParticle;
}
