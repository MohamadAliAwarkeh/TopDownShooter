using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPopUI : MonoBehaviour {

    public Weapon weapon;

    public Text nameText;

    public Slider damageSlider;
    public Slider fireRateSlider;
    public Slider bulletSpreadSlider;
    public Slider rangeSlider;

	void Start () {
        nameText.text = weapon.wepName;

        damageSlider.value = weapon.damage;
        fireRateSlider.value = weapon.fireRate;
        bulletSpreadSlider.value = weapon.bulletSpread;
        rangeSlider.value = weapon.range;
	}
	

}
