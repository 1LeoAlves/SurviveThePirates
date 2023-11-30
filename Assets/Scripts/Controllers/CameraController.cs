using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _followVelocity;
	void Update()
    {
        FollowTarget();
	}
    void FollowTarget()
    {
        if (_target != null)
        {
            float cameraZAxisPos = transform.position.z;
            Vector3 actualPosition = transform.position;
            Vector3 newPosition = new Vector3(_target.position.x, _target.position.y, cameraZAxisPos);
            transform.position = Vector3.Slerp(actualPosition, newPosition, Time.deltaTime * _followVelocity);
        }
    }
    public Transform GetTarget()
    {
        return _target;
    }
    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
	}
}
