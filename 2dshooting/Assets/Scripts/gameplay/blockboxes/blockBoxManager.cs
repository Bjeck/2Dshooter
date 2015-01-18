using UnityEngine;
using System.Collections;

public class blockBoxManager : MonoBehaviour {

	bool canPlaceBlock = false;
	public bool isInTopCorner = false;
	public bool isOutOfBounds = false;

	public GameObject CurrentBlockofEvaluation;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	//	Debug.Log ( + " outofboungs: " + isOutOfBounds + "  canplace: " + canPlaceBlock);
		if (CurrentBlockofEvaluation != null) {
				//Debug.Log (CurrentBlockofEvaluation + " canplace: " + canPlaceBlock+" topcorner: " + isInTopCorner);	
		}

		if (isInTopCorner && !isOutOfBounds) {
			canPlaceBlock = true;
		}
		else{
			canPlaceBlock = false;
		}
	}


	public void SetInTopCorner(bool b){
		//Debug.Log("GETTING "+b+" "+counter);
		isInTopCorner = b;
	}

	public void SetOutOfBounds(bool b){
//		Debug.Log ("OUT OF BOUNDS SET: " + b);
		isOutOfBounds = b;
	}

	public bool CanPlaceBlock(){
		return canPlaceBlock;
	}




}
