using UnityEngine;
using System.Collections;

public class blockBoxManager : MonoBehaviour {

	bool canPlaceBlock = false;
	bool isInTopCorner = false;
	bool insideBottomCorner = false;
	bool isOutOfBounds = false;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.Log ("topcorner: " + isInTopCorner + " outofboungs: " + isOutOfBounds + "  canplace: " + canPlaceBlock);

		if (isInTopCorner && !isOutOfBounds) {
			canPlaceBlock = true;
		}
		else{
			canPlaceBlock = false;
		}
	
	}


	public void SetInTopCorner(bool b){
		isInTopCorner = b;
	}

	public void SetInBottomCorner(bool b){

	}

	public void SetOutOfBounds(bool b){
		isOutOfBounds = b;
	}

	public bool CanPlaceBlock(){
		return canPlaceBlock;
	}




}
