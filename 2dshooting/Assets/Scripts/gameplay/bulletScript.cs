using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {

	public GameObject singleton;
	GlobalSingleton sS;

	public float constantSpeed = 5;


	ParticleSystem ownParticles;
	Color particleColor;
	float lerpingSpeed = 8f;

	public Vector3 dir;
	GameObject player;
	playerMovement playerS;
	float timer;
	private float timerStart = 5f;
	TrailRenderer trail;
	GameObject bulletManager;

	bool starting = true;

	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		ownParticles = GetComponentInChildren<ParticleSystem> ();
		particleColor = ownParticles.startColor;
		ownParticles.Play ();


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


		/*if (starting) {
			ownParticles.gameObject.transform.localScale = new Vector3(1,1,1);
			starting = false;
		}
		else{
			transform.localScale = new Vector3(Mathf.Lerp(ownParticles.gameObject.transform.localScale.x,0.2678681f,lerpingSpeed*Time.fixedDeltaTime),
			                                   Mathf.Lerp(ownParticles.gameObject.transform.localScale.y,0.2678681f,lerpingSpeed*Time.fixedDeltaTime),
			                                   Mathf.Lerp(ownParticles.gameObject.transform.localScale.z,0.2678681f,lerpingSpeed*Time.fixedDeltaTime));
		}
		*/

		if (starting) {
			ownParticles.gameObject.transform.localScale = new Vector3(4f,4f,4f);
			starting = false;
		}
		else{
			ownParticles.gameObject.transform.localScale = new Vector3(Mathf.Lerp(ownParticles.gameObject.transform.localScale.x,0.4792895f,lerpingSpeed*Time.fixedDeltaTime), // 0.4792895f
			                                                           Mathf.Lerp(ownParticles.gameObject.transform.localScale.y,0.4792895f,lerpingSpeed*Time.fixedDeltaTime),
			                                                           Mathf.Lerp(ownParticles.gameObject.transform.localScale.z,0.4792895f,lerpingSpeed*Time.fixedDeltaTime));
		}




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


	void OnCollisionEnter(Collision col){
	//	renderer.material.color = Color.black;
		if(ownParticles != null){
			ownParticles.startColor = Color.black;
			StartCoroutine (WaitForStart ());
			GameObject cPart = (GameObject)Instantiate(Resources.Load("collisionParticles",typeof(GameObject)));
			cPart.transform.position = col.contacts[0].point;
		}
	}



	IEnumerator WaitForStart(){
		float t = 0;
		while (t<0.5) {
			t += Time.deltaTime;
			yield return 0;
		}
		//renderer.material.color = particleColor;
		ownParticles.startColor = particleColor;
		yield return 0;
	}


	
}
