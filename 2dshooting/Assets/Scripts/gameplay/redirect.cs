using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This script controls how much the player can redirect, as well as showing this through the lights on the right side of the screen.

public class redirect : MonoBehaviour {

	public GameObject singleton;
	GlobalSingleton sS;
	//camera camScript;

	public bool canRedirect = true;
	public float RedirectCounter = 0;
	public float redirectCoolCurrentGoal = 12;
	public int numberOfRedirectsAvailable = 0;
	ParticleSystem redirectParticles;
	AudioSource redirectSound;
	public float Redpct;
	public GameObject particleSystemHolder;
	List<ParticleSystem> redirectLights = new List<ParticleSystem>();
	int activeLights;
	public ParticleSystem chargeFeedback;

	Color readyColor = new Color((216f/255f),(75f/255f),0f);
	Color chargingColor = new Color (255, 255, 255);


	public List<GameObject> lights = new List<GameObject>();
	int curEnume = 0;

	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		redirectParticles = GetComponent<ParticleSystem>();
		redirectSound = GetComponent<AudioSource>();

		Reset ();

		foreach(ParticleSystem p in redirectLights){
			p.startColor = Color.black;
		}

		if(sS.inMenu)
			RedirectCounter = redirectCoolCurrentGoal;// ????

	}
	
	// Update is called once per frame
	void Update () {
		CheckLights();
		ChargingUpLight ();
		CheckRedirect ();
		chargeFeedback.transform.position = redirectLights [curEnume].transform.position;
	}


	void CheckRedirect(){
		Redpct = (int)((RedirectCounter / redirectCoolCurrentGoal)*100);
	//	if(!sS.inMenu){
			if(RedirectCounter >= redirectCoolCurrentGoal){ //redirect is available again.
				canRedirect = true;
				RedirectCounter = 0;
				numberOfRedirectsAvailable++;
			}
			else{
				canRedirect = false;
			}
		//}
	//	else{
		//	canRedirect = true;
		//}
	}



	public bool CanRedirect(){
		//if (sS.inMenu) {
		//	return canRedirect;
		//}
		//else{
			if (numberOfRedirectsAvailable > 0) {
				canRedirect = true;	
			}
			else{
				canRedirect = false;
			}
		//}
		return canRedirect;
	}

	public void Redirect(){
		//RedirectCounter = RedirectCounter - redirectCoolCurrentGoal;
		redirectParticles.Play();
		redirectSound.Play ();
		numberOfRedirectsAvailable--;
	}



	void CheckLights(){

		int lightThatShouldBeActive = 0;
		lightThatShouldBeActive = numberOfRedirectsAvailable;


		activeLights = 0;
		foreach(ParticleSystem p in redirectLights){
			if(p.gameObject != null){
				if(p.startColor == readyColor){
					activeLights++;
				}
			}
		}

		int diff = 0;
		if(numberOfRedirectsAvailable == activeLights){
			//DO NOTHING, ALL LIGHTS ARE GREAT
		}
		else{
			//Debug.Log("THERE'S DIFF");
			diff = numberOfRedirectsAvailable - activeLights;
			if(diff > 0){
			//	Debug.Log("DIFF MORE");
				for(int i = 0;i<diff;i++){
					CanRedirectMore();
				}
			}
			else if(diff < 0){
			//	Debug.Log("DIFF LESS");
				for(int i = 0;i>diff;i--){
					CanRedirectLess();
				}
			}

		}
	}




	public void ChargingUpLight(){
		//Debug.Log ("CHARGING  "+curEnume+" "+redirectLights[curEnume].startColor);
		if (curEnume <= lights.Count) {
			if(lights[curEnume] != null){
					redirectLights[curEnume].startColor = new Color((((Redpct*2.55f))/chargingColor.r),
					                                                (((Redpct*2.55f))/chargingColor.g),
					                                                (((Redpct*2.55f))/chargingColor.b));
			}
		}
	}




	public void CanRedirectMore(){
		if (curEnume <= lights.Count) {
			if(lights[curEnume] != null){
				//Debug.Log("MORE "+lights[curEnume]);
				redirectLights[curEnume].startColor = readyColor;

			}
			curEnume++;
		}
	}

	public void CanRedirectLess(){
		//Debug.Log("LESS1 "+lights[curEnume]);
		if(lights[curEnume] != null){
		//	Debug.Log("LESS2 "+lights[curEnume]);
		//	lights [curEnume].SetActive (false);
			redirectLights[curEnume].startColor = Color.black;
		}
		curEnume--;
		if(curEnume < 0)
			curEnume = 0;
	}



	public void Reset(){
		if(sS.inMenu){

			RedirectCounter = redirectCoolCurrentGoal;
		}
		else{

			if(sS.isPlayingForReal){
				RedirectCounter = 0;
			}
			else{
				RedirectCounter = redirectCoolCurrentGoal*4;
			}
		}

		lights.Clear();
		redirectLights.Clear();
//		tempList.


		particleSystemHolder = GameObject.FindGameObjectWithTag("RedirectParticleHolder");
		redirectLights.AddRange(particleSystemHolder.GetComponentsInChildren<ParticleSystem>(true));
		//lights.AddRange(particleSystemHolder.GetComponentsInChildren<Transform>(true));

//		Debug.Log(particleSystemHolder+"   "+redirectLights.Count+" "+lights.Count);
		foreach(ParticleSystem g in redirectLights){
			//redirectLights.Add(g);
			lights.Add (g.gameObject);
		}
		//	redirectLights.AddRange();
		
	//
	//	Debug.Log("RESET "+redirectLights.Count+" "+lights.Count);

	}


}
