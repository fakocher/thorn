using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    public float health = 50f;
    public float speed = 1f;

    private bool direction; // Right or left
    private Animator animator;
    private SpriteRenderer sr;

    public List<GameObject> bonuses;
    public float bonusDropChance = 0.1f;

    // Either set spawn locations on the map or min / max locations.
    public List<GameObject> spawnLocations;
    public Vector2 minSpawnPosition;
    public Vector2 maxSpawnPosition;

    public AudioClip[] sounds;
    public AudioClip deathAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip bloodAudioClip;
    private AudioSource audioSource;

    private float flashTimer = 0.0f;
    private float flashTimerMax = 0.5f;
    private Color initialColor;

    // Use this for initialization
    void Start () {

        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Init animation
        animator.SetBool("walking", true);

        // Init direction to random
        direction = Random.value > 0.5f;
        if (!direction) {
            sr.flipX = true;
        }

        if (spawnLocations.Count == 0)
        {
            transform.position = new Vector2(Random.Range(minSpawnPosition.x, maxSpawnPosition.x), Random.Range(minSpawnPosition.y, maxSpawnPosition.y));
        }
        else
        {
            System.Random rand = new System.Random();
            int index = rand.Next(spawnLocations.Count);
            transform.position = spawnLocations[index].transform.position;
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
                Instantiate(bonuses[Random.Range(0, bonuses.Count)], transform.position, Quaternion.identity);
            }
            animator.SetBool("death", true);
            transform.Rotate(new Vector3(0, 0, 90));
            gameObject.layer = 12;
            sr.material.color = Color.grey;
            audioSource.PlayOneShot(deathAudioClip);
            Destroy(this);
        }

        // Flip direction if reaching sides of map
        /*
        if(transform.position.x > maxSpawnPosition.x || transform.position.y < minSpawnPosition.x) {
            flip();
        }*/

        // Play random sound at random moments
        float chanceToPlay = 0.5f;
        float randomFloat = Random.Range(0.0f, 100.0f);
        if (randomFloat < chanceToPlay)
        {
            float pitch = Random.Range(0.5f, 1.5f);
            AudioClip randomSound = sounds[Random.Range(0, sounds.Length)];
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
        initialColor = sr.material.color;
        Color newColor = sr.material.color;
        newColor.b = 0.0f;
        newColor.g = 0.0f;
        sr.material.color = newColor;
    }

    void flip()
    {
        direction = !direction;
        sr.flipX = !sr.flipX;
    }
}
