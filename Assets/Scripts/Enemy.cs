using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	public int speed;

	private CharacterController charController;
	
	
	
	// Use this for initialization
	void Start ()
	{
		charController = this.GetComponent<CharacterController>();
	}

	
	// Update is called once per frame
	void Update () {
		move();
	}

	void move()
	{
		charController.SimpleMove(transform.forward * speed);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("TopMiddleBlock"))
		{
			int random = Random.Range(0, 3);
			if (random == 0)
			{
				rotate();
			}
			else if (random == 1)
			{
				destroyAndRespawnFromTop();
			}
			else
			{
				transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		else if(other.gameObject.CompareTag("BottomMiddleBlock"))
		{
			int random = Random.Range(0, 3);
			if (random == 0)
			{
				rotate();
			}
			else if (random == 1)
			{
				destroyAndRespawnFromBottom();
			}
			else
			{
				transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		else if (other.gameObject.CompareTag("TopDoor"))
		{
			destroyAndRespawnFromTop();
		}
		else if (other.gameObject.CompareTag("BottomDoor"))
		{
			destroyAndRespawnFromBottom();
		}
		else
		{
			//Do-Nothing
		}
	}

	void OnTriggerExit(Collider other)
	{
		transform.GetChild(0).gameObject.SetActive(true);
	}

	void destroyAndRespawnFromTop()
	{
		DoorsControl doors = GameObject.FindGameObjectWithTag("Doors").GetComponent<DoorsControl>();
		doors.newEnemyFromTop();
		Destroy(gameObject);
	}

	void destroyAndRespawnFromBottom()
	{
		DoorsControl doors = GameObject.FindGameObjectWithTag("Doors").GetComponent<DoorsControl>();
		doors.newEnemyFromBottom();
		Destroy(gameObject);
	}

	void rotate()
	{
		transform.Rotate(0,180,0);
		/**
		Quaternion rotation = this.transform.localRotation;
		rotation.y+= 180;
		transform.localRotation = rotation;
		**/
	}
}
