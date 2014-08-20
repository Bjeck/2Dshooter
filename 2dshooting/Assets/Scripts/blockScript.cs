using UnityEngine;
using System.Collections;

public class blockScript : MonoBehaviour {


	public int durability = 20;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (durability <= 0) {
			GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovement>().addToBlocks();
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag == "ball") {

			durability--;
		//	Debug.Log("OH SHIT! "+durability);
		}
	}
}
