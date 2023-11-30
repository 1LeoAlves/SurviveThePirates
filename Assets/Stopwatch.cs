using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stopwatch : MonoBehaviour
{
	[SerializeField] TMP_Text display;
	private bool _status;
	public bool typeUp, startAwake;
	private float _time;

	void Start()
	{
		if (startAwake)
		{
			TurnOnOff();
		}
		_time = PlayerPrefs.GetFloat("GameSessionTime") * 60;
	}
	void Update()
	{
		Work();
	}
	void Work()
	{
		if (_status)
		{
			if (typeUp)
			{
				_time += Time.deltaTime;
			}
			else
			{
				if (_time > 0)
					_time -= Time.deltaTime;
			}
			UpdateDisplay();
		}
	}
	public void UpdateDisplay()
	{
		display.text = FormatToTime(_time);
	}
	public void TurnOnOff()
	{
		_status = !_status;
		UpdateDisplay();
	}
	public void ChangeType()
	{
		typeUp = !typeUp;
		UpdateDisplay();
	}
	public float GetTime()
	{
		return _time;
	}
	public void SetTime(float time)
	{
		_time = time;
	}
	public bool GetStatus()
	{
		return _status;
	}
	public static string FormatToTime(float time) //Respons�vel por transformar o float em string (retorna um string) e colocar no TMP_text (que seriam os cronometros) que � o Text da biblioteca do TMPro.
	{
		float segundos = time;
		int minutos = 0;

		while (segundos >= 60)
		{
			minutos++;
			segundos -= 60;
		}

		if (minutos == 0) //Os if s�o usados para verificar se os segundos ou minutos tem mais uma casa numerica, pois vai v�rias o tamanho da string a depender 
		{
			if (segundos < 10)
				return $"0{minutos}:0{segundos.ToString().Substring(0, 1)}"; // Tostring -> transforma o valor/variavel "segundos" em string e o Substring(0,1) pega apenas os caract�res do 0 at� o 1, Ex: "P�o" -> "P�o".Substring(0,1) -> "P".
			else
				return $"0{minutos}:{segundos.ToString().Substring(0, 2).Replace(",", "")}"; //O replace substitui a "," por "" (nada).
		}
		else if (minutos > 0)
		{
			if (segundos < 10)
				return $"0{minutos}:0{segundos.ToString().Substring(0, 1)}";
			else
				return $"0{minutos}:{segundos.ToString().Substring(0, 2).Replace(",", "")}";
		}
		else if (minutos > 10)
		{
			if (segundos < 10)
				return $"{minutos}:0{segundos.ToString().Substring(0, 1)}";
			else
				return $"{minutos}:{segundos.ToString().Substring(0, 2).Replace(",", "")}";
		}
		else
			return "ERROR";
	}
}
