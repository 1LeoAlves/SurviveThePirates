using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
	[SerializeField] ContactFilter2D contactFilter;
	[SerializeField] GameObject explosionPrefab;
	Ship ship;
	public string launchedShipName;
	private void OnTriggerEnter2D(Collider2D coll)
	{
		GetColidedShip(coll);
		if (!ship.gameObject.name.Equals(launchedShipName))
		{
			Explode();
		}
	}
	void GetColidedShip(Collider2D coll)
	{
		if (coll.GetComponent<Collider2D>().GetComponent<Ship>())
		{
			Ship ship = null;
			if (coll.GetComponent<Collider2D>().GetComponent<EnemyShipController>())
			{
				ship = (EnemyShipController)coll.GetComponent<Collider2D>().GetComponent<Ship>();
			}
			else if (coll.GetComponent<Collider2D>().GetComponent<PlayerController>())
			{
				ship = (PlayerController)coll.GetComponent<Collider2D>().GetComponent<Ship>();
			}
			if (ship != null)
			{
				this.ship = ship;
			}
		}
	}
	void Explode()
	{
		ship.GetDamage();
		Instantiate(explosionPrefab,transform.position,Quaternion.identity);
		Destroy(gameObject);
	}
}
