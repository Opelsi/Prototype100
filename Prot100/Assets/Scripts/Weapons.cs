using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum DAMAGETYPE
{
	SOLID, FIRE, POISON, BLEEDING, CRITING
};

public class Weapon
{
	int damage;
	DAMAGETYPE damageType;
	int armorPenetration;
	float switchSpeed;
	float range;
	float critRate;
	float critDamage;
	float weight;
	float attackArc;
}
enum ELEMENT
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