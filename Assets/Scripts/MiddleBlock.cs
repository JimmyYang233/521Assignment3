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
		
		if (gameObject.CompareTag("TopMiddleBlock")&&other.gameObject.CompareTag("Enemy"))
		{
			int random = Random.Range(0, 3);
			if (random == 0)
			{
				other.GetComponent<Enemy>().rotate();
			}
			else if (random == 1)
			{
				other.GetComponent<Enemy>().destroyAndRespawnFromTop();
			}
			else
			{
				other.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		else if(gameObject.CompareTag("BottomMiddleBlock")&&other.gameObject.CompareTag("Enemy"))
		{
			int random = Random.Range(0, 3);
			if (random == 0)
			{
				other.GetComponent<Enemy>().rotate();
			}
			else if (random == 1)
			{
				other.GetComponent<Enemy>().destroyAndRespawnFromBottom();
			}
			else
			{
				other.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		Debug.Log("Was here");
		if (other.gameObject.CompareTag("Enemy"))
		{
			other.transform.GetChild(0).gameObject.SetActive(true);
		}
		
	}
}
