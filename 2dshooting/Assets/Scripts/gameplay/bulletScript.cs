using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {

	public GameObject singleton;
	GlobalSingleton sS;

	public float constantSpeed = 5;

	public Vector3 dir;
	GameObject player;
	playerMovement playerS;
	float timer;
	private float timerStart = 5f;
	TrailRenderer trail;
	GameObject bulletManager;

	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		player = GameObject.Find ("Player");
		trail = GetComponent<TrailRenderer> ();
		playerS = player.GetComponent<playerMovement> ();
		if(!sS.inMenu){
			bulletManager = GameObject.FindGameObjectWithTag ("BulletManager");
			timerStart = bulletManager.GetComponent<BulletManager> ().bulletTimer;

		}
		timer = timerStart;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = constantSpeed * (rigidbody.velocity.normalized);

		if(!sS.inMenu){
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
		}
		else{
			trail.time = 1;
		}

		//Debug.Log (isDamagingBullet);
	}


	
}
