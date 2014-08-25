using UnityEngine;
using System.Collections;

public class goalScript : MonoBehaviour {


	TextMesh scoreText;
	int score = 0;
	public AudioSource ScoreSound;
	public AudioClip scoreSoundClip;

	// Use this for initialization
	void Start () {
		scoreText = GetComponentInChildren<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Score: " + score;
	}


	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag == "ball") {
			score++;
			ScoreSound.pitch += Random.Range(-1,1);
			if(ScoreSound.pitch < 0.9f)
				ScoreSound.pitch = 0.9f;
			if(ScoreSound.pitch > 1.1f)
				ScoreSound.pitch = 1.1f;

			//if(!ScoreSound.isPlaying)
			//	StartCoroutine(silenceSound());

			ScoreSound.volume = 1;

			ScoreSound.PlayOneShot (scoreSoundClip);
		}
	}


	IEnumerator silenceSound(){

		while (ScoreSound.volume > 0.01f) {
			ScoreSound.volume -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		yield return 0;

	}


}
