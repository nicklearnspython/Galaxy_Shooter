using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _angularSpeed = 7.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioClip _explosionAudio;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();


        if (_spawnManager == null)
        {
            Debug.LogError("Asteroid::Start() _spawnManager is NULL.")
;       }

        if(_audioSource == null)
        {
            Debug.LogError("Asteroid:: Start() _audioSource is NULL.");
        }
        else
        {
            _audioSource.clip = _explosionAudio;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, _angularSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject.GetComponent<Collider2D>());
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            
            _spawnManager.StartSpawning();
            
            Destroy(this.gameObject, 0.5f);
        }
    }

}
