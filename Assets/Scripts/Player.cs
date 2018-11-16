using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.UI;
using UnityEngineInternal.Input;

public class Player : MonoBehaviour
{

	[SerializeField]private float speed;
	[SerializeField]private float mouseSensitivity;
	[SerializeField]private CameraSwitch cameraSwitch;
	[SerializeField]private Text text;
	
	private GameObject camera;
	private float xAxisClamp;
	private bool isJumping;
	private bool isWin;
	private CharacterController charController;
	private int collectNumber;

	public bool moveType;

	public string name;
	// Use this for initialization
	void Start ()
	{
		camera = transform.GetChild(0).gameObject;
		charController = gameObject.GetComponent<CharacterController>();
		xAxisClamp = 0;
		isJumping = false;
		isWin = false;
		GameObject[] alcoves = GameObject.FindGameObjectsWithTag("Alcove");
		int random = Random.Range(0, 10);
		transform.position = alcoves[random].transform.position;
		collectNumber = 0;
		text.text = name + ": " + collectNumber + " collect  Alive";
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (!isWin)
		{
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);
			collectNumber ++;
			text.text = name + ": " + collectNumber + " collect  Alive";
		}

		else if (other.CompareTag("FieldOfView"))
		{
			cameraSwitch.playerDead();
			Destroy(gameObject);
			text.text = name + ": " + collectNumber + " collect  Dead";
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
		float xAxis = Input.GetAxis("Horizontal");
		float zAxis = Input.GetAxis("Vertical");
		Vector3 newPosition = Vector3.forward* zAxis * speed+ Vector3.right*xAxis*speed;
		charController.SimpleMove(newPosition);
	}


}
