using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	public GameObject singleton;
	GlobalSingleton sS;

	public float intensity = 0f;
	float intensityGoal = 0f;
	Camera cam;
	bool isLerping = false;
	float lerpingSpeed = 1f;
	float actionLerpSpeed = 20f;
	float originalLerpSpeed;
	bool firstLerp = true;

	bool isZooming = false;
	Vector3 cameraPos;

	public ParticleSystem introParticles;
	public GameObject player;
	playerMovement playerScript;

	Vector2 centerCoords = new Vector2(0,0);
	public float moveSpeed = 0.5f;

	float intensityLerpGoal = 0f;
	float intensityAdder;
	float intensitySubtracter;
	bool changeIntensity = false;


	// Use this for initialization
	void Start () {


		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();
		playerScript = player.GetComponent<playerMovement> ();

		cam = this.gameObject.camera;
		originalLerpSpeed = lerpingSpeed;

		if (sS.inMenu) {
			intensity = -7f;		
		}
		else{
			intensity = 500f;
		}



	}
	
	// Update is called once per frame
	void FixedUpdate () {

		CenterOfBullets(); //setting camera to move slightly according to bulletpositions
		if(centerCoords != new Vector2(0,0)){
			
			centerCoords.y = Mathf.Clamp(centerCoords.y,0,1.5f);
			centerCoords.x = Mathf.Clamp(centerCoords.x,-2,2);
			
			moveSpeed =  ((float)playerScript.bulletList.Count)/40;
			
			transform.position = new Vector3 (Mathf.Lerp(transform.position.x,centerCoords.x,moveSpeed*Time.fixedDeltaTime), 
			                                  Mathf.Lerp(transform.position.y,centerCoords.y,moveSpeed*Time.fixedDeltaTime), 
			                                  transform.position.z);
		}

/*
		if(blockHits > 0){
			BlockHitToLerp ();
			intensityGoal = -(blockHits / 100f);
		}

		//intensity += intensityGoal;
*/

		if (!firstLerp) {
			lerpingSpeed = Mathf.Abs(intensity*4)+originalLerpSpeed;		
		}

		if (changeIntensity) {
			intensity = intensity + intensityAdder - intensitySubtracter;
			intensityAdder = 0;
			intensitySubtracter = 0;
			changeIntensity = false;
		}


		if (intensityLerpGoal != 0) {
			LerpIntensity(intensityLerpGoal);		
		}

		if (intensity != 0) {
			Lerp ();
		}
		cam.orthographicSize = 8+intensity;

		//Debug.Log (intensity + " Goal: "+intensityLerpGoal+"  +: " + intensityAdder+" -: "+intensitySubtracter+"  " + lerpingSpeed+"  "+firstLerp);
	} //update end


	public void AddToIntensity(float a){
		if (firstLerp) {
			firstLerp = !firstLerp;		
			lerpingSpeed = originalLerpSpeed;
		}
		intensityAdder += 0.1f*a;
		changeIntensity = true;
	}

	public void SubtractFromIntensity(float a){
		if (firstLerp) {
			firstLerp = !firstLerp;		
			lerpingSpeed = originalLerpSpeed;
		}
		intensitySubtracter += 0.1f*a;
		changeIntensity = true;
	}


	void LerpIntensity(float goal){
		intensity = Mathf.Lerp (intensity, goal, actionLerpSpeed * Time.fixedDeltaTime);

		intensityAdder = Mathf.Lerp (intensityAdder, 0, 1 * Time.fixedDeltaTime);
		intensitySubtracter = Mathf.Lerp (intensitySubtracter, 0, 1 * Time.fixedDeltaTime);
	}


	void Lerp(){

		if(firstLerp){
			if(sS.inMenu){
				introParticles.Play();
				lerpingSpeed = 5f;
			}
			else{
				introParticles.Play();
				lerpingSpeed = 12f;
			}
		}
		if(intensity > 0){
			intensity = Mathf.Lerp(intensity,0,lerpingSpeed*Time.fixedDeltaTime);

		}
		else if(intensity < 0){
			intensity = Mathf.Lerp(intensity,0,lerpingSpeed*Time.fixedDeltaTime);
		}
		else{
			isLerping = false;
			if(firstLerp){
				lerpingSpeed = originalLerpSpeed;
				firstLerp = false;
			}
		}
		if(introParticles.isPlaying){
			//Debug.Log(introParticles.startSize);
			introParticles.startSize = Mathf.Lerp(introParticles.startSize,0,15f*Time.fixedDeltaTime);
		}
	}




	void CenterOfBullets(){

		Vector2 sum = new Vector2(0.0f,0.0f);
		foreach (GameObject b in playerScript.bulletList) {
			sum += new Vector2(b.transform.position.x,b.transform.position.y);
		}
		centerCoords.x = sum.x/ playerScript.bulletList.Count;
		centerCoords.y = sum.y/ playerScript.bulletList.Count;

	}


	/*

	IEnumerator LerpBackToZero(bool lowerThan0){
		isLerping = true;
		//Debug.Log ("CALLED");
		if (lowerThan0) {
			while((int)intensity+0.9 < 0){
			//	Debug.Log ("HEIGTHENING");
				intensity += 0.1f;
				yield return true;
			}
		}
		else{
			while((int)intensity+0.9 > 0){
			//	Debug.Log ("LOWERING");
				intensity -= 0.1f;
				yield return true;
				}
			}
		intensity = 0;
		yield return false;
		isLerping = false;
		}*/

}
