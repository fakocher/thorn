using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public float health = 500f;
    public float speed = 3f;
    public int jumpForce = 10;
    public float timeMinBetweenJump = 5.0f;

    private bool direction; // Right or left
    public Vector2 minSpawnPosition;
    public Vector2 maxSpawnPosition;

    private Animator animator;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public AudioClip deathAudioClip;
    public AudioClip hitAudioClip;
    public AudioClip bloodAudioClip;
    private AudioSource audioSource;

    private float flashTimer = 0.0f;
    private float flashTimerMax = 0.5f;

    private float timeBossDead = 3.0f;
    private float timeJumping = 5.0f;
    private bool bossDead = false;
    private bool isJumping = false;
    private float jumpChance = 0.02f;


    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("walking", true);

        direction = false;
        sr.flipX = true;
        transform.position = new Vector2(maxSpawnPosition.x - 2, minSpawnPosition.y);
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
            if (!bossDead) { 
                animator.SetBool("death", true);
                transform.Rotate(new Vector3(0, 0, 90));
                bossDead = true;
            }
            timeBossDead -= Time.deltaTime;
            if(timeBossDead <= 0)
            {
                GameManager.instance.bossDead = true;
                Destroy(this);
            }
        }

        // Flip when reach square's border
        if(transform.position.x <= minSpawnPosition.x || transform.position.x >= maxSpawnPosition.x)
        {
            flip();
        }

        // Boss Jump
        if(!bossDead && !isJumping && Random.value < jumpChance)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
        if (isJumping)
        {
            timeJumping -= Time.deltaTime;

            if(timeJumping <= 0) {
                isJumping = false;
                timeJumping = timeMinBetweenJump;
            }
        }
    }

    void flip()
    {
        direction = !direction;
        sr.flipX = !direction;
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
