using UnityEngine;
using System.Collections;

public class antiblockBox : MonoBehaviour {
	
	public bool cannotPlaceBlock = false;

	public GameObject blockboxManagerObject;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//	Debug.Log ("Can place block? "+canPlaceBlock);
	}
	
	
/*void OnTriggerEnter(Collider col){
		Debug.Log ("TRIGGER"+col.name);
		if (col.gameObject.tag == "blockUnplaced") {
			cannotPlaceBlock = true;
		}
	}
	
	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "blockUnplaced") {
			cannotPlaceBlock = false;
		}
	}*/
	void OnTriggerStay(Collider col){
		if (col.gameObject.tag == "blockUnplaced") {
		//	blockboxManagerObject.GetComponent<blockBoxManager>().SetInTopCorner(false);
			//cannotPlaceBlock = true;
		}
	}
	
	
	public bool CannotPlaceBlock(){
		return cannotPlaceBlock;
	}
	
	
	
}
