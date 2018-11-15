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
	
	private GameObject camera;
	private float xAxisClamp;
	private bool isJumping;
	private bool isWin;
	private CharacterController charController;

	public bool moveType;
	// Use this for initialization
	void Start ()
	{
		camera = transform.GetChild(0).gameObject;
		charController = gameObject.GetComponent<CharacterController>();
		xAxisClamp = 0;
		isJumping = false;
		isWin = false;
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
