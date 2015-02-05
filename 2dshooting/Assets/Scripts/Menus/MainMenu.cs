using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {


	public GameObject[] objectsToSet;

	public bool inMenu = true;
	public GameObject singleton;
	GlobalSingleton sS;

	public GameObject endingObject;
	Ending ending;

	TextMesh menuText;
	public TextMesh menuIntroText;

	public GameObject redirectObject;

	redirect rediScript;
	
	public TextMesh blocktext;
	public TextMesh balltext;
	public TextMesh repellertext;
	public TextMesh redirecttext;
	public TextMesh redirectpcttext;
	Color rediColor = new Color((216f/255f),(75f/255f),0f);


	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		inMenu = sS.inMenu;

		rediScript = redirectObject.GetComponent<redirect> ();
//		Debug.Log (inMenu);

		menuText = GetComponent<TextMesh> ();


		if (inMenu) {
			foreach(GameObject g in objectsToSet){
				g.SetActive(false);
				//g.GetComponent<TextMesh>().text = "";
			}
			menuText.text = "This is the Menu. Press Space to Start";

		}
		else{
			menuText.text = "";
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space)){ //
 			sS.inMenu = !sS.inMenu;
			endingObject.GetComponent<Ending>().SpaceEndedIt = true;
			endingObject.GetComponent<Ending>().RestartGame();
		}

		if (ending == null) {
			ending = endingObject.GetComponent<Ending>();
		}


//GOING FROM AND TO MENU: LOGIC THAT DETERMINES WHETHER WE SHOULD BE DOING TUTORIAL OR NOT

		if (GlobalSingleton.instance.inMenu) 
		{
			if(Input.GetAxis("Fire1") > 0){ // DO TUTORIAL
				if(GlobalSingleton.instance.hasCompletedTutorialGeneral){
					GlobalSingleton.instance.isDoingTutorial = false;
					GlobalSingleton.instance.inMenu = false;
					ending.SpaceEndedIt = true;
					if(GlobalSingleton.instance.isPaused){
						ending.UnPauseGame();
					}
					ending.RestartGame();
				}
				else{
					GlobalSingleton.instance.isDoingTutorial = true;
					GlobalSingleton.instance.inMenu = false;
					ending.SpaceEndedIt = true;
					if(GlobalSingleton.instance.isPaused){
						ending.UnPauseGame();
					}
					ending.RestartGame();
				}
			}
			else if(Input.GetAxis("Fire2") > 0){ // AVOID TUTORIAL
				GlobalSingleton.instance.isDoingTutorial = false;
				GlobalSingleton.instance.hasCompletedTutorialNow = false;
				GlobalSingleton.instance.hasCompletedTutorialGeneral = true;
				GlobalSingleton.instance.inMenu = false;
				ending.SpaceEndedIt = true;
				if(GlobalSingleton.instance.isPaused){
					ending.UnPauseGame();
				}
				ending.RestartGame();
			}
			else if(Input.GetAxis("Fire3") > 0){
				if(GlobalSingleton.instance.hasCompletedTutorialGeneral){ // REPEAT TUTORIAL
					GlobalSingleton.instance.isDoingTutorial = true;
					GlobalSingleton.instance.hasCompletedTutorialNow = false;
					GlobalSingleton.instance.hasCompletedTutorialGeneral = false;
					GlobalSingleton.instance.inMenu = false;
					ending.SpaceEndedIt = true;
					if(GlobalSingleton.instance.isPaused){
						ending.UnPauseGame();
					}
					ending.RestartGame();
				}
			}
		}
		else{
			if(GlobalSingleton.instance.isPaused){ // IF PAUSED, GO BACK TO THE MENU
				if(Input.GetAxis("Fire1") > 0){
					GlobalSingleton.instance.isDoingTutorial = true;
					GlobalSingleton.instance.hasCompletedTutorialNow = false;
					GlobalSingleton.instance.hasCompletedTutorialGeneral = false;
					GlobalSingleton.instance.inMenu = true;
					ending.SpaceEndedIt = true;
					ending.UnPauseGame();
					ending.RestartGame();
				}
			}
		}




// ---------------------- TEXTS

		if(sS.inMenu){
			if(!GlobalSingleton.instance.hasCompletedTutorialGeneral){
				menuIntroText.text = "Press A to play Tutorial. Press B to Skip";
				balltext.text = " ";
				blocktext.text = "";
				repellertext.text = " ";
				redirecttext.text = " ";
				redirectpcttext.text = " ";
			}
			else{
				menuIntroText.text = "Press A to Play. Press X to replay Tutorial";

				balltext.text = "RT: Particles";
				blocktext.text = "Blocks: LT";
				repellertext.text = "Repeller: LB";
				if (rediScript.CanRedirect()) {
					redirecttext.text = "RB! Redirect";
					redirectpcttext.text = ""+rediScript.Redpct+"%";
					redirecttext.color = rediColor;
					redirectpcttext.color = rediColor;
				}
				else{
					redirecttext.text = "RB: Redirect";
					redirectpcttext.text = ""+rediScript.Redpct+"%";
					redirecttext.color = Color.red;
					redirectpcttext.color = Color.red;
				}

			}
		}
		else{
			menuIntroText.text = " ";
			balltext.text = " ";
			blocktext.text = "";
			repellertext.text = " ";
			redirecttext.text = " ";
			redirectpcttext.text = " ";
		}


	
	}
}
