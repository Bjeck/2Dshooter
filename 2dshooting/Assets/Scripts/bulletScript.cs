using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {
	public float constantSpeed = 5;

	public Vector3 dir;
	GameObject player;
	playerMovement playerS;
	float timer;
	public float timerStart = 5f;
	TrailRenderer trail;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		trail = GetComponent<TrailRenderer> ();
		playerS = player.GetComponent<playerMovement> ();
		timer = timerStart;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = constantSpeed * (rigidbody.velocity.normalized);

		if(timer>0){
			timer -= Time.fixedDeltaTime;
		}
		else if(timer <= 0){
			playerS.RemoveBullet(this.gameObject);
		}

		trail.time = timer / timerStart;

		if(timer <= 1){
			gameObject.collider.enabled = false;
			transform.localScale *= timer;
		}


		//Debug.Log (isDamagingBullet);
	}


	
}
