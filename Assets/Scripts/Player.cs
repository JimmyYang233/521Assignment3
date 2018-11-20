using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;
using UnityEngineInternal.Input;

public class Player : MonoBehaviour, Agent
{

	[SerializeField]private float speed;
	[SerializeField]private float mouseSensitivity;
	[SerializeField]private CameraSwitch cameraSwitch;
	[SerializeField]private Text text;
	[SerializeField]private Text middleMessage;
	
	private GameObject camera;
	private float xAxisClamp;
	public bool isWin;
	private CharacterController charController;
	public int collectNumber;
	public bool moveType;
	public int numOfTeleportTrap;
	public string name;

	public GameObject aiPrefab;
	// Use this for initialization
	void Start ()
	{
		camera = transform.GetChild(0).gameObject;
		charController = gameObject.GetComponent<CharacterController>();
		xAxisClamp = 0;
		isWin = false;
		moveType = false;
		moveToRandomAlcove();
		collectNumber = 0;
		text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Alive";
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!isWin)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				UseTeleportTrap();
			}
			
			if (moveType)
			{
				
				firstPersonControl();
			}
			else
			{
				thirdPersonControl();
			}
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
			middleMessage.text = "You don't have any Teleport trap anymore!";
		}

		else
		{
			GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("Enemy");
			GameObject agent = GameObject.FindGameObjectWithTag("AI");
			HashSet<GameObject> objects = new HashSet<GameObject>(npcObjects);
			if (agent != null)
			{
				objects.Add(agent);
			}

			GameObject theObject = findClosestObject(objects);
			if (theObject.CompareTag("AI"))
			{
				AI ai = theObject.GetComponent<AI>();
				Debug.Log(ai.numOfTeleportTrap + ", " + ai.collectNumber);
				GameObject newAI = Instantiate(aiPrefab);
				newAI.GetComponent<AI>().text = ai.text;
				newAI.GetComponent<AI>().message = ai.message;
				newAI.GetComponent<AI>().numOfTeleportTrap = ai.numOfTeleportTrap;
				newAI.GetComponent<AI>().name = ai.name;
				newAI.GetComponent<AI>().collectNumber = ai.collectNumber;
				Destroy(theObject);
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
			cameraSwitch.playerDead();
			gameObject.SetActive(false);
			text.text = name + ": " + collectNumber + " collect, " + numOfTeleportTrap +" teleports, Dead";
		}
	}

	private void CameraRotation()
	{
		float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity;
		float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity;

		xAxisClamp += mouseY;

		if (xAxisClamp > 90.0f)
		{
			xAxisClamp = 90.0f;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(270.0f);
		}
		
		else if (xAxisClamp < -90.0f)
		{
			xAxisClamp = -90.0f;
			mouseY = 0.0f;
			ClampXAxisRotationToValue(90.0f);
		}
		
		camera.transform.Rotate(Vector3.left*mouseY);
		transform.Rotate(Vector3.up*mouseX);
		
		//Debug.Log(mouseX + ", " + mouseY);
	}

	private void CameraMovement()
	{
		float xAxis = Input.GetAxis("Horizontal");
		float zAxis = Input.GetAxis("Vertical");

		Vector3 newPosition = transform.forward * zAxis * speed+ transform.right*xAxis*speed;

		charController.SimpleMove(newPosition);
	}

	private void ClampXAxisRotationToValue(float value)
	{
		Vector3 eulerRotation = camera.transform.eulerAngles;
		eulerRotation.x = value;
		camera.transform.eulerAngles = eulerRotation;
	}

	private void firstPersonControl()
	{
		CameraMovement();
		CameraRotation();
	}

	private void thirdPersonControl()
	{
		//float xAxis = Input.GetAxis("Horizontal");
		//float zAxis = Input.GetAxis("Vertical");
		//Vector3 newPosition = Vector3.forward* zAxis * speed+ Vector3.right*xAxis*speed;
		Vector3 newPosition = new Vector3();
		if (Input.GetKey(KeyCode.W))
		{
			newPosition += Vector3.forward * speed;
		}

		if (Input.GetKey(KeyCode.S))
		{
			newPosition += Vector3.back * speed;
		}
		
		if (Input.GetKey(KeyCode.A))
		{
			newPosition += Vector3.left * speed;
		}
		if (Input.GetKey(KeyCode.D))
		{
			newPosition += Vector3.right * speed;
		}

		charController.SimpleMove(newPosition);
	}

}
