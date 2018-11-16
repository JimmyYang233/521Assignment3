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
	private bool switchable;
	
	// Use this for initialization
	void Start ()
	{
		view = false;
		switchable = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.V))
		{
			if (switchable)
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
	}

	public void playerDead()
	{
		switchToMainCamera();
		switchable = false;
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
