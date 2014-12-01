using UnityEngine;
using System.Collections;

public class textMover : MonoBehaviour {

	camera cam;
	float textMoveSpeed = 1f;

	// Use this for initialization
	void Start () {

		cam = Camera.main.GetComponent<camera> ();
	
	}
	
	// Update is called once per frame
	void Update () {


		transform.position = new Vector3 (Mathf.Lerp (transform.position.x, cam.gameObject.transform.position.x, textMoveSpeed * Time.deltaTime),
		                                  Mathf.Lerp (transform.position.y, cam.gameObject.transform.position.y-0.7f, textMoveSpeed * Time.deltaTime),
		                                 transform.position.z);
	
	}
}
