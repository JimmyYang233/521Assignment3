using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		newEnemyFromTop();
		newEnemyFromBottom();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void newEnemyFromTop()
	{
		int random = Random.Range(0, 2);
		if (random == 0)
		{
			transform.GetChild(0).gameObject.GetComponent<Door>().respawnNewEnemy();
		}
		else
		{
			transform.GetChild(1).gameObject.GetComponent<Door>().respawnNewEnemy();
		}
	}

	public void newEnemyFromBottom()
	{
		int random = Random.Range(0, 2);
		if (random == 0)
		{
			transform.GetChild(2).gameObject.GetComponent<Door>().respawnNewEnemy();
		}
		else
		{
			transform.GetChild(3).gameObject.GetComponent<Door>().respawnNewEnemy();
		}
	}
}
