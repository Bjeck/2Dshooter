using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobalSingleton : MonoBehaviour {

	public bool inMenu = true;
	public bool isPlayingForReal = true;
	public bool duringStartUp = false;
	public bool isPaused = false;
	public bool isDoingTutorial;
	public bool hasCompletedTutorialGeneral = false;
	public bool hasCompletedTutorialNow = false;
	public bool newSceneisLoaded = false;
	public bool hasEnded = false;


	public Ending ending;
	public AudioSource timefreezeSound;


	private static GlobalSingleton _instance;

	public static GlobalSingleton instance 
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GlobalSingleton>();

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


	void Start(){
		//Application.targetFrameRate = 60;
		if (!isDoingTutorial) {
			hasCompletedTutorialNow = true;
			hasCompletedTutorialGeneral = true;
		}


	}

	void Update(){

		if (newSceneisLoaded) {
			ResetReferences();
			newSceneisLoaded = false;
		}
	}


	public void ResetReferences(){
	}

	public void TutorialInit(){

	}

	public void FreezeGame(){
		timefreezeSound.Play ();
		GlobalSingleton.instance.isPaused = true;
		giantParticle.instance.SetPauseParticles (true);
	}
	
	public void UnFreezeGame(){
		GlobalSingleton.instance.isPaused = false;
		giantParticle.instance.SetPauseParticles (false);
	}


}
