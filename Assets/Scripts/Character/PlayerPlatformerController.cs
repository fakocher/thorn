using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject {

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Use this for initialization
    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

    public void Die(bool direction) {
        rb2d.constraints = 0;
        animator.SetBool("death", true);
        rb2d.AddForce(new Vector2(500 * (direction ? -1 : 1), 500));
        GameManager.instance.GameOver();
        Destroy(this);
    }
}