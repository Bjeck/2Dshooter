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

	// Use this for initialization
	void Start () {
		bulletTimerText = GetComponent<TextMesh> ();
		//warningSystem = GetComponentInChildren<ParticleSystem> ();
		playerS = player.GetComponent<playerMovement> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(!lost){
			bulletCountdown -= Time.fixedDeltaTime;
			bulletCountdown = Mathf.Round (bulletCountdown * 100f) / 100f;

			bulletTimerText.text = ""+bulletCountdown;
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

		if (bulletCountdown < 6f){ //&& !warningSystem.isPlaying) {
			giantParticle.instance.ChangeBackgroundColor(new Color(0.9f,0.0f,0f,0.2f));
			giantParticle.instance.globalBackgroundParticles.gravityModifier = 4f;
			warningSystem.Play ();
			warningSystem.startSize = 0;
			warningSystem.startSpeed = 5f;
		}
		else if(bulletCountdown > 6f){ //&& warningSystem.isPlaying){
			warningSystem.Stop ();
			giantParticle.instance.globalBackgroundParticles.gravityModifier = 0f;
		}

		if (warningSystem.isPlaying) {
			parSize = bulletCountdown;
			parSize /= 6;
			parSize *= 3;
			parSize = 3- parSize;
			//parSize -= parSize;
			//Debug.Log(parSize);
			warningSystem.startSize = parSize;

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
			bulletTimerText.color = Color.yellow;
		}
		else{
			bulletTimerText.color = Color.white;
		}
	}
}
