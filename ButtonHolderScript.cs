using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHolderScript : MonoBehaviour 
{
	public void PlayButton()
	{
		GameControllerScript.instance.level02Cleared = false;
		GameControllerScript.instance.level03Cleared = false;
		GameControllerScript.instance.doubleJumpUnlocked = false;
		GameControllerScript.instance.vineUnlocked = false;
		GameControllerScript.instance.speech01Triggered = false;
		GameControllerScript.instance.speech02Triggered = false;
		GameControllerScript.instance.speech03Triggered = false;
		GameControllerScript.instance.speech04Triggered = false;
		GameControllerScript.instance.speech05Triggered = false;
		SceneManager.LoadScene ("Level01");
	}


	public void ExitButton()
	{
		Application.Quit ();
	}


	public void MainMenuButton()
	{
		SceneManager.LoadScene ("MainMenu");
	}


	public void OptionsButton()
	{
		SceneManager.LoadScene ("OptionsMenu");
	}


	public void OrientationButton()
	{
		if (PlayerPrefs.GetString ("Orientation") == "Landscape")
		{
			PlayerPrefs.SetString ("Orientation", "Portrait");
		}
		else if (PlayerPrefs.GetString ("Orientation") == "Portrait")
		{
			PlayerPrefs.SetString ("Orientation", "Landscape");
		}
	}


	public void CharacterSelectMenuButton()
	{
		SceneManager.LoadScene ("CharacterSelectMenu");
	}


	public void PauseButton()
	{
		if (Time.timeScale != 0)
		{
			Time.timeScale = 0f;
			GameControllerScript.instance.pauseMenuPanel.gameObject.SetActive (true);
			CameraScript.instance.audioSource.Pause ();
		}
		else
		{
			Time.timeScale = 1f;
			GameControllerScript.instance.pauseMenuPanel.gameObject.SetActive (false);
			CameraScript.instance.audioSource.Play ();
		}
	}


	public void LeftAttack()
	{
		StartCoroutine (PlayerScript.instance.Attack("Left"));
	}


	public void RightAttack()
	{
		StartCoroutine (PlayerScript.instance.Attack("Right"));
	}


	public void JumpButton()
	{
		PlayerScript.instance.JumpWithButton ();
	}


	public void MobileControlsButton()
	{
		if (PlayerPrefs.GetString ("MobileControls") == "True")
		{
			PlayerPrefs.SetString ("MobileControls", "False");
		}
		else if (PlayerPrefs.GetString ("MobileControls") == "False")
		{
			PlayerPrefs.SetString ("MobileControls", "True");
		}
	}


	public void ChangeCharacterButton()
	{
		if (PlayerPrefs.GetString ("Character") == "Jumpkin")
		{
			if (PlayerPrefs.GetString ("ZombieUnlocked") == "True")
			{
				PlayerPrefs.SetString ("Character", "Zombie");
			}
			else
			{
				PlayerPrefs.SetString ("Character", "Jumpkin");
			}
		}
		else if (PlayerPrefs.GetString ("Character") == "Zombie")
		{
			PlayerPrefs.SetString ("Character", "Jumpkin");
		}
	}


	public void BuyZombieButton()
	{
		if (PlayerPrefs.GetString ("ZombieUnlocked") != "True" && PlayerPrefs.GetInt("Candy") >= 100)
		{
			PlayerPrefs.SetString ("ZombieUnlocked", "True");
			PlayerPrefs.SetInt ("Candy", PlayerPrefs.GetInt("Candy") -100);
		}
		else
		{
			return;
		}
	}


	public void FacebookButton()
	{
		Application.OpenURL("https://www.facebook.com/madanttgames/");
	}

	#if UNITY_ANDROID
	public void ShowRewardAdButton()
	{
		GameControllerScript.instance.ShowRewardedVideo ();
	}
	#endif


	public void PayForLivesWithCandyButton()
	{
		PlayerPrefs.SetInt ("Candy", PlayerPrefs.GetInt("Candy") -100);
		PlayerPrefs.SetInt ("Lives", 3);
		GameControllerScript.instance.deathContinuePanel.gameObject.SetActive (false);
		Time.timeScale = 1;
	}
}