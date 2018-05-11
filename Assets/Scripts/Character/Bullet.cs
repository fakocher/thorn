using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed = 100;
    public float damage = 10;
    public float explosion = 0;
    public GameObject explosionPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Vector3.right * speed * 0.5f * Time.deltaTime);
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") && other.gameObject.GetComponent<Zombie>() != null) {
            other.gameObject.GetComponent<Zombie>().health -= damage;


            if(explosion > 0) {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosion);
                int i = 0;
                while (i < hitColliders.Length) {
                    if (hitColliders[i].gameObject.CompareTag("Enemy") && hitColliders[i].gameObject.GetComponent<Zombie>() != null) {
                        hitColliders[i].gameObject.GetComponent<Zombie>().health -= damage;
                        hitColliders[i].GetComponent<Rigidbody2D>().AddForce((hitColliders[i].transform.position - transform.position).normalized * 1000 * damage / (hitColliders[i].transform.position - transform.position).magnitude);
                    }
                    i++;
                }
            }

            Destroy(gameObject);
        }
    }
}
