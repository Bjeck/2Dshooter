using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {
	float constantSpeed = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody.velocity = constantSpeed * (rigidbody.velocity.normalized);
	}
}
