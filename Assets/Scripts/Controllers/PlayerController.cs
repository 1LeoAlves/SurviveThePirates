using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : Ship
{
	[SerializeField] GameObject explosionPrefab;
	[SerializeField] Transform _shipTransform;
	[SerializeField] GameObject _attackPrefab;
    [SerializeField] SpriteRenderer _sailSpriteRenderer,_shipSpriteRenderer;
	[Space]
	[Header("Player Settings")]
	[Space]
	[Range(0, 100)]
    [SerializeField] float _shipHealth;
	[SerializeField] float _shipSpeed;
    [SerializeField] float _rotationSpeed;
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

    bool canAttackFoward = true, canAttackSideWays = true;

    void Update()
    {
        Movement();
        Rotate();
		FowardAttack();
        SideAttack();
        ControlShipState();
	}
    void Movement()
    {
		float direction = Input.GetAxisRaw("Vertical");

		if (direction > 0 && !DetectFowardCollision()) 
        {
            Vector3 playerPosition = _shipTransform.position;
            transform.position = playerPosition + (_shipTransform.up * _shipSpeed * Time.deltaTime);
        }
    }
    void Rotate()
    {
        int invertDirection = -1;
        float rotationDirection = Input.GetAxis("Horizontal") * invertDirection;

		if (rotationDirection != 0)
        {
            Vector3 newEulerAngles = new Vector3(_shipTransform.eulerAngles.x, _shipTransform.eulerAngles.y, _shipTransform.eulerAngles.z + (_rotationSpeed * rotationDirection));
			_shipTransform.eulerAngles = newEulerAngles;
        }
    }
    async void FowardAttack()
    {
        if (Input.GetAxis("Fire1") != 0 && canAttackFoward)
        {
            canAttackFoward = false;
            _attackPrefab.hideFlags = HideFlags.HideInHierarchy;
			GameObject CanonBomb = Instantiate(_attackPrefab,new Vector2(_shipTransform.position.x,_shipTransform.position.y), _shipTransform.rotation);
			CanonBall canonball = CanonBomb.GetComponent<CanonBall>();
			canonball.shiplauncherName = name;
			canonball.bombDamage = _attackDamage;

			Rigidbody2D CanonBombRigid = CanonBomb.GetComponent<Rigidbody2D>();
			CanonBombRigid.AddForce(_shipTransform.up * _attackSpeed,ForceMode2D.Impulse);
			
            await Task.Delay(SecondsToMiliseconds(_fowardattackRate));
			canAttackFoward = true;
		}
    }
	async void SideAttack()
    {
		if (Input.GetAxis("Fire2") != 0 && canAttackSideWays)
		{
            canAttackSideWays = false;

			_attackPrefab.hideFlags = HideFlags.HideInHierarchy;
			Debug.Log("A");
			for (int i = -1; i < 2; i++)
            {
				Debug.Log("B");
				GameObject CanonBomb = Instantiate(_attackPrefab, new Vector2(_shipTransform.position.x, _shipTransform.position.y + i), _shipTransform.rotation);
				CanonBall canonball = CanonBomb.GetComponent<CanonBall>();
				canonball.shiplauncherName = name;
                canonball.bombDamage = _attackDamage;

				Rigidbody2D CanonBombRigid = CanonBomb.GetComponent<Rigidbody2D>();
				CanonBombRigid.AddForce(_shipTransform.right * _attackSpeed, ForceMode2D.Impulse);
			}

			await Task.Delay(SecondsToMiliseconds(_sideattackRate));
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
    public int SecondsToMiliseconds(int seconds)
    {
        return seconds * 1000;
    }
    void ControlShipState()
    {
        if(_shipHealth >= 75 && _shipHealth <= 100)
        {
			_shipSpriteRenderer.sprite = shipParts[0];
            _sailSpriteRenderer.sprite = sailsParts[0];
		}
        else if (_shipHealth >= 35 & _shipHealth < 75)
        {
			_shipSpriteRenderer.sprite = shipParts[1];
			_sailSpriteRenderer.sprite = sailsParts[0];
		}
		else if (_shipHealth >= 10 && _shipHealth < 35)
        {
			_shipSpriteRenderer.sprite = shipParts[2];
			_sailSpriteRenderer.sprite = sailsParts[1];
		}
		else
        {
            Die();
        }
	}
    public override void Die()
    {
        Destroy(gameObject);
    }
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_shipTransform.position,transform.position + _shipTransform.up * _raycastRangeDetection);
	}
}
