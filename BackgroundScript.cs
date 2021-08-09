using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundScript : MonoBehaviour 
{
	public float backgroundXOffset, backgroundYOffset;

	Vector3 topLeft, bottomRight, backgroundTrans;
	GameObject jumpkin, zombie;
	float playerPosX, playerPosY, transX, transY, transZ = 0;

	void Start()
	{
		if (GameControllerScript.currentBuildIndex > GameControllerScript.levelStartBuildIndex)
		{
			topLeft = GameObject.Find ("TopLeft").transform.position;
			bottomRight = GameObject.Find ("BottomRight").transform.position;
		}
	}


	void LateUpdate()
	{
		if (GameControllerScript.currentBuildIndex > GameControllerScript.levelStartBuildIndex)
		{
			playerPosX = GameObject.FindWithTag ("Player").transform.position.x;
			playerPosY = GameObject.FindWithTag ("Player").transform.position.y;

			if (playerPosX < (topLeft.x + backgroundXOffset))
			{
				transX = (topLeft.x + backgroundXOffset);
			}
			else if (playerPosX > (bottomRight.x - backgroundXOffset))
			{
				transX = (bottomRight.x - backgroundXOffset);
			}
			else
			{
				transX = playerPosX;
			}

			backgroundTrans = new Vector3 (transX, CameraScript.instance.transform.position.y, transZ);
			transform.position = backgroundTrans;
		}
	}
}