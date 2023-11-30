using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
		CheckCollision(coll);
	}
	void CheckCollision(Collider2D coll)
	{
		if (!coll.name.Equals(shiplauncherName))
		{
			if (!coll.gameObject.layer.Equals(6))
			{
				if (!coll.gameObject.IsUnityNull())
				{
					coll.GetComponent<Ship>().GetDamage(bombDamage);
				}
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
