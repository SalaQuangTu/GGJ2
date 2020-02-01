using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public Animator transition;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadScene(int index)
    {
        transition.SetTrigger("Start");
        AsyncOperation load = SceneManager.LoadSceneAsync(index);
        while (!load.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return 0;
    }
}
