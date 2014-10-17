using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {
	public float constantSpeed = 5;

	public bool isDamagingBullet = true;
	public ParticleSystem fire;
	public Vector3 dir;
	GameObject player;
	playerMovement playerS;

	// Use this for initialization
	void Start () {
		GetComponent<SphereCollider> ().radius = 2f;
		fire = GetComponentInChildren<ParticleSystem> ();
		fire.Play ();
		player = GameObject.Find ("Player");
		playerS = player.GetComponent<playerMovement> ();

		fire.gameObject.transform.rotation = Quaternion.LookRotation (rigidbody.velocity);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = constantSpeed * (rigidbody.velocity.normalized);

		//Debug.Log (isDamagingBullet);
	}


	void OnCollisionEnter(Collision c){

		if (c.gameObject.tag == "ball" && isDamagingBullet) {
			if(c.gameObject.GetComponent<bulletScript>().isDamagingBullet)
			{}
			else{
				if(c.gameObject != null)
					playerS.RemoveBullet(c.gameObject);
			}
		}

		//Debug.Log (isDamagingBullet);
		if ((c.gameObject.tag == "boundary" || c.gameObject.tag == "goal" || c.gameObject.tag == "block") && isDamagingBullet) {
			ChangeFromDamaging();
		}

	}



	void ChangeFromDamaging(){
		isDamagingBullet = false;
		GetComponent<SphereCollider> ().radius = 0.5f;
		//fire.Stop ();
		Destroy (fire.gameObject);

	}
	
}
