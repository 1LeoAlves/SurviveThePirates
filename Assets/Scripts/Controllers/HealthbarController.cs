using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
	PlayerController _playerController;
	[SerializeField] Slider _healthBarSlider;

	private void Start()
	{
		_playerController = transform.parent.GetComponent<PlayerController>();
	}
	private void Update()
	{
		ControlHealthbarSize();
	}
	void ControlHealthbarSize()
	{
		_healthBarSlider.value = _playerController.GetPlayerHealth();
	}
}
