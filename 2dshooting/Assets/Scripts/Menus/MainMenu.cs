using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {


	public GameObject[] objectsToSet;

	public bool inMenu = true;
	public GameObject singleton;
	GlobalSingleton sS;

	public GameObject endingObject;
	Ending ending;

	TextMesh menuText;


	// Use this for initialization
	void Start () {
		singleton = GameObject.FindGameObjectWithTag ("DontDestroy");
		sS = singleton.GetComponent<GlobalSingleton> ();

		inMenu = sS.inMenu;

//		Debug.Log (inMenu);

		menuText = GetComponent<TextMesh> ();


		if (inMenu) {
			foreach(GameObject g in objectsToSet){
				g.SetActive(false);
				//g.GetComponent<TextMesh>().text = "";
			}
			menuText.text = "Press Space to Start";

		}
		else{
			menuText.text = "";
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space)){ //
 			sS.inMenu = !sS.inMenu;
			endingObject.GetComponent<Ending>().SpaceEndedIt = true;
			endingObject.GetComponent<Ending>().EndGame();


		}
	
	}
}
