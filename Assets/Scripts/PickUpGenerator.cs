using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class PickUpGenerator : MonoBehaviour
{

	public GameObject pickUp;
	private List<Vector3> positions;

	// Use this for initialization
	void Start ()
	{
		positions = new List<Vector3>();
		positions.Add(new Vector3(0.3f, 0.2f, 0.3f));
		positions.Add(new Vector3(0.3f, 0.2f, -0.3f));
		positions.Add(new Vector3(0.3f, 0.2f, 0f));
		positions.Add(new Vector3(0.0f, 0.2f, 0.3f));
		positions.Add(new Vector3(0.0f, 0.2f, -0.3f));
		positions.Add(new Vector3(0.0f, 0.2f, 0f));
		positions.Add(new Vector3(-0.3f, 0.2f, 0.3f));
		positions.Add(new Vector3(-0.3f, 0.2f, -0.3f));
		positions.Add(new Vector3(-0.3f, 0.2f, 0f));

		int random = Random.Range(0, 9);
		GameObject newPickUp = GameObject.Instantiate(pickUp, this.transform);
		newPickUp.transform.localPosition = positions[random];

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
