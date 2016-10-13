using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour 
{
	public Slider healthSlider; //a reference to the health bar in the bottom left of the screen--assiged via unity editor
	public float speed; // a constant that controls how quickly the player rolls
	public GameObject camera;//the main camera--for local coordinates calculations
	public bool alive;//is the player alive
	private float jumpHeight; //a constant--how high a jump takes you
	private int count; //the player's score--how many cubes they have picked up
	private bool touchingGround; //is the player touching the ground
	private float friction;//controls how strongly forces affect the object, used to simulate friction
	private GameObject currentGround;//the ground that the player is currently touching
	private int cubeHealing;//how much one health cube heals you
	private int fallsCount;//counts how many times you've fallen and had to reset using r as a badge of shame

	//called at initialization
	void Start()
	{
		jumpHeight = 6f;
		count = 0;
		touchingGround = true;//it starts on the ground
		friction = 1f;//default
		healthSlider.value = healthSlider.maxValue;
		alive = true;
		cubeHealing = 10;
	}

    // called before rendering a frame
    void Update()
	{
		//return to start if r key is pressed
		if (Input.GetKey (KeyCode.R)) 
		{
			/*
			 * If the ball has fallen lower than the lowest platform, add to the falls
			 * Technically this makes the falls counter not update if you fall and press r quickly enough,
			 * However, I can't just have fallsCount++ without a condition 
			 * because then accidentally pressing R for too long causes it to repeatedly increment
			 * This was the best work-around I could find.
			 */ 
			if(this.GetComponent<Rigidbody>().position.y < -3f) 
				fallsCount++;//you fell once
			this.GetComponent<Rigidbody>().position=new Vector3(0,1.1f,0);//drop back down onto the starting point
			this.GetComponent<Rigidbody>().velocity=new Vector3(0,0,0); //set velocities equal to zero
		}

    }

    // called before performing physics calculations
    void FixedUpdate()
    {
		//only allow movement if you're alive
		if (alive) 
		{
			Vector3 povx = camera.transform.right.normalized;//the camera's xhat axis, always in the xz plan(confirm this!)
			Vector3 up = new Vector3 (0, 1, 0);//points up in world coordinates
			Vector3 povz = cross (povx, up);
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");
			Vector3 movement = new Vector3 (0, 0, 0);
			movement = movement + povx * moveHorizontal;
			movement = movement + povz * moveVertical;

			//you can't if you're already up ie already jumping (this would need to be modified if there was any way to move up and down other than jumping) 
			if (Input.GetKey (KeyCode.Space) && touchingGround) 
			{ 
					//to account for floating point rounding error
					GetComponent<Rigidbody>().AddForce (Vector3.up * jumpHeight, ForceMode.Impulse);
			}
			GetComponent<Rigidbody>().AddForce (friction * movement * speed * Time.deltaTime);
		}
    }

	// called by unity when player gameobject collides with an object with a trigger attached
	// other is passed back--it's the object that the player collided with 
	void OnTriggerEnter(Collider other)
	{
		// tag values assigned in unity editor
		if (other.gameObject.tag == "PickUp") 
		{
			other.gameObject.SetActive (false);
			count++;
		}
		if (other.gameObject.tag == "HealthCube") {
			other.gameObject.SetActive (false);
			addHealth (cubeHealing);
		}
	}

	// a collision starts
	void OnCollisionEnter(Collision collision){
		if (collision.collider.gameObject.tag == "Ground" || collision.collider.gameObject.tag=="MovingGround") 
		{
			touchingGround=true;
			MeshRenderer mr=collision.collider.gameObject.GetComponent <MeshRenderer>();
			Material mat=mr.material;
			friction=getSurfaceFriction (mat);
			currentGround=collision.collider.gameObject;//save a reference to the grond that the player is touching
		}
	}

	// a collision just ended
	void OnCollisionExit(Collision collision){
		if (collision.collider.tag == "Ground" || collision.collider.tag == "MovingGround") 
		{
			touchingGround=false;
		}
		currentGround = null;//it's not touching ground anymore
	}

	// called by enemy objects if they damage the player, hence it has to be public
	public void deductHealth(int damage){
		healthSlider.value = healthSlider.value - 0.01f*damage;
		if (healthSlider.value <= 0)
			alive = false;//you're dead
	}

	public void addHealth(int healing){
		float newHealth = healthSlider.value + 0.01f * healing;
		if (newHealth > healthSlider.maxValue)
			healthSlider.value = healthSlider.maxValue;
		else
			healthSlider.value = newHealth;
	}

	/*
	 * Simulates the fact that different surfaces have different coefficients of friction.
	 */
	public float getSurfaceFriction (Material mat)
	{
		if (mat.name == "Ice big Mat (Instance)")
			return 1.8f;//As it has been said, Ice is slippery--it's easy to get going out of control.
		if (mat.name == "Sand pattern 08 (Instance)")
			return 0.6f;//movement in sand is slow
		return 1f;//default
	}

	//cross product
	public Vector3 cross(Vector3 a, Vector3 b)
	{
		return new Vector3(a.y*b.z-a.z*b.y, a.z*b.x-a.x*b.z, a.x*b.y-a.y*b.x); //that formula no one can remember
	}

	/*
	 * Getter methods
	 */ 
	public int getFallsCount()
	{
		return fallsCount;
	}

	public int getCount()
	{
		return count;
	}

	public GameObject getCurrentGround()
	{
		return currentGround;
	}
}
