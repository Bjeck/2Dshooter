using UnityEngine;
using System.Collections;

public class goalScript : MonoBehaviour {


	public GameObject singleton;
	GlobalSingleton sS;
	
	TextMesh scoreText;
	int score = 0;
	public AudioSource ScoreSound;
	public AudioClip scoreSoundClip;
	public float pitch;

	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		scoreText = GetComponentInChildren<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {

		pitch = ScoreSound.pitch;

	//	Debug.Log ("From goal: " + singleton.activeSelf + " " + singleton.activeInHierarchy + " " + this.gameObject);
		if(!sS.inMenu && this.gameObject != null){
			scoreText.text = "" + score;
		}
	}


	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag == "ball") {
			score++;
			ScoreSound.pitch = 1;
			ScoreSound.pitch += Random.Range(-0.1f,0.1f);
			//if(ScoreSound.pitch < 0.9f)
			//	ScoreSound.pitch = 0.9f;
			//if(ScoreSound.pitch > 1.1f)
			//	ScoreSound.pitch = 1.1f;

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
