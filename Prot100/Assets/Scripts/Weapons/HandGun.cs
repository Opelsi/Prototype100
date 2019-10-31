using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGun : Weapon, IShootable, IReloadable
{
	public Animator animator;
	public Transform bulletPoint;
	public Projectile bullet;

	public float rateOfFire { get; set; }
	public float projectileSpeed { get; set; }
	public void shoot()
	{
		animator.SetBool("IsShoot", true);
	}
	public float reloadRate { get; set; }
	public void reload()
	{
		animator.SetBool("IsReload", true);
	}
	public void spawnBullet()
	{
		bullet.transform.position = bulletPoint.position;
		if (transform.parent.localScale.x > 0f)
		{
			Debug.Log(transform.parent.localScale.x);
			bullet.forwardVelocity = 600;
			bullet.transform.localScale = (new Vector2(1, 1));
		}
		else
		{
			Debug.Log(transform.parent.localScale.x);
			bullet.forwardVelocity = -600;
		}
		Instantiate(bullet);
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
