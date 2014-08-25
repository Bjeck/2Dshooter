using UnityEngine;
using System.Collections;

public class timerScript : MonoBehaviour {


	TextMesh bulletTimerText;
	public float bulletCountdown = 20;
	ParticleSystem warningSystem;
	public GameObject player;
	public GameObject heart;
	playerMovement playerS;
	float parSize;
	float parSpeed;
	public bool lost = false;


	// Use this for initialization
	void Start () {
		bulletTimerText = GetComponent<TextMesh> ();
		warningSystem = GetComponentInChildren<ParticleSystem> ();
		playerS = player.GetComponent<playerMovement> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		if(!lost){
			bulletCountdown -= Time.fixedDeltaTime;
			bulletCountdown = Mathf.Round (bulletCountdown * 100f) / 100f;
			bulletTimerText.text = ""+bulletCountdown;
		}
		if (bulletCountdown <= 0) {
			//StartCoroutine( playerS.Redirect(heart.transform.position));
			bulletTimerText.text = "YOU LOSE!";
			lost = true;
			//bulletCountdown = 10;
		}


		if (bulletCountdown < 4f && !warningSystem.isPlaying) {
			warningSystem.Play ();
			warningSystem.startSize = 0;
			warningSystem.startSpeed = 5f;
		}
		else if(bulletCountdown > 4f && warningSystem.isPlaying){
			warningSystem.Stop ();
		}

		if (warningSystem.isPlaying) {
			parSize = bulletCountdown;
			parSize /= 4;
			parSize *= 3;
			parSize = 3- parSize;
			//parSize -= parSize;
			//Debug.Log(parSize);
			warningSystem.startSize = parSize;

			bulletTimerText.color = Color.red;

			parSpeed = bulletCountdown;
			parSpeed /= 4;
			parSpeed *= 10;
			parSpeed = 10 - parSpeed;
			parSpeed += 3;
			warningSystem.startSpeed = parSpeed;
		}
		else{
			bulletTimerText.color = Color.white;
		}

	}
}
