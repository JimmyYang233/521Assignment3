using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	

	public GameObject Enemy;

	public GameObject Enemies;

	public bool isLeftWall;

	public bool isTop;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<Enemy>().destroyAndRespawn();
		}
		else
		{
			//Do-Nothing
		}
	}

	public void respawnNewEnemy()
	{
		GameObject theEnemy = Instantiate(Enemy, Enemies.transform);
		if (isLeftWall)
		{
			Vector3 position = new Vector3(transform.position.x+1.8f, 1, transform.position.z);
			theEnemy.transform.position = position;
			theEnemy.GetComponent<Enemy>().isTop = isTop;
		}
		else
		{
			Vector3 position = new Vector3(transform.position.x-1.8f, 1, transform.position.z);
			theEnemy.transform.position = position;
			theEnemy.transform.Rotate(0,180,0);
			theEnemy.GetComponent<Enemy>().isTop = isTop;
		}
		
	}
}
