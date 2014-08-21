using UnityEngine;
using System.Collections;

public class timerScript : MonoBehaviour {


	public GameObject bulletTimerObj;
	TextMesh bulletTimerText;
	public float bulletCountdown = 20;
	ParticleSystem warningSystem;
	float parSize;
	float parSpeed;


	// Use this for initialization
	void Start () {
	
		bulletTimerObj = GameObject.Find ("bulletTimerText");
		bulletTimerText = bulletTimerObj.GetComponent<TextMesh> ();
		warningSystem = GetComponentInChildren<ParticleSystem> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		bulletCountdown -= Time.deltaTime;
		bulletCountdown = Mathf.Round (bulletCountdown * 100f) / 100f;
		bulletTimerText.text = "Bullet timer: " + bulletCountdown;



		if (bulletCountdown < 7f && !warningSystem.isPlaying) {
			warningSystem.Play ();
			warningSystem.startSize = 0;
			warningSystem.startSpeed = 5f;
		}
		else if(bulletCountdown > 7f && warningSystem.isPlaying){
			warningSystem.Stop ();
		}

		if (warningSystem.isPlaying) {
			parSize = bulletCountdown;
			parSize /= 7;
			parSize *= 4;
			parSize = 4 - parSize;
			//parSize -= parSize;
			//Debug.Log(parSize);
			warningSystem.startSize = parSize;

			parSpeed = bulletCountdown;
			parSpeed /= 7;
			parSpeed *= 10;
			parSpeed = 10 - parSpeed;
			parSpeed += 3;
			warningSystem.startSpeed = parSpeed;


		}

	}
}
