using UnityEngine;
using System.Collections;

public class timerScript : MonoBehaviour {


	TextMesh bulletTimerText;
	public float bulletCountdown = 20;
	public ParticleSystem warningSystem;
	public GameObject player;
	public GameObject heart;
	playerMovement playerS;
	float parSize;
	float parSpeed;
	public bool lost = false;
	public GameObject endingObject;
	Ending ending;
	public GameObject scaleObject;
	public GameObject scaleObject2;
	Vector3 scaleSize;
	Vector3 scaleInitialSize;
	Vector3 scaleSize2;
	Vector3 scaleInitialSize2;

	float flashTimer = 0.2f;
	bool isTextYellow = true;

	public GameObject timeBar;
	public GameObject timeBar2;
	Vector3 barScale;


	// Use this for initialization
	void Start () {
		bulletTimerText = GetComponent<TextMesh> ();
		//warningSystem = GetComponentInChildren<ParticleSystem> ();
		playerS = player.GetComponent<playerMovement> ();
		scaleInitialSize = scaleObject.transform.localScale;
		scaleInitialSize2 = scaleObject2.transform.localScale;
		barScale = new Vector3(0f,0.4775151f,1);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (GlobalSingleton.instance.isPaused) {
			return;		
		}
	
		if(!lost){
			bulletCountdown -= Time.fixedDeltaTime;
			bulletCountdown = Mathf.Round (bulletCountdown * 100f) / 100f;

			bulletTimerText.text = ""+bulletCountdown;

			barScale.x = (bulletCountdown / 20)*14;
			timeBar.transform.localScale = barScale;
			timeBar2.transform.localScale = barScale;

		}

		if (bulletCountdown <= 0) { //if time over: LOSE

			//bulletTimerText.text = "YOU LOSE!";
			lost = true;
			endingObject.GetComponent<Ending>().EndGame();
			//bulletCountdown = 10;
		}

		if (bulletCountdown < 10f) { //changing position depending on the number to the left of the comma.
			transform.position = new Vector3(-1.35f,transform.position.y,transform.position.z);
			
		}
		else{
			transform.position = new Vector3(-2.43f,transform.position.y,transform.position.z);
		}

		//START WARNING SYSTEM
		if (bulletCountdown < 6f){ //&& !warningSystem.isPlaying) {
			//giantParticle.instance.ChangeBackgroundColor(new Color(0.9f,0.0f,0f,0.2f));
			//giantParticle.instance.globalBackgroundParticles.gravityModifier = 4f;
			//warningSystem.Play ();
			warningSystem.startSize = 0;
			warningSystem.startSpeed = 5f;

			scaleSize = scaleObject.transform.localScale;
			scaleSize.x = 3.1f*(6-bulletCountdown);
			scaleSize.y = 40f;
			scaleObject.transform.localScale = scaleSize;

			if(scaleObject.renderer.material.color.a < 0.7f)
				scaleObject.renderer.material.color = new Color(scaleObject.renderer.material.color.r,scaleObject.renderer.material.color.g,scaleObject.renderer.material.color.b,(6-bulletCountdown)/6);

			scaleSize2 = scaleObject2.transform.localScale;
			scaleSize2.x = 3.1f*(6-bulletCountdown);
			scaleSize2.y = 40f;
			scaleObject2.transform.localScale = scaleSize;

			if(scaleObject2.renderer.material.color.a < 0.7f)
				scaleObject2.renderer.material.color = new Color(scaleObject2.renderer.material.color.r,scaleObject2.renderer.material.color.g,scaleObject2.renderer.material.color.b,(6-bulletCountdown)/6);


		}
		else if(bulletCountdown > 6f){ //&& warningSystem.isPlaying){
			//warningSystem.Stop ();
			//giantParticle.instance.globalBackgroundParticles.gravityModifier = 0f;

			scaleObject.transform.localScale = scaleInitialSize;
			scaleObject2.transform.localScale = scaleInitialSize2;

		}

		if (warningSystem.isPlaying) {
			parSize = bulletCountdown;
			parSize /= 6;
			parSize *= 3;
			parSize = 3- parSize;
			//parSize -= parSize;
			//Debug.Log(parSize);
			warningSystem.startSize = parSize;

			//NOT USED ATM
			int rand = Random.Range(0,3);
			if(rand == 0){
				warningSystem.startColor = Color.red;
			}
			else if(rand == 1){
				warningSystem.startColor = Color.yellow;
			}
			else if(rand == 2){
				warningSystem.startColor = Color.cyan;
			}
			else if(rand == 3){
				warningSystem.startColor = Color.magenta;
			}



			parSpeed = bulletCountdown;
			parSpeed /= 6;
			parSpeed *= 10;
			parSpeed = 10 - parSpeed;
			parSpeed += 3;
			warningSystem.startSpeed = parSpeed;
		}

		if(bulletCountdown < 4f){
			if(flashTimer <= 0){
				if(isTextYellow){
					bulletTimerText.color = Color.red;
					isTextYellow = false;
				}
				else{
					bulletTimerText.color = Color.yellow;
					isTextYellow = true;
				}
				flashTimer = 0.2f;
			}
			else{
				flashTimer -= Time.deltaTime;
			}

		}
		else{
			bulletTimerText.color = Color.white;
		}
	}
}
