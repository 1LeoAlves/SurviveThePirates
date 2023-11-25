using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Header("Player Settings")]
	[Space]
	[Range(0, 100)]
    [SerializeField] float PlayerHealth;
    [SerializeField] Transform _shipTransform;
    [SerializeField] float _playerVelocity;
    [SerializeField] float _rotationSpeed;
    [SerializeField] int sideTripleShootDelay;

	void Start()
    {
        
    }

    void Update()
    {
        Movement();
        Rotate();
        Attack();
	}
    void Movement()
    {
		float direction = Input.GetAxisRaw("Vertical");

		if (direction > 0) 
        {
            Vector3 playerPosition = _shipTransform.position;
            transform.position = playerPosition + (_shipTransform.up * _playerVelocity * Time.deltaTime);
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
    void Attack()
    {
        if (Input.GetKeyDown("Fire1"))
        {

        }
        if (Input.GetKeyDown("Fire1"))
        {

        }
    }
    public float GetPlayerHealth()
    {
        return PlayerHealth;
    }
    public void GetDamage(float amount)
    {
        PlayerHealth -= amount;
    }
}
