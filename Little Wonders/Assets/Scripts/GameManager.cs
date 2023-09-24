using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool canControl = true;

    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gamePausedPanel;

    private int completedCastleCells = 0;
    private float timeInGame;
    private bool isPaused;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OperateGameStart();
        timeInGame = 0f;
    }

    private void Update()
    {
        timeInGame += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OperateGamePause();
        }
    }

    private void OperateGamePause()
    {
        gamePausedPanel.SetActive(isPaused);
        if (isPaused )
            FreezeGame();
        else 
            ResumeGame();
        isPaused = !isPaused;
    }

    public void AddCastleCellCount()
    {
        completedCastleCells++;
        if (completedCastleCells == 9)
            OperateGameOver();
    }

    private void OperateGameStart()
    {
        gameStartPanel.SetActive(true);
        FreezeGame();
    }

    private void OperateGameOver()
    {
        gameOverPanel.SetActive(true);
        gameOverPanel.GetComponentInChildren<TMP_Text>().text = 
            $"Поздравляем! Вы прошли игру! Ваше время \n{Mathf.FloorToInt(timeInGame / 60)}:{Mathf.FloorToInt(timeInGame % 60)}" +
            $"\nВы можете продолжить игру или улучшить результат.";
        FreezeGame();
    }

    public void FreezeGame()
    {
        Time.timeScale = 0f;
        canControl = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        canControl = true;
    }

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
