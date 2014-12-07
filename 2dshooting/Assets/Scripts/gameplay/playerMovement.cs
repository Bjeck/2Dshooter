﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerMovement : MonoBehaviour {


	//general
	public GameObject singleton;
	GlobalSingleton sS;
	camera camScript;


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
	public ParticleSystem playerSystem;

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
	public List<GameObject> bulletList = new List<GameObject> ();
	Vector3 lookVector;
	public GameObject blockManagerObject;
	blockManager blockMan;

	//blocks
	GameObject blockInstance;
	blockScript BlockS;
	public GameObject blockBox;
	blockBoxManager bBoxScript;
	bool isBlockSpawning = false;
	float blockRotationSpeed = 20;
	public int blocksLeft = 1;
	float blockAdder = 0;
	public float blockThreshold = 10;
	Shader blockShader;
	float blockInstantPlaceTimer;
	bool blockInstantTimerRunning = false;
	public GameObject blockInMiddle;
	public AudioSource blockPlaceSound;
	public AudioSource blockPullSound;

	//Repeller
	GameObject repeller;
	bool repellerActive = false;
	public AudioSource repellerSound;
	public AudioClip repellerSoundClip;
	public float repellerCoolDown = 12;
	public float repellerCoolDownMax;
	Vector3 tPos;
	TrailRenderer trail;

	//redirect
	GameObject goal;
	public bool canRedirect = true;
	public float RedirectCounter;
	public float redirectCoolCurrentGoal = 12;
	public ParticleSystem redirectParticles;
	public AudioSource redirectSound;
	public bool redButtonDown = false;
	public Light redirectLight;

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

		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		moveSound = GetComponent<AudioSource> ();
		goal = GameObject.Find ("goal");

		transform.position = new Vector3 (-4f, 0f, -2f);
	
		RedirectCounter = redirectCoolCurrentGoal;
		//targetLight = GetComponentInChildren<Light> ();
		flashSound = targetLight.GetComponent<AudioSource> ();
		flashOffSound = targetLight.GetComponent<AudioSource> ();
		trail = GetComponent<TrailRenderer>();

		blockTextObj = GameObject.Find ("blockText");
		ballTextObj = GameObject.Find ("ballText");
		repellerTextObj = GameObject.Find ("repellerText");
		redirectTextObj = GameObject.Find ("redirectText");

		if(!sS.inMenu){
			timerS = GameObject.Find ("bulletTimerText").GetComponent<timerScript> ();
		}
		blocktext = blockTextObj.GetComponent<TextMesh> ();
		balltext = ballTextObj.GetComponent<TextMesh> ();
		repellertext = repellerTextObj.GetComponent<TextMesh> ();
		redirecttext = redirectTextObj.GetComponent<TextMesh> ();
		repellertext.text = "LB: Repeller";


		bBoxScript = blockBox.GetComponent<blockBoxManager> ();
		blockMan = blockManagerObject.GetComponent<blockManager>();

		repellerCoolDownMax = repellerCoolDown;
		camScript = Camera.main.GetComponent<camera> ();


	}
	// Update is called once per frame
	void FixedUpdate () {


		//Debug.Log(Input.GetAxis("Lbumper"));




		//bulletCounter += Time.deltaTime*0.3f;
		bullets = bulletList.Count;
		if(!sS.inMenu){
			balltext.text = "Bullets: "+bullets;
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

			if(targeter != new Vector3(0,0,0)){ //&& canShoot
		//		Debug.Log("SHOOT");
				Shoot(targeter);
			}
		}

// ------------------------------------------------------------------------------------------------------------------------------------- MOVEMENT

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

		//	Debug.Log (Input.GetAxis ("Horizontal"));

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

		lookVector = new Vector3 (Input.GetAxis ("RightStickX"), Input.GetAxis ("RightStickY"), 0);

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

		volume = rigidbody.velocity.magnitude ;
		volume /= speed;
		//Debug.Log (volume);
		moveSound.volume = volume;


		if (rigidbody.velocity != new Vector3 (0, 0, 0)) {
			moveSound.Play();
		}




// ---------------------------------------------------------------------------------------- ACTIONS


	// -------------------------------------- BLOCK

		if (blockAdder >= blockThreshold) {
			blockAdder = 0;
			//blocksLeft++;
			blockMan.UpdateAmountOfAvailableBlocks(1);
		}

//		Debug.Log(Input.GetAxis("Rbumper"));

		if(!sS.inMenu){
			blocktext.text = "Blocks: " + blockMan.GetAmountOfBlocksInMiddle();
		}
		//BLOCK GRABBING
		if (Input.GetAxis ("LTrigger") > 0 && !isBlockSpawning && blockMan.GetAmountOfBlocksInMiddle() > 0) {
			isBlockSpawning = true;
			blockInstance = blockMan.TakeBlockFromMiddle(); //(GameObject)Instantiate(Resources.Load("block",typeof(GameObject)));
			BlockS = blockInstance.GetComponent<blockScript>();
			blockInstantTimerRunning = true;
			blockPullSound.Play();
			BlockS.ChangeParticlesSize(0.08f);
			blockInstance.renderer.enabled = false;
			//blockInstance.collider.isTrigger = true;

		}

		//BLOCK DRAGGING
		if(isBlockSpawning && blockInstance != null){
			Vector3 blockTempPos = transform.position;
			blockTempPos.x += Input.GetAxis("RightStickX");
			blockTempPos.y += Input.GetAxis ("RightStickY");
			blockTempPos.z = -3;

			Vector3 vectorToTarget = transform.position - blockInstance.transform.position;
			float distanceToTarget = Vector2.Distance(transform.position,blockInstance.transform.position);

			float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
			Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
			distanceToTarget *= 2f;

			float scaler = Mathf.Clamp(distanceToTarget*1f,0.1f,1);

			float lerpingSpeed = (1/distanceToTarget)+30f;


			blockInstance.transform.position = new Vector3(Mathf.Lerp(blockInstance.transform.position.x,blockTempPos.x,lerpingSpeed*Time.deltaTime),
			                                               Mathf.Lerp(blockInstance.transform.position.y,blockTempPos.y,lerpingSpeed*Time.deltaTime),blockTempPos.z);

			//blockInstance.transform.position = blockTempPos;

			//blockInstance.transform.rotation = Quaternion.Slerp(blockInstance.transform.rotation, q, Time.deltaTime * blockRotationSpeed);
			blockInstance.transform.rotation = q;

			//float scaler = Mathf.Clamp(Mathf.Abs(Input.GetAxis("RightStickX"))+ Mathf.Abs(Input.GetAxis("RightStickY"))*1.4f,0.1f,1);


//			Debug.Log(distanceToTarget);
			blockInstance.transform.localScale = new Vector3( scaler*0.3f,scaler*2.0f, 1f);

			if(!bBoxScript.CanPlaceBlock()){
				blockInstance.renderer.material.color = Color.red;
				BlockS.ChangeParticlesColor(Color.red);
			}
			else{
				blockInstance.renderer.material.color = Color.white;
				BlockS.ChangeParticlesColor(Color.white);
			}

			if(blockInstantTimerRunning){
				blockInstantPlaceTimer += Time.fixedDeltaTime;
			}

			//BLOCK PLACING
			if(Input.GetAxis ("LTrigger") <= 0){
				isBlockSpawning = false;
				//Debug.Log("Tries to place block");


				if(!bBoxScript.CanPlaceBlock()){

					//Debug.Log("can't place block");
					blockMan.PlaceBlockBackInMiddle();
					Destroy(blockInstance);
					StartCoroutine(WaitToResetBlockBool());
				//	bBoxScript.canPlaceBlock = false;
					return;
				}
			/*	if(blockInstantPlaceTimer <= 0.5f){ //If it was an instant press
					Vector3 vectorToGoal = goal.transform.position - blockInstance.transform.position;
					float angleToGoal = Mathf.Atan2(vectorToGoal.y, vectorToGoal.x) * Mathf.Rad2Deg;
					Quaternion q2 = Quaternion.AngleAxis(angleToGoal, Vector3.forward);
					blockInstance.transform.rotation = q2;
					//blockInstance.transform.localScale = new Vector3(0.3f,2.0f, 1f);
					//blockInstance.transform.LookAt(goal.transform.position);
				}
*/

				if(Mathf.Abs(Input.GetAxis("RightStickX"))+ Mathf.Abs(Input.GetAxis("RightStickY")) == 0){
					blockMan.PlaceBlockBackInMiddle();
					Destroy(blockInstance);
					//Debug.Log("from player"+bBoxScript.isInTopCorner);
					StartCoroutine(WaitToResetBlockBool());

					//Debug.Log("from player2"+bBoxScript.isInTopCorner);
				//	blocksLeft++;
					return;
				}
//				Debug.Log("Can place block");
				//bBoxScript.canPlaceBlock = false;
			//	blocksLeft--;
				//blockInstance.collider.isTrigger = false;
				Vector3 temp = blockInstance.transform.position; //Places the block in place.
				temp.z += 2;
				blockInstance.transform.position = temp;
				blockInstance.tag = "block";
				ParticleSystem blockParticles = blockInstance.GetComponentInChildren<ParticleSystem>();
			//	blockParticles.transform.eulerAngles += new Vector3(0,0,90f);
				blockParticles.Play ();
				blockPlaceSound.Play();
				StartCoroutine(WaitToResetBlockBool());
				BlockS.SpreadOutParticles();
			}
		}

	//------------------------------------------------ REPELLER

		if (Input.GetAxis ("Lbumper") > 0 && !repellerActive) { //&& repellerCoolDown > 1   SPAWN REPELLER 
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
	//		repellerCoolDown -= Time.deltaTime;
			if(repellerCoolDown <= 0){
				DeSpawnRepeller();
			}
		}

		if (Input.GetAxis ("Lbumper") < 1 && repellerActive) { //DESPAWN REPELLER
			DeSpawnRepeller();
		}
/*
		if (!repellerActive && repellerCoolDown < repellerCoolDownMax) { //ON REPELLER INACTIVE
			repellerCoolDown += Time.deltaTime;
			//if(!repeller.GetComponent<Animator> ().IsInTransition){}
		}
*/
	//	repellerCoolDown = Mathf.Round (repellerCoolDown * 100f) / 100f;
		//Debug.Log (repellerCoolDown);
	/*	if(!sS.inMenu){
			repellertext.text = "LB: Repeller";
		}
		else{
			repellertext.text = "LB: Repeller";
		}
*/


	
	// --------------------------------------------------- ReDIRECT

		if(!sS.inMenu){
			if(RedirectCounter >= redirectCoolCurrentGoal){ //redirect is available again.
				canRedirect = true;
				redirectLight.intensity = 2;
				//Redirecttimer = 12;
			}
			else{
				canRedirect = false;
				redirectLight.intensity = 0;
			}
		}
		else{
			canRedirect = true;
			redirectLight.intensity = 0;
		}

		float pct = (int)((RedirectCounter / redirectCoolCurrentGoal)*100);

		if(!sS.inMenu){
			if (canRedirect) {
					redirecttext.text = "Redirect! "+RedirectCounter+ "/"+redirectCoolCurrentGoal+", "+pct+"%";
					redirecttext.color = Color.green;
			}
			else{
					redirecttext.text = "Redirect: "+RedirectCounter+ "/"+redirectCoolCurrentGoal+", "+pct+"%";
					redirecttext.color = Color.red;
			}
		}
		
		if (Input.GetAxis ("Rbumper") > 0 && canRedirect && !redButtonDown) {
			//Debug.Log("redirect");
			redButtonDown = true;
			camScript.SubtractFromIntensity(1f);
			StartCoroutine(Redirect(goal.transform.position));	
			if(!sS.inMenu)
			RedirectCounter = RedirectCounter - redirectCoolCurrentGoal;
			//canRedirect = false;
		}

		if (redButtonDown && Input.GetAxis ("Rbumper") < 1) {
			redButtonDown = false;
		}


	} //end update



	void Shoot(Vector3 vel){
		camScript.AddToIntensity(1f);

		bullet = (GameObject)Instantiate(Resources.Load("bullet",typeof(GameObject)));
		Vector3 bulletTemp = transform.position;
		bulletTemp += lookVector.normalized/3f;
		bulletTemp.z = -1;

		bullet.transform.position = bulletTemp;
		bullet.rigidbody.velocity = vel*bulletSpeed;

		transform.rigidbody.AddForce((-vel)*1000f);
		shootSound.Play ();

		shootEffect.transform.rotation = targetLight.transform.rotation;
		shootEffect.Play ();

		targeter = new Vector3 (0, 0, 0);
		bulletList.Add(bullet);
		if(!sS.inMenu)
			timerS.bulletCountdown += bulletCountdownAdder;

		redirectCoolCurrentGoal++;
		//bulletCounter++;
		blockAdder++;
	}


	public void RemoveBullet(GameObject b){
		bulletList.Remove (b);
		Destroy(b);
		redirectCoolCurrentGoal--;
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

		playerSystem.Stop ();
		trail.enabled = false;

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
		trail.enabled = true;

		StartCoroutine(DestroyRepeller());
	}

	IEnumerator DestroyRepeller(){
		float destroyer = 0;
		while (destroyer < 0.20f) {

			destroyer += Time.deltaTime;
			if(destroyer > 0.10f){
				playerSystem.Play ();
			}
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
			if(g != null){
				Vector3 vectorToTarget = targetPos - g.transform.position;
				g.rigidbody.velocity = vectorToTarget;
				g.rigidbody.velocity.Normalize();
				i--;

				if(i == 0){
					yield return 0;
					i = 10;
				}
			}
		}
		//Redirecttimer = redirectCoolInitial;
		redirectParticles.Play ();

		redirectSound.Play ();
	//	StartCoroutine(redirectCooldown());
		yield return 0;
	}


	IEnumerator WaitToResetBlockBool()
	{
		//returning 0 will make it wait 1 frame
		yield return 0;
		
		//code goes here
		bBoxScript.SetInTopCorner(false);
		
		
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
