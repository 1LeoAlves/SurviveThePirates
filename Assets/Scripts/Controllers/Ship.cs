using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
	public List<Sprite> shipParts, sailsParts;
	public virtual void GetDamage(float amount)
    {

    }
	public virtual float GetShipHealth()
	{
		return 0;
	}
}
