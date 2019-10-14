using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongSword : Weapon, ISwingable, IDefendable
{

	public float swingRate { get; set; }
	
	public void swing()
	{
		transform.Rotate(new Vector3(0f, 0f, attackArc));
	}

	public float parryRate { get; set; }

	public void parry()
	{

	}

	override public void attack()
	{
		
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
