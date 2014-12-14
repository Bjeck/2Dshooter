using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class redirect : MonoBehaviour {


	private static redirect _instance;
	
	public static redirect instance 
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<redirect>();
				
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





	public List<GameObject> lights = new List<GameObject>();
	int curEnume = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CanRedirectMore(){
		if (curEnume <= lights.Count) {
			lights [curEnume].SetActive (true);
			curEnume++;
		}
	}

	public void CanRedirectLess(){
		lights [curEnume].SetActive (false);
		curEnume--;
	}


}
