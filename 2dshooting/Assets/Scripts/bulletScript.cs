using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {
	public float constantSpeed = 5;

	public Vector3 dir;
	GameObject player;
	playerMovement playerS;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		playerS = player.GetComponent<playerMovement> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		rigidbody.velocity = constantSpeed * (rigidbody.velocity.normalized);

		//Debug.Log (isDamagingBullet);
	}


	
}
