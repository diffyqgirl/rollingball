using UnityEngine;
using System.Collections;
/*
 * Code that rotates the cubes
 * I did not write this code--it's from a unity tutorial that I was working through to learn the software
 * However, it is exactly what I needed so I decided to use it.
 */ 
public class Rotator : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		transform.Rotate(new Vector3(15, 30, 45)*Time.deltaTime);
	}
}
