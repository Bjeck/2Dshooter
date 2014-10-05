using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {
	public float constantSpeed = 5;

	public bool isDamagingBullet = true;
	public ParticleSystem fire;
	public Vector3 dir;

	// Use this for initialization
	void Start () {
		GetComponent<SphereCollider> ().radius = 2f;
		fire = GetComponentInChildren<ParticleSystem> ();
		fire.Play ();

		fire.gameObject.transform.rotation = Quaternion.LookRotation (rigidbody.velocity);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = constantSpeed * (rigidbody.velocity.normalized);

		//Debug.Log (isDamagingBullet);
	}


	void OnCollisionEnter(Collision c){

		if (c.gameObject.tag == "ball" && isDamagingBullet) {
			Destroy(c.gameObject);		
		}

		//Debug.Log (isDamagingBullet);
		if ((c.gameObject.tag == "boundary" || c.gameObject.tag == "goal") && isDamagingBullet) {
			ChangeFromDamaging();
			Debug.Log (isDamagingBullet);
		}

	}



	void ChangeFromDamaging(){
		isDamagingBullet = false;
		GetComponent<SphereCollider> ().radius = 0.5f;
		fire.Stop ();
		Destroy (fire.gameObject);

	}
	
}
