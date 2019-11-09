using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon, ISwingable, IDefendable
{
	public Animator animator;

	public float swingRate { get; set; }
	
	public void swing()
	{
		animator.SetBool("IsSwing",true);
	}

	public float parryRate { get; set; }

	public void parry()
	{
		animator.SetBool("IsParry", true);
	}
	override public void attack()
	{
		swing();
	}
	override public void resetWeapon()
	{
		animator.SetBool("IsSwing", false);
		animator.SetBool("IsParry", false);
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
