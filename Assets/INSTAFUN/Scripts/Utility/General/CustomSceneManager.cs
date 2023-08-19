using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : SceneSingleton<CustomSceneManager>
{
    private static IEnumerator _LoadSceneIE(string sceneName, Action completed_)
    {
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while (!asyncLoadLevel.isDone)
        {
            Debug.Log("Loading the Scene...");
            yield return null;
        }
        completed_?.Invoke();
    }

    public void LoadScene(string sceneName_, Action completed_)
    {
        this.StartCoroutine(_LoadSceneIE(sceneName_, completed_));
    }
}
