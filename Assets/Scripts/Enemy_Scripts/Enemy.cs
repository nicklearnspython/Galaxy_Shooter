using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;
    private bool isExploding = false;

    [SerializeField]
    private AudioClip _explosionAudio;
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _enemyLaserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player_1").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
        {
            Debug.LogError("Enemy::Start() _player is null.");
        }
        
        _anim = gameObject.GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("Enemy::Start() _anim is null");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio source on enemy is NULL.");
        }
        else
        {
            _audioSource.clip = _explosionAudio;
        }

        StartCoroutine(EnemyFireLaserRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
           Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            DestructionSequence();
            
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
               _player.AddScore(10);
            }

            DestructionSequence();
        }
    }

    private void DestructionSequence()
    {
        isExploding = true;
        _anim.SetTrigger("OnEnemyDeath");
        _audioSource.Play();
        _speed /= 1.5f;
        Destroy(this.gameObject.GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.6f);
    }

    IEnumerator EnemyFireLaserRoutine()
    {
        while (true)
        {
            
            if(isExploding == false)
            {
                Instantiate(_enemyLaserPrefab, transform.position + new Vector3(0, -0.65f, 0), Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
    }
}
