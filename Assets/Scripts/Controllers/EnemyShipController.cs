using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using EnemyClass;

public class EnemyShipController : Ship
{
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] Transform _shipTransform;
	[SerializeField] GameObject _attackPrefab;
	[SerializeField] SpriteRenderer _sailSpriteRenderer, _shipSpriteRenderer;
	[SerializeField] ParticleSystem particleSys;
	[Space]
	[Header("Ship Settings")]
	[Space]
	[Range(0, 100)]
	[SerializeField] float _shipHealth;
	[SerializeField] float _shipSpeed;
	[SerializeField] float _rotationSpeed;
	[SerializeField] ShipStatus shipStatus;
	[Space]
	[Header("Enemy Settings")]
	[Space]
	[SerializeField] EnemyType enemyType;
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

	bool canAttackFoward = true, canAttackSideWays = true, hasDied;

	void Start()
    {

	}
	private void Update()
	{
		ControlShipState();
		ChangeShipSprites();
	}

	public override void GetDamage(float amount)
    {
		_shipHealth -= amount;
	}
	public override float GetShipHealth()
	{
		return _shipHealth;
	}
	void ControlShipState()
	{
		if (_shipHealth >= 75 && _shipHealth <= 100)
		{
			shipStatus = ShipStatus.Normal;
		}
		else if (_shipHealth >= 35 && _shipHealth < 75)
		{
			shipStatus = ShipStatus.Damaged;
		}
		else if (_shipHealth >= 10 && _shipHealth < 35)
		{
			shipStatus = ShipStatus.Almost_Destroyed;
		}
		else
		{
			shipStatus = ShipStatus.Destroyed;
		}
	}
	public void ChangeShipSprites()
	{
		switch (shipStatus)
		{
			case ShipStatus.Normal:
				_shipSpriteRenderer.sprite = shipParts[0];
				_sailSpriteRenderer.sprite = sailsParts[0];
				break;
			case ShipStatus.Damaged:
				_shipSpriteRenderer.sprite = shipParts[1];
				_sailSpriteRenderer.sprite = sailsParts[0];
				break;
			case ShipStatus.Almost_Destroyed:
				_shipSpriteRenderer.sprite = shipParts[2];
				_sailSpriteRenderer.sprite = sailsParts[1];
				break;
			case ShipStatus.Destroyed:
				_shipSpriteRenderer.sprite = shipParts[3];
				_sailSpriteRenderer.sprite = null;
				Die();
				break;
		}
	}
	public void Die()
	{
		if (hasDied is false)
		{
			hasDied = true;
			Explode();
		}
	}
	async void Explode()
	{
		particleSys.Play();
		await Task.Delay(5000);
		Destroy(gameObject);
	}
}
