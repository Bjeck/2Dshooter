using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour {


	//moving
	Vector3 mover;
	float speed = 24;
	bool lockPosition = false;

	AudioSource moveSound;
	float volume = 0;
	public Light targetLight;
	AudioSource flashSound;
	AudioSource flashOffSound;
	bool flashTurn = false;

	//shooting
	GameObject bullet;
	Vector3 targeter;
	bool isTargeting = false;
	float bulletSpeed = 5;
	int bullets = 0;
	float bulletCounter = 0;
	bool canShoot = false;
	public AudioSource shootSound;
	public ParticleSystem shootEffect;
	float bulletCountdown = 10;

	//blocks
	GameObject blockInstance;
	bool isBlockSpawning = false;
	float blockRotationSpeed = 20;
	public int blocksLeft = 2;
	float blockAdder = 0;

	//Repeller
	GameObject repeller;
	bool repellerActive = false;
	public AudioSource repellerSound;

	//redirect
	List<GameObject> bulletList = new List<GameObject> ();
	GameObject goal;
	bool canRedirect = true;
	float Redirecttimer;
	public float redirectCool = 10;


	//Texts:
	GameObject blockTextObj;
	public GameObject ballTextObj;
	GameObject repellerTextObj;
	GameObject redirectTextObj;
	public GameObject bulletTimerObj;
	TextMesh blocktext;
	TextMesh balltext;
	TextMesh repellertext;
	TextMesh redirecttext;
	TextMesh bulletTimerText;


	// Use this for initialization
	void Start () {

		moveSound = GetComponent<AudioSource> ();
		goal = GameObject.Find ("goal");

		transform.position = new Vector3 (-4f, 0f, -2f);
	
		Redirecttimer = redirectCool;
		//targetLight = GetComponentInChildren<Light> ();
		flashSound = targetLight.GetComponent<AudioSource> ();
		flashOffSound = targetLight.GetComponent<AudioSource> ();

		blockTextObj = GameObject.Find ("blockText");
		ballTextObj = GameObject.Find ("ballText");
		repellerTextObj = GameObject.Find ("repellerText");
		redirectTextObj = GameObject.Find ("redirectText");
		bulletTimerObj = GameObject.Find ("bulletTimerText");

		blocktext = blockTextObj.GetComponent<TextMesh> ();
		balltext = ballTextObj.GetComponent<TextMesh> ();
		repellertext = repellerTextObj.GetComponent<TextMesh> ();
		redirecttext = redirectTextObj.GetComponent<TextMesh> ();
		bulletTimerText = bulletTimerObj.GetComponent<TextMesh> ();


	}
	
	// Update is called once per frame
	void Update () {

		//if(repeller != null)
		//	Debug.Log (repeller);

		//Debug.Log (bulletCounter);
		//Debug.Log(Input.GetAxis("Lbumper"));

// ------------ MOVEMENT


		//bulletCounter += Time.deltaTime*0.3f;
		bullets = bulletList.Count;
		balltext.text = "Bullets: "+bullets;

		bulletCountdown -= Time.deltaTime;

		bulletTimerText.text = "Bullet timer: " + bulletCountdown;

		if (bulletCountdown <= 0) {
			if(bulletList.Count > 0){
				GameObject temp = bulletList[0].gameObject;
				Destroy(temp);
				bulletList.Remove(bulletList[0]);
			}
			bulletCountdown = 10f;
		}

		if (bullets > 0) {
			canShoot = true;		
		}
		else{
			canShoot = false;
		}

		//if(canShoot)
		//Debug.Log (targeter);

		//SHOOT
		if(Input.GetAxis("Trigger") > 0){
			lockPosition = true;
			//rigidbody.velocity = new Vector3(0,0,0);
		}
		else{
			lockPosition = false;

			if(targeter != new Vector3(0,0,0)){ //&& canShoot !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//		Debug.Log("SHOOT");
				Shoot(targeter);
			}
		}


		//MOVE
		if(!lockPosition){
			rigidbody.drag = 0;
		//	targetLight.transform.rotation = Quaternion.LookRotation( new Vector3(0,0,0));

			mover = transform.rigidbody.velocity;
			mover.x = Input.GetAxis ("Horizontal");
			mover.y = Input.GetAxis ("Vertical");

			Vector2 temp;
			temp.x = mover.x;
			temp.y = mover.y;

			temp.x *= speed;
			temp.y *= speed;

			if(transform.position.x < -6 && temp.x < 0)
				temp.x = 0;
			if(transform.position.y > 6 && temp.y > 0)
				temp.y = 0;
			if(transform.position.y < -6 && temp.y < 0)
				temp.y = 0;

			transform.rigidbody.velocity = temp;

			//Debug.Log (Input.GetAxis ("Vertical"));

		}
		else{
			//Debug.Log("SHOULD STOP MOVING");
			transform.position = transform.position;
			rigidbody.drag = 20;

			Vector3 temp = new Vector3();
			temp.x = Input.GetAxis ("RightStickX");
			temp.y = Input.GetAxis ("RightStickY");

			temp.Normalize();

			targeter = temp;
			//if(!canShoot){
			//	targeter = new Vector3(0,0,0);
			//}

			Debug.DrawRay(transform.position,targeter);

		}

		Vector3 lookVector = new Vector3 (Input.GetAxis ("RightStickX"), Input.GetAxis ("RightStickY"), 0);

		if(lookVector == (new Vector3(0,0,0)) || isBlockSpawning){
			//Debug.Log("0000");
			//targetLight.transform.rotation = Quaternion.LookRotation(lookVector);
			targetLight.intensity = 0f;
			if(flashTurn){
				flashOffSound.pitch -= 0.2f;
				flashOffSound.Play();
				flashTurn = false;
			}
		}
		else{
			if(!flashTurn){
				flashOffSound.pitch += 0.2f;
				flashSound.Play();
				flashTurn = true;
			}
			targetLight.transform.rotation = Quaternion.LookRotation(lookVector);
			targetLight.intensity = 3.3f;
		}


		//if (isBlockSpawning) {
		//	targetLight.intensity = 0f;
		//		}
		//else{
		//	targetLight.intensity = 3.3f;
		//}


		volume = rigidbody.velocity.magnitude ;
		volume /= speed;
		//Debug.Log (volume);
		moveSound.volume = volume;


		if (rigidbody.velocity != new Vector3 (0, 0, 0)) {
			moveSound.Play();
		}



// --------------- ACTIONS

		if (blockAdder >= 5) {
			blockAdder = 0;
			blocksLeft++;
		}


		blocktext.text = "Blocks: " + blocksLeft;

		//BLOCK SPAWNING
		if (Input.GetAxis ("LTrigger") > 0 && isBlockSpawning == false && blocksLeft > 0) {
			isBlockSpawning = true;
			blockInstance = (GameObject)Instantiate(Resources.Load("block",typeof(GameObject)));
			blockInstance.collider.isTrigger = true;
			blockInstance.transform.position = transform.position;

			}

		//BLOCK PLACING
		if(isBlockSpawning && blockInstance != null){
			Vector3 blockTempPos = transform.position;
			blockTempPos.x += Input.GetAxis("RightStickX");
			blockTempPos.y += Input.GetAxis ("RightStickY");
			blockTempPos.z = -1;
			blockInstance.transform.position = blockTempPos;

			Vector3 vectorToTarget = transform.position - blockInstance.transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			blockInstance.transform.rotation = Quaternion.Slerp(blockInstance.transform.rotation, q, Time.deltaTime * blockRotationSpeed);

			float scaler = Mathf.Clamp(Mathf.Abs(Input.GetAxis("RightStickX"))+ Mathf.Abs(Input.GetAxis("RightStickY")),0.1f,1);

			blockInstance.transform.localScale = new Vector3( 0.2f,scaler*1.5f, 1f);


			if(Input.GetAxis ("LTrigger") == 0){
				isBlockSpawning = false;
				blocksLeft--;
				blockInstance.collider.isTrigger = false;
				if(Mathf.Abs(Input.GetAxis("RightStickX"))+ Mathf.Abs(Input.GetAxis("RightStickY")) == 0){
					Destroy(blockInstance);
					blocksLeft++;
				}
			}
		}

		//REPELLER
		if (Input.GetAxis ("Lbumper") > 0 && !repellerActive) {
			repellerActive = true;

			if(repellerSound.isPlaying)
				repellerSound.Stop ();

			repellerSound.pitch = 1;
			repellerSound.Play();
			SpawnRepeller();
			repeller.GetComponent<Animator> ().SetBool ("spawn", true);
			Vector3 tPos = transform.position;
			tPos.z = -1;
			repeller.transform.position = tPos;
		}
		if (repellerActive) {
			Vector3 tPos = transform.position;
			tPos.z = -1;
			repeller.transform.position = tPos;
			speed = 12;
		}

		if (Input.GetAxis ("Lbumper") < 1 && repellerActive) {
			repeller.GetComponent<Animator> ().SetBool ("spawn", false);

			//if(repellerSound.isPlaying)
			//	repellerSound.Stop();

			//repellerSound.pitch = 0.5f;
			//repellerSound.Play();
			repellerActive = false;
			speed = 24;
		}

		//Redirect
		if (Input.GetAxis ("Rbumper") > 0 && canRedirect) {
			//Debug.Log("redirect");
			StartCoroutine(Redirect());		
			canRedirect = false;
		}


	} //end update






	void Shoot(Vector3 vel){


		bullet = (GameObject)Instantiate(Resources.Load("bullet",typeof(GameObject)));
		Vector3 bulletTemp = transform.position;
		bulletTemp.z = -1;
		bullet.transform.position = bulletTemp;
		bullet.rigidbody.velocity = vel*bulletSpeed;

		transform.rigidbody.AddForce((-vel)*1000f);
		shootSound.Play ();

		shootEffect.transform.rotation = targetLight.transform.rotation;
		shootEffect.Play ();

		targeter = new Vector3 (0, 0, 0);
		bulletList.Add(bullet);
		//bulletCounter++;
		blockAdder++;
	}


	public void addToBlocks(){
		blocksLeft++;
	}





	void SpawnRepeller(){
		if (repeller != null) {
		//	Debug.Log("repeller is not null!");
			Destroy(repeller);	
		}

		repeller = (GameObject)Instantiate(Resources.Load("repeller",typeof(GameObject)));
		Vector3 tPos = transform.position;
		tPos.z = -1;
		repeller.transform.position = tPos;
	}





	IEnumerator Redirect(){
		Vector3 goalPos = goal.transform.position;
		int i = 10;
		foreach (GameObject g in bulletList) {
			Vector3 vectorToTarget = goalPos - g.transform.position;
			g.rigidbody.velocity = vectorToTarget;
			g.rigidbody.velocity.Normalize();
			i--;

			if(i == 0){
				yield return 0;
				i = 10;
			}
		}
		Redirecttimer = redirectCool;
		StartCoroutine(redirectCooldown());
		yield return 0;
	}



	IEnumerator redirectCooldown(){

		while (Redirecttimer > 0f) {
			redirecttext.text = "Redirect: "+Redirecttimer;
			//Debug.Log ("TIMER TIMER"+ Redirecttimer);
			Redirecttimer -= Time.deltaTime;
			yield return 0;
		}
		canRedirect = true;
		yield return 0;
	}















	/*
	static Vector2 CircleToSquare(Vector2 point){
		return CircleToSquare (point, 0);
	}
	static Vector2 CircleToSquare(Vector2 point, double innerRoundness){
		const float PiOverFour = Mathf.PI / 4;

		var angle = Mathf.Atan2 (point.y, point.x) + Mathf.PI;

		Vector2 squared = new Vector2();
		if(angle <= PiOverFour || angle > 7*PiOverFour)
			squared = point*(float)(1/Mathf.Cos(angle));
		else if(angle > PiOverFour && angle <= 3*PiOverFour)
			squared = point * (float)(1/Mathf.Sin (angle));
		else if(angle > 3 * PiOverFour && angle <= 5*PiOverFour)
			squared = point*(float)(-1/Mathf.Cos(angle));
		else if(angle > 5 * PiOverFour && angle <= 7*PiOverFour)
			squared = point*(float)(-1/Mathf.Sin(angle));
		else{
			Debug.Log("invalid angle");
		}

		//if(innerRoundness == 0)
		return squared;

		//var length = Vector2. ;
		//var factor = (float)

	}
*/










}
