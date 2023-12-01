using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSessionPanel : MonoBehaviour
{
    [SerializeField] TMP_Text scoreDisplay;
    [SerializeField] int _score;
	private void Update()
	{
		UpdateDisplay();
	}
	public void SetScore(int score)
    {
        _score = score;
	}
	void UpdateDisplay()
	{
		scoreDisplay.text = _score.ToString();
	}
	
}
