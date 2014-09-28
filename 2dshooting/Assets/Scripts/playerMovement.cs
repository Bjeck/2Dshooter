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
	public float bulletSpeed = 5;
	int bullets = 0;
	float bulletCounter = 0;
	bool canShoot = false;
	public AudioSource shootSound;
	public ParticleSystem shootEffect;
	timerScript timerS;
	public float bulletCountdownAdder = 5;
	//float bulletRestTimer;


	//blocks
	GameObject blockInstance;
	public GameObject blockBox;
	blockBox bBoxScript;
	bool isBlockSpawning = false;
	float blockRotationSpeed = 20;
	public int blocksLeft = 1;
	float blockAdder = 0;
	public float blockThreshold = 10;

	//Repeller
	GameObject repeller;
	bool repellerActive = false;
	public AudioSource repellerSound;
	public AudioClip repellerSoundClip;
	public float repellerCoolDown = 10;
	Vector3 tPos;

	//redirect
	List<GameObject> bulletList = new List<GameObject> ();
	GameObject goal;
	public bool canRedirect = true;
	public float Redirecttimer;
	public float redirectCool = 12;
	public ParticleSystem redirectParticles;
	public AudioSource redirectSound;


	//Texts:
	GameObject blockTextObj;
	public GameObject ballTextObj;
	GameObject repellerTextObj;
	GameObject redirectTextObj;
	TextMesh blocktext;
	TextMesh balltext;
	TextMesh repellertext;
	TextMesh redirecttext;


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
		timerS = GameObject.Find ("bulletTimerText").GetComponent<timerScript> ();

		blocktext = blockTextObj.GetComponent<TextMesh> ();
		balltext = ballTextObj.GetComponent<TextMesh> ();
		repellertext = repellerTextObj.GetComponent<TextMesh> ();
		redirecttext = redirectTextObj.GetComponent<TextMesh> ();

		bBoxScript = blockBox.GetComponent<blockBox> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//if(repeller != null)
		//	Debug.Log (repeller);

		//Debug.Log (bulletCounter);
		//Debug.Log(Input.GetAxis("Lbumper"));

// ------------ MOVEMENT


		//bulletCounter += Time.deltaTime*0.3f;
		bullets = bulletList.Count;
		balltext.text = "Bullets: "+bullets;




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

		if (blockAdder >= blockThreshold) {
			blockAdder = 0;
			blocksLeft++;
		}


		blocktext.text = "Blocks: " + blocksLeft;

		//BLOCK SPAWNING
		if (Input.GetAxis ("LTrigger") > 0 && isBlockSpawning == false && blocksLeft > 0) {
			isBlockSpawning = true;
			blockInstance = (GameObject)Instantiate(Resources.Load("block",typeof(GameObject)));
			//blockInstance.collider.isTrigger = true;

		}

		//BLOCK PLACING
		if(isBlockSpawning && blockInstance != null){
			Vector3 blockTempPos = transform.position;
			blockTempPos.x += Input.GetAxis("RightStickX");
			blockTempPos.y += Input.GetAxis ("RightStickY");
			blockTempPos.z = -3;
			blockInstance.transform.position = blockTempPos;

			Vector3 vectorToTarget = transform.position - blockInstance.transform.position;
			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			blockInstance.transform.rotation = Quaternion.Slerp(blockInstance.transform.rotation, q, Time.deltaTime * blockRotationSpeed);

			float scaler = Mathf.Clamp(Mathf.Abs(Input.GetAxis("RightStickX"))+ Mathf.Abs(Input.GetAxis("RightStickY")),0.1f,1);

			blockInstance.transform.localScale = new Vector3( scaler*0.3f,scaler*2.0f, 1f);


			if(Input.GetAxis ("LTrigger") == 0){
				isBlockSpawning = false;
				//Debug.Log("Tries to place block");
				if(!bBoxScript.CanPlaceBlock()){

					//Debug.Log("can't place block");
					Destroy(blockInstance);
					return;
				}
				//Debug.Log("Can place block");
				bBoxScript.canPlaceBlock = false;
				blocksLeft--;
				//blockInstance.collider.isTrigger = false;
				Vector3 temp = blockInstance.transform.position;
				temp.z += 2;
				blockInstance.transform.position = temp;
				if(Mathf.Abs(Input.GetAxis("RightStickX"))+ Mathf.Abs(Input.GetAxis("RightStickY")) == 0){
					Destroy(blockInstance);
					blocksLeft++;
				}
			}
		}

		//REPELLER
		if (Input.GetAxis ("Lbumper") > 0 && !repellerActive && repellerCoolDown > 1) { //SPAWN REPELLER
			repellerActive = true;

			//if(repellerSound.isPlaying)
			//	repellerSound.Stop ();

			SpawnRepeller();

		}

		if (repellerActive) { //ON REPELLER ACTIVE
			Vector3 tPos2 = transform.position;
			tPos2.z = -1;
			repeller.transform.position = tPos2;
			speed = 12;
			repellerCoolDown -= Time.deltaTime;
			if(repellerCoolDown <= 0){
				DeSpawnRepeller();
			}
		}

		if (Input.GetAxis ("Lbumper") < 1 && repellerActive) { //DESPAWN REPELLER
			DeSpawnRepeller();
		}

		if (!repellerActive && repellerCoolDown < 10) { //ON REPELLER INACTIVE
			repellerCoolDown += Time.deltaTime;
			//if(!repeller.GetComponent<Animator> ().IsInTransition){}
		}

		repellerCoolDown = Mathf.Round (repellerCoolDown * 100f) / 100f;
		//Debug.Log (repellerCoolDown);
		repellertext.text = "Repeller: " + repellerCoolDown;





	// --------- ReDIRECT
		if (canRedirect) {
			redirecttext.text = "Redirect!";
			redirecttext.color = Color.green;
		}
		else{
			redirecttext.text = "Redirect: "+Redirecttimer;
			redirecttext.color = Color.red;
		}

		if (Input.GetAxis ("Rbumper") > 0 && canRedirect) {
			//Debug.Log("redirect");
			StartCoroutine(Redirect(goal.transform.position));		
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
		timerS.bulletCountdown += bulletCountdownAdder;
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

		repellerSound.pitch = 1;
		repellerSound.PlayOneShot(repellerSoundClip);
		
		repeller.GetComponent<Animator> ().SetBool ("spawn", true);
	}

	void DeSpawnRepeller(){
		repeller.GetComponent<Animator> ().SetBool ("spawn", false);
		
		//if(repellerSound.isPlaying)
		//	repellerSound.Stop();
		
		//repellerSound.pitch = 0.8f;
		//repellerSound.PlayOneShot(repellerSoundClip);
		repellerActive = false;
		speed = 24;
		StartCoroutine(DestroyRepeller());
	}

	IEnumerator DestroyRepeller(){
		float destroyer = 0;
		while (destroyer < 0.20f) {
			destroyer += Time.deltaTime;
			yield return 0;
		}
		if(!repellerActive)
			Destroy(repeller);

		yield return 0;
	}





	public IEnumerator Redirect(Vector3 targetPos){
		//Vector3 goalPos = goal.transform.position;
		int i = 10;
		foreach (GameObject g in bulletList) {
			Vector3 vectorToTarget = targetPos - g.transform.position;
			g.rigidbody.velocity = vectorToTarget;
			g.rigidbody.velocity.Normalize();
			i--;

			if(i == 0){
				yield return 0;
				i = 10;
			}
		}
		Redirecttimer = redirectCool;
		redirectParticles.Play ();

		redirectSound.Play ();
	//	StartCoroutine(redirectCooldown());
		yield return 0;
	}



/*	IEnumerator redirectCooldown(){

		while (Redirecttimer > 0f) {

			//Debug.Log ("TIMER TIMER"+ Redirecttimer);
			Redirecttimer -= Time.deltaTime;
			yield return 0;
		}
		canRedirect = true;
		yield return 0;
	}
*/














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
