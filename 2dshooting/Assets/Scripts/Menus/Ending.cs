using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ending : MonoBehaviour {

	public bool canEnd = false;
	public bool hasEnded = false;
	public bool SpaceEndedIt = false;
	bool startButtonDown;
	public GameObject UIObjects;


	public GameObject redirectObj;
	public List<TextMesh> endTexts = new List<TextMesh>();
	public TextMesh pauseText;
	public Light dirLight;

	GameObject scoreObject;
	GameObject Player;
	GameObject globalTimer; 

	public AudioSource pauseSound;
	public AudioSource unPauseSound;
	public TextMesh menuInfoText;




	// Use this for initialization
	void Start () {

	//	endTexts.Add(GetComponent<TextMesh>());
		endTexts.AddRange(GetComponentsInChildren<TextMesh>());
		GlobalSingleton.instance.hasEnded = false;


		if (!hasEnded) {
			foreach(TextMesh t in endTexts){
				t.text = "";
			}		
		}
	}
	
	// Update is called once per frame
	void Update () {

// ---------- IF HAS ENDED
		if (hasEnded) {
			menuInfoText.text = "Press A to go to Menu. Press B to Play Again";

			if(Input.GetKey(KeyCode.Space)){ //
				RestartGame();
			}

			if(Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button16)){ // GO TO MENU
				RestartGame();
				GlobalSingleton.instance.inMenu = true;
				SpaceEndedIt = true;
				if(GlobalSingleton.instance.isPaused){
					UnPauseGame();
				}
				RestartGame();
			}
			else if(Input.GetKey(KeyCode.Joystick1Button1) || Input.GetKey(KeyCode.Joystick1Button17)){
				GlobalSingleton.instance.inMenu = false;
				SpaceEndedIt = true;
				if(GlobalSingleton.instance.isPaused){
					UnPauseGame();
				}
				RestartGame();
			}
		}

		if(!hasEnded){
			if(Input.GetKeyDown(KeyCode.LeftShift)){
				Time.timeScale = 0.5f;
			}
			else if(Input.GetKeyUp(KeyCode.LeftShift)){
				Time.timeScale = 1f;
			}
		}




		if (GlobalSingleton.instance.isPaused && !startButtonDown) {

			if(!UIObjects.activeInHierarchy && GlobalSingleton.instance.isDoingTutorial){
				UIObjects.SetActive(true);	
			}
				
			if(Input.GetAxis("Start") > 0){
				UnPauseGame();
				if(GlobalSingleton.instance.isDoingTutorial){
					UIObjects.SetActive(false);	
				}
				startButtonDown = true;
			}
		}
		else{
			if(Input.GetAxis("Start") > 0 && !startButtonDown){
				PauseGame();
				startButtonDown = true;
			}
		}
		if (startButtonDown && Input.GetAxis ("Start") <= 0) {
			startButtonDown = false;		
		}


		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		}
	
	}

	public void EndGame(){
//		Debug.Log ("The game has ended");
		if(canEnd){
			if(!SpaceEndedIt){
				ShowScoreText();
			}

			GlobalSingleton.instance.hasEnded = true;

			Time.timeScale = 0;
			hasEnded = true;

		}
	}

	void ShowScoreText(){

		if (!GlobalSingleton.instance.inMenu) {
			scoreObject = GameObject.FindGameObjectWithTag("goal");
			Player = GameObject.FindGameObjectWithTag("Player");


			endTexts[0].text = "The Game is Over";
			endTexts[1].text = "Score: "+ scoreObject.GetComponentInChildren<goalScript>().score;
			endTexts[2].text = "Bullets: "+ Player.GetComponent<playerMovement>().bulletList.Count+" & Total Bullets: "+Player.GetComponent<playerMovement>().totalBulletCount;

			if (!GlobalSingleton.instance.inMenu) {
				globalTimer = GameObject.FindGameObjectWithTag("GlobalTimer");
				endTexts[3].text = "Total Time: "+globalTimer.GetComponent<globalTimer>().globaltimer;
			}
		}

	}

	public void RestartGame(){
		//redirectObj.GetComponent<redirect>().Reset();
		GlobalSingleton.instance.newSceneisLoaded = true;
		Application.LoadLevel (Application.loadedLevel);
		Time.timeScale = 1;
	}



	public void PauseGame(){
		GlobalSingleton.instance.isPaused = true;
		giantParticle.instance.SetPauseParticles (true);
		pauseText.gameObject.SetActive(true);
		dirLight.gameObject.SetActive (false);
		pauseSound.Play ();
	}
	
	public void UnPauseGame(){
		GlobalSingleton.instance.isPaused = false;
		giantParticle.instance.SetPauseParticles (false);
		pauseText.gameObject.SetActive(false);
		dirLight.gameObject.SetActive (true);
		unPauseSound.Play ();
	}



}
