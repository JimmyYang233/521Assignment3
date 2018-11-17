using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour {

	[SerializeField]private Text text;
	
	private bool isWin;
	private int collectNumber;
	private Vector3 destination;
	UnityEngine.AI.NavMeshAgent agent;

	public string name;
	// Use this for initialization
	void Start ()
	{
		
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isWin = false;
		GameObject[] alcoves = GameObject.FindGameObjectsWithTag("Alcove");
		int random = Random.Range(0, 10);
		transform.position = alcoves[random].transform.position;
		findClosestPickUp();
		collectNumber = 0;
		text.text = name + ": " + collectNumber + " collect  Alive";
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!isWin)
		{
			autoMoveToPickUp();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);
			collectNumber ++;
			text.text = name + ": " + collectNumber + " collect  Alive";
			findClosestPickUp();
		}

		else if (other.CompareTag("FieldOfView"))
		{
			Destroy(gameObject);
			text.text = name + ": " + collectNumber + " collect  Dead";
		}
	}

	private void autoMoveToPickUp()
	{
		agent.SetDestination(destination);
	}
	
	public void findClosestPickUp()
	{
		
		GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
		if (pickUps.Length == 0)
		{
			destination = transform.position;
			return;
		}
		
		float shortestDistance = 1000000f;
		GameObject ans = null;
		
		foreach (GameObject pickUp in pickUps)
		{
			//Debug.Log(pickUp.transform.position);
			float x = this.transform.position.x - pickUp.transform.position.x;
			float z = this.transform.position.z - pickUp.transform.position.z;
			float distance = Mathf.Sqrt(x * x + z * z);
			//Debug.Log(pickUp.GetComponent<Rotator>().ID + ", " + distance);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				ans = pickUp;
			}
		}

		destination = ans.transform.position;
	}

}
