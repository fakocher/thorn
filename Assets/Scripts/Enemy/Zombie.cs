using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    public float health = 50f;
    public float speed = 1f;

    private bool direction; // Right or left
    private float flipChance = 0.005f;
    private Animator animator;
    private SpriteRenderer sr;

    public List<GameObject> bonuses;
    private float bonusDropChance = 0.1f;
    
    public Vector2 minSpawnPosition;
    public Vector2 maxSpawnPosition;
    private GameObject[] spawns;

    public AudioClip[] sounds;
    public AudioClip deathAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip bloodAudioClip;
    private AudioSource audioSource;
    private float groanChance = 0.005f;

    private float flashTimer = 0.0f;
    private float flashTimerMax = 0.5f;

    // Use this for initialization
    void Start () {

        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spawns = GameObject.FindGameObjectsWithTag("Spawn");

        // Init animation
        animator.SetBool("walking", true);

        // Init direction to random
        direction = Random.value > 0.5f;
        sr.flipX = !direction;
        
        if (spawns.Length == 0)
        {
            transform.position = new Vector2(Random.Range(minSpawnPosition.x, maxSpawnPosition.x), Random.Range(minSpawnPosition.y, maxSpawnPosition.y));
        }
        else
        {
            transform.position = spawns[Random.Range(0, spawns.Length)].transform.position;
        }
    }

    void Update()
    {
        // Flashing
        if (flashTimer > 0.0f)
        {
            flashTimer -= Time.deltaTime;
            Color newColor = sr.material.color;
            newColor.b += Time.deltaTime / flashTimerMax;
            newColor.g += Time.deltaTime / flashTimerMax;
            sr.material.color = newColor;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {

        // Handle movement
        transform.Translate(Vector3.right * (direction ? 1 : -1) * speed * 1.5f * Time.deltaTime);

        // Handle death
        if (health <= 0 || transform.position.y < minSpawnPosition.y - 10) {
            // Drop bonus?
            if (Random.value < bonusDropChance) {
                Vector2 bonusPosition = transform.position;
                bonusPosition.y += 0.5f;
                Instantiate(bonuses[Random.Range(0, bonuses.Count - 1)], bonusPosition, Quaternion.identity);
            }
            animator.SetBool("death", true);
            transform.Rotate(new Vector3(0, 0, 90));
            gameObject.layer = 12;
            sr.material.color = Color.grey;
            audioSource.PlayOneShot(deathAudioClip);
            Destroy(this);
        }

        // Flip direction if reaching sides of map
        // DEPRECATED: now use triggers with tag "ZombieLimit"
        /*
        if(transform.position.x > maxSpawnPosition.x || transform.position.y < minSpawnPosition.x) {
            flip();
        }*/

        // Random flip of direction
        if (Random.value < flipChance)
        {
            flip();
        }


        // Play random sound at random moments
        if (Random.value < groanChance)
        {
            float pitch = Random.Range(0.5f, 1.5f);
            AudioClip randomSound = sounds[Random.Range(0, sounds.Length - 1)];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(randomSound);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Handle collision with player
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerPlatformerController>().Hit(transform.position.x - other.transform.position.x > 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision with limits
        if (collision.gameObject.CompareTag("ZombieLimit"))
        {
            flip();
        }
    }

    public void hit(float damage)
    {
        health -= damage;
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitAudioClip);
        audioSource.PlayOneShot(bloodAudioClip);

        // Flashing
        flashTimer = flashTimerMax;
        Color newColor = sr.material.color;
        newColor.b = 0.0f;
        newColor.g = 0.0f;
        sr.material.color = newColor;
    }

    void flip()
    {
        direction = !direction;
        sr.flipX = !direction;
    }
}
