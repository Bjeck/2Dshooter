using UnityEngine;
using System.Collections;

public class Ending : MonoBehaviour {


	public bool canEnd = false;
	public bool hasEnded = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (hasEnded) {
			if(Input.GetKey(KeyCode.Space)){
				RestartGame();
			}
		}
		else if(!hasEnded){
			if(Input.GetKeyDown(KeyCode.LeftShift)){
				Time.timeScale = 0.5f;
			}
			else if(Input.GetKeyUp(KeyCode.LeftShift)){
				Time.timeScale = 1f;
			}
		}
	
	}

	public void EndGame(){
//		Debug.Log ("The game has ended");
		if(canEnd){
			Time.timeScale = 0;
			hasEnded = true;
		}
	}

	public void RestartGame(){
		Application.LoadLevel (Application.loadedLevel);
		Time.timeScale = 1;
	}
}
