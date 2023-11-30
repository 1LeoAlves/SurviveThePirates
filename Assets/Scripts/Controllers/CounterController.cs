using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CounterController : MonoBehaviour
{
	[SerializeField] TMP_Text _valueDisplay;
    [SerializeField] float _value;
	private void Update()
	{
		UpdateDisplay();
	}
	public void UpdateDisplay()
	{
		_valueDisplay.text = _value.ToString();
	}
	public void AddKill(int quantity)
    {
		_value += quantity;
    }
	
}
