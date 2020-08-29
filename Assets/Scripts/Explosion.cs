using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null)
        {
            Debug.LogError("Explosion::Start() Audio component is NULL");
        }

        Destroy(this.gameObject, 3f);
    }
}
