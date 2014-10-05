using UnityEngine;
using System.Collections;

public class blockScript : MonoBehaviour {


	public int durability = 20;
	public GameObject player;
	playerMovement playerS;
	AudioSource blockHitSound;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerS = player.GetComponent<playerMovement> ();
		blockHitSound = GetComponent<AudioSource> ();
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

			blockHitSound.Play ();
			if(!playerS.canRedirect){
				playerS.Redirecttimer--;
			}
			if(playerS.Redirecttimer == 0){
				playerS.canRedirect = true;
				playerS.Redirecttimer = playerS.redirectCoolInitial;
			}
			durability--;

			//Debug.Log("OH SHIT! "+playerS.Redirecttimer);
		}
		if (c.gameObject.name == "blockBox") {
			//Debug.Log("HITTING BLOCKBOX");
		}
	}
}
