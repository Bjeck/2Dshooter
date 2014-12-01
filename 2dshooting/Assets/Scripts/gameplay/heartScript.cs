using UnityEngine;
using System.Collections;

public class heartScript : MonoBehaviour {

	public GameObject singleton;
	GlobalSingleton sS;

	public GameObject lifeTextObject;
	TextMesh lifeText;
	public int life = 10;
	public GameObject endingObject;
	Ending ending;

	public ParticleSystem heartSystem;
	
	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		lifeText = lifeTextObject.GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {

		if(!sS.inMenu){
			lifeText.text = ""+life;
		}

		heartSystem.startSize = (life/16f)*0.2f;


	}
	
	
	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag == "ball") {
			life--;		

			if(life <= 0){
				endingObject.GetComponent<Ending>().EndGame();
			}
		}
	}
}
