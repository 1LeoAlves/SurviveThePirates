using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : Ship
{
	[Header("Player Settings")]
	[Space]
	[Range(0, 100)]
	[SerializeField] float shipHealth;
	[SerializeField] Transform _shipTransform;
	[SerializeField] GameObject _attackPrefab;
	[SerializeField] float _shipSpeed;
	[SerializeField] float _rotationSpeed;
	[SerializeField] int sideTripleShootDelay;
	[Space]
	[Header("Attack Settings")]
	[Space]
	[SerializeField] float attackSpeed;
	[SerializeField] int FowardattackRate;
	[SerializeField] int SideWaysattackRate;
	[Space]
	[Header("Collision Settings")]
	[Space]
	[SerializeField] float _raycastRangeDetection;
	[SerializeField] LayerMask raycastMask;

	void Start()
    {
        
    }

    void Update()
    {
    }
    public override void GetDamage()
    {
		Debug.Log("A");
    }
	public override float GetShipHealth()
	{
		return shipHealth;
	}
}
