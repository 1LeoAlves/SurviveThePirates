using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyShipController : Ship
{
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] Transform _shipTransform;
	[SerializeField] GameObject _attackPrefab;
	[SerializeField] SpriteRenderer _sailSpriteRenderer, _shipSpriteRenderer;
	[Space]
	[Header("Player Settings")]
	[Space]
	[Range(0, 100)]
	[SerializeField] float _shipHealth;
	[SerializeField] float _shipSpeed;
	[SerializeField] float _rotationSpeed;
	[SerializeField] int _sideTripleShootDelay;
	[Space]
	[Header("Attack Settings")]
	[Space]
	[SerializeField] int _attackDamage;
	[SerializeField] float _attackSpeed;
	[SerializeField] int _fowardattackRate;
	[SerializeField] int _sideattackRate;
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
    public override void GetDamage(float amount)
    {
		_shipHealth -= amount;
	}
	public override float GetShipHealth()
	{
		return _shipHealth;
	}
	public override void Die()
	{
		Instantiate(explosionPrefab,_shipTransform.position,Quaternion.identity);
		Destroy(gameObject);
	}
}
