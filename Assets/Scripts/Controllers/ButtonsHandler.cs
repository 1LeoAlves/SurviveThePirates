using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ButtonsHandler : MonoBehaviour
{
	[SerializeField] SceneController sceneControllerRef;
	[SerializeField] Transform _canvasTransformReference;
    [SerializeField] GameObject _faderOutObject;
	[SerializeField] GameObject _faderInObject;
	[SerializeField] List<TMP_InputField> inputFields;
	GameObject ObjectToActivate;
	bool _canClickOnButtons = true;

	public void SetActiveObjectWithTransition(GameObject gameObject)
    {
		if (_canClickOnButtons)
		{
			_canClickOnButtons = false;
			StartCoroutine(Transition(gameObject));
		}
	}
	public void DelayedSetActiveObject(GameObject gameObject, float delay = 0)
	{
		if (_canClickOnButtons)
		{
			_canClickOnButtons = false;
			StartCoroutine(SetActiveDelay(gameObject, delay));
		}
	}
	public void ChangeScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    public void ChangeSceneWithTransition(string scenename)
    {
		if (_canClickOnButtons)
		{
			_canClickOnButtons = false;
			StartCoroutine(Transition(scenename));
		}
	}
	public IEnumerator SetActiveDelay(GameObject gameObject,float delay)
	{
		yield return new WaitForSeconds(delay);
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
		}
		_canClickOnButtons = true;
	}
	public IEnumerator Transition(string scenename)
    {
        Animator faderAnimator = Instantiate(_faderOutObject, _canvasTransformReference).GetComponentInChildren<Animator>();
        yield return new WaitForSeconds(faderAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.25f);
		_canClickOnButtons = true;
		ChangeScene(scenename);
	}
	public IEnumerator Transition(GameObject gameObject)
	{
		float errorTimeOfset = 0.2f;
		Animator faderOutAnimator = Instantiate(_faderOutObject, _canvasTransformReference).GetComponentInChildren<Animator>();
		yield return new WaitForSeconds(faderOutAnimator.GetCurrentAnimatorClipInfo(0).Length - errorTimeOfset);
		Instantiate(this._faderInObject, _canvasTransformReference);
		ObjectToActivate.SetActive(true);

		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
		else
		{
			gameObject.SetActive(true);
		}
		_canClickOnButtons = true;
	}
	public void SaveInputFieldData(TMP_InputField inputField)
    {
        PlayerPrefs.SetFloat(inputField.name, float.Parse(inputField.text));
    }
	public void SetObjectToActivate(GameObject gameObject)
	{
		ObjectToActivate = gameObject;
	}
	public void SaveConfig() 
	{
		if(inputFields[0].text != string.Empty)
		{
			float value = float.Parse(inputFields[0].text.Trim());
			if(value >= 1 && value <= 3)
			{
				PlayerPrefs.SetFloat("GameSessionTime", value);
			}
			else
			{
				DefaultConfig();
			}
		}
		else
		{
			DefaultConfig();
		}
		if (inputFields[1].text != string.Empty)
		{
			float value = float.Parse(inputFields[1].text.Trim());
			PlayerPrefs.SetFloat("EnemySpawnRate", value);
		}
	}
	void DefaultConfig()
	{
		PlayerPrefs.SetFloat("GameSessionTime", 3);
		inputFields[0].text = "3";
	}
}
