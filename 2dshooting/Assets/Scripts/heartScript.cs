using UnityEngine;
using System.Collections;

public class heartScript : MonoBehaviour {

	TextMesh lifeText;
	public int life = 10;
	
	// Use this for initialization
	void Start () {
		lifeText = GetComponentInChildren<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		lifeText.text = "Lives: " + life;
	}
	
	
	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag == "ball") {
			life--;		
		}
	}
}
