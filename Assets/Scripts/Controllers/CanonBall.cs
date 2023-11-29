using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class CanonBall : MonoBehaviour
{
	[SerializeField] ContactFilter2D contactFilter;
	[SerializeField] GameObject explosionPrefab;
	public int bombDamage;
	public string shiplauncherName;

	private void Start()
	{
		Invoke("Explode",2.5f);
	}
	private void OnTriggerEnter2D(Collider2D coll)
	{
		Task.Delay(1000);
		if (!coll.name.Equals(shiplauncherName))
		{
			if (!coll.gameObject.layer.Equals(6))
			{
				coll.GetComponent<Ship>().GetDamage(bombDamage);
			}
			Explode();
		}
	}
	void Explode()
	{
		Instantiate(explosionPrefab,transform.position,Quaternion.identity);
		Destroy(gameObject);
	}
}
