using UnityEngine;
using System.Collections;

public class blockBox : MonoBehaviour {

	public bool canPlaceBlock = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("Can place block: "+canPlaceBlock);
	}


	void OnTriggerEnter(Collider col){
		//Debug.Log ("TRIGGER"+col.name);
		if (col.gameObject.tag == "block") {
			canPlaceBlock = true;
		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "block") {
			canPlaceBlock = false;
		}
	}


	public bool CanPlaceBlock(){
		return canPlaceBlock;
	}



}
