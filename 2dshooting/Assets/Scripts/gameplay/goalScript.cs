using UnityEngine;
using System.Collections;

public class goalScript : MonoBehaviour {


	public GameObject singleton;
	GlobalSingleton sS;
	
	TextMesh scoreText;
	public int score = 0;
	public AudioSource ScoreSound;
	public AudioClip scoreSoundClip;
	public float pitch;

	public ParticleSystem goalSystem;
	public ParticleSystem scoreParticles;


	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		scoreText = GetComponentInChildren<TextMesh> ();

		if (!GlobalSingleton.instance.inMenu) {
			scoreParticles.loop = true;	
			scoreParticles.Play();
		}
		else{
			scoreParticles.loop = false;
			scoreParticles.Play();
		}


	}
	
	// Update is called once per frame
	void Update () {

		pitch = ScoreSound.pitch;

	//	Debug.Log ("From goal: " + singleton.activeSelf + " " + singleton.activeInHierarchy + " " + this.gameObject);


	//SHOW TEXT
		if(!sS.inMenu && this.gameObject != null){
			scoreText.text = "" + score;
		}

	//	goalSystem.maxParticles = score*4+100;
		//giantParticle.maxParticles = score;
		scoreParticles.emissionRate = score;
		giantParticle.instance.SetEmissionRate((score*4)+200);
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
			giantParticle.instance.ChangeBackgroundColor(new Color(0.65f,0.6f,0.18f,0.06f));

		}
	}


	IEnumerator silenceSound(){

		while (ScoreSound.volume > 0.01f) {
			ScoreSound.volume -= 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		yield return 0;

	}

	public void SetParticleSpeed(int f){
		scoreParticles.startSpeed = ((float)f*0.25f)+10f;
	}


}
