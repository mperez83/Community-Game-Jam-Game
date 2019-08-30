﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    public Image loadingPanel;

    public void LevelButton(string levelToLoad)
    {
        loadingPanel.enabled = true;
        StartCoroutine(LoadYourAsyncScene(levelToLoad));
    }

    IEnumerator LoadYourAsyncScene(string levelToLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelToLoad);

        AudioManager.Instance.Stop("Menu Scene Soundtrack");
        AudioManager.Instance.Play("Building Soundtrack");


        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}