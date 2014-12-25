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

	// Use this for initialization
	void Start () {
		particleColor = globalBackgroundParticles.startColor;
	}
	
	// Update is called once per frame
	void Update () {
	//	
	//	Debug.Log (globalBackgroundParticles.emissionRate);
	}


	public void ChangeBackgroundColor(Color inpCol){

		if(globalBackgroundParticles != null){
		//	Debug.Log ("CHANGE");
			globalBackgroundParticles.startColor = inpCol;
			//globalBackgroundParticles
			StartCoroutine (WaitForStart ());
		}
	}


	
	IEnumerator WaitForStart(){
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


	public void SetEmissionRate(int f){
		globalBackgroundParticles.emissionRate = f;
	}



}
