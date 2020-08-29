using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3;
    [SerializeField] 
    private int powerupID; //0 = TripleShot, 1 = Speed, 2 = Shield

    [SerializeField]
    private float _volume = 1f;
    [SerializeField]
    private AudioClip _powerupClip;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                AudioSource.PlayClipAtPoint(_powerupClip, new Vector3(0f, 0f, -10f), _volume);

                switch(powerupID)
                {
                    case 0:
                        player.TripleShotCollected();
                        break;
                    case 1:
                        player.SpeedBoostCollected();
                        break;
                    case 2:
                        player.ShieldCollected();
                        break;
                    default:
                        Debug.Log("No Powerup on record collected");
                        break;
                }

                Destroy(this.gameObject);
            }  
        }
    }

}
