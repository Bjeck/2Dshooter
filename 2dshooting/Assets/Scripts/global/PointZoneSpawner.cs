using UnityEngine;
using System.Collections;

public class PointZoneSpawner : MonoBehaviour {

	public float PosLimit = 4f;

	public float ScaleLimMin = 4f; //RELATIONSHIP: 2(Pos), 8(scalemin)     3, 6      4, 4       5, 2        6,  0   
	float ScaleLimMax = 12f;
	
	float zVal = 0.65f;

	public float zoneTimerMin;
	public float zoneTimeMax;


	public float ZoneTimer;

	GameObject previousZone;
	GameObject previousZone2;

	ParticleSystem part1;
	ParticleSystem part2;


	// Use this for initialization
	void Start () {
		SpawnZone ();

		ZoneTimer = Random.Range (zoneTimerMin, zoneTimeMax);
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKeyDown (KeyCode.Z)) {
			SpawnZone();		
		}

		ZoneTimer -= Time.fixedDeltaTime;
		if(ZoneTimer <= 0){
			SpawnZone();
		}

		/*if (previousZone != null) {
			float size = ZoneTimer;
			size /= zoneTimeMax;
			size *= 0.3f;
			previousZone.GetComponentInChildren<ParticleSystem> ().startSize = size;
		}
		if (previousZone2 != null) {
			float size = ZoneTimer;
			size /= zoneTimeMax;
			size *= 0.3f;
			previousZone2.GetComponentInChildren<ParticleSystem> ().startSize = size;
		}*/

	}


	void SpawnZone(){
		/*int ZoneAmountDecider = Random.Range (0, 2); // CLEAR ZONES
		if (ZoneAmountDecider == 0) {
			Destroy (previousZone);
			previousZone = null;
			if(previousZone2!=null){
				Destroy (previousZone2);
				previousZone2 = null;
			}
		}
		else{
			if(previousZone2!=null){
				Destroy (previousZone2);
				previousZone2 = null;
			}
		}*/




		Vector3 pos = Vector3.zero;
		Vector3 scale = Vector3.zero;;

		int posChooser =  Random.Range (0, 4);
		Debug.Log (posChooser);
		switch (posChooser) {
		case 0: // Y is negative
			pos.x = Random.Range (-PosLimit, PosLimit);
			pos.y = -PosLimit;
			scale.x = Random.Range (ScaleLimMin,ScaleLimMax-(Mathf.Abs(pos.x*2)));
			scale.y = ScaleLimMin;
			break;
		case 1: //Y is positive
			pos.x = Random.Range (-PosLimit, PosLimit);
			pos.y = PosLimit;
			scale.x = Random.Range (ScaleLimMin,ScaleLimMax-(Mathf.Abs(pos.x*2)));
			scale.y = ScaleLimMin;
			break;
		case 2: //X is negative
			pos.x = -PosLimit;
			pos.y = Random.Range (-PosLimit, PosLimit);
			scale.x = ScaleLimMin;
			scale.y = Random.Range (ScaleLimMin,ScaleLimMax-(Mathf.Abs(pos.y*2)));
			break;
		case 3: //X is positive
			pos.x = PosLimit;
			pos.y = Random.Range (-PosLimit, PosLimit);
			scale.x = ScaleLimMin;
			scale.y = Random.Range (ScaleLimMin,ScaleLimMax-(Mathf.Abs(pos.y*2)));
			break;
		}
		pos.z = zVal;
		scale.z = 1f;

		Debug.Log (pos + " " + scale);

		GameObject pointZoneToSpawn = (GameObject)Instantiate(Resources.Load("pointZone",typeof(GameObject)));
		pointZoneToSpawn.transform.position = pos;
		pointZoneToSpawn.transform.localScale = scale;
		pointZoneToSpawn.GetComponent<pointZoneScript> ().InitializeSelf(Random.Range (zoneTimerMin, zoneTimeMax));

		/*if (previousZone == null) {
			previousZone = pointZoneToSpawn;	
		}
		else{
			previousZone2 = pointZoneToSpawn;
		}*/

		ZoneTimer = Random.Range (zoneTimerMin/4, zoneTimeMax/4);

	}

}
