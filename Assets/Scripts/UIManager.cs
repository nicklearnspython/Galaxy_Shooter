using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText, _bestScoreText;
    private int _bestScore = 0;
    
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Image _LivesImg;
    [SerializeField]
    private GameObject _pausePanel;
    [SerializeField]
    private Animator _pausePanelAnimator;
    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _bestScore = PlayerPrefs.GetInt("Best Score", 0);
        if(_bestScoreText != null)
        {
            _bestScoreText.text = "Best: " + _bestScore.ToString();
        }
        _scoreText.text = "Score: " + 0;

        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }

        if (_pausePanelAnimator == null)
        {
            Debug.LogError("UIManager::Start() Pause Menu Panel Animator object is NULL.");
        }

        _pausePanelAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    
    public void CheckForBestScore(int playerScore)
    {
        if (playerScore > _bestScore)
        {
            Debug.Log("New Highscore!");
            _bestScore = playerScore;
            if (_bestScoreText != null)
            {
                _bestScoreText.text = "Best: " + _bestScore.ToString();
            }
            PlayerPrefs.SetInt("Best Score", _bestScore);
            
        }
    }

    public int CheckForBestScore(int playerScore, int bestScore)
    {
        if(playerScore > bestScore)
        {
            return playerScore;
        }
        else
        {
            return bestScore;
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives < 1)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);

        StartCoroutine(GameOverFlicker());
    }

    IEnumerator GameOverFlicker()
    {
        float flickerTime = 0.5f;
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(flickerTime);
            _gameOverText.text = "";
            yield return new WaitForSeconds(flickerTime);
        }
    }

    public void PauseGame()
    {
        _pausePanel.SetActive(true);
        _pausePanelAnimator.SetBool("isPaused", true);
    }

    public void ResumeGame()
    {
        _pausePanel.SetActive(false);
    }
}
