using TMPro;
using UnityEngine;

public class CounterController : MonoBehaviour
{
	[SerializeField] TMP_Text _valueDisplay;
    [SerializeField] int _value;
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
	public int GetKills()
	{
		return _value;
	}
	
}
