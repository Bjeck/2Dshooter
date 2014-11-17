﻿using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	public GameObject singleton;
	GlobalSingleton sS;

	float intensity = 0f;
	Camera cam;
	bool isLerping = false;
	public float lerpingSpeed = 5f;
	bool firstLerp = true;

	Vector3 cameraPos;

	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		cam = this.gameObject.camera;

		if (sS.inMenu) {
			intensity = -7f;		
		}
		else{
			intensity = 500f;
		}

	}
	
	// Update is called once per frame
	void Update () {

		cam.orthographicSize = 8+intensity;

		//Debug.Log (intensity);

		if (intensity != 0) {
			Lerp ();
		}


	} //update end


	public void AddToIntensity(float a){

		a /= 10f;

		intensity -= a;
		//StartCoroutine (LerpBackToZero (true));
	}



	void Lerp(){
		float originalLerpSpeed = lerpingSpeed;
		if(firstLerp){
			if(sS.inMenu){
				lerpingSpeed = 3f;
			}
			else{
				lerpingSpeed = 5f;
			}
		}
		if(intensity > 0){
			float lerpInit = intensity;
			intensity = Mathf.Lerp(lerpInit,0,lerpingSpeed*Time.fixedDeltaTime);
		}
		else if(intensity < 0){
			float lerpInit = intensity;
			intensity = Mathf.Lerp(lerpInit,0,lerpingSpeed*Time.fixedDeltaTime);
		}
		else{
			isLerping = false;
			if(firstLerp){
				lerpingSpeed = originalLerpSpeed;
			}
		}
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