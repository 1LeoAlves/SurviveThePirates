using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Ship
{
	[SerializeField] SceneController sceneControllerRef;
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] Transform _shipTransform;
	[SerializeField] GameObject _attackPrefab, _tripleAttackPrefab;
    [SerializeField] SpriteRenderer _sailSpriteRenderer,_shipSpriteRenderer;
	[SerializeField] ParticleSystem particleSys;
	[Space]
	[Header("Ship Settings")]
	[Space]
	[Range(0, 100)]
    [SerializeField] float _shipHealth;
	[SerializeField] float _shipSpeed;
    [SerializeField] float _rotationSpeed;
	[SerializeField] ShipStatus shipStatus;
	[SerializeField] string _uniqueIDHash;
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

	private void Start()
	{
		_uniqueIDHash = gameObject.GetHashCode().ToString();
	}
	void Update()
    {
        Movement();
        Rotate();
		FowardAttack();
        SideAttack();
        ControlShipState();
		ChangeShipSprites();
	}
	void Movement()
    {
		float direction = Input.GetAxisRaw("Vertical");

		if (direction > 0 && !DetectFowardCollision() && !hasDied) 
        {
            Vector3 playerPosition = _shipTransform.position;
            transform.position = playerPosition + (_shipTransform.up * _shipSpeed * Time.deltaTime);
        }
    }
    void Rotate()
    {
        int invertDirection = -1;
        float rotationDirection = Input.GetAxis("Horizontal") * invertDirection;

		if (rotationDirection != 0 && !hasDied)
        {
            Vector3 newEulerAngles = new Vector3(_shipTransform.eulerAngles.x, _shipTransform.eulerAngles.y, _shipTransform.eulerAngles.z + (_rotationSpeed * rotationDirection * Time.deltaTime));
			_shipTransform.eulerAngles = newEulerAngles;
        }
    }
    async void FowardAttack()
    {
        if (Input.GetAxis("Fire1") != 0 && canAttackFoward && !hasDied)
        {
            canAttackFoward = false;
            _attackPrefab.hideFlags = HideFlags.HideInHierarchy;
			GameObject CanonBomb = Instantiate(_attackPrefab,new Vector2(_shipTransform.position.x,_shipTransform.position.y), _shipTransform.rotation);
			CanonBall canonball = CanonBomb.GetComponent<CanonBall>();
			canonball._shiplauncherHashID = _uniqueIDHash;
			canonball.bombDamage = _attackDamage;

			Rigidbody2D CanonBombRigid = CanonBomb.GetComponent<Rigidbody2D>();
			CanonBombRigid.AddForce(_shipTransform.up * _attackSpeed,ForceMode2D.Impulse);
			
            await Task.Delay(SceneController.SecondsToMiliseconds(_fowardattackRate));
			canAttackFoward = true;
		}
    }
	async void SideAttack()
    {
		if (Input.GetAxis("Fire2") != 0 && canAttackSideWays && !hasDied)
		{
            canAttackSideWays = false;

			GameObject TripleAttackGameObject = _tripleAttackPrefab;
			CanonBall[] canonBalls = TripleAttackGameObject.GetComponentsInChildren<CanonBall>();

			foreach (CanonBall canonball in canonBalls)
			{
				canonball._shiplauncherHashID = _uniqueIDHash;
				canonball.bombDamage = _attackDamage;
			}
			TripleAttackGameObject = Instantiate(TripleAttackGameObject, transform.position, _shipTransform.rotation);
			CanonBall[] launchedcanonBalls = TripleAttackGameObject.GetComponentsInChildren<CanonBall>();
			foreach (CanonBall canonball in launchedcanonBalls)
			{
				Rigidbody2D CanonBombRigid = canonball.gameObject.GetComponent<Rigidbody2D>();
				CanonBombRigid.AddForce(_shipTransform.right * _attackSpeed, ForceMode2D.Impulse);
			}

			await Task.Delay(SceneController.SecondsToMiliseconds(_sideattackRate));
			Destroy(TripleAttackGameObject.gameObject);
			canAttackSideWays = true;
		}
	}
    public override float GetShipHealth()
    {
        return _shipHealth;
    }
    public override void GetDamage(float amount)
    {
        _shipHealth -= amount;
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
    void ControlShipState()
    {
        if(_shipHealth >= 75 && _shipHealth <= 100)
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
		await Task.Delay(2500);
		sceneControllerRef.EndSession();
		if (!gameObject.IsUnityNull())
			Destroy(gameObject);
	}
}

