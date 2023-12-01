using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyShipController : Ship
{
	[SerializeField] SceneController _sceneControllerRef;
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] Transform _shipTransform;
	[SerializeField] GameObject _attackPrefab, _tripleAttackPrefab;
	[SerializeField] SpriteRenderer _sailSpriteRenderer, _shipSpriteRenderer;
	[SerializeField] ParticleSystem particleSys;
	[SerializeField] Transform _target;
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
	[SerializeField] string _uniqueIDHash;
	[Space]
	[Header("Attack Settings")]
	[Space]
	[SerializeField] int _collisionDamage;
	[SerializeField] int _attackDamage;
	[SerializeField] float _attackSpeed;
	[SerializeField] int _fowardattackRate;
	[SerializeField] int _sideattackRate;
	[SerializeField] float _raycastShootRange;
	[Space]
	[Header("Collision Settings")]
	[Space]
	[SerializeField] float _raycastRangeDetection;
	[SerializeField] LayerMask raycastMask;

	
	bool canAttackFoward = true, hasDied, hasKamikazed;

	void Start()
    {
		enemyType = (EnemyType)Random.Range(0, 2);
		_sceneControllerRef = FindObjectOfType<SceneController>();
		_target = FindObjectOfType<PlayerController>().transform;
		_uniqueIDHash = gameObject.GetHashCode().ToString();
	}
	private void Update()
	{
		if(_target!= null)
		{
			Movement();
			ControlShipState();
			ChangeShipSprites();
		}
	}
	private void OnCollisionEnter2D(Collision2D coll)
	{
		if (enemyType.Equals(EnemyType.Chaser) && coll.gameObject.CompareTag("Player"))
		{
			hasKamikazed = true;
			ExplodeItSelf(coll.collider.transform.parent.parent.GetComponent<PlayerController>());
		}
	}
	void Movement()
	{
		if (!hasDied)
		{
			switch (enemyType) 
			{
				case EnemyType.Shooter:
					if(Vector2.Distance(_shipTransform.position, _target.position) < 10)
					{
						AimAndShoot();
					}
					else
					{
						ChasePlayer();
					}
					break;
				case EnemyType.Chaser:
					ChasePlayer();
					break;
			}
		}
	}
	void AimAndShoot()
	{
		RotateToTarget();
		Shoot();
	}
	async void Shoot()
	{
		RaycastHit2D[] hits = Physics2D.RaycastAll(_shipTransform.position, _shipTransform.up, _raycastShootRange);
		foreach(RaycastHit2D hit in hits)
		{
			if (hit.collider.CompareTag("Player") && canAttackFoward)
			{
				canAttackFoward = false;
				GameObject CanonBomb = Instantiate(_attackPrefab, new Vector2(_shipTransform.position.x, _shipTransform.position.y), _shipTransform.rotation);
				CanonBall canonball = CanonBomb.GetComponent<CanonBall>();
				canonball._shiplauncherHashID = _uniqueIDHash;
				canonball.bombDamage = _attackDamage;

				Rigidbody2D CanonBombRigid = CanonBomb.GetComponent<Rigidbody2D>();
				CanonBombRigid.AddForce(_shipTransform.up * _attackSpeed, ForceMode2D.Impulse);

				await Task.Delay(SceneController.SecondsToMiliseconds(_fowardattackRate));
				canAttackFoward = true;
			}
		}	
	}
	void ChasePlayer()
	{
		RotateToTarget();
		GoFoward();
	}
	void RotateToTarget()
	{
		float angle = Mathf.Atan2(_target.position.y - _shipTransform.position.y, _target.position.x - _shipTransform.position.x) * Mathf.Rad2Deg;
		Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
		_shipTransform.rotation = Quaternion.RotateTowards(_shipTransform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
	}
	void GoFoward()
	{
		Vector3 shipPosition = _shipTransform.position;
		if (enemyType.Equals(EnemyType.Chaser))
		{
			if(Vector2.Distance(_shipTransform.position, _target.position) <= 5f)
			{
				transform.position = shipPosition + (_shipTransform.up * (_shipSpeed * 2f) * Time.deltaTime);
			}
			else
			{
				transform.position = shipPosition + (_shipTransform.up * _shipSpeed * Time.deltaTime);
			}
		}
		else
		{
			transform.position = shipPosition + (_shipTransform.up * _shipSpeed * Time.deltaTime);
		}
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
	public bool DetectFowardCollision()
	{
		RaycastHit2D hit = Physics2D.Raycast(_shipTransform.position, _shipTransform.up, _raycastRangeDetection, raycastMask);
		if (hit)
		{
			return true;
		}
		return false;
	}
	public void Die()
	{
		if (hasDied is false)
		{
			hasDied = true;
			if (!hasKamikazed)
			{
				_sceneControllerRef.AddKillToCounter(1);
			}
			Explode();
		}
	}
	void ExplodeItSelf(Ship shipToDamage)
	{
		shipToDamage.GetDamage(_collisionDamage);
		_shipHealth = 0;
	}
	public void Explode()
	{
		_shipHealth = 0;
		particleSys.Play();
		Destroy(gameObject,5);
	}
}
