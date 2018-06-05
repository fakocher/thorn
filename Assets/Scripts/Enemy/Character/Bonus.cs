using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {
    public float shootSpeed = 0;
    public float moveSpeed = 0;
    public float bulletSize = 0;
    public float jumpHeight = 0;
    public float explosiveAmmo = 0;
    public int lifeIncrease = 0;
    public int heal = 0;

    private Vector2 initialPosition;
    private float initialTime;
    private float maxTime = 10.0f; // seconds
    private float timer = 0.0f;
    private SpriteRenderer sr;

    private void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        initialTime = Time.time;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        // Destroy after max time
        if (timer >= maxTime)
        {
            Destroy(this);
        }

        // Flashing after certain time
        sr.enabled = !(
            timer >= maxTime * 0.75f && timer % 0.5f < 0.25f ||
            timer >= maxTime * 0.5f  && timer % 1.0f < 0.25f
        );

        // Movement
        transform.position = initialPosition + new Vector2(0.0f, Mathf.Sin((Time.time - initialTime) * 2) / 4);
    }
}
