using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
	public static CameraScript instance;

	public AudioSource audioSource;
	public GameObject player;
	float xOffset, yOffset;
	public Vector3 topLeft, bottomRight, cameraTrans;
	public AudioClip mainMusic, scaryMusic;

	float playerPosX, playerPosY, transX, transY, transZ = -10;

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null && instance != this)
		{
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}


	void OnEnable()
	{
		SceneManager.sceneLoaded += FindEverything;
	}


	void OnDisable()
	{
		SceneManager.sceneLoaded -= FindEverything;
	}


	void FindEverything(Scene scene, LoadSceneMode mode)
	{
		if (SceneManager.GetActiveScene ().buildIndex > GameControllerScript.levelStartBuildIndex)
		{
			Debug.Log ("Find everything started");
			topLeft = GameObject.Find ("TopLeft").transform.position;
			bottomRight = GameObject.Find ("BottomRight").transform.position;
			player = GameObject.FindGameObjectWithTag ("Player");
			Debug.Log (player);

			if (GameControllerScript.currentBuildIndex > GameControllerScript.oldBuildIndex)
			{
				transform.position = new Vector3 (topLeft.x + xOffset, bottomRight.y + yOffset, transZ);
			}
			else if (GameControllerScript.currentBuildIndex < GameControllerScript.oldBuildIndex)
			{
				transform.position = new Vector3 (bottomRight.x - xOffset, bottomRight.y + yOffset, transZ);
			}

			Debug.Log ("Find everything finished");
		}
	}


	void Update()
	{
		if (SceneManager.GetActiveScene ().name == "OptionsMenu")
		{
			Text mobileControlsText = GameObject.Find ("MobileControlsText").GetComponent<Text> ();

			if (PlayerPrefs.GetString ("MobileControls") == "True")
			{
				mobileControlsText.text = "Mobile Controls: On";
			}
			else
			{
				mobileControlsText.text = "Mobile Controls: Off";
			}
		}

		if (SceneManager.GetActiveScene ().name == "CharacterSelectMenu")
		{
			Text characterText = GameObject.Find ("CharacterText").GetComponent<Text> (), candyText = GameObject.Find ("CandyText").GetComponent<Text> ();

			characterText.text = PlayerPrefs.GetString ("Character");
			candyText.text = "Candy: " + PlayerPrefs.GetInt ("Candy").ToString();
		}
	}

	void FixedUpdate()
	{
		if (GameControllerScript.currentBuildIndex > GameControllerScript.levelStartBuildIndex)
		{
			playerPosX = player.transform.position.x;
			playerPosY = player.transform.position.y;

			if (playerPosX < (topLeft.x + xOffset * 1.5))
			{
				transX = (topLeft.x + xOffset);
			}
			else if (playerPosX > (bottomRight.x - xOffset * 1.5f))
			{
				transX = (bottomRight.x - xOffset);
			}
			else if (player.GetComponent<Rigidbody2D> ().velocity.x < 0)
			{
				transX = playerPosX - xOffset * .5f;
			}
			else if (player.GetComponent<Rigidbody2D> ().velocity.x > 0)
			{
				transX = playerPosX + xOffset * .5f;
			}
			else
			{
				transX = playerPosX;
			}

			if (playerPosY < (bottomRight.y + yOffset))
			{
				transY = (bottomRight.y + yOffset);
			}
			else if (playerPosY > (topLeft.y - yOffset))
			{
				transY = (topLeft.y - yOffset);
			}
			else
			{
				transY = playerPosY;
			}

			cameraTrans = new Vector3 (transX, transY, transZ);
			transform.position = Vector3.MoveTowards (transform.position, cameraTrans, .1f);
		}
	}


	public void EnterPortraitMode()
	{
		xOffset = 4;
		yOffset = 6;
		Camera.main.orthographicSize = 6;
	}


	public void EnterLandscapeMode()
	{
		xOffset = 8;
		yOffset = 5.5f;
		Camera.main.orthographicSize = 4;
	}


	public void PlaySoundEffect(AudioClip sound)
	{
		audioSource.PlayOneShot (sound);
	}


	public void PlayMusic(AudioClip music)
	{
		audioSource.clip = music;
		audioSource.Play ();
	}
}