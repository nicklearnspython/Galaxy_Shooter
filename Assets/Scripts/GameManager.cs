using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager;

    private Player _player;
    [SerializeField]
    private bool _isGameOver;

    public bool isCoopMode = false;
    public bool isPaused = false;

    private int _bestScore;

    private void Start()
    {
        _player = GameObject.Find("Player_1").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("GameManager::Start() Player_1 is NULL.");
        }
        
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("GameManager::Start() UI Manager is NULL.");
        }

        _bestScore = PlayerPrefs.GetInt("Best_Score", 0);
    }

    private void Update()
    {
        // if the r key was pressed 
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            if (isCoopMode)
            {
                SceneManager.LoadScene(2); // Coop Mode Scene
            }
            else
            {
                SceneManager.LoadScene(1); // Current Game Scene (using a int (1) is faster than using a str "Game")
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void GameOver()
    {
        Debug.Log("GameManager::GameOver() Called");
        _isGameOver = true;
    }

    private void PauseGame()
    {
        isPaused = true;
        Debug.Log("GameManager::PauseGame() Called");
        Time.timeScale = 0;
        _uiManager.PauseGame();
    }

    public void ResumeGame()
    {
        isPaused = false;
        Debug.Log("GameManager::ResumeGame()");
        Time.timeScale = 1;
        _uiManager.ResumeGame();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0); // Main Menu Scene
    }
}
