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
	float moveSpeed;
	public float moveSpeedModifier = 80;
	public float shakeAmount = 0.1f;
	Vector3 shakeOrigin;
	public float shakeTime = 0.2f;
	bool shouldShake = false;
	public float shakeSmooth = 1;
	public float shakeStrength = 1;

	float intensityLerpGoal = 0f;
	float intensityAdder;
	float intensitySubtracter;
	bool changeIntensity = false;



	public float duration = 0.5f;
	public float speed = 1.0f;
	public float magnitude = 0.1f;
	
	public bool test = false;



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
			
			moveSpeed =  ((float)playerScript.bulletList.Count)/moveSpeedModifier;
			
			transform.position = new Vector3 (Mathf.Lerp(transform.position.x,centerCoords.x,moveSpeed*Time.fixedDeltaTime), 
			                                  Mathf.Lerp(transform.position.y,centerCoords.y,moveSpeed*Time.fixedDeltaTime), 
			                                  transform.position.z);
		}

		if (Input.GetKeyDown (KeyCode.T)) {
			PlayShake(magnitude);
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



	public void PlayShake(float m) {
		
		StopAllCoroutines();
		StartCoroutine(pShake(m));
	}


	IEnumerator pShake(float m) {
		
		float elapsed = 0.0f;
		
		Vector3 originalCamPos = Camera.main.transform.position;
		float randomStart = Random.Range(-10.0f, 10.0f);
		
		while (elapsed < duration) {
			
			elapsed += Time.deltaTime;			
			
			float percentComplete = elapsed / duration;			
			
			// We want to reduce the shake from full power to 0 starting half way through
			float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);
			
			// Calculate the noise parameter starting randomly and going as fast as speed allows
			float alpha = randomStart + speed * percentComplete;
			
			// map noise to [-1, 1]
			float x = Util.Noise.GetNoise(alpha, 0.0f, 0.0f) * 2.0f - 1.0f;
			float y = Util.Noise.GetNoise(0.0f, alpha, 0.0f) * 2.0f - 1.0f;
			
			x *= m * damper;
			y *= m * damper;

			y+= originalCamPos.y;
			x+= originalCamPos.x;
			
			Camera.main.transform.position = new Vector3(x, y, originalCamPos.z);
			
			yield return null;
		}

		Camera.main.transform.position = originalCamPos;
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
