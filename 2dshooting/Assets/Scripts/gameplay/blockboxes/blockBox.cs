using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class blockBox : MonoBehaviour {

	public bool canPlaceBlock = false;
	bool insideTopCorner = false;
	bool areBlocksOutOfBounds = false;
	public List<GameObject> antiBlocks = new List<GameObject> ();

	public GameObject blockboxManagerObject;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	//	Debug.Log ("Can place block? "+canPlaceBlock);

		areBlocksOutOfBounds = false;

	/*	foreach(GameObject g in antiBlocks){
			if(g.GetComponent<antiblockBox>().cannotPlaceBlock){
				areBlocksOutOfBounds = true;
			}
		}

		if (insideTopCorner) {
			if(areBlocksOutOfBounds){
				canPlaceBlock = false;
			}
			else{
				canPlaceBlock = true;
			}
		}
		else{
			canPlaceBlock = false;
		}*/
//		Debug.Log ("topcorner: "+insideTopCorner);

	}


//	void OnTriggerEnter(Collider col){
//		//Debug.Log ("TRIGGER"+col.name);
//		if (col.gameObject.tag == "blockUnplaced") {
//				insideTopCorner = true;
//		}
//	}

/*	void OnTriggerExit(Collider col){
		Debug.Log ("OUTTRIGGER"+col.name);
		if (col.gameObject.tag == "blockUnplaced") {
			insideTopCorner = false;
		}
	}
*/
	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "blockUnplaced") {
			blockboxManagerObject.GetComponent<blockBoxManager>().SetInTopCorner(true);
			//insideTopCorner = true;
		}
	}

	public bool GetInTopCorner(){
		return insideTopCorner;
	}

/*
	public bool CanPlaceBlock(){
		return canPlaceBlock;
	}
*/


}
