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
		Debug.Log("Was here");
	}
}
