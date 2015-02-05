using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class heartScript : MonoBehaviour {

	public GameObject singleton;
	GlobalSingleton sS;


	public GameObject lifeTextObject;
	TextMesh lifeText;
	public int life = 16;
	public GameObject endingObject;
	Ending ending;

	public ParticleSystem heartSystem;
	public GameObject cam;

	public GameObject particleSystemHolder;
	List<ParticleSystem> lifeParticles = new List<ParticleSystem>();
	int currentlyUnlitParticles = 0;

	Color initLightColor;
	
	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		lifeText = lifeTextObject.GetComponent<TextMesh> ();
	
		Reset ();

		foreach(ParticleSystem p in lifeParticles){
			p.gameObject.SetActive(true);
		}
		initLightColor = lifeParticles [0].startColor;


		if(!sS.inMenu){
			lifeText.text = "";
		}

	}
	
	// Update is called once per frame
	void Update () {

		heartSystem.startSize = (life/16f)*0.2f;
	}
	
	
	void OnCollisionEnter(Collision c){

		if (c.gameObject.tag == "ball" && GlobalSingleton.instance.isPlayingForReal) {
			giantParticle.instance.ChangeBackgroundColor(new Color(0.65f,0.0f,0.18f,0.1f));

			if(GlobalSingleton.instance.isDoingTutorial){
				// DO NOTHING OTHER THAN FEEDBACK
			}
			else{
				life--;		

				lifeParticles[currentlyUnlitParticles].startColor = Color.black;
				currentlyUnlitParticles++;
			}
			//cam.GetComponent<camera>().StartCameraShake(1);
			cam.GetComponent<camera>().PlayShake(cam.GetComponent<camera>().magnitude);

			if(life <= 0 && GlobalSingleton.instance.isPlayingForReal){


				endingObject.GetComponent<Ending>().EndGame();



			}
		}
	}


	public void LoseLife(int amount){
		life -= amount;	
		if(life < 0)
			life = 0;

		for (int i = 0; i< amount; i++) {
			if(currentlyUnlitParticles < lifeParticles.Count-1){
				lifeParticles[currentlyUnlitParticles].startColor = Color.black;
				currentlyUnlitParticles++;
			}
		}

		if(life <= 0 && GlobalSingleton.instance.isPlayingForReal){
			endingObject.GetComponent<Ending>().EndGame();
		}
		else{
			cam.GetComponent<camera>().PlayShake(2f);
		}


	}


	public void ColorParticlesForWarning(int amount){
		for (int i = 0; i< amount; i++) {
			if(currentlyUnlitParticles+i <= lifeParticles.Count-1){
				lifeParticles[currentlyUnlitParticles+i].startColor = Color.white;
			}
		}
	}

	public void ResetParticleColor(int amount){
		for (int i = amount-1; i>= 0; i--) {
			if(currentlyUnlitParticles+i <= lifeParticles.Count-1){
				lifeParticles[currentlyUnlitParticles+i].startColor = initLightColor;
			}
		}
	}


	public void Reset(){
		
		if(sS.inMenu){

		}
		else{

		}
		lifeParticles.Clear();

		particleSystemHolder = GameObject.FindGameObjectWithTag("LifeParticleHolder");
		lifeParticles.AddRange(particleSystemHolder.GetComponentsInChildren<ParticleSystem>(true));
			
	}



}
