﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;
    public Image loadingOverlay;

    public void ActivateGameOver(string causeOfGameOver)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        gameOverText.text = causeOfGameOver;
    }

    public void RetryButton()
    {
        Time.timeScale = 1;
        loadingOverlay.enabled = true;
        GameMaster.instance.LoadSceneRaw(SceneManager.GetActiveScene().name);
    }

    public void QuitButton()
    {
        Time.timeScale = 1;
        loadingOverlay.enabled = true;
        GameMaster.instance.LoadSceneRaw("LevelSelect");
    }
}