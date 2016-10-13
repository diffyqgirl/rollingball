using UnityEngine;
using System.Collections;
/*
 * Translates a ground plane periodically to make it an elevator
 */ 
public class ElevatorController : MonoBehaviour {
	public float speed;//speed at which the elevator moves
	public float period;//how long it takes to go up or down
	public Vector3 dir;//the direction this elevator moves in
	private float timer;//times elapsed time since the elevator last changed direction
	private bool touchingPlayer;//whether the player and this elevator are touching
	private GameObject player;//reference to the player

	// initialization
	void Start () 
	{
		period = 10f;//measured in seconds
		//since this script is generic for all elevators, elevator directions are assigned individually to elevators in the unity editor
		timer = 0f;//timer starts at 0
		//speed = 1.5f;
		player = GameObject.FindGameObjectWithTag ("Player");//find the player by tag and save a reference to the player
		touchingPlayer = false;//the player doesn't start on an elevator
	}
	//called once per frame
	void Update () 
	{
		timer += Time.deltaTime;//Time.deltaTime is the elapsed time since update was called, so timer is keeping track of the total elapsed time since it was zeroed out
		if (timer >= period) 
		{
			dir = -dir;//elevator changes direction
			timer = 0;//reset the timer
		}
		transform.position = this.transform.position + Time.deltaTime * dir * speed;
		if (touchingPlayer) 
		{	
			//make the player move horizontally (x-z plane) with the elevator. Vertical motion is taken care of by gravity and normal forces
			player.GetComponent<Rigidbody>().position += Time.deltaTime*dir.x*speed*Vector3.right;
			player.GetComponent<Rigidbody>().position += Time.deltaTime*dir.z*speed*Vector3.forward;

		}
	}


	/*
	 * The two methods below will keep track of whether this elevator and the player are touching.
	 * We need to know this to know whether or not to translate the player along with the elevator.
	 */ 

	// a collision is beginning
	void OnCollisionEnter(Collision collision){
		if (collision.collider.tag == "Player") {
			touchingPlayer=true;
		}
	}

	// a collision is ending
	void OnCollisionExit(Collision collision){
		if (collision.collider.tag == "Player") {
			touchingPlayer=false;
		}
	}
}
