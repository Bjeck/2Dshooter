using UnityEngine;
using System.Collections;

public class GlobalSingleton : MonoBehaviour {

	public bool inMenu = true;
	public bool isPlayingForReal = true;
	public bool duringStartUp = false;
	public bool isPaused = false;

	private static GlobalSingleton _instance;

	public static GlobalSingleton instance 
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<GlobalSingleton>();

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


	void Start(){
	}
}
