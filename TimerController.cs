using UnityEngine;
using System.Collections;
/*
 * Keeps track of the total elapsed time and displays it in the top left corner of the screen
 */ 
public class TimerController : MonoBehaviour {

	GUIText mytext;
	int elapsedTime;//make elapsed time an int b/c it's measured in seconds and we don't care about milliseconds
	int minutes;
	int seconds;

	// Use this for initialization
	void Start () {
		mytext = this.GetComponent<GUIText> ();
		elapsedTime = 0;
		minutes = 0;
		seconds = 0;
		mytext.text = "0:00";
	}
	
	
	// Update is called once per frame
	void Update () {
		elapsedTime= (int) Time.realtimeSinceStartup;//unity tracks the total elapsed time since the game began with realtimeSinceStartup
		seconds = elapsedTime % 60;
		minutes = (elapsedTime - seconds) / 60;

		//formatting the time--we want 7 seconds to appear as 0:07, but we don't want 17 seconds to appear as 0:017
		if (seconds < 10)
			mytext.text = minutes + ":0" + seconds;
		else
			mytext.text = minutes + ":" + seconds;
	}
}
