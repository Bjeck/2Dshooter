using UnityEngine;
using System.Collections;

public class blockScript : MonoBehaviour {


	public int durability;
	int maxDurability;
	public GameObject player;
	playerMovement playerS;
	AudioSource blockHitSound;
	camera camScript;
	float durColorVal;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerS = player.GetComponent<playerMovement> ();
		blockHitSound = GetComponent<AudioSource> ();
		camScript = Camera.main.GetComponent<camera> ();
		maxDurability = durability;
	}
	
	// Update is called once per frame
	void Update () {
	
		if (durability <= 0) {
			GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovement>().addToBlocks();
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision c){
		//Debug.Log ("COLLUSION");
		if (c.gameObject.tag == "ball") {
			blockHitSound.pitch = 1;
			blockHitSound.pitch += Random.Range(-0.1f,0.1f);
			camScript.SubtractFromIntensity(0.25f);

			blockHitSound.Play ();
			//if(!playerS.canRedirect){ //redirect charge up
				//playerS.Redirecttimer--;
				playerS.RedirectCounter++;
			//}

			durability--;
			durColorVal = (float)durability/maxDurability;
			renderer.material.color = new Color(durColorVal,durColorVal,durColorVal);

			//Debug.Log("OH SHIT! "+playerS.Redirecttimer);
		}
		if (c.gameObject.name == "blockBox") {
			//Debug.Log("HITTING BLOCKBOX");
		}
	}
}
