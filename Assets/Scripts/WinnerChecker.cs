using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerChecker : MonoBehaviour
{


	public Text middleMessage;

	public AI ai;

	public Player player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if ((player.gameObject.active == false && ai.gameObject.active == false) ||
		    (GameObject.FindGameObjectsWithTag("PickUp").Length == 0))
		{
			player.isWin = true;
			int playerCollect = player.collectNumber;
			int aiCollect = ai.collectNumber;
			if (playerCollect > aiCollect)
			{
				middleMessage.text = player.name + " win!!";
			}
			else if (aiCollect > playerCollect)
			{
				middleMessage.text = ai.name + " win!!";
			}
			else
			{
				middleMessage.text = "Draw";
			}
		}
	}
}
