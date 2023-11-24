using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsHandler : MonoBehaviour
{
    [SerializeField] Transform canvasTransformReference;
    [SerializeField] GameObject faderOutInObject;
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ChangeScene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    public void ChangeSceneWithTransition(string scenename)
    {
        StartCoroutine(Transition(scenename));
	}
    public IEnumerator Transition(string scenename)
    {
        Animator faderAnimator = Instantiate(faderOutInObject, canvasTransformReference).GetComponentInChildren<Animator>();
        yield return new WaitForSeconds(faderAnimator.GetCurrentAnimatorClipInfo(0).Length);
        ChangeScene(scenename);
    }
}
