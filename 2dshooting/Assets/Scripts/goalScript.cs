using UnityEngine;
using System.Collections;

public class goalScript : MonoBehaviour {


	TextMesh scoreText;
	int score = 0;

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
		}
	}



}
