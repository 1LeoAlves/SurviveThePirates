using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : Ship
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

    bool canAttackFoward = true, canAttackSideWays = true;
	void Start()
    {
        
    }

    void Update()
    {
        Movement();
        Rotate();
		FowardAttack();
        SideAttack();
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
			
            GameObject CanonBomb = Instantiate(_attackPrefab, _shipTransform.position, _shipTransform.rotation);
            CanonBomb.GetComponent<CanonBall>().launchedShipName = name;

			Rigidbody2D CanonBombRigid = CanonBomb.GetComponent<Rigidbody2D>();
			CanonBombRigid.AddForce(_shipTransform.up * attackSpeed,ForceMode2D.Impulse);
			
            await Task.Delay(SecondsToMiliseconds(FowardattackRate));
			canAttackFoward = true;
		}
    }
	async void SideAttack()
    {
		if (Input.GetAxis("Fire2") != 0 && canAttackSideWays)
		{
            canAttackSideWays = false;


			await Task.Delay(SecondsToMiliseconds(SideWaysattackRate));
			canAttackSideWays = true;
		}
        
	}
    public override float GetShipHealth()
    {
        return shipHealth;
    }
    public void GetDamage(float amount)
    {
        shipHealth -= amount;
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
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_shipTransform.position,transform.position + _shipTransform.up * _raycastRangeDetection);
	}
}
