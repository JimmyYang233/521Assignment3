using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AI : MonoBehaviour, Agent {

	public Text text;
	public Text message;
	private Vector3 initialPosition;
	
	public bool isWin;
	public int collectNumber;
	private Vector3 destination;
	UnityEngine.AI.NavMeshAgent agent;

	public string name;
	public int numOfTeleportTrap;
	// Use this for initialization
	void Awake ()
	{
		initialPosition = transform.position;
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		isWin = false;
		moveToRandomAlcove();
		numOfTeleportTrap = 2;
		collectNumber = 0;
		
	}

	void Start()
	{
		text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Alive";
	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!isWin)
		{
			autoMove();
			AInextDecision();
		}
		else
		{
			destination = transform.position;
			autoMove();
		}
		
		//Test
		//GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		//GameObject closestEnemy = findClosestObject(new HashSet<GameObject>(enemies));
		//GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
		//GameObject closestPickUp = findClosestObject(new HashSet<GameObject>(pickUps));
		//Debug.Log(isTooClose(closestEnemy));
		//Debug.Log(isTowardMe(closestEnemy));
		//Debug.Log(hasPickUpInAlcove());
		//Debug.Log(cannotTakeThatPickUp(closestPickUp, closestEnemy));
	}
	
	public void moveToRandomAlcove()
	{
		GameObject[] alcoves = GameObject.FindGameObjectsWithTag("Alcove");
		int random = Random.Range(0, 10);
		transform.position = initialPosition;
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
			message.text = name + " uses teleport trap number " + (2-numOfTeleportTrap) + ". ";
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
		String message = "";
		GameObject enemy = getClosestEnemy();
		if (playerCloseToMe()&&numOfTeleportTrap>0)
		{
			UseTeleportTrap();
		}
		if (inSafeSpot())
		{
			message += "In safe spot, ";
			if (hasPickUpInAlcove())
			{
				message += "has pickup in alcove, ";
				moveToNextPickUp();
			}
			else if (isTooClose(enemy))
			{
				message += "too close to enemy, ";
				wait();
			}
			else
			{
				message += "moving out, ";
				if (noMorePickUpsOnMySide())
				{
					message += "no more pick ups on my side";
					moveToClosestMiddlePoint();
				}
				else
				{
					message += "go to next pickUp";
					moveToNextPickUp();
				}
			}
		}
		else
		{
			message += "Not in safe spot, ";
			if (isWayTooClose(enemy))
			{
				message += "enemy way too close to me, ";
				UseTeleportTrap();
			}
			else if (isTooClose(enemy))
			{
				message += "enemy too close to me, ";
				moveToClosestAlcove(enemy);				
			}
			else
			{
				message += "no enemy close to me, ";
				if (noMorePickUpsOnMySide())
				{
					message += "no more pick ups on my side";
					moveToClosestMiddlePoint();

				}
				else
				{
					message += "go to next pickUp";
					moveToNextPickUp();
				}
			}
		}
		//Debug.Log(message);
	}

	void moveToClosestMiddlePoint()
	{
		GameObject middlePoint = findClosestMiddlePoint();
		Vector3 position;
		if (isOnTop())
		{
			position = new Vector3(middlePoint.transform.position.x, middlePoint.transform.position.y, transform.position.z-0.8f);
		}
		else
		{
			position = new Vector3(middlePoint.transform.position.x, middlePoint.transform.position.y, transform.position.z+0.8f);
		}

		destination = position;
	}
	GameObject findClosestMiddlePoint()
	{
		GameObject[] middlePoints = GameObject.FindGameObjectsWithTag("MiddlePoint");
		return findClosestObject(new HashSet<GameObject>(middlePoints));
	}

	bool noMorePickUpsOnMySide()
	{
		HashSet<GameObject> pickUps = isOnTop() ? getPickUpsAtTop() : getPickUpsAtBottom();
		if (pickUps.Count <= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void moveToClosestAlcove(GameObject enemy)
	{
		GameObject[] alcoves = GameObject.FindGameObjectsWithTag("Alcove");
		GameObject[] middlePoints = GameObject.FindGameObjectsWithTag("MiddlePoint");
		HashSet<GameObject> theAlcoves = new HashSet<GameObject>(alcoves);
		theAlcoves.Add(middlePoints[0]);
		theAlcoves.Add(middlePoints[1]);
		GameObject alcove1 = findClosestObject(theAlcoves);
		theAlcoves.Remove(alcove1);
		GameObject alcove2 = findClosestObject(theAlcoves);
		if (cannotTakeThat(alcove1, enemy))
		{
			destination = alcove2.transform.position;
		}
		else
		{
			destination = alcove1.transform.position;
		}
			
	}

	bool isWayTooClose(GameObject enemy)
	{
		if (isTowardMe(enemy) && Math.Abs(enemy.transform.position.x - transform.position.x) <= 8.2f)
		{
			return true;
		}
		else if (!isTowardMe(enemy)&&Math.Abs(enemy.transform.position.x - transform.position.x) <= 2f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	//tested
	bool isTooClose(GameObject enemy)
	{
		if (isTowardMe(enemy) && Math.Abs(enemy.transform.position.x - transform.position.x) <= 14f)
		{
			return true;
		}
		else if (!isTowardMe(enemy)&&Math.Abs(enemy.transform.position.x - transform.position.x) <= 6f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//tested
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

	//tested
	bool inSafeSpot()
	{
		if (transform.position.z >= 16.25f || transform.position.z <= -16.25f)
		{
			return true;
		}
		
		else if (transform.position.z <= 1.5f && transform.position.z >= -1.5f)
		{
			return true;
		}

		return false;
	}

	//tested
	bool hasPickUpInAlcove()
	{
		HashSet<GameObject> pickUps = isOnTop() ? getPickUpsAtTop() : getPickUpsAtBottom();
		GameObject closestOne = findClosestObject(pickUps);
		if (closestOne!=null&&Math.Abs(closestOne.transform.position.x - this.transform.position.x) <= 5f)
		{
			return true;
		}

		return false;
	}

	//tested
	bool cannotTakeThat(GameObject pickUp, GameObject enemy)
	{
		if (isTowardMe(enemy))
		{
			float pickUpAiDistance = transform.position.x - pickUp.transform.position.x;
			float enemyAiDistance = transform.position.x - enemy.transform.position.x;
			if (pickUpAiDistance>0f&&enemyAiDistance>0f)
			{
				return true;
			}
			else if (pickUpAiDistance < 0f && enemyAiDistance < 0f)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		return false;
	}

	bool isOnTop()
	{
		if (transform.position.z > 0f)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	void wait()
	{
		destination = transform.position; 
	}
	
	void moveToNextPickUp()
	{
		HashSet<GameObject> pickUps = isOnTop() ? getPickUpsAtTop() : getPickUpsAtBottom();
		if (pickUps.Count == 0)
		{
			destination = transform.position;
		}
		else
		{
			GameObject pickUp = findClosestObject(pickUps);
			destination = pickUp.transform.position;
		}
	}

	HashSet<GameObject> getPickUpsAtTop()
	{
		HashSet<GameObject> ans = new HashSet<GameObject>(); 
		GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
		foreach(GameObject pickUp in pickUps)
		{
			if (pickUp.transform.position.z > 0)
			{
				ans.Add(pickUp);
			}
		}

		return ans;
	}
	
	HashSet<GameObject> getPickUpsAtBottom()
	{
		HashSet<GameObject> ans = new HashSet<GameObject>(); 
		GameObject[] pickUps = GameObject.FindGameObjectsWithTag("PickUp");
		foreach(GameObject pickUp in pickUps)
		{
			if (pickUp.transform.position.z < 0)
			{
				ans.Add(pickUp);
			}
		}

		return ans;
	}

	GameObject getClosestEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
		{
			if (isOnTop() && enemy.transform.position.z > 0)
			{
				return enemy;
			}
			else if (!isOnTop() && enemy.transform.position.z < 0)
			{
				return enemy;
			}
		}

		return null;
	}

	bool playerCloseToMe()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		if (player == null)
		{
			return false;
		}
		float x = player.transform.position.x - transform.position.x;
		float z = player.transform.position.z - transform.position.z;
		float distance = (float)Math.Sqrt(x * x + z * z);
		if (distance <= 4)
		{
			return true;
		}

		return false;
	}

}
