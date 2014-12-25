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

	public GameObject particleSystemHolder;
	List<ParticleSystem> lifeParticles = new List<ParticleSystem>();
	int currentlyUnlitParticles = 0;
	
	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		lifeText = lifeTextObject.GetComponent<TextMesh> ();

		Reset ();

		foreach(ParticleSystem p in lifeParticles){
			p.gameObject.SetActive(true);
		}

		if(!sS.inMenu){
			lifeText.text = "";
		}

	}
	
	// Update is called once per frame
	void Update () {



		heartSystem.startSize = (life/16f)*0.2f;


	}
	
	
	void OnCollisionEnter(Collision c){
		giantParticle.instance.ChangeBackgroundColor(new Color(0.65f,0.0f,0.18f,0.1f));
		if (c.gameObject.tag == "ball" && GlobalSingleton.instance.isPlayingForReal) {
			life--;		

			lifeParticles[currentlyUnlitParticles].startColor = Color.black;
			currentlyUnlitParticles++;


			if(life <= 0 && GlobalSingleton.instance.isPlayingForReal){
				endingObject.GetComponent<Ending>().EndGame();
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
