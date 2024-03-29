﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
	public Transform currentWeapon;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnAttackEvent;
	private bool m_wasAttacking = false;
	public BoolEvent OnParryEvent;
	private bool m_wasParrying = false;
	public BoolEvent OnReloadEvent;
	private bool m_wasReloading = false;

	float useTime = 0f;
	float coolDownTime = 0f;
	public bool canUse;
	public Animator animator;

	public void UseWeapon( bool attack, bool parry, bool reload )
	{
		if (GetComponent<Health>().isDamage)//Deactivate weapon when getting damage
		{
			animator.SetBool("IsAttack", false);
			currentWeapon.gameObject.SetActive(false);
		}
		else
		{
			if ((attack || parry || reload) && (!m_wasAttacking))//Set player body animation to Attack (left hand is vertical) or Bow (left hand is horizontal)
			{
				animator.SetBool("IsAttack", true);
				if (!reload && currentWeapon.gameObject.GetComponent<IShootable>() != null) animator.SetBool("IsBow", true);
				currentWeapon.gameObject.SetActive(true);
				useTime = Time.time;
			}
			if (attack && !m_wasAttacking)//Attack
			{
				currentWeapon.gameObject.GetComponent<Weapon>().attack();
				m_wasAttacking = true;
			}
			if (currentWeapon.gameObject.GetComponent<IDefendable>() != null)//Parry
				if (parry && !m_wasParrying)
				{
					currentWeapon.gameObject.GetComponent<IDefendable>().parry();
					m_wasParrying = true;
				}
			if (currentWeapon.gameObject.GetComponent<IReloadable>() != null)//Reload
				if (reload && !m_wasReloading)
				{
					currentWeapon.gameObject.GetComponent<IReloadable>().reload();
					m_wasReloading = true;
				}
		}
	}


	public void CoolDown()
	{
		if (Time.time - coolDownTime > currentWeapon.GetComponent<Weapon>().attackRate) canUse = true;
	}

	private void FinishAttack()
	{
		currentWeapon.gameObject.GetComponent<Weapon>().resetWeapon();
		currentWeapon.gameObject.SetActive(false);
		animator.SetBool("IsAttack",false);
		animator.SetBool("IsBow", false);
		m_wasAttacking = false;
		m_wasReloading = false;
		m_wasParrying = false;
		canUse = false;
		coolDownTime = Time.time;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (m_wasAttacking && useTime > -1f && Time.time - useTime > 0.25f) FinishAttack();
		CoolDown();
    }
}
