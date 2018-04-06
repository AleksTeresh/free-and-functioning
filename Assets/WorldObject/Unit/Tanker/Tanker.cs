using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using Newtonsoft.Json;

public class Tanker : MeleeUnit {

	public override bool CanAttack()
	{
		return true;
	}

    public override bool IsMajor()
    {
        return true;
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
