using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentControl : MonoBehaviour
{

	public GameObject playerPrefab;
	public GameObject aiPrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void teleportPlayer()
	{
		GameObject theObject = getPlayer();
		Player player = theObject.GetComponent<Player>();
		Debug.Log(player.numOfTeleportTrap + ", " + player.collectNumber);
		GameObject newPlayer = Instantiate(playerPrefab, transform);
		newPlayer.GetComponent<Player>().text = player.text;
		newPlayer.GetComponent<Player>().middleMessage = player.middleMessage;
		newPlayer.GetComponent<Player>().numOfTeleportTrap = player.numOfTeleportTrap;
		newPlayer.GetComponent<Player>().name = player.name;
		newPlayer.GetComponent<Player>().collectNumber = player.collectNumber;
		newPlayer.GetComponent<Player>().cameraSwitch = player.cameraSwitch;
		Destroy(theObject);
	}

	public void teleportAI()
	{
		GameObject theObject = getAI();
		AI ai = theObject.GetComponent<AI>();
		Debug.Log(ai.numOfTeleportTrap + ", " + ai.collectNumber);
		GameObject newAI = Instantiate(aiPrefab, transform);
		newAI.GetComponent<AI>().text = ai.text;
		newAI.GetComponent<AI>().message = ai.message;
		newAI.GetComponent<AI>().numOfTeleportTrap = ai.numOfTeleportTrap;
		newAI.GetComponent<AI>().name = ai.name;
		newAI.GetComponent<AI>().collectNumber = ai.collectNumber;
		Destroy(theObject);
	}

	private GameObject getPlayer()
	{
		GameObject child1 = transform.GetChild(0).gameObject;
		if (child1.CompareTag("Player"))
		{
			return child1;
		}
		else
		{
			return transform.GetChild(1).gameObject;
		}
	}

	private GameObject getAI()
	{
		GameObject child1 = transform.GetChild(0).gameObject;
		if (child1.CompareTag("AI"))
		{
			return child1;
		}
		else
		{
			return transform.GetChild(1).gameObject;
		}
	}
}
