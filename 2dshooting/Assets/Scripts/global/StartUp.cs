using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Script that controls the startup, mainly the animation, and the delay on spawning elements in the level (player and corners)

public class StartUp : MonoBehaviour {


	public GameObject singleton;
	GlobalSingleton sS;
	camera cam;
	public GameObject timers;

	bool isStarting = true;

	public GameObject player;
	public GameObject managers;

	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();
		if(sS.inMenu){
			player.SetActive(true);
			managers.SetActive(true);
			return;
		}
		cam = Camera.main.GetComponent<camera>();
		StartCoroutine(WaitToBegin());
	//	Debug.Log("start");
	}
	
	// Update is called once per frame
	void Update () {

		if(!sS.inMenu){
		//	Debug.Log("starting: "+isStarting);
			sS.duringStartUp = isStarting;
		}
			
	}


	IEnumerator WaitToBegin(){
		float inten = cam.intensity;
		//Debug.Log("1 "+inten);
		while (inten >= 1f || inten <= -1f) {
		//	Debug.Log(inten);
			inten = cam.intensity;

		//	if(inten >= 2f || inten <= -2f){
		//		managers.SetActive(true);
			//}
			yield return 0;
		}
		float timer = 0;
		while(timer < 0.0f){
			timer += Time.deltaTime;
			yield return 0;
		}

		player.SetActive(true);
		timers.SetActive(true);
		managers.SetActive(true);	

		isStarting = false;
		
		yield return 0;
	}
}
