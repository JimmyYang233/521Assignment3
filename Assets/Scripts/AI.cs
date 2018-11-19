using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
		//moveToRandomAlcove();
		collectNumber = 0;
		text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Alive";
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{/**
		if (!isWin)
		{
			autoMove();
			AInextDecision();
		}
		**/
		//Test
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject closestEnemy = findClosestObject(new HashSet<GameObject>(enemies));
		//Debug.Log(isTooClose(closestEnemy));
		Debug.Log(isTowardMe(closestEnemy));
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
			GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("Enemy");
			GameObject agent = GameObject.FindGameObjectWithTag("Player");
			HashSet<GameObject> objects = new HashSet<GameObject>(npcObjects);
			if (agent != null)
			{
				objects.Add(agent);
			}
			GameObject theObject = findClosestObject(objects);
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

	public GameObject findClosestObject(HashSet<GameObject> objects)
	{
		
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

	private void autoMove()
	{
		agent.SetDestination(destination);
	}
	

	void AInextDecision()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		GameObject enemy = findClosestObject(new HashSet<GameObject>(enemies));
		if (inAlcove())
		{
			
			if (isTooClose(enemy)&&!hasPickUpInAlcove())
			{
				wait();
			}
			else
			{
				GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
				findbestPickUp(new HashSet<GameObject>(pickUps), enemy);
			}
		}
		else
		{
			/**
			GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
			findbestPickUp(new HashSet<GameObject>(pickUps), enemy);
			**/
		}
		
	}
	//tested
	bool isTooClose(GameObject enemy)
	{
		if (Math.Abs(enemy.transform.position.x - transform.position.x) <= 10)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool isTowardMe(GameObject enemy)
	{
		if (((enemy.transform.position.x - transform.position.x) > -1)&&(enemy.transform.rotation.w < 0f))
		{
			return true;
		}
		else if (((enemy.transform.position.x - transform.position.x) < 1) && (enemy.transform.rotation.w > 0f))
		{
			return true;
		}

		return false;
	}

	bool inAlcove()
	{
		if (transform.position.z >= 16.4 || transform.position.z <= -16.4)
		{
			return true;
		}

		return false;
	}

	bool hasPickUpInAlcove()
	{
		GameObject[] alcoves = GameObject.FindGameObjectsWithTag("PickUp");
		GameObject closestOne = findClosestObject(new HashSet<GameObject>(alcoves));
		if (Math.Abs(closestOne.transform.position.x - this.transform.position.x) <= 5)
		{
			return true;
		}

		return false;
	}

	void wait()
	{
		destination = transform.position; 
	}
	
	void moveToNextPickUp()
	{
		GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
		if (pickUps.Length == 0)
		{
			destination = transform.position;
		}
		else
		{
			GameObject pickUp = findClosestObject(new HashSet<GameObject>(pickUps));
			destination = pickUp.transform.position;
		}
	}

	GameObject findbestPickUp(HashSet<GameObject> pickUps, GameObject enemy)
	{
		GameObject pickUp = findClosestObject(new HashSet<GameObject>(pickUps));
		float pickUpEnemyDistance = Math.Abs(pickUp.transform.position.x - enemy.transform.position.x);
		float aiEnemyDistance = Math.Abs(transform.position.x - enemy.transform.position.x);
		if ((pickUpEnemyDistance < aiEnemyDistance)&&(isTowardMe(enemy)))
		{
			pickUps.Remove(pickUp);
			pickUp =  findbestPickUp(pickUps, enemy);
		}
		return pickUp;		
	}

}
