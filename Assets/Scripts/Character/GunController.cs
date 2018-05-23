using UnityEngine;

public class GunController : MonoBehaviour {

    public GameObject bulletTemplate;
    public float firerate = 1;
    private float shootTimer = 0;
    public float explosion = 0;

    private void Update()
    {
        // Update firerate timer
        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    private void FixedUpdate ()
    {
        // If the firing button is used
		if(Input.GetAxis("Fire1") > 0 && shootTimer <= 0)
        {
            // Shake camera
            GetComponent<CameraShake>().Shake(.02f,0.2f);

            // Fire a new bullet
            GameObject newBullet = (GameObject)Instantiate(bulletTemplate, transform.position, transform.rotation);
            newBullet.GetComponent<Bullet>().explosion = explosion;
            newBullet.transform.localScale *= GetComponentInParent<PlayerPlatformerController>().bulletSize;
            newBullet.transform.Rotate(new Vector3(0, 0, Random.Range(-2f, 2f)));
            newBullet.GetComponent<Bullet>().damage *= GetComponentInParent<PlayerPlatformerController>().bulletSize;

            // Play gun sound
            GetComponent<AudioSource>().Play();

            // Recoil
            GetComponentInParent<Rigidbody2D>().transform.Translate(-transform.right * .1f);

            // Destroy bullet after 2 seconds to prevent it to go indefinitely
            Destroy(newBullet, 2f);

            // Reset firerate timer
            shootTimer = 1 / (firerate * GetComponentInParent<PlayerPlatformerController>().shootSpeed);
        }
	}
}
