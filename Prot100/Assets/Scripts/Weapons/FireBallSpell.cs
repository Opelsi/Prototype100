using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpell : Weapon, IMagical, IShootable
{
	public Animator animator;
	public Transform bulletPoint;

	public int manaCost { get; set; }
	public ELEMENT element { get; set; }

	public float rateOfFire { get; set; }
	public float projectileSpeed { get; set; }
	public void shoot()
	{
		animator.SetBool("IsShoot", true);
	}

	public override void attack()
	{
		shoot();
	}
	public override void resetWeapon()
	{
		animator.SetBool("IsShoot", false);
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
