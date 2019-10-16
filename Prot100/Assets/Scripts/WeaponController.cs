using System.Collections;
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

	public float attackTime = -1f;
	public Animator animator;

	public void UseWeapon( bool attack, bool parry)
	{
		if (attack && !m_wasAttacking)
		{
			animator.SetBool("IsAttack", true);
			currentWeapon.gameObject.SetActive(true);
			currentWeapon.gameObject.GetComponent<Weapon>().attack();
			attackTime = Time.time;
			m_wasAttacking = true;
		}
		if(parry && !m_wasParrying)
		{
			animator.SetBool("IsAttack", true);
			currentWeapon.gameObject.SetActive(true);
			currentWeapon.gameObject.GetComponent<IDefendable>().parry();
			attackTime = Time.time;
		}
	}

	private void FinishAttack()
	{
		currentWeapon.gameObject.SetActive(false);
		animator.SetBool("IsAttack", false);
		m_wasAttacking = false;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (attackTime >-1f && Time.time - attackTime > 0.5f) FinishAttack();   
    }
}
