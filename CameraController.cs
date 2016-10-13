using UnityEngine;
using System.Collections;
/*
 * Controls the camera
 */ 
public class CameraController : MonoBehaviour {
	private GameObject player;
	private Vector3 offset;
	private float speed = 10f;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		offset = transform.position-player.transform.position;
	}
	
	// Update is called once per frame
	// LateUpdate is better than Update for stuff involving info about last known states
	void LateUpdate () {
		/*
		Vector3 newPos = player.transform.position + offset;
		transform.position = Vector3.Lerp (transform.position, newPos, speed * Time.deltaTime);
		*/
		Vector3 velocity = player.GetComponent<Rigidbody>().velocity;

		//don't bother updating if the player isn't moving much
		if (velocity.magnitude >= 0.1) 
		{
			/*
			 * Causes the camera to look down at the player from behind the player's direction of motion
			 */ 
			Vector3 newOffset = new Vector3 ();
			newOffset.y = offset.y;//we always want to look down at 45 degrees no matter what
			//work now in the xz plane
			Vector3 vPlanar = velocity;
			vPlanar.y = 0;//vPlanar is the velocity in the  x z
			Vector3 offsetxz = offset;
			offsetxz.y = 0;
			Vector3 newOffsetxz = -vPlanar.normalized * offsetxz.magnitude;//in the opposite direction of the xz velocity with magnitude of the xz planar offset
			newOffset.x = newOffsetxz.x;
			newOffset.z = newOffsetxz.z;
			Vector3 newPos = player.transform.position + newOffset;//new position for camera to transition to
			transform.position = Vector3.Lerp (transform.position, newPos, speed * Time.deltaTime);//lerp does a gradual transition
			transform.LookAt (player.transform.position);//causes the camera to point at the player
		}
	}

	//dot product
	float dot(Vector3 a, Vector3 b)
	{
		return (float)(a.x * b.x + a.y * b.y + a.z * b.z);
	}
}
