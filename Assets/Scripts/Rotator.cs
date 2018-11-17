using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rotator : MonoBehaviour
{

	private float randomX;
	private float randomY;
	private float randomZ;

	public int ID;

	void Start()
	{
		randomX = Random.Range(-2f, 2f);
		randomY = Random.Range(-2f, 2f);
		randomZ = Random.Range(-2f, 2f);
	}

	
	
	// Update is called once per frame
	void Update ()
	{
		
		transform.Rotate(new Vector3(45*randomX, 45*randomY, 45*randomZ) * Time.deltaTime);
	}
}
