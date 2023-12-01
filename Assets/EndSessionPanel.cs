using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndSessionPanel : MonoBehaviour
{
    [SerializeField] TMP_Text scoreDisplay;
    [SerializeField] int _score;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void SetScore(int score)
    {
        _score = score;
	}
}
