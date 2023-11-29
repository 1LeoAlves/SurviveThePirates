using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ship : MonoBehaviour
{
    public virtual void GetDamage()
    {

    }
	public virtual float GetShipHealth()
	{
		return 0;
	}

}
