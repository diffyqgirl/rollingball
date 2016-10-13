using UnityEngine;
using System.Collections;

/*
 *The idea for this grew out of the 6 hours of physics 16 lab that I spent analyzing a ball on a rotating turntable. 
 *Assumes rolling without slipping and constant angular velocity of the platform (hence no azimuthal forces or translational forces)
 *However, the coriolis force and the centrifugal force exist and are accounted for
 */
public class RotatingGround : MonoBehaviour {
	public int rotationSpeed;//speed at which this platform rotates
	private GameObject player;//reference to the player
	private bool touchingPlayer;//whether the rotatingground and the player are touching
	private Vector3 radius;//distance of the player from the central platform
	private Vector3 OMEGA;//rotation speed of the platform, constant
	private Vector3 deltaV;// change in velocity of the player resulting from being rotated along with the platform (rotating coordinate system)
	private Vector3 Fcentrifugal;//centrifugal force
	private Vector3 Fcoriolis;//coriolis force

	//spiked ball variables--so that the spiked ball can also translate with the rotating platform
	public GameObject spikedBall;//saves a reference to the spiked ball that patrols the rotating platform
	private Vector3 spikedBallRadius;//distance of the spikedBallRadius from the central platform
	private Vector3 spikedBallDeltaV;// change in velocity of the spiked ball resulting from being rotated along with the platform (rotating coordinate system)

	
	// Use this for initialization
	void Start () {
		touchingPlayer = false;
		player = GameObject.FindGameObjectWithTag ("Player");//finds the player and saves a reference to it
		OMEGA = -rotationSpeed * Vector3.up;//constant, or the physics would be a lot tricker
	}

	//called before physics calculations
	void FixedUpdate () {
		radius = player.GetComponent<Rigidbody>().position - this.transform.position-0.5f*Vector3.up;//subtract out the fact that the ball is elevated from the plane by 0.5 units
		this.transform.RotateAround (this.transform.position, Vector3.up, -rotationSpeed * Time.deltaTime);//rotate around the platform's center about the y axis with speed rotation speed
		if (touchingPlayer) 
		{	
			//rotating reference frame--make the player move horizontally with the rotating platform. 
			//Vertical motion is taken care of by gravity and normal forces
			deltaV=Cross (OMEGA,radius)*Time.deltaTime;//update
			player.GetComponent<Rigidbody>().position+=deltaV*Time.deltaTime;// delta x = delta v * delta t

			//simulate a centrifugal force
			//this gets out of control quickly if OMEGA is large
			Fcentrifugal=-player.GetComponent<Rigidbody>().mass*Cross (OMEGA, Cross(OMEGA, radius));//Fcent= - m * w x (w x r)
			player.GetComponent<Rigidbody>().AddForce (Fcentrifugal*Time.deltaTime);

			//simulates the coriolis force
			Fcoriolis = -2*player.GetComponent<Rigidbody>().mass*Cross(OMEGA, player.GetComponent<Rigidbody>().velocity);//Fcoriolis = - 2m * (w x r)
			player.GetComponent<Rigidbody>().AddForce (Fcoriolis*Time.deltaTime);

		}
		//keep the spiked ball in the rotating reference frame
		//unrealistically, the spikedBall doesn't experience coriolis and centrifugal forces because that would throw it off
		spikedBallRadius = (spikedBall.GetComponent<Rigidbody>().position - this.transform.position - 0.5f * Vector3.up);
		spikedBallDeltaV = Cross (OMEGA, spikedBallRadius) * Time.deltaTime;
		spikedBall.GetComponent<Rigidbody>().position += spikedBallDeltaV * Time.deltaTime;//delta x = delta v * delta t

	}

	/*
	 * The below two methods keep track of whether the player is on the rotating platform
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

	//cross product
	public Vector3 Cross(Vector3 a, Vector3 b)
	{
		return new Vector3(a.y*b.z-a.z*b.y, a.z*b.x-a.x*b.z, a.x*b.y-a.y*b.x);
	}
}
