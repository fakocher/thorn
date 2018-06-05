using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public float health = 50f;
    public float speed = 2f;

    private bool direction; // Right or left
    public Vector2 minSpawnPosition;
    public Vector2 maxSpawnPosition;

    private Animator animator;
    private SpriteRenderer sr;

    public AudioClip deathAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip bloodAudioClip;
    private AudioSource audioSource;

    private float flashTimer = 0.0f;
    private float flashTimerMax = 0.5f;

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("walking", true);

        direction = Random.value > 0.5f;
        transform.position = new Vector2(Random.Range(minSpawnPosition.x, maxSpawnPosition.x), Random.Range(minSpawnPosition.y, maxSpawnPosition.y));
    }
	
	void Update () {
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

    private void FixedUpdate()
    {
        // Handle movement
        transform.Translate(Vector3.right * (direction ? 1 : -1) * speed * 1.5f * Time.deltaTime);

        // Handle death
        if (health <= 0)
        {
            animator.SetBool("death", true);
            transform.Rotate(new Vector3(0, 0, 90));
            Destroy(this);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Handle collision with player
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerPlatformerController>().Hit(transform.position.x - other.transform.position.x > 0);
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
}
