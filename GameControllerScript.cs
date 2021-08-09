using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameControllerScript : MonoBehaviour
{
	public static GameControllerScript instance;

	public Canvas mainMenuPortraitCanvas, mainMenuLandscapeCanvas, gamePortraitCanvas, gameLandscapeCanvas, endGamePortraitCanvas, endGameLandscapeCanvas;
	public GUISkin guiSkin;
	public GameObject bg, joystick, leftAttack, rightAttack, jumpButton, endGamePortal, limboBoss, jumpkin, zombie;
	public GameObject[] enemyArray, playerArray;
	public Text candyText, orientationText;
	public Image fadeImage, healthImage, pauseMenuPanel, deathContinuePanel;
	public Sprite heart1, heart2, heart3;
	public Vector2 bgOffset, movingPlatformVelocity;
	public AudioSource audioSource;
	public string oldOrientation;
	public float fadeTime = .01f;
	public static int lives = 3, oldLives, candy, levelStartBuildIndex = 3, currentBuildIndex, oldBuildIndex;
	public bool vineUnlocked = false, doubleJumpUnlocked = false;
	public bool speech01Triggered = false, speech02Triggered = false, speech03Triggered = false, speech04Triggered = false, speech05Triggered;
	public bool level02Cleared = false, level03Cleared = false;

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

		if (SceneManager.GetActiveScene ().name == "MainMenu")
		{
			PlayerPrefs.SetInt ("Lives", 3);
		}

		if(PlayerPrefs.GetString ("Character") == "")
		{
			PlayerPrefs.SetString ("Character", "Jumpkin");
		}

		if(PlayerPrefs.GetString ("ZombieUnlocked") == "")
		{
			PlayerPrefs.SetString ("ZombieUnlocked", "False");
		}

		if(PlayerPrefs.GetString ("MobileControls") == "")
		{
			PlayerPrefs.SetString ("MobileControls", "True");
		}

		#if UNITY_ANDROID
		PlayerPrefs.SetString ("MobileControls", "True");
		#else
		PlayerPrefs.SetString ("MobileControls", "False");
		#endif

		if (PlayerPrefs.GetString ("Orientation") == "")
		{
			PlayerPrefs.SetString ("Orientation", "Landscape");
		}
	}


	void OnEnable()
	{
		SceneManager.sceneLoaded += FindEverything;
	}


	void OnDisable()
	{
		SceneManager.sceneLoaded -= FindEverything;
	}


	void Update()
	{
		if (PlayerPrefs.GetInt ("Candy") < 0)
		{
			PlayerPrefs.SetInt ("Candy", 0);
		}

		if (SceneManager.GetActiveScene ().name == "MainMenu")
		{
			CheckForOrientationChange ();

			orientationText.text = PlayerPrefs.GetString ("Orientation");
		}			

		if (currentBuildIndex > levelStartBuildIndex)
		{
			MobileControls ();

			enemyArray = GameObject.FindGameObjectsWithTag ("Enemy");

			if (PlayerPrefs.GetInt ("Lives") == 1)
			{
				healthImage.sprite = heart1;
			}
			else if (PlayerPrefs.GetInt ("Lives") == 2)
			{
				healthImage.sprite = heart2;
			}
			else if (PlayerPrefs.GetInt ("Lives") == 3)
			{
				healthImage.sprite = heart3;
			}
			else
			{
				//if lives == 0 pause game and show deathContinuePanel
				deathContinuePanel.gameObject.SetActive (true);
				Time.timeScale = 0;
			}

			if (SceneManager.GetActiveScene ().name == "Level02" && enemyArray.Length == 0)
			{
				level02Cleared = true;
			}

			if (SceneManager.GetActiveScene ().name == "Level03" && enemyArray.Length == 0)
			{
				level03Cleared = true;
			}
		}
	}


	void LateUpdate()
	{
		// This sets candyText and OldLives at the end of each frame
		if (currentBuildIndex > levelStartBuildIndex)
		{
			candyText.text = "Candy: " + PlayerPrefs.GetInt ("Candy");

			PlayerPrefs.SetInt ("OldLives", PlayerPrefs.GetInt("Lives"));
		}
			
		// This sets oldOrientation to the current orientation
		oldOrientation = PlayerPrefs.GetString ("Orientation");
	}


	void FindEverything(Scene scene, LoadSceneMode mode)
	{
		currentBuildIndex = scene.buildIndex;

		if (scene.name == "MainMenu")
		{
			mainMenuPortraitCanvas = GameObject.Find ("MainMenuPortraitCanvas").GetComponent<Canvas> ();
			mainMenuLandscapeCanvas = GameObject.Find ("MainMenuLandscapeCanvas").GetComponent<Canvas> ();

			SceneManagerScript.instance.currentDoorNumber = 0;

			if (PlayerPrefs.GetString ("Orientation") == "Landscape")
			{
				EnterLandscapeMode ();
			}

			if (PlayerPrefs.GetString ("Orientation") == "Portrait")
			{
				EnterPortraitMode ();
			}
		}

		if (scene.buildIndex > levelStartBuildIndex)
		{
			bg = GameObject.Find ("BG");
			gamePortraitCanvas = GameObject.Find ("GamePortraitCanvas").GetComponent<Canvas> ();
			gameLandscapeCanvas = GameObject.Find ("GameLandscapeCanvas").GetComponent<Canvas> ();
			enemyArray = GameObject.FindGameObjectsWithTag ("Enemy");

			if (SceneManager.GetActiveScene ().name == "Level01")
			{
				limboBoss = GameObject.Find ("LimboBoss");
				limboBoss.SetActive (false);
				endGamePortal = GameObject.Find ("EndGamePortal");
				endGamePortal.SetActive (false);
			}

			if (PlayerPrefs.GetString ("Orientation") == "Landscape")
			{
				EnterLandscapeMode ();
			}

			if (PlayerPrefs.GetString ("Orientation") == "Portrait")
			{
				EnterPortraitMode ();
			}
		}

		if (scene.name == "EndGameScreen")
		{
			endGamePortraitCanvas = GameObject.Find ("EndGamePortraitCanvas").GetComponent<Canvas> ();
			endGameLandscapeCanvas = GameObject.Find ("EndGameLandscapeCanvas").GetComponent<Canvas> ();

			if (PlayerPrefs.GetString ("Orientation") == "Landscape")
			{
				EnterLandscapeMode ();
			}

			if (PlayerPrefs.GetString ("Orientation") == "Portrait")
			{
				EnterPortraitMode ();
			}
		}
	}


	void EnterPortraitMode()
	{
		Screen.orientation = ScreenOrientation.Portrait;

		if (SceneManager.GetActiveScene ().name == "MainMenu")
		{
			mainMenuPortraitCanvas.gameObject.SetActive (true);
			mainMenuLandscapeCanvas.gameObject.SetActive (false);

			orientationText = GameObject.Find ("PortraitOrientationText").GetComponent<Text> ();
			orientationText.text = PlayerPrefs.GetString ("Orientation");

			CameraScript.instance.EnterPortraitMode ();
		}

		if (currentBuildIndex > levelStartBuildIndex)
		{
			gamePortraitCanvas.gameObject.SetActive (true);
			gameLandscapeCanvas.gameObject.SetActive (false);

			candyText = GameObject.Find ("PortraitCandyText").GetComponent<Text> ();
			fadeImage = GameObject.Find ("PortraitFadeImage").GetComponent<Image> ();
			healthImage = GameObject.Find ("PortraitHealthImage").GetComponent<Image> ();
			deathContinuePanel = GameObject.Find ("PortraitDeathContinuePanel").GetComponent<Image> ();
			pauseMenuPanel = GameObject.Find ("PortraitPauseMenuPanel").GetComponent<Image> ();

			joystick = GameObject.Find ("PortraitJoystickContainer");
			leftAttack = GameObject.Find ("PortraitLeftAttackButton");
			rightAttack = GameObject.Find ("PortraitRightAttackButton");

			fadeImage.gameObject.SetActive (true);
			pauseMenuPanel.gameObject.SetActive (false);
			deathContinuePanel.gameObject.SetActive (false);
			FadeScreen (false);
			Time.timeScale = 1f;
		}

		if (SceneManager.GetActiveScene ().name == "EndGameScreen")
		{
			endGamePortraitCanvas.gameObject.SetActive (true);
			endGameLandscapeCanvas.gameObject.SetActive (false);
		}
	}


	void EnterLandscapeMode()
	{
		Screen.orientation = ScreenOrientation.LandscapeLeft;

		if (SceneManager.GetActiveScene ().name == "MainMenu")
		{
			mainMenuPortraitCanvas.gameObject.SetActive (false);
			mainMenuLandscapeCanvas.gameObject.SetActive (true);

			orientationText = GameObject.Find ("LandscapeOrientationText").GetComponent<Text> ();
			orientationText.text = PlayerPrefs.GetString ("Orientation");

			CameraScript.instance.EnterLandscapeMode ();
		}

		if (currentBuildIndex > levelStartBuildIndex)
		{
			gamePortraitCanvas.gameObject.SetActive (false);
			gameLandscapeCanvas.gameObject.SetActive (true);

			candyText = GameObject.Find ("LandscapeCandyText").GetComponent<Text> ();
			fadeImage = GameObject.Find ("LandscapeFadeImage").GetComponent<Image> ();
			healthImage = GameObject.Find ("LandscapeHealthImage").GetComponent<Image> ();
			deathContinuePanel = GameObject.Find ("LandscapeDeathContinuePanel").GetComponent<Image> ();
			pauseMenuPanel = GameObject.Find ("LandscapePauseMenuPanel").GetComponent<Image> ();

			joystick = GameObject.Find ("LandscapeJoystickContainer");
			leftAttack = GameObject.Find ("LandscapeLeftAttackButton");
			rightAttack = GameObject.Find ("LandscapeRightAttackButton");
			jumpButton = GameObject.Find ("LandscapeJumpButton");

			fadeImage.gameObject.SetActive (true);
			pauseMenuPanel.gameObject.SetActive (false);
			deathContinuePanel.gameObject.SetActive (false);
			FadeScreen (false);
			Time.timeScale = 1f;
		}

		if (SceneManager.GetActiveScene ().name == "EndGameScreen")
		{
			endGamePortraitCanvas.gameObject.SetActive (false);
			endGameLandscapeCanvas.gameObject.SetActive (true);
		}
	}


	void MobileControls ()
	{
		if (PlayerPrefs.GetString ("MobileControls") == "True")
		{
			joystick.SetActive (true);

			if (vineUnlocked)
			{
				leftAttack.gameObject.SetActive (true);
				rightAttack.gameObject.SetActive (true);
			}
			else
			{
				leftAttack.gameObject.SetActive (false);
				rightAttack.gameObject.SetActive (false);
			}
		}
		else if (PlayerPrefs.GetString ("MobileControls") == "False")
		{
			joystick.SetActive (false);
			leftAttack.gameObject.SetActive (false);
			rightAttack.gameObject.SetActive (false);
			jumpButton.SetActive (false);
		}
	}


	public void FadeScreen(bool fade)
	{
		if (fade == true)
		{
			fadeImage.CrossFadeAlpha (1, 0, false);
		}
		else if (fade == false)
		{
			fadeImage.CrossFadeAlpha (0, fadeTime, false);
		}
	}


	public void EndGame()
	{
		FadeScreen(true);
		SceneManager.LoadScene ("EndGameScreen");
		FadeScreen (false);
	}

	#if UNITY_ANDROID
	public void ShowRewardedVideo ()
	{
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		Advertisement.Show("rewardedVideo", options);
	}

	void HandleShowResult (ShowResult result)
	{
		if(result == ShowResult.Finished) {
			PlayerPrefs.SetInt ("Lives", 3);
			deathContinuePanel.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
		else if(result == ShowResult.Skipped) 
		{
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}
		else if(result == ShowResult.Failed) 
		{
			Debug.LogError("Video failed to show");
		}
	}
	#endif


	void CheckForOrientationChange()
	{
		if (oldOrientation != PlayerPrefs.GetString ("Orientation"))
		{
			if (PlayerPrefs.GetString ("Orientation") == "Landscape")
			{
				EnterLandscapeMode ();
			}

			if (PlayerPrefs.GetString ("Orientation") == "Portrait")
			{
				EnterPortraitMode ();
			}
		}
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