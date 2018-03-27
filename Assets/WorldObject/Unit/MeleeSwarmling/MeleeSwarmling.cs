using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;

public class MeleeSwarmling : MeleeUnit {

	// public float weaponAimSpeed;

	private Quaternion aimRotation;

	public override void SaveDetails(JsonWriter writer)
	{
		base.SaveDetails(writer);
		SaveManager.WriteQuaternion(writer, "AimRotation", aimRotation);
	}

	protected override void Start()
	{
		base.Start();

		var stateController = GetComponent<StateController>();

		if (stateController)
		{
			List<Transform> wayPoints = new List<Transform>();
			var patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoint");
			foreach (GameObject pp in patrolPoints)
			{
				wayPoints.Add(pp.transform);
			}


			stateController.SetupAI(true, wayPoints);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (aiming)
		{
			transform.rotation = Quaternion.RotateTowards(transform.rotation, aimRotation, weaponAimSpeed);
			CalculateBounds();
			//sometimes it gets stuck exactly 180 degrees out in the calculation and does nothing, this check fixes that
			Quaternion inverseAimRotation = new Quaternion(-aimRotation.x, -aimRotation.y, -aimRotation.z, -aimRotation.w);
			if (transform.rotation == aimRotation || transform.rotation == inverseAimRotation)
			{
				aiming = false;
			}
		}
	}

	public override bool CanAttack()
	{
		return true;
	}

	protected override void AimAtTarget()
	{
		base.AimAtTarget();
		aimRotation = Quaternion.LookRotation(target.transform.position - transform.position);
	}

	protected override void UseWeapon()
	{
		base.UseWeapon ();
		target.TakeDamage (damage);
	}

	protected override void HandleLoadedProperty(JsonTextReader reader, string propertyName, object readValue)
	{
		base.HandleLoadedProperty(reader, propertyName, readValue);
		switch (propertyName)
		{
		case "AimRotation": aimRotation = LoadManager.LoadQuaternion(reader); break;
		default: break;
		}
	}
}
