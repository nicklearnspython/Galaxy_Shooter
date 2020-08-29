using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool isPlayerOne = false;
    public bool isPlayerTwo = false;
    
    [SerializeField]
    private float _speed = 6f, _boostedSpeed = 10f;
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab;
    [SerializeField]
    private float _laserOffset = 0.8f;
    private Vector3 _tripleShotOffset = new Vector3(0.63f, 0f, 0f);
    [SerializeField] 
    private float _fireRate = 0.15f;
    private float _canFire = -1;
    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;

    private int isTripleShotActive = 0;
    private int isSpeedBoostActive = 0;
    private bool isShieldsActive = false;

    [SerializeField]
    private GameObject _playerShield;

    [SerializeField]
    private GameObject[] _EngineFires;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _explosionPrefab;

    private GameManager _gameManager;

    public int highscore;

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("HighScore", highscore);
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_audioSource == null)
        {
            Debug.LogError("Player::Start() Audio source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if (_uiManager == null)
        {
            Debug.LogError("Player::Start() The UI Manager is NULL.");
        }

        if (_spawnManager == null)
        {
            Debug.LogError("Player::Start() The spawn manager is NULL.");
        }

        if(_gameManager == null)
        {
            Debug.LogError("Player::Start() Game Manager is NULL.");
        }



        if (_gameManager.isCoopMode == false)
        {
            transform.position = new Vector3(0, -4, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (isPlayerOne && Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
        else if (isPlayerTwo && Input.GetKeyDown(KeyCode.Return) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = 0.0f;
        float verticalInput = 0.0f;

        if (isPlayerOne)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }
        else if (isPlayerTwo)
        {
            
            // Move Up/down
            if (Input.GetKey(KeyCode.UpArrow))
            {
                verticalInput = 1f;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                verticalInput = -1f;
            }

            // Move right/left
            if (Input.GetKey(KeyCode.RightArrow))
            {
                horizontalInput = 1f;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                horizontalInput = -1f;
            }
        }

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if(isSpeedBoostActive > 0)
        {
            transform.Translate(direction * _boostedSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        

        if (transform.position.x > 11.25)
        {
            transform.position = new Vector3(-11.24f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.25)
        {
            transform.position = new Vector3(11.24f, transform.position.y, 0);
        }

        float clamped_y = Mathf.Clamp(transform.position.y, -5f, 2f);
        transform.position = new Vector3(transform.position.x, clamped_y, 0);
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (isTripleShotActive > 0)
        {
            Instantiate(_tripleShotPrefab, transform.position + _tripleShotOffset, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset * Vector3.up, Quaternion.identity);
        }

        _audioSource.Play();
        
    }

    public void Damage()
    {
        if (isShieldsActive == true)
        {
            isShieldsActive = false;
            _playerShield.SetActive(false);
            return;
        }

        _lives--;


        if (_lives == 2)
        {
            _EngineFires[0].SetActive(true);
        }
        else if(_lives == 1)
        {
            _EngineFires[1].SetActive(true);
        }


        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore(_score);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void TripleShotCollected()
    {
        isTripleShotActive++;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(8.0f);
        isTripleShotActive--;
    }

    public void SpeedBoostCollected()
    {
        isSpeedBoostActive++;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        isSpeedBoostActive--;
    }

    public void ShieldCollected()
    {
        isShieldsActive = true;
        _playerShield.SetActive(true);
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public int ScoreCheck()
    {
        return _score;
    }
}
