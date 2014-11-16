using UnityEngine;
using System.Collections;

public class globalTimer : MonoBehaviour {

	TextMesh globalTimerText;
	float globaltimer = 0.0f;

	// Use this for initialization
	void Start () {
		globalTimerText = GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		globaltimer += Time.fixedDeltaTime;
	//	globaltimer = Mathf.Round (globaltimer * 100f) / 100f;
		globalTimerText.text = ""+(int)globaltimer;


	/*	if (globaltimer < 10f) { //changing position depending on the number to the left of the comma.
			transform.position = new Vector3(-13.5f,transform.position.y,transform.position.z);
			
		}
		else{
			transform.position = new Vector3(-14f,transform.position.y,transform.position.z);
		}
*/
	}
}
