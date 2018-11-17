using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour, Agent {

	[SerializeField]private Text text;
	
	private bool isWin;
	public int collectNumber;
	private Vector3 destination;
	UnityEngine.AI.NavMeshAgent agent;

	public string name;
	public int numOfTeleportTrap;
	// Use this for initialization
	void Start ()
	{
		
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isWin = false;
		GameObject[] alcoves = GameObject.FindGameObjectsWithTag("Alcove");
		int random = Random.Range(0, 10);
		transform.position = alcoves[random].transform.position;
		collectNumber = 0;
		text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Alive";
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!isWin)
		{
			autoMoveToPickUp();
			findClosestPickUp();
		}
	}
	
	public void moveToRandomAlcove()
	{
		GameObject[] alcoves = GameObject.FindGameObjectsWithTag("Alcove");
		int random = Random.Range(0, 10);
		transform.position = alcoves[random].transform.position;
	}

	public void UseTeleportTrap()
	{
		if (numOfTeleportTrap <= 0)
		{
			//TO-DO
		}

		else
		{
			GameObject theObject = findClosestObject();
			if (theObject.CompareTag("Player"))
			{
				theObject.GetComponent<Player>().moveToRandomAlcove();
			}
			else if (theObject.CompareTag("Enemy"))
			{
				theObject.GetComponent<Enemy>().destroyAndRespawn();
			}
			numOfTeleportTrap--;
			text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Alive";
		}
	}

	public GameObject findClosestObject()
	{
		GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject agent = GameObject.FindGameObjectWithTag("Player");
		HashSet<GameObject> objects = new HashSet<GameObject>(npcObjects);
		if (agent != null)
		{
			objects.Add(agent);
		}
		
		float shortestDistance = 1000000f;
		GameObject ans = null;

		foreach (GameObject stuff in objects)
		{
			//Debug.Log(pickUp.transform.position);
			float x = stuff.transform.position.x - this.transform.position.x;
			float z = stuff.transform.position.z - this.transform.position.z;
			float distance = Mathf.Sqrt(x * x + z * z);
			//Debug.Log(pickUp.GetComponent<Rotator>().ID + ", " + distance);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				ans = stuff;
			}
		}

		return ans;
	}

	

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);
			collectNumber ++;
			text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Alive";
		}

		else if (other.CompareTag("FieldOfView"))
		{
			gameObject.SetActive(false);
			text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Dead";
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
