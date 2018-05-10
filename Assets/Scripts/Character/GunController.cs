 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public GameObject bullet;
    public float firerate = 1;
    private float shoot_timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Input.GetAxis("Fire1") > 0) {
            if(shoot_timer <= 0) {
                GetComponent<CameraShake>().Shake(.02f,0.2f);
                GameObject b = (GameObject)Instantiate(bullet, transform.position, transform.rotation);
                b.transform.localScale *= GetComponentInParent<PlayerPlatformerController>().bulletSize;
                b.transform.Rotate(new Vector3(0, 0, Random.Range(-2f, 2f)));
                b.GetComponent<Bullet>().damage *= GetComponentInParent<PlayerPlatformerController>().bulletSize;
                GetComponent<AudioSource>().Play();
                GetComponentInParent<Rigidbody2D>().transform.Translate(-transform.right * .1f);
                Destroy(b, 2f);
                shoot_timer = 1 / (firerate * GetComponentInParent<PlayerPlatformerController>().shootSpeed);
            }
        }
        shoot_timer -= Time.deltaTime;
	}
}
