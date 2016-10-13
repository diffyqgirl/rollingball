using UnityEngine;
using System.Collections;
/*
 * Updates the text for a UI that keeps track of how many times the user has fallen and had to reset their position with the "R" key
 */ 
public class FallsTextController : MonoBehaviour {
	GUIText mytext;//the GUIText component that contains the text that this gameobject displays
	GameObject player; //reference to the player
	PlayerController playerScript; //the script attached to the player

	// Use this for initialization
	void Start () {
		mytext = this.GetComponent<GUIText> ();//retrieve a component from this object of type GUIText
		player = GameObject.FindGameObjectWithTag ("Player");//reference to the player
		playerScript = player.GetComponent<PlayerController> ();//retrieves the script attached to the player so that I can access variables, methods
		mytext.text = "Falls: 0";
	}
	
	// Update is called once per frame
	void Update () {
		//update with the current value of the number of times the player has fallen from the player's script
		mytext.text = "Falls: " + playerScript.getFallsCount (); 
	}
}
