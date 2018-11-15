using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{


	public Camera playerCamera;

	public Camera mainCamera;
	
	public Player player;

	private Boolean view;
	
	// Use this for initialization
	void Start ()
	{
		view = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (view)
			{
				switchToMainCamera();
			}
			else
			{
				switchToPlayerCamera();
			}

			view = !view;
		}
	}

	public void switchToMainCamera()
	{
		mainCamera.enabled = true;
		playerCamera.enabled = false;
		player.moveType = false;
	}

	public void switchToPlayerCamera()
	{
		playerCamera.enabled = true;
		mainCamera.enabled = false;
		player.moveType = true;
	}
}
