using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements;
using UnityEngine;

public class PickUpGenerator : MonoBehaviour
{

	public GameObject pickUp;
	public int id;
	private List<Vector3> positions;

	private void Awake()
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
		newPickUp.GetComponent<Rotator>().ID = id;
		newPickUp.transform.localPosition = positions[random];

	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
