using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 10;
    public float jumpTakeOffSpeed = 13;
    public float shootSpeed = 1;
    public float explosiveBullets = 0;
    public float bulletSize = 1;
    public int hp = 3;
    public int maxHp = 6;
    public AudioClip dyingSound;
    public AudioClip dyingByFallingSound;
    public AudioClip hitSound;
    public AudioClip takeBonusAudioClip;
    public AudioClip[] gruntSounds;
    // TODO Capacity to slow time down??

    private int currentHp;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    private float flashTimer = 0.0f;
    private float flashTimerMax = 0.5f;

    // Use this for initialization
    void Awake() {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currentHp = hp;
        GetComponent<HealthUI>().updateHealth(currentHp, hp);
    }

    new void Update() {
        base.Update();
        if(transform.position.y < -3) {
            audioSource.PlayOneShot(dyingByFallingSound);
            Die();
        }

        // Flashing
        if (flashTimer > 0.0f)
        {
            flashTimer -= Time.deltaTime;
            Color newColor = spriteRenderer.material.color;
            newColor.b += Time.deltaTime / flashTimerMax;
            newColor.g += Time.deltaTime / flashTimerMax;
            spriteRenderer.material.color = newColor;
        }
    }

    protected override void ComputeVelocity() {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded) {
            velocity.y = jumpTakeOffSpeed;
        }
        else if (Input.GetButtonUp("Jump")) {
            if (velocity.y > 0) {
                velocity.y = velocity.y * 0.5f;
            }
        }
        
        if (move.x < - 0.05f && transform.localScale.x > 0) {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            transform.GetComponentInChildren<GunController>().transform.Rotate(new Vector3(0, 0, 180));
        } else if (move.x > 0.05f && transform.localScale.x < 0) {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            transform.GetComponentInChildren<GunController>().transform.Rotate(new Vector3(0, 0, 180));
        }

        animator.SetBool("walking", move.x != 0);
        //animator.SetBool("grounded", grounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    public void Hit(bool direction) {
        currentHp--;
        GetComponent<HealthUI>().updateHealth(currentHp, hp);

        if(currentHp <= 0)
        {
            audioSource.PlayOneShot(dyingSound);
            Die(direction);
        }
        else
        {
            AudioClip randomGruntSound = gruntSounds[Random.Range(0, gruntSounds.Length)];
            audioSource.PlayOneShot(randomGruntSound);
            audioSource.PlayOneShot(hitSound);
            rb2d.AddForce(new Vector2(300 * (direction ? -1 : 1), 300));
            Invoke("StopForce", 1);

            // Flashing
            flashTimer = flashTimerMax;
            Color newColor = spriteRenderer.material.color;
            newColor.b = 0.0f;
            newColor.g = 0.0f;
            spriteRenderer.material.color = newColor;
        }
    }

    void StopForce() {
        rb2d.velocity = Vector2.zero;
    }

    void Die(bool direction = false) {
        rb2d.AddForce(new Vector2(500 * (direction ? -1 : 1), 500));
        rb2d.constraints = 0;
        animator.SetBool("death", true);
        GameManager.instance.GameOver();
        Destroy(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {    
        // Apply bonuses
        if (collision.tag == "Bonus")
        {
            // Move speed
            maxSpeed += collision.GetComponent<Bonus>().moveSpeed;

            // Jump height
            jumpTakeOffSpeed += collision.GetComponent<Bonus>().jumpHeight;

            // Shooting speed
            shootSpeed += collision.GetComponent<Bonus>().shootSpeed;

            // Bullet size
            bulletSize += collision.GetComponent<Bonus>().bulletSize;

            // Explosive ammo
            float explosiveAmmo = collision.GetComponent<Bonus>().explosiveAmmo;
            float explosion = GetComponentInChildren<GunController>().explosion;
            if (explosiveAmmo > 0.0f && explosion == 0.0f)
            {
                GetComponentInChildren<GunController>().explosion += explosiveAmmo;
                shootSpeed /= 2.0f;
                explosiveBullets += explosiveAmmo;
            }

            // HP
            currentHp = currentHp < hp ? collision.GetComponent<Bonus>().heal + hp : hp;
            currentHp = currentHp > hp ? hp : currentHp;
            hp += hp < maxHp ? collision.GetComponent<Bonus>().lifeIncrease : 0;
            GetComponent<HealthUI>().updateHealth(currentHp, hp);

            // Play sound
            audioSource.PlayOneShot(takeBonusAudioClip);

            // Destroy bonus
            Destroy(collision.gameObject);
        }
    }
}