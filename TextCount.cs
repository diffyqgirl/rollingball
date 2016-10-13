using UnityEngine;
using System.Collections;
/*
 * Displays the score at the top left of the screen
 */ 
public class TextCount : MonoBehaviour {
	int score;//how many cubes the player has collected so far
	int numCubes;//total number of cubes in the game
	GUIText mytext;
	GameObject player;//reference to the player
	PlayerController playerscript;//reference to the code attached to the player

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");//finds the player, which is tagged thusly in unity
		mytext = this.GetComponent<GUIText> ();
		score = 0;//you start with 0 points
		numCubes = GameObject.FindGameObjectsWithTag ("PickUp").Length; // the total number of cubes in this game--not hardcoded :)
		playerscript = player.GetComponent<PlayerController> ();//finds the script attached to the player
		mytext.text = "SCORE: " + score + "/" + numCubes;//text to display

	}

	
	// Update is called once per frame
	void Update () 
	{
		score = playerscript.getCount ();//retrieves from the playerscript the count of how many cubes have been picked up by the player
		mytext.text = "SCORE: " + score + "/" + numCubes;//update text to display
	}
}
