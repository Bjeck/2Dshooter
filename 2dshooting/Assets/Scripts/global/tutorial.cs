using UnityEngine;
using System.Collections;

public class tutorial : MonoBehaviour {

	public GameObject Heart;
	public GameObject Goal;
	public GameObject Player;
	public GameObject Redirects;
	public GameObject LevelObjects;
	public GameObject LifeParticles;
	public GameObject UIObjects;
	public GameObject blockBoxManager;
	public GameObject blockManager;

	bool tutorialHasBegun = false;
	public int tutorialEnumerator = 0;
	public float textFadeInTime = 1f;
	public bool shouldChange = false;
	GameObject textObject;
	public TextMesh TutText;
	Vector3 textPos;
	public float timeToDelayAction = 1.0f;

	bool firstTimeHere = false;
	bool hasmovedstick = false;
	float timer = 0.0f;
	bool pulledtrigger = false;
	bool localBool = false;

	goalScript gScr;
	heartScript hScr;
	playerMovement pScr;
	blockManager bManScr;
	blockBoxManager bBoxScr;
	redirect rediScr;
	timerScript timerScr;

	// Use this for initialization
	void Start () {

	}


//  BUUUUG :  When you shoot too many bullets a block spawns that cannot find the redirect object, obviously. Also, there are two blocks in the middle, that probably shouldn't be there until they are needed.
	//		  They appear there because of the stupid one-frame activation of every object that I have to do. Maybe, I can figure out a way to not make them spawn if tutorial is set. I would like to avoid 
	//		  tutorial dependencies outside tutorial script as much as possible, though.

	
	// Update is called once per frame
	void Update () {
		if (!GlobalSingleton.instance.inMenu && GlobalSingleton.instance.isDoingTutorial && !GlobalSingleton.instance.hasCompletedTutorialGeneral && !GlobalSingleton.instance.duringStartUp && !tutorialHasBegun) {
			StartTutorial();
			pScr = Player.GetComponent<playerMovement>();
		}
		if (GlobalSingleton.instance.isPaused) {
			return;		
		}


	    //IF TUTORIAL HAS BEEN DETERMINED TO SWITCH
		if (shouldChange) {
			switch (tutorialEnumerator)
			{
			case 0:
				textPos = new Vector3(-9f,0f,-6f);
				FadeText("Left Stick Moves",textPos);
				tutorialEnumerator++;
				break;
			case 1:
				textPos = new Vector3(9f,0f,-6f);
				FadeText("Right Stick Aims",textPos);
				tutorialEnumerator++;
				break;
			case 2:
				FadeText("RT to Shoot\nwhile Aiming",textPos);
				tutorialEnumerator++;
				break;
			case 3:
				textPos = new Vector3(3f,7.5f,-6f);
				FadeText("Hit the Yellow Corner\nto score",textPos);
				Goal.SetActive (true);
				gScr = Goal.GetComponentInChildren<goalScript>();
				tutorialEnumerator++;
				break;
			case 4:
				textPos = new Vector3(-9f,3f,-6f);
				FadeText("LB\nto Repel Particles",textPos);
				tutorialEnumerator++;
				break;
			case 5:
				textPos = new Vector3(-9f,-3.2f,-6f);
				FadeText("Protect the\nRed Corner",textPos);
				Heart.SetActive(true);
				LifeParticles.SetActive(true);
				hScr = Heart.GetComponentInChildren<heartScript>();
				tutorialEnumerator++;
				break;
			case 6:
				textPos = new Vector3(-9.5f,-2f,-6f);
				FadeText("Hold LT to\ngrab a Block\nfrom the Middle",textPos);
				blockManager.SetActive(true);
				for(int i=0;i<2;i++){
					bManScr.UpdateAmountOfAvailableBlocks(1);
				}
				blockBoxManager.SetActive(true);
				bBoxScr = blockBoxManager.GetComponent<blockBoxManager>();
				Redirects.SetActive(true);
				tutorialEnumerator++;
				break;
			case 7:
				//textPos = new Vector3(10f,5f,-6f);
				FadeText("Hold LT and\nuse Right Stick to\nchoose Location",textPos);
				tutorialEnumerator++;
				break;
			case 8:
				textPos = new Vector3(-9.5f,-2f,-6f);
				FadeText("Release LT to\nPlace Block\nwhile Aiming",textPos);
				tutorialEnumerator++;
				break;
			case 9:
				textPos = new Vector3(9.3f,4f,-6f);
				rediScr = Redirects.GetComponentInChildren<redirect>();
				FadeText("Particles hit blocks.\nThis Charges Redirect",textPos);
				tutorialEnumerator++;
				break;
			case 10:
				textPos = new Vector3(9.5f,4f,-6f);
				FadeText("These orbs show\nhow many Redirects\nare available",textPos);
				tutorialEnumerator++;
				break;
			case 11:
				FadeText("Redirect makes all\nparticles target\nthe Yellow Corner",textPos);
				tutorialEnumerator++;
				break;
			case 12:
				textPos = new Vector3(-7f,7f,-6f);
				FadeText("The timer will\nstart once the tutorial\nis over",textPos);
				UIObjects.SetActive(true);
				timerScr.bulletCountdown = 20;
				timerScr.FreezeTime();
				tutorialEnumerator++;
				break;
			case 13:
				FadeText("You only Have\n16 Health Orbs",textPos);
				tutorialEnumerator++;
				break;
			case 14:
				tutorialEnumerator++;
				FadeText("GO!",textPos);
				GlobalSingleton.instance.isDoingTutorial = false;
				GlobalSingleton.instance.hasCompletedTutorialGeneral = true;
				GlobalSingleton.instance.hasCompletedTutorialNow = true;
				timerScr.ResumeTime();
				//bManScr.ResetBlocksInMiddle();

				break;
			}
			shouldChange = false;
		}




		if (!shouldChange) {
			switch (tutorialEnumerator)
			{
			case 0: //DUMMY
				
				break;
			case 1: 
	 // --------------- LS to move

				if(!hasmovedstick){
					//Debug.Log("JOYSTICK: "+Mathf.Abs(Input.GetAxis ("Horizontal")+Input.GetAxis ("Horizontal")));
					if(Mathf.Abs( Input.GetAxis ("Horizontal")+Input.GetAxis ("Horizontal")) > 0.5f){
						//Debug.Log("has moved");
						hasmovedstick = true;
					}
				}
				else{
					timer+= Time.deltaTime;
					//Debug.Log("TIMER "+timer);
					if(timer > timeToDelayAction){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}
				
				break;
				
			case 2: 
	// --------------- RS to AIM

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;

				}

				
				if(!hasmovedstick){
					if(Mathf.Abs( Input.GetAxis ("RightStickX")+Input.GetAxis ("RightStickY")) > 0.5f){
						hasmovedstick = true;
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}
				
				break;
			case 3: 
	// --------------- RT TO SHOOT

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
				}

				if(!hasmovedstick){
					if(!pulledtrigger){
						if(Input.GetAxis("Trigger") > 0){
							pulledtrigger = true;
						}
					}
					else{
						if(Input.GetAxis("Trigger") < 1){
							pulledtrigger = false;
							timer++;
						}
					}
					if(timer > 2){
						Debug.Log("TIMER: "+timer);
						hasmovedstick = true;
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}


				break;
			case 4:
	// --------------- YELLOW CORNER SCORES

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
				}

				if(!hasmovedstick){
					if(gScr.score > 2){
						hasmovedstick = true;
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > 0.5f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 5:
	// --------------- LB REPELS

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
				}
				
				if(!hasmovedstick){
					if(pScr.repellerActive){

						hasmovedstick = true;
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction+2f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 6:
	// --------------- PROTECT THE CORNER
	
				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
				}
				
				if(!hasmovedstick){
					if(!pulledtrigger){
						timer+= Time.deltaTime;
						if(timer > 6f){
							FadeText ("Don't worry.\nYou can't lose in\nthe tutorial",textPos);
							timer = 0;
							pulledtrigger = true;
						}
					}
					else{
						timer+= Time.deltaTime;
						if(timer > 7f){
							timer = 0;
							hasmovedstick = true;
						}
					}
					
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 7:
	// ------------------ LT TO PULL BLOCKS

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}

				
				if(!hasmovedstick){
					if(!pulledtrigger){
						if(Input.GetAxis("LTrigger") > 0){
							hasmovedstick = true;
						}
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 8:
	// ------------------ LT+RS TO MOVE BLOCKS

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}
				
				
				if(!hasmovedstick){
					if(!pulledtrigger)
					{
						if(Input.GetAxis("LTrigger") > 0 && Mathf.Abs( Input.GetAxis ("RightStickX")+Input.GetAxis ("RightStickY")) > 0.5f){
							hasmovedstick = true;
						}
					}
					
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction+1f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 9:
	// ------------------ RELEASE LT TO PLACE BLOCKS

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}
				
				
				if(!hasmovedstick){
					if(!pulledtrigger)
					{
						if(Input.GetAxis("LTrigger") < 1){
							pulledtrigger = true;
						}
					}
					else
					{
						timer+= Time.deltaTime;
						if(timer > timeToDelayAction)
						{
							if(!localBool){
								textPos = new Vector3(9.3f,5f,-6f);
								FadeText("Blocks need to be\nplaced in the top right",textPos);
								timer = 0;
								localBool = true;
							}
							else{
								timer+= Time.deltaTime;
								if(timer > timeToDelayAction+2f){
									timer = 0;
									hasmovedstick = true;
								}
							}
						}
					}
					
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction+2f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 10:
	// ------------------ SHOOT BLOCK TO CHARGE REDIRECT

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}
				
				
				if(!hasmovedstick){
					if(!pulledtrigger){
						timer+= Time.deltaTime;
						if(timer > 6f){
							FadeText ("Hit blocks "+(rediScr.redirectCoolCurrentGoal-rediScr.RedirectCounter)+" times\n with Particles",textPos);
							timer = 0;
							pulledtrigger = true;
						}
					}
					else{
						ChangeText ("Hit blocks "+(rediScr.redirectCoolCurrentGoal-rediScr.RedirectCounter)+" times\n with Particles",textPos);
						if(rediScr.CanRedirect()){
							hasmovedstick = true;
							ChangeText ("",textPos);
						}
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 11:
	// ------------------ REDIRECT IS READY

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}
				
				
				if(!hasmovedstick){
					if(!pulledtrigger){
						timer+= Time.deltaTime;
						if(timer > 6f){
							FadeText ("RB to Redirect",textPos);
							timer = 0;
							pulledtrigger = true;
						}
					}
					else{
						if((Input.GetAxis ("Rbumper") > 0)){
							hasmovedstick = true;
						}
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction+1f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 12:
	// ------------------ REDIRECT DESCRIPTION

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}
				
				
				if(!hasmovedstick){
					timer+= Time.deltaTime;
					if(timer > 6f){
						timer = 0;
						hasmovedstick = true;
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction+1f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 13:
	// ----------------- TIMER INTRODUCTION

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}
				
				
				if(!hasmovedstick)
				{
					if(!pulledtrigger)
					{
						timer+= Time.deltaTime;
						if(timer > timeToDelayAction+5f){
							timer = 0;
							FadeText("Every particle shot will add\n4 seconds",textPos);
							pulledtrigger = true;
						}
					}
					else
					{
						if(!localBool)
						{
							timer+= Time.deltaTime;
							if(timer > timeToDelayAction+5f){
								timer = 0;
								FadeText("If the timer reaches 0\nYou lose 4 Health Orbs",textPos);
								localBool = true;
							}
						}
						else
						{
							timer+= Time.deltaTime;
							if(timer > timeToDelayAction+5f)
							{
								timer = 0;
								FadeText("When the Red Corner is Hit\nYou lose 1 Health Orb",textPos);
								hasmovedstick = true;
							}
						}
					}
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction+5f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}

				break;
			case 14:
	// ----------------  ARE YOU READY?

				if(firstTimeHere){
					firstTimeHere = false;
					timer = 0;
					hasmovedstick = false;
					pulledtrigger = false;
					localBool = false;
				}
				
				
				if(!hasmovedstick){
					timer+= Time.deltaTime;
					if(timer > 6f){
						timer = 0;
						FadeText("Are you ready?",textPos);
						hasmovedstick = true;
					}
		
				}
				else{
					timer+= Time.deltaTime;
					if(timer > timeToDelayAction+5f){
						ExecuteNextTutorial();
						firstTimeHere = true;
					}
				}
				break;
			}

		} // END OF SWITCH



	}


	public void StartTutorial(){
		Heart = GameObject.FindGameObjectWithTag ("heart");
		Goal = GameObject.FindGameObjectWithTag ("goal");
		Player = GameObject.FindGameObjectWithTag ("Player");
		Redirects = GameObject.FindGameObjectWithTag ("redirectMainObject");
		LifeParticles = GameObject.FindGameObjectWithTag ("LifeParticleHolder");
		UIObjects = GameObject.FindGameObjectWithTag ("UIObjects");
		blockBoxManager = GameObject.FindGameObjectWithTag ("BlockBoxManager");
		blockManager = GameObject.FindGameObjectWithTag ("BlockManager");

		//Destroying the stupid blocks that appear first
		bManScr = blockManager.GetComponent<blockManager>();
		timerScr = UIObjects.GetComponentInChildren<timerScript> ();
		//foreach (GameObject g in bManScr.blocksInMiddle) {
		//	Destroy(g);		
		//}

		Heart.SetActive (false);
		Goal.SetActive (false);
		Player.SetActive (true);
		Redirects.SetActive (false);
		LifeParticles.SetActive (false);
		UIObjects.SetActive (false);
		tutorialHasBegun = true;
		blockBoxManager.SetActive (false);
		blockManager.SetActive (false);

		ExecuteNextTutorial ();
	}



	
	public void ExecuteNextTutorial(){
		shouldChange = true;
	}














	// TEXT FADING AND MOVING FUNCTIONS. DON'T CHANGE THESE. ONLY TOUCH TEXT IN THE UPDATE FUNCTION

	void FadeText(string text, Vector3 pos){

		if (tutorialEnumerator == 0) {
			StartCoroutine(FadeInText());	
			ChangeText(text,pos);

		}
		else{
			StartCoroutine (FadeOutText (text,pos));
		}

	}

	void ChangeText(string text, Vector3 pos){
		TutText.text = text;
		transform.position = pos;
	}

	IEnumerator FadeInText(){
		Color tempColor = TutText.color;
		tempColor.a = 0;
		TutText.color = tempColor;


		float t = 0.0f;
		while(t <= 1.0f){
			t+= Time.deltaTime;
			tempColor.a = t;
			TutText.color = tempColor;
			//FADE IN TEXT
			yield return 0;
		}

		tempColor.a = 1;
		TutText.color = tempColor;

		if (tutorialEnumerator == 15) {
			FadeOutText("GO!",textPos);		
		}

		yield return 0;

	}

	
	IEnumerator FadeOutText(string text, Vector3 pos){
		Color tempColor = TutText.color;
		tempColor.a = 0;
		TutText.color = tempColor;

		float t = 1;
		while(t >= 0){
			t -= Time.deltaTime;
			tempColor.a = t;
			TutText.color = tempColor;
			//FADE OUT TEXT
			yield return 0;
		}

		tempColor.a = 0;
		TutText.color = tempColor;
		

		//shouldChange = true;
		if(tutorialEnumerator != 15){
			ChangeText(text,pos);
			StartCoroutine(FadeInText());
		}

		yield return 0;
	}



}
