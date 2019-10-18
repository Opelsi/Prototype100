using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Weapon Class Start
public enum DAMAGETYPE
{
	SOLID, FIRE, POISON, BLEEDING, CRITING
};

public abstract class Weapon: MonoBehaviour
{
	public int damage, armorPenetration;
	public DAMAGETYPE damageType;
	public float switchSpeed, range, critRate, critDamage, weight, attackArc;
	public abstract void attack();
	public abstract void resetWeapon();
}
//Weapon Class End

//Interfaces Start
public enum ELEMENT
{
	AIR, EARTH, FIRE, WATER
};

interface IStabable
{
	float stabRate { get; set; }
	void stab();
}

interface ISwingable
{
	float swingRate { get; set; }
	void swing();
}

interface IMagical
{
	int manaCost { get; set; }
	ELEMENT element { get; set; }
}

interface IShootable
{
	float rateOfFire { get; set; }
	float projectileSpeed { get; set; }
	void shoot();
}

interface IReloadable
{
	float reloadRate { get; set; }
	void reload();
}

interface IDefendable
{
	float parryRate { get; set; }
	void parry();
}
//Interfaces End