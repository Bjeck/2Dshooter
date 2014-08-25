using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {
	float constantSpeed = 5;

	public AudioSource scoreSound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = constantSpeed * (rigidbody.velocity.normalized);
	}
	
}
