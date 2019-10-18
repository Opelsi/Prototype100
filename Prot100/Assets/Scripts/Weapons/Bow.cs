using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon, IReloadable, IShootable
{
	public Animator animator;
	public Transform bulletPoint;

	public float rateOfFire { get; set; }
	public float projectileSpeed { get; set; }
	public void shoot()
	{
		animator.SetBool("IsShoot", true);
		//Spawn bullet
	}
	public float reloadRate { get; set; }
	public void reload()
	{
		animator.SetBool("IsReload", true);
	}

	override public void attack()
	{
		shoot();
	}
	override public void resetWeapon()
	{
		animator.SetBool("IsShoot", false);
		animator.SetBool("IsReload", false);
	}
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
