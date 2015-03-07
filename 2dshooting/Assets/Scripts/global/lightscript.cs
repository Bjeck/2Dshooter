using UnityEngine;
using System.Collections;

public class lightscript : MonoBehaviour {

	Quaternion rotation;
	Vector3 rot;
	public float speed = 1;
	Light light;
	float inten;

	// Use this for initialization
	void Start () {
		rotation = transform.rotation;
		light = GetComponent<Light> ();
	}
	
	// Update is called once per frame
	void Update () {

		rot = new Vector3(speed*Time.fixedDeltaTime,-speed*Time.fixedDeltaTime,0);

		transform.Rotate (rot);

		inten = transform.rotation.eulerAngles.z;
		inten /= 180;
		inten *= 0.5f;

		light.intensity = inten;



	}
}
