using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class blockBox : MonoBehaviour {


	public bool canPlaceBlock = false;
	bool insideTopCorner = false;
	bool areBlocksOutOfBounds = false;
	public List<GameObject> antiBlocks = new List<GameObject> ();

	public GameObject blockboxManagerObject;
	blockBoxManager blboxman;

	// Use this for initialization
	void Start () {
		blboxman = blockboxManagerObject.GetComponent<blockBoxManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	//	Debug.Log ("Can place block? "+canPlaceBlock);
	//	areBlocksOutOfBounds = false;
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject == blboxman.CurrentBlockofEvaluation){
			//Debug.Log("something");
			blboxman.SetInTopCorner(true);
			//insideTopCorner = true;
		}
	}

	void OnTriggerExit(Collider col){
//		Debug.Log ("OUTTRIGGER"+col.name);
		if (col.gameObject == blboxman.CurrentBlockofEvaluation){
			  blboxman.SetInTopCorner(false);
		}
	}

/*	void OnTriggerStay(Collider col){
		if (col.gameObject == blboxman.CurrentBlockofEvaluation){
			//Debug.Log("something");
			blboxman.SetInTopCorner(true);
			//insideTopCorner = true;
		}
	}
*/

}
