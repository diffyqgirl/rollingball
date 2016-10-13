using UnityEngine;
using System.Collections;
/*
 * Controls stationary bombs that blow up with a flash and damage the player if the player rolls over them
 */ 
public class BombController : MonoBehaviour {
	private GameObject player;
	public int damage; // how much damage a bomb does
	private bool exploded; //whether the bomb has exploded yet or not
	private Behaviour myHalo;//halo of red light around the bomb that appears when the bomb explods

	// initialization
	void Start () {
		myHalo = (this.GetComponent ("Halo") as Behaviour);
		myHalo.enabled = false;
		player = GameObject.FindGameObjectWithTag ("Player");
		damage = 5;
		exploded = false;//no bombs have exploded at initialization
	}
	


	//collides with a triger
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") {
			myHalo.enabled=true; //turns on the red halo aka "explosion"
			//prevent it from exploding multiple times
			if (!exploded)
			{
				exploded=true;//it wont try to explode a second time
				this.GetComponent<AudioSource> ().Play ();//explosion sound, downloaded from unity store
				player.GetComponent<PlayerController>().deductHealth (damage);//call the deduct health method in the player's script to take damage
			}
			this.Invoke ("Explode", 1);//waits for 1 second before invoking the explode method
		}
	}

	/*
	 * I experimented with downloading other people's more sophisticated explosion animations but
	 * in the end I decided that I'd rather make something that looked less cool but was actually mine
	 * This is more like an end explosion method, since it turns off the red light and hides the bomb;
	 */ 
	void Explode(){
		myHalo.enabled = false;//turns off red halo
		this.gameObject.SetActive(false); //hides bomb
		Destroy (this);//removes the bomb completely from the game
	}
}
