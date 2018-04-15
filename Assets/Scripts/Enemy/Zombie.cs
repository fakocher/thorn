using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

    public float health = 50f;
    public float speed = 1f;
    public Vector2 min;
    public Vector2 max;

    private bool direction;
    private Animator animator;

    public List<GameObject> bonuses;
    public float bonusDropChance = 0.1f;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        animator.SetBool("walking", true);
        direction = Random.value > 0.5f;
        if (!direction) {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        transform.position = new Vector2(Random.Range(min.x, max.x), Random.Range(min.y, max.y));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.right * (direction ? 1 : -1) * speed * 1.5f * Time.deltaTime);
        if (health <= 0 || transform.position.y < min.y - 10) {
            // Drop bonus?
            if (Random.value < bonusDropChance) {
                Instantiate(bonuses[Random.Range(0, bonuses.Count)], transform.position, Quaternion.identity);
            }
            animator.SetBool("death", true);
            transform.Rotate(new Vector3(0,0,90));
            gameObject.layer = 12;
            Destroy(this);
        }
        if(transform.position.x > max.x || transform.position.y < min.x) {
            direction = !direction;
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
	}

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerPlatformerController>().Hit(transform.position.x - other.transform.position.x > 0);
        }
    }
}
