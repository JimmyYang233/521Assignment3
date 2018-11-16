using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	

	public GameObject Enemy;

	public GameObject Enemies;

	public bool isLeftWall;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void respawnNewEnemy()
	{
		GameObject theEnemy = Instantiate(Enemy, Enemies.transform);
		if (isLeftWall)
		{
			theEnemy.transform.position = this.transform.position + Vector3.right*1.8f;
		}
		else
		{
			theEnemy.transform.position = this.transform.position + Vector3.left*1.8f;
			theEnemy.transform.Rotate(0,180,0);
		}
		
	}
}
