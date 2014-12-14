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
	public ParticleSystem blockParticles;
	float particleScaler = 1f;
	Vector3 OriginalParticleScale;
	bool scaleParticles = false;
	bool getParticleScaleOnce = false;
	GameObject redirectObj;

	public GameObject blockManager;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		blockManager = GameObject.FindGameObjectWithTag ("BlockManager");
		playerS = player.GetComponent<playerMovement> ();
		blockHitSound = GetComponent<AudioSource> ();
		camScript = Camera.main.GetComponent<camera> ();
		maxDurability = durability;
		blockParticles = GetComponentInChildren<ParticleSystem> ();
//		Debug.Log (blockParticles);
		blockParticles.startSize = 0.0f;
		OriginalParticleScale = blockParticles.transform.localScale;
		redirectObj = GameObject.Find("redirectParticles");
	}
	
	// Update is called once per frame
	void Update () {
	//	Debug.Log(particleScaler);

		if (scaleParticles) {
			if(getParticleScaleOnce){
				OriginalParticleScale = blockParticles.transform.localScale;
				getParticleScaleOnce = false;
			}

			particleScaler = Mathf.Lerp(particleScaler,1f,Time.deltaTime*20f);
			Vector3 particleScale = OriginalParticleScale*particleScaler;
			particleScale.x *= particleScaler;
			blockParticles.transform.localScale = particleScale;
			if(particleScaler <= 1){
				scaleParticles = false;
			}
		}

		if (durability <= 0) {
			GameObject.FindGameObjectWithTag("Player").GetComponent<playerMovement>().addToBlocks();
			blockManager.GetComponent<blockManager>().PlaceBlockBackInMiddle();
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter(Collision c){
		if (c.gameObject.tag == "ball") {
			blockHitSound.pitch = 1;
			blockHitSound.pitch += Random.Range(-0.1f,0.1f);
			camScript.SubtractFromIntensity(0.25f);

			blockHitSound.Play ();
			//if(!playerS.canRedirect){ //redirect charge up
				//playerS.Redirecttimer--;
			redirectObj.GetComponent<redirect>().RedirectCounter++;
			//}

			durability--;
			durColorVal = (float)durability/maxDurability;
			renderer.material.color = new Color(durColorVal,durColorVal,durColorVal);
			blockParticles.startColor = new Color(durColorVal,durColorVal,durColorVal);

		}
		if (c.gameObject.name == "blockBox") {
		}
	}


	public void ChangeParticlesSize(float f){
		blockParticles.startSize = f;
	}

	public void ChangeParticlesColor(Color c){
		blockParticles.startColor = c;
	}


	public void SpreadOutParticles(){
		particleScaler = 2f;
		scaleParticles = true;
		getParticleScaleOnce = true;
		//StartCoroutine (ParticleMergingAgain ());
	}


	IEnumerator ParticleMergingAgain(){

		while(particleScaler > 1){


			yield return 0;
		}
		yield return 0;
	}
	//1.264082, 0.08445112, 1.037827






}
