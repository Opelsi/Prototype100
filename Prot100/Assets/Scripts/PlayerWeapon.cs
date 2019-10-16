using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
	bool isAttack = false;
	bool isParry = false;
	public WeaponController controller;
	// Update is called once per frame
	void Update()
    {
		if (Input.GetKeyDown(KeyCode.E))
		{
			isAttack = true;
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			isParry = true;
		}
	}
	private void FixedUpdate()
	{
		controller.UseWeapon(isAttack, isParry);
		isAttack = false;
		isParry = false;
	}
}
