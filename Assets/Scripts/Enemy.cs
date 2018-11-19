using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

	public int speed;
	public bool isWin;
	
	private CharacterController charController;

	public bool isTop;
	
	
	// Use this for initialization
	void Start ()
	{
		isWin = false;
	}

	
	// Update is called once per frame
	void Update () {
		if (!isWin)
		{
			move();
		}	
	}

	void move()
	{
		//charController.SimpleMove(transform.forward * speed);
		transform.position += transform.forward * speed*Time.deltaTime;
	}
	

	public void destroyAndRespawnFromTop()
	{
		DoorsControl doors = GameObject.FindGameObjectWithTag("Doors").GetComponent<DoorsControl>();
		doors.newEnemyFromTop();
		Destroy(gameObject);
	}

	public void destroyAndRespawnFromBottom()
	{
		DoorsControl doors = GameObject.FindGameObjectWithTag("Doors").GetComponent<DoorsControl>();
		doors.newEnemyFromBottom();
		Destroy(gameObject);
	}

	public void rotate()
	{
		transform.Rotate(0,180,0);
		/**
		Quaternion rotation = this.transform.localRotation;
		rotation.y+= 180;
		transform.localRotation = rotation;
		**/
	}

	public void destroyAndRespawn()
	{
		if (isTop)
		{
			destroyAndRespawnFromTop();
		}
		else
		{
			destroyAndRespawnFromBottom();
		}
	}
}
