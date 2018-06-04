﻿using System.Collections;
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

    // Use this for initialization
    void Start () {

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
            Destroy(this);
        }

        // Flip direction if reaching sides of map
        if(transform.position.x > maxSpawnPosition.x || transform.position.y < minSpawnPosition.x) {
            direction = !direction;
            sr.flipX = !sr.flipX;
        }
	}

    void OnCollisionEnter2D(Collision2D other) {

        // Handle collision with player
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerPlatformerController>().Hit(transform.position.x - other.transform.position.x > 0);
        }
    }
}
