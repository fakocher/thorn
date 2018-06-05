using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 100;
    public float damage = 10;
    public float explosion = 0;
    public GameObject explosionPrefab;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // Move bullet
        transform.Translate(Vector3.right * speed * 0.5f * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with Zombies
        if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<Zombie>() != null)
        {
            other.gameObject.GetComponent<Zombie>().hit(damage);

            if (explosion > 0)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosion);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    if (hitColliders[i].gameObject.CompareTag("Enemy") && hitColliders[i].gameObject.GetComponent<Zombie>() != null)
                    {
                        hitColliders[i].gameObject.GetComponent<Zombie>().hit(damage);
                        hitColliders[i].GetComponent<Rigidbody2D>().AddForce((hitColliders[i].transform.position - transform.position).normalized * 20 * damage / (hitColliders[i].transform.position - transform.position).magnitude);
                    }
                    i++;
                }
            }

            Destroy(gameObject);
        }

        // Handle collision with Boss
        if (other.gameObject.CompareTag("Boss") && other.gameObject.GetComponent<Boss>() != null)
        {
            other.gameObject.GetComponent<Boss>().hit(damage);
        }
    }
}
