using UnityEngine;
using System.Collections;

public class giantParticle : MonoBehaviour {

	private static giantParticle _instance;
	
	public static giantParticle instance 
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<giantParticle>();
				
				DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}
	
	void Awake(){
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad( this);
		}
		else{
			if(this != _instance){
				Destroy(this.gameObject);
			}
		}
	}




	public ParticleSystem globalBackgroundParticles;
	Color particleColor;
	bool pauseParticles = false;

	// Use this for initialization
	void Start () {
		particleColor = globalBackgroundParticles.startColor;
	}
	
	// Update is called once per frame
	void Update () {
	//	
	//	Debug.Log (globalBackgroundParticles.emissionRate);

		if(pauseParticles){
			int rand = Random.Range(0,3);
			if(rand == 0){
				globalBackgroundParticles.startColor = new Color(Color.red.r, Color.red.g, Color.red.b, 0.05f);
			}
			else if(rand == 1){
				globalBackgroundParticles.startColor = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.05f);
			}
			else if(rand == 2){
				globalBackgroundParticles.startColor = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.05f);
			}
			else if(rand == 3){
				globalBackgroundParticles.startColor =  new Color(Color.magenta.r, Color.magenta.g, Color.magenta.b, 0.05f);
			}

		}
	}


	public void ChangeBackgroundColor(Color inpCol){

		if(globalBackgroundParticles != null){
		//	Debug.Log ("CHANGE");
			globalBackgroundParticles.startColor = inpCol;
			//globalBackgroundParticles
			StartCoroutine (WaitASecondBeforeResettingColor ());
		}
	}

	IEnumerator WaitASecondBeforeResettingColor(){

		float t = 0;
		while (t<0.5) {
			t += Time.deltaTime;
			yield return 0;
		}
		//Debug.Log ("CHANGE BACK");
		//renderer.material.color = particleColor;
		globalBackgroundParticles.startColor = particleColor;
		yield return 0;

	}


	public void SetEmissionRate(int i){
		globalBackgroundParticles.emissionRate = i;
	}

	public void SetParticleSpeed(int f){
		globalBackgroundParticles.startSpeed = (float)f/4.0f;
		globalBackgroundParticles.startLifetime = (float)f/40f+1f;
	}



	public void SetPauseParticles(bool b){
		pauseParticles = b;

		if (b == false) {
			globalBackgroundParticles.startColor = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.05f);
			globalBackgroundParticles.startLifetime = 1;
			globalBackgroundParticles.startSize = 2.86f;
			globalBackgroundParticles.startSpeed = 0.5f;
			StartCoroutine (ResumeAfterPause ());
		}
		else{
			pauseParticles = true;
			globalBackgroundParticles.startLifetime = 2;
			globalBackgroundParticles.startSize = 1;
			globalBackgroundParticles.startSpeed = 0.3f;

		}
	}

	IEnumerator ResumeAfterPause(){
		
		float t = 0;
		while (t<0.3) {
			t += Time.deltaTime;
			yield return 0;
		}
		//Debug.Log ("CHANGE BACK");
		//renderer.material.color = particleColor;
		globalBackgroundParticles.startColor = particleColor;
		yield return 0;
		
	}

}
