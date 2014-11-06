using UnityEngine;
using System.Collections;

public class camera : MonoBehaviour {

	public float intensity = 0f;
	Camera cam;
	bool isLerping = false;

	Vector3 cameraPos;

	// Use this for initialization
	void Start () {
		cam = this.gameObject.camera;
	}
	
	// Update is called once per frame
	void Update () {
		cam.orthographicSize += intensity;

		Debug.Log (intensity);

		if (!isLerping && intensity != 0) {
			if(intensity > 0){
				StartCoroutine (LerpBackToZero (false));
			}
			else{
				StartCoroutine (LerpBackToZero (false));
			}
		}

	}


	public void AddToIntensity(float a){

		a /= 10f;

		intensity -= a;
		StartCoroutine (LerpBackToZero (true));
	}




	IEnumerator LerpBackToZero(bool lowerThan0){
		isLerping = true;
		//Debug.Log ("CALLED");
		if (lowerThan0) {
			while(intensity < 0){
				//Debug.Log ("HEIGTHENING");
				intensity += 0.1f;
				yield return true;
			}
		}
		else{
			while(intensity > 0){
				intensity -= 0.1f;
				yield return true;
				}
			}
		//intensity = 0;
		yield return false;
		isLerping = false;
		}

}
