using UnityEngine;
using System.Collections;

public class destroyTimer : MonoBehaviour {

	public float timeToDestroy = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timeToDestroy -= Time.deltaTime;
		if(timeToDestroy <= 0){
			Destroy(this.gameObject);
		}
	
	}
}
