using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
	public static SceneManagerScript instance;

	public GameObject player;
	public GameObject[] doorArray;
	public Graphic fadeImage;
	public int currentDoorNumber;
	public float fadeTime;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
	}


	void Start()
	{
		if (player == null)
		{
			player = GameObject.FindGameObjectWithTag ("Player");
		}

		if (doorArray.Length == 0)
		{
			doorArray = GameObject.FindGameObjectsWithTag ("Door");
		}
	}


	void LevelWasLoaded( Scene scene, LoadSceneMode mode)
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		doorArray = GameObject.FindGameObjectsWithTag ("Door");

		// This checks everything in doorArray to see if the doorNumber matches the currentDoorNumber
		// If current door number is set to 0 at some point the player would spawn where the prefab was placed in the scene
		for (int i = 0; i < doorArray.Length; i++)
		{
			if (doorArray [i].GetComponent<DoorScript> ().doorNumber == currentDoorNumber)
			{
				if (doorArray [i].transform.position.x < 0)
				{
					player.transform.position = new Vector3 (doorArray [i].transform.position.x + 1f, doorArray [i].transform.position.y, doorArray [i].transform.position.z);
				}
				else if (doorArray [i].transform.position.x > 0)
				{
					player.transform.position = new Vector3 (doorArray [i].transform.position.x - 1f, doorArray [i].transform.position.y, doorArray [i].transform.position.z);
				}
			}
		}
	}


	public void LoadScene (int passedDoorNumber, string doorSide)
	{
		currentDoorNumber = passedDoorNumber;
		GameControllerScript.oldBuildIndex = SceneManager.GetActiveScene ().buildIndex;

		if (doorSide == "Left")
		{
			SceneManager.LoadScene (GameControllerScript.currentBuildIndex - 1);
		}

		if (doorSide == "Right")
		{
			SceneManager.LoadScene (GameControllerScript.currentBuildIndex + 1);
		}
	}


	void OnEnable()
	{
		SceneManager.sceneLoaded += LevelWasLoaded;
	}


	void OnDisable()
	{
		SceneManager.sceneLoaded -= LevelWasLoaded;
	}
}