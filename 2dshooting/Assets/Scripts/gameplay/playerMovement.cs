using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XboxCtrlrInput;

public class playerMovement : MonoBehaviour {
	
	
	//general
	public GameObject singleton;
	GlobalSingleton sS;
	camera camScript;
	
	
	//moving
	Vector3 mover;
	float speed = 24;
	public float moveSpeed = 24f;
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
	public timerScript timerS;
	public float bulletCountdownAdder = 5;
	//float bulletRestTimer;
	public List<GameObject> bulletList = new List<GameObject> ();
	public int totalBulletCount = 0;
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
	public AudioSource blockErrorSound;
	
	//Repeller
	public GameObject repeller;
	public bool repellerActive = false;
	public AudioSource repellerSound;
	public AudioClip repellerSoundClip;
	public float repellerCoolDown = 12;
	public float repellerCoolDownMax;
	Vector3 tPos;
	TrailRenderer trail;
	
	//redirect
	GameObject goal;
	goalScript goalScr;
	//	public bool canRedirect = true;
	//	public float RedirectCounter;
	//	public float redirectCoolCurrentGoal = 12;
	public ParticleSystem redirectParticles;
	public AudioSource redirectSound;
	public bool redButtonDown = false;
	public Light redirectLight;
	public List<GameObject> redirectObjects = new List<GameObject>();
	Color rediColor = new Color((216f/255f),(75f/255f),0f);
	public GameObject redirectObj;
	redirect rediScript;
	
	//Texts:
	public int ControllerNR = 0;
	public Vector3 StartPosition;
	
	// Use this for initialization
	void Start () {

		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();
		
		moveSound = GetComponent<AudioSource> ();
		//goal = GameObject.Find ("goal");
		goal = GameObject.FindGameObjectWithTag("goal");
		if(!GlobalSingleton.instance.inMenu){
			//			goalScr = goal.GetComponentInChildren<goalScript>();
		}

		transform.position = StartPosition;
		
		//		RedirectCounter = redirectCoolCurrentGoal;
		//targetLight = GetComponentInChildren<Light> ();
		flashSound = targetLight.GetComponent<AudioSource> ();
		flashOffSound = targetLight.GetComponent<AudioSource> ();
		trail = GetComponent<TrailRenderer>();
		
		//		Debug.Log(redirectpcttext.gameObject);
		//	repellertext.text = "LB: Repeller";
		
		
		bBoxScript = blockBox.GetComponent<blockBoxManager> ();
		blockMan = blockManagerObject.GetComponent<blockManager>();
		
		repellerCoolDownMax = repellerCoolDown;
		camScript = Camera.main.GetComponent<camera> ();
		
		
		//	redirectObj = GameObject.Find("redirectParticles");
		rediScript = redirectObj.GetComponent<redirect>();
		
		
	}
	// Update is called once per frame
	void FixedUpdate () {
		
		
		//		Debug.Log(rediColor+" "+redirectObjects[0].renderer.material.color);
		
		if (GlobalSingleton.instance.isPaused) {
			transform.position = transform.position;
			transform.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
			return;		
		}
		
		
		//bulletCounter += Time.deltaTime*0.3f;
		bullets = bulletList.Count;
		
		
		
		//----------------------------------- SHOOT
		
		if(XCI.GetAxis(XboxAxis.RightTrigger,ControllerNR) > 0){
			lockPosition = true;
			//rigidbody.velocity = new Vector3(0,0,0);
			//Debug.Log("LOCKED");
		}
		else{
			lockPosition = false;
			if(targeter != new Vector3(0,0,0)){ //&& canShoot
				//Debug.Log("SHOOT");
				Shoot(targeter);
			}
		}
		
		// ------------------------------------------------------------------------------------------------------------------------------------- MOVEMENT
		/*
		if (!GlobalSingleton.instance.inMenu) {
			if (!repellerActive) {
				speed = timerS.bulletCountdown;
			} else {
				speed = timerS.bulletCountdown / 2;
			}
		} else {
			speed = 24f;
		}
		*/
		if (repellerActive) {
			speed = moveSpeed/2;		
		}else{
			speed = moveSpeed;
		}
		
		
		
		
		if(!lockPosition){
			GetComponent<Rigidbody>().drag = 0;
			//	targetLight.transform.rotation = Quaternion.LookRotation( new Vector3(0,0,0));
			
			mover = transform.GetComponent<Rigidbody>().velocity;
			mover.x = XCI.GetAxis(XboxAxis.LeftStickX,ControllerNR);
			mover.y = XCI.GetAxis(XboxAxis.LeftStickY,ControllerNR);
			
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
			
			transform.GetComponent<Rigidbody>().velocity = temp;
			
			//	Debug.Log (Input.GetAxis ("Horizontal"));
			
		}
		else{
			//Debug.Log("SHOULD STOP MOVING");
			transform.position = transform.position;
			GetComponent<Rigidbody>().drag = 20;
			
			Vector3 temp = new Vector3();
			temp.x = XCI.GetAxis(XboxAxis.RightStickX,ControllerNR);
			temp.y = XCI.GetAxis(XboxAxis.RightStickY,ControllerNR);
			
			temp.Normalize();
			
			if(temp.x == 0.0 && temp.y == 0.0){
				temp.x = Random.Range(-1f,1f);
				temp.y = Random.Range(-1f,1f);
			}
			
			targeter = temp;
			
		}
		
		lookVector = new Vector3 ( XCI.GetAxis(XboxAxis.RightStickX,ControllerNR),  XCI.GetAxis(XboxAxis.RightStickY,ControllerNR), 0);
		
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
		
		volume = GetComponent<Rigidbody>().velocity.magnitude ;
		volume /= speed;
		//Debug.Log (volume);
		moveSound.volume = volume;
		
		
		if (GetComponent<Rigidbody>().velocity != new Vector3 (0, 0, 0)) {
			moveSound.Play();
		}
		
		
		
		
		// ---------------------------------------------------------------------------------------- ACTIONS
		
		
		// -------------------------------------- BLOCK
		
		if (blockAdder >= blockThreshold) {
			blockAdder = 0;
			//blocksLeft++;
			blockMan.UpdateAmountOfAvailableBlocks(1);
		}
		
		
		
		
		//BLOCK GRABBING
		if (XCI.GetButton(XboxButton.LeftBumper,ControllerNR) && !isBlockSpawning && blockMan.GetAmountOfBlocksInMiddle() > 0) {
			isBlockSpawning = true;
			blockInstance = blockMan.TakeBlockFromMiddle(); //(GameObject)Instantiate(Resources.Load("block",typeof(GameObject)));
			BlockS = blockInstance.GetComponent<blockScript>();
			blockInstantTimerRunning = true;
			blockPullSound.Play();
			BlockS.ChangeParticlesSize(0.08f);
			blockInstance.GetComponent<Renderer>().enabled = false;
			bBoxScript.CurrentBlockofEvaluation = blockInstance;
			//blockInstance.collider.isTrigger = true;
			
		}
		
		//BLOCK DRAGGING
		if(isBlockSpawning && blockInstance != null){
			Vector3 blockTempPos = transform.position;
			blockTempPos.x +=  XCI.GetAxis(XboxAxis.RightStickX,ControllerNR);
			blockTempPos.y +=  XCI.GetAxis(XboxAxis.RightStickY,ControllerNR);
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
			blockInstance.transform.rotation = q;
			blockInstance.transform.localScale = new Vector3( scaler*0.3f,scaler*2.0f, 1f);
			
			if(!bBoxScript.CanPlaceBlock()){
				blockInstance.GetComponent<Renderer>().material.color = Color.red;
				BlockS.ChangeParticlesColor(Color.red);
			}
			else{
				blockInstance.GetComponent<Renderer>().material.color = Color.white;
				BlockS.ChangeParticlesColor(Color.white);
			}
			
			if(blockInstantTimerRunning){
				blockInstantPlaceTimer += Time.fixedDeltaTime;
			}
			
			//BLOCK PLACING
			if(!XCI.GetButton(XboxButton.LeftBumper,ControllerNR)){
				isBlockSpawning = false;
				//Debug.Log("Tries to place block");
				
				
				if(!bBoxScript.CanPlaceBlock()){ //  CANNOT PLACE BLOCK
					
					//Debug.Log("can't place block");
					blockMan.PlaceBlockBackInMiddle();
					Destroy(blockInstance);
					StartCoroutine(WaitToResetBlockBool());
					//	bBoxScript.canPlaceBlock = false;
					blockErrorSound.Play();
					return;
				}
				
				if(Mathf.Abs(XCI.GetAxis(XboxAxis.RightStickX,ControllerNR))+ Mathf.Abs(XCI.GetAxis(XboxAxis.RightStickY,ControllerNR)) == 0){
					blockMan.PlaceBlockBackInMiddle();
					Destroy(blockInstance);
					StartCoroutine(WaitToResetBlockBool());
					
					return;
				}
				
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
				bBoxScript.CurrentBlockofEvaluation = null;
			}
		}
		
		//------------------------------------------------ REPELLER
		//Debug.Log (repellerActive);
		//Debug.Log(Input.GetAxis("Lbumper"));
		
		
		if (XCI.GetAxis(XboxAxis.LeftTrigger,ControllerNR) > 0 && !repellerActive) { //&& repellerCoolDown > 1   SPAWN REPELLER 
			repellerActive = true;
			
			//if(repellerSound.isPlaying)
			//	repellerSound.Stop ();
			
			SpawnRepeller();
			
		}
		
		if (repellerActive) { //ON REPELLER ACTIVE
			Vector3 tPos2 = transform.position;
			tPos2.z = -1;
			repeller.transform.position = tPos2;
			//		repellerCoolDown -= Time.deltaTime;
			if(repellerCoolDown <= 0){
				DeSpawnRepeller();
			}
		}
		
		if (XCI.GetAxis(XboxAxis.LeftTrigger,ControllerNR) == 0 && repellerActive) { //DESPAWN REPELLER
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
		
		
		
		
		
		// --------------------------------------------------- ReDIRECT
		
		
			if(rediScript.CanRedirect()){
			redirectLight.intensity = 2;
			foreach(GameObject g in redirectObjects){
				g.GetComponent<Renderer>().material.color = rediColor;
			}
		}
		else{
			redirectLight.intensity = 0;
			foreach(GameObject g in redirectObjects){
				g.GetComponent<Renderer>().material.color = Color.black;
			}
		}



		if (XCI.GetButton(XboxButton.RightBumper,ControllerNR)){ 
			if(rediScript.CanRedirect()){
				if(!redButtonDown) {
					redButtonDown = true;
					camScript.SubtractFromIntensity(1f);
					StartCoroutine(Redirect(transform.position));	
				}
			}
			else{
				if(!redButtonDown){
					blockErrorSound.Play();	
					blockMan.RedirectErrorEffect();
					redButtonDown = true;
				}
			}
		}

		if (redButtonDown && !XCI.GetButton(XboxButton.RightBumper,ControllerNR)) {
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
		bullet.GetComponent<Rigidbody>().velocity = vel*bulletSpeed;
		
		transform.GetComponent<Rigidbody>().AddForce((-vel)*1000f);
		shootSound.Play ();
		
		shootEffect.transform.rotation = targetLight.transform.rotation;
		shootEffect.Play ();
		
		targeter = new Vector3 (0, 0, 0);
		bulletList.Add(bullet);
		if(!sS.inMenu)
			timerS.bulletCountdown += bulletCountdownAdder;
		
		rediScript.redirectCoolCurrentGoal++;
		//bulletCounter++;
		blockAdder++;
		totalBulletCount++;
		giantParticle.instance.SetParticleSpeed(bullets);
		if(goal == null){
			goal = GameObject.FindGameObjectWithTag("goal");
			if(!GlobalSingleton.instance.inMenu){
				goalScr = goal.GetComponentInChildren<goalScript>();
				goalScr.SetParticleSpeed(bullets);
			}
		}
		
	}
	
	
	public void RemoveBullet(GameObject b){
		bulletList.Remove (b);
		Destroy(b);
		rediScript.redirectCoolCurrentGoal--;
		giantParticle.instance.SetParticleSpeed(bullets);
		if(!GlobalSingleton.instance.inMenu){
			goalScr.SetParticleSpeed(bullets);
		}
	}
	
	
	public void addToBlocks(){
		blocksLeft++;
	}
	
	
	void SpawnRepeller(){
		//Debug.Log ("SPAWNING");
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
		//Debug.Log ("DESPAWNING");
		repeller.GetComponent<Animator> ().SetBool ("spawn", false);
		
		//if(repellerSound.isPlaying)
		//	repellerSound.Stop();
		
		//repellerSound.pitch = 0.8f;
		//repellerSound.PlayOneShot(repellerSoundClip);
		repellerActive = false;
		//speed = 24;
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
		foreach (GameObject g in bulletList) { // I figured out the bug. It's because a bullet can be deleted during one of these frames. AH I KNOW. make boolean. if this is true, wait a few frames with destroy bullet. :)
			if(g != null){
				Vector3 vectorToTarget = targetPos - g.transform.position;
				g.GetComponent<Rigidbody>().velocity = vectorToTarget;
				g.GetComponent<Rigidbody>().velocity.Normalize();
				i--;
				
				if(i == 0){
					yield return 0;
					i = 10;
				}
			}
		}
		//Redirecttimer = redirectCoolInitial;
		rediScript.Redirect ();
		
		//	StartCoroutine(redirectCooldown());
		yield return 0;
	}
	
	
	IEnumerator WaitToResetBlockBool() // Resets block boolean after block has been placed.
	{
		//returning 0 will make it wait 1 frame
		yield return 0;
		
		//code goes here
		bBoxScript.SetInTopCorner(false);
		bBoxScript.SetOutOfBounds (false);
		
		
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