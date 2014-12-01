using UnityEngine;
using System.Collections;

public class blockBoxManager : MonoBehaviour {

	float counter = 0f;
	bool canPlaceBlock = false;
	public bool isInTopCorner = false;
	bool isOutOfBounds = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	//	Debug.Log ("topcorner: " + isInTopCorner + " outofboungs: " + isOutOfBounds + "  canplace: " + canPlaceBlock);

		if (isInTopCorner && !isOutOfBounds) {
			canPlaceBlock = true;
		}
		else{
			canPlaceBlock = false;
		}
	
	}


	public void SetInTopCorner(bool b){
		counter++;
		//Debug.Log("GETTING "+b+" "+counter);
		isInTopCorner = b;
	}

	public void SetOutOfBounds(bool b){
		isOutOfBounds = b;
	}

	public bool CanPlaceBlock(){
		return canPlaceBlock;
	}




}
