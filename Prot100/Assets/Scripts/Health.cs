using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
	public float health = 100;
	Animator animator;
	public bool isDamage = false;
	float damageTime = -1;
	public LayerMask EnemyAttackMask;
	public void GetDamage(Weapon weapon)
	{
		if (health - weapon.damage > 0) health -= weapon.damage;
		isDamage = true;
		damageTime = Time.time;
	}
    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		animator.SetBool("IsDamage", isDamage);
		if (isDamage&&(Time.time-damageTime>0.1f))
		{
			isDamage = false;
		}
    }

	private void OnTriggerEnter2D( Collider2D collider )
	{
		if (EnemyAttackMask==(EnemyAttackMask|(1<<collider.gameObject.layer)))
		{
			GetDamage(collider.gameObject.GetComponent<Weapon>());
		}
	}
}
