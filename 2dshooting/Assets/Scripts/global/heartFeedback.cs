using UnityEngine;
using System.Collections;

public class heartFeedback : MonoBehaviour {

	public ParticleSystem heartFeedbackSys;
	public AudioSource[] heartHitSounds;
	float size;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision c){
		if(c.gameObject.tag == "ball"){
			heartFeedbackSys.Stop ();

			int rand = Random.Range(0,1);
			heartHitSounds[rand].Play ();



			StartCoroutine (WaitForStart ());
		}
	}

	IEnumerator WaitForStart(){
		float t = 0;
		while (t<1) {
			t += Time.deltaTime;
			yield return 0;
		}
		heartFeedbackSys.Play ();
		yield return 0;
	}

}
