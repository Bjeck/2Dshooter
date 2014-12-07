using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//

public class blockManager : MonoBehaviour {

	List<GameObject> blocksInMiddle = new List<GameObject>();
	List<float> TakenBlockPositions = new List<float>();
	List<float> AvailableBlockPositions = new List<float>(){-0.5f,0.5f,-1.5f,1.5f,-2.5f,2.5f,-3.5f,3.5f,-4.5f,4.5f,-5.5f,5.5f};
	int currentBlockToTake = 0;
	int currentBlockToPlace = 0;

	public float BlocksTotal;
	float blocksAvailable;
	bool hasdoneitonce = true;

	public GameObject player;
	playerMovement pls;

	float blockSlotX = 0.5f;
	float blockSlotY = -0.5f;

	ParticleSystem blockMoveEffect;


	// Use this for initialization
	void Start () {
		pls = player.GetComponent<playerMovement> ();
		blocksAvailable = BlocksTotal;
		blockMoveEffect = GetComponentInChildren<ParticleSystem> ();

	}
	
	// Update is called once per frame
	void Update () {
		//blocksAvailable = pls.blocksLeft;

		//Debug.Log (currentBlockToTake + "  " + blocksInMiddle.Count + " " + blocksAvailable);

		if(hasdoneitonce){ //spawns the right amount of blocks in the beginning.
			for(int j = 0;j<blocksAvailable;j++){
				blocksInMiddle.Add((GameObject)Instantiate(Resources.Load("block",typeof(GameObject))));
			}

			PlaceAllBlocks();
			hasdoneitonce = false;
		}
	
	}


	public void PlaceAllBlocks(){
		currentBlockToTake = 0;
		
		int i = 0;
		foreach (GameObject g in blocksInMiddle) {
			//g.transform.position = new Vector3(0.5f+i,-0.5f-i,0f);
			g.transform.position = PlaceBlockInMiddle();
			g.transform.eulerAngles = new Vector3(0,0,45);
			i++;
		}
	}


	Vector3 PlaceBlockInMiddle(){

		blockSlotX = AvailableBlockPositions [currentBlockToTake];
		blockSlotY = -blockSlotX;
		currentBlockToTake++;

		return new Vector3(blockSlotX, blockSlotY, 0);
	}
	

	public void PlaceBlockBackInMiddle(){

		blocksInMiddle.Add((GameObject)Instantiate(Resources.Load("block",typeof(GameObject))));
		//blockSlotX = AvailableBlockPositions [currentBlockToTake];
		
		////blockSlotY = -blockSlotX;
		//currentBlockToTake++;
		PlaceAllBlocks ();

	}


	public GameObject TakeBlockFromMiddle(){
		GameObject returner = null;
		if (blocksInMiddle.Count > 0) {
			returner = blocksInMiddle [currentBlockToTake-1];
			blocksInMiddle.Remove(returner);
			currentBlockToTake--;	
		}

		blockMoveEffect.transform.position = returner.transform.position;
		blockMoveEffect.transform.eulerAngles = new Vector3(0,0,-45);
		blockMoveEffect.Play ();

		return returner;
	}



	public void UpdateAmountOfAvailableBlocks(float add){
		if (blocksAvailable >= 12) {
			return;		
		}
		blocksAvailable += add;
		blocksInMiddle.Add((GameObject)Instantiate(Resources.Load("block",typeof(GameObject))));

		PlaceAllBlocks();
	}

	public float GetAmountOfBlocksInMiddle(){
		return blocksInMiddle.Count;
	}


}
