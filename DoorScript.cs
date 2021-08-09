using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
	public int doorNumber;

	void OnTriggerEnter2D( Collider2D col)
	{
		if (col.tag == "Player")
		{
			if(SceneManager.GetActiveScene().name == "Level01")
			{
				if (col.transform.position.x < 0)
				{
					SceneManagerScript.instance.LoadScene (doorNumber, "Left");
				}
				else if (col.transform.position.x > 0)
				{
					SceneManagerScript.instance.LoadScene (doorNumber, "Right");
				}
			}
			else if(SceneManager.GetActiveScene().name == "Level02")
			{
				if (GameControllerScript.instance.enemyArray.Length == 0 || GameControllerScript.instance.level02Cleared)
				{
					if (col.transform.position.x < 0)
					{
						SceneManagerScript.instance.LoadScene (doorNumber, "Left");
					}
					else if (col.transform.position.x > 0)
					{
						SceneManagerScript.instance.LoadScene (doorNumber, "Right");
					}
				}
			}
			else if(SceneManager.GetActiveScene().name == "Level03")
			{
				if (GameControllerScript.instance.enemyArray.Length == 0 || GameControllerScript.instance.level03Cleared)
				{
					if (col.transform.position.x < 0)
					{
						SceneManagerScript.instance.LoadScene (doorNumber, "Left");
					}
					else if (col.transform.position.x > 0)
					{
						SceneManagerScript.instance.LoadScene (doorNumber, "Right");
					}
				}
			}
			else
			{
				StartCoroutine(PlayerScript.instance.EnemyAliveText ());
			}
		}
	}
}