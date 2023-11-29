using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{
	Ship _controller;
	[SerializeField] Slider _healthBarSlider;

	private void Start()
	{
		_controller = transform.parent.GetComponent<Ship>();
	}
	private void Update()
	{
		UpdateHealthbar();
	}
	void UpdateHealthbar()
	{
		_healthBarSlider.value = _controller.GetShipHealth();
	}
}
