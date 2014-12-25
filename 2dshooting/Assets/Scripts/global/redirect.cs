using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class redirect : MonoBehaviour {
/*	private static redirect _instance;
	public static redirect instance 
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<redirect>();
				
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	void Awake(){
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad( this);
		}
		else{
			if(this != _instance){
				Destroy(this.gameObject);
			}
		}
	}
*/

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

	//	redirectLights.AddRange();

//		Debug.Log(particleSystemHolder+"   "+redirectLights.Count+" "+lights.Count+" "+tempList.Length);
		if(sS.inMenu)
			RedirectCounter = redirectCoolCurrentGoal;// ????

	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log(curEnume);
		CheckLights();
		ChargingUpLight ();
		CheckRedirect ();
		//figure out when to call it. I only need to call it when redirect thing changes.
	//s	 Redpct
	}


	void CheckRedirect(){
		Redpct = (int)((RedirectCounter / redirectCoolCurrentGoal)*100);
		if(!sS.inMenu){
			if(RedirectCounter >= redirectCoolCurrentGoal){ //redirect is available again.
				canRedirect = true;
				RedirectCounter = 0;
				numberOfRedirectsAvailable++;
			}
			else{
				canRedirect = false;
			}
		}
		else{
			canRedirect = true;
		}
	}



	public bool CanRedirect(){
		if (numberOfRedirectsAvailable > 0) {
			canRedirect = true;	
		}
		else{
			canRedirect = false;
		}

		return canRedirect;
	}

	/*
	public bool CanRedirect(){
		bool canRedi = false;
		if(!sS.inMenu){
			if(RedirectCounter >= redirectCoolCurrentGoal){ //redirect is available again.
				canRedi = true;
			}
			else{
				canRedi = false;
			}
		}
		else{
			canRedi = true;
		}
		Redpct = (int)((RedirectCounter / redirectCoolCurrentGoal)*100);

//		Debug.Log(canRedi);
		canRedirect = canRedi;
		return canRedi;
	}
*/

	public void Redirect(){
		//RedirectCounter = RedirectCounter - redirectCoolCurrentGoal;
		redirectParticles.Play();
		redirectSound.Play ();
		numberOfRedirectsAvailable--;
	}



	void CheckLights(){
		//Debug.Log("HERE "+lights.Count);
		int lightThatShouldBeActive = 0;
	//	int debugger = 

		//lightThatShouldBeActive = (int)(Redpct/100);
		lightThatShouldBeActive = numberOfRedirectsAvailable;
	//	for(int i = 0;i<Redpct;i+=100){
		//	lightThatShouldBeActive++;
//			Debug.Log("ENUM "+i+" "+lightThatShouldBeActive);
	//	}

		/*for(int i= 0;i<lightThatShouldBeActive;i++){
			lights[i].SetActive(true);
		}
		*/
//		Debug.Log(lightThatShouldBeActive+"   "+Redpct/100+"   "+(int)Redpct/100);

	/*	int activeLights = 0;
		foreach(GameObject g in lights){
			if(g != null){
				if(g.activeInHierarchy == true){
					activeLights++;
				}
			}
		}*/

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
//		Debug.Log(diff+"    "+lightThatShouldBeActive+" "+" "+activeLights+" "+RedirectCounter+" "+redirectCoolCurrentGoal);
	}




	public void ChargingUpLight(){
		//Debug.Log ("CHARGING  "+curEnume+" "+redirectLights[curEnume].startColor);
		if (curEnume <= lights.Count) {
			if(lights[curEnume] != null){
				/*if(activeLights != 0){
					redirectLights[curEnume].startColor = new Color((((Redpct-100)/activeLights)*2.55f)/chargingColor.r,
					                                                (((Redpct-100)/activeLights)*2.55f)/chargingColor.g,
					                                                (((Redpct-100)/activeLights)*2.55f)/chargingColor.b);
				}*/
				//else if(activeLights == 0){
					redirectLights[curEnume].startColor = new Color((((Redpct*2.55f))/chargingColor.r),
					                                                (((Redpct*2.55f))/chargingColor.g),
					                                                (((Redpct*2.55f))/chargingColor.b));
				//}
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
