using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform _shipTransform;
    [SerializeField] float _playerVelocity;

	void Start()
    {
        
    }

    void Update()
    {
        Movement();
        Rotate();
	}
    void Movement()
    {
		float direction = Input.GetAxisRaw("Vertical");

		if (direction != 0) 
        {
            Vector3 playerPosition = _shipTransform.position;
            transform.position = playerPosition + (_shipTransform.up * _playerVelocity * direction * Time.deltaTime);
        }
    }
    void Rotate()
    {
        float rotationDirection = Input.GetAxis("Horizontal");

		if (rotationDirection != 0)
        {

        }
    }
}
