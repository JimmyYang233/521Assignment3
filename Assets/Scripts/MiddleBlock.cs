using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleBlock : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		
		if (other.gameObject.CompareTag("Enemy"))
		{
			int random = Random.Range(0, 3);
			if (random == 0)
			{
				other.GetComponent<Enemy>().rotate();
			}
			else if (random == 1)
			{
				other.GetComponent<Enemy>().destroyAndRespawn();
			}
			else
			{
				other.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		else
		{
			//Do-nothing
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			other.transform.GetChild(0).gameObject.SetActive(true);
		}
	}
}
