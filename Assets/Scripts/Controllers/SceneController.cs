using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] EndSessionPanel _endSessionPanel;
    [SerializeField] int _enemySpawnRate;
	[SerializeField] float _gameSessionTime;
    [SerializeField] Stopwatch _stopwatch;
    [SerializeField] CounterController _counterController;
	[SerializeField] GameObject _pausePanel;
	[SerializeField] GameObject _enemyPrefab;

	bool isSessionPaused;
	[Space]
    [Header("Enemy Spawn Settings")]
    [Space]
    [SerializeField] List<Vector2> enemySpawnPositions;

	void Start()
    {
        SetConfig();
	}
	private void Update()
	{
		ControlTime();
	}
    void SetConfig()
    {
        if (PlayerPrefs.GetFloat("GameSessionTime").IsUnityNull())
        {
            _gameSessionTime = 1;
		}
		else
        {
			_gameSessionTime = PlayerPrefs.GetFloat("GameSessionTime");
		}
		if (PlayerPrefs.GetFloat("GameSessionTime").IsUnityNull())
		{
			_enemySpawnRate = 15;
		}
		else
		{
			_enemySpawnRate = int.Parse(PlayerPrefs.GetFloat("EnemySpawnRate").ToString("F0"));
		}
		StartCoroutine(EnemySpawn());
	}
    void ControlTime()
    {
        if(_stopwatch.GetTime() <= 0)
        {
            if(!isSessionPaused)
            {
				EndSession();
			}
		}
    }
	IEnumerator EnemySpawn()
    {
		if (isSessionPaused)
		{
			yield return null;
		}
		yield return new WaitForSeconds(_enemySpawnRate);

        int randomPos = UnityEngine.Random.Range(0, enemySpawnPositions.Count);
		Instantiate(_enemyPrefab, enemySpawnPositions[randomPos], Quaternion.identity);
        if (isSessionPaused)
        {
            yield return new WaitUntil(() => isSessionPaused == false);
		}
        if(_stopwatch.GetTime() <= 0)
        {
			yield return null;
        }
        if (isSessionPaused)
        {
            yield return null;
		}
        StartCoroutine(EnemySpawn());
        yield return null;
    }
    public void AddKillToCounter(int quantity)
    {
        _counterController.AddKill(quantity);
	}
	public static int SecondsToMiliseconds(int seconds)
	{
		return seconds * 1000;
	}
    public void EndSession()
    {
        Destroy(FindObjectOfType<EnemyShipController>());
		_pausePanel.SetActive(true);
		isSessionPaused = true;
		_endSessionPanel.SetScore(_counterController.GetKills());
        foreach(EnemyShipController ship in FindObjectsOfType<EnemyShipController>())
        {
            ship.Explode();
        }
	}
}
