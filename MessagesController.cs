using UnityEngine;
using System.Collections;
/*
 *Controls a UI that has red text in the middle of the screen, used to display messages to the user
 */
public class MessagesController : MonoBehaviour {
	public GameObject player;
	private GUIText myText;
	private int numCubes;//total number of cubes in the game

	// Use this for initialization
	void Start () {
		myText=this.GetComponent<GUIText> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		numCubes = GameObject.FindGameObjectsWithTag ("PickUp").Length; // the total number of cubes in this game--not hardcoded :)
	}
	
	// Update is called once per frame
	void Update () {
		//if the player has fallen 
		if (player.GetComponent<Rigidbody>().position.y <= -10)
			myText.text = "You fell! Press R to return to the starting point.";//alert the user that all is not lost
		else if (player.GetComponent<PlayerController> ().getCount () == numCubes) 
			myText.text = "Congrats! You collected all the cubes!";//you won
		else
			myText.text = "";//clear old messages away so there aren't giant red letter in the middle of the screen
	}
}
