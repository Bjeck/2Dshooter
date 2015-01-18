using UnityEngine;
using System.Collections;

public class edgeBlockBox : MonoBehaviour {

	
	public GameObject blockboxManagerObject;
	blockBoxManager blboxman;
	
	// Use this for initialization
	void Start () {
		blboxman = blockboxManagerObject.GetComponent<blockBoxManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		//	Debug.Log ("Can place block? "+canPlaceBlock);
	}
	
	
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "blockUnplaced") {
			Debug.Log("ENTERING TRIGGER");
		}

		//Debug.Log ("TRIGGER"+col.name);
		if (col.gameObject == blboxman.CurrentBlockofEvaluation){
			Debug.Log("OUT OF BOUNDS");
			blboxman.SetOutOfBounds(true);
		}
	}
	
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "blockUnplaced") {
			Debug.Log("EXITING TRIGGER");
		}

		if (col.gameObject == blboxman.CurrentBlockofEvaluation){
			Debug.Log("IN BOUNDS");
			blboxman.SetOutOfBounds(false);
		}
	}
/*
	void OnTriggerStay(Collider col){
		if (col.gameObject == blboxman.CurrentBlockofEvaluation){
			Debug.Log("OUT OF BOUNDS");
			blboxman.SetOutOfBounds(true);
		}
	}
*/
	
	
	
}



