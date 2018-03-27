﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;

public class MeleeSwarmling : MeleeUnit {
	public override bool CanAttack()
	{
		return true;
	}

	protected override void UseWeapon()
	{
		base.UseWeapon ();
		target.TakeDamage (damage);
	}
}
