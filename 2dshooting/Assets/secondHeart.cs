using UnityEngine;
using System.Collections;

public class secondHeart : MonoBehaviour {

	public heartScript firstHeart;
	public GameObject cam;
	public ParticleSystem heartSystem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		heartSystem.startSize = (firstHeart.life/16f)*0.1f;
	
	}

	void OnCollisionEnter(Collision c){
		
		if (c.gameObject.tag == "ball" && GlobalSingleton.instance.isPlayingForReal) {
			if(c.gameObject.GetComponent<bulletScript>().canHitHeart){
				giantParticle.instance.ChangeBackgroundColor(new Color(0.65f,0.0f,0.18f,0.1f));
				
				if(GlobalSingleton.instance.isDoingTutorial){
					// DO NOTHING OTHER THAN FEEDBACK
				}
				else{
					firstHeart.LoseLife(1);
				}
				//cam.GetComponent<camera>().StartCameraShake(1);
				cam.GetComponent<camera>().PlayShake(cam.GetComponent<camera>().magnitude-0.3f);
			}
		}
	}
}
