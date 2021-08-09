using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour 
{
	float zRotation = 20f;

	void Update()
	{
		if (name == "EndGamePortal")
		{
			transform.Rotate (new Vector3 (0, 0, -zRotation * 10 * Time.deltaTime));
		}
		else
		{
			transform.Rotate (new Vector3 (0, 0, zRotation * Time.deltaTime));
		}
	}
}