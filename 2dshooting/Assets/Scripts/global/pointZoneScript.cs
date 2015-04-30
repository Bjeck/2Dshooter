using UnityEngine;
using System.Collections;

public class pointZoneScript : MonoBehaviour {

	ParticleSystem partSys;
	float timeToDestruction;
	float initTime;

	// Use this for initialization
	void Start () {

		//timeToDestruction = 0.5f;

		partSys = GetComponentInChildren<ParticleSystem> ();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		timeToDestruction -= Time.fixedDeltaTime;
		if (timeToDestruction <= 0) {
			Destroy(this.gameObject);		
		}

		float size = timeToDestruction;
		size /= initTime;
		size *= 0.3f;
		partSys.startSize = size;
	
	}

	public void InitializeSelf(float time){
		timeToDestruction = time;
		initTime = timeToDestruction;
	}
}
