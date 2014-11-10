using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	public float bulletTimerInit = 40f;
	public float bulletTimer;
	public float bulletTimerMultiplier = 0.25f;

	// Use this for initialization
	void Start () {
		bulletTimer = bulletTimerInit;
	}
	
	// Update is called once per frame
	void Update () {

		bulletTimer += Time.deltaTime * bulletTimerMultiplier;
	}
}
