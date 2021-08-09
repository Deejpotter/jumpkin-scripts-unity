using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueScript : MonoBehaviour 
{
	Canvas playerCanvas;
	Text playerText;

	void Awake()
	{
		playerCanvas = GameObject.Find ("PlayerCanvas").GetComponent<Canvas> ();
		playerText = GameObject.Find ("PlayerText").GetComponent<Text> ();
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player")
		{
			if (name == "Speech01" && GameControllerScript.instance.speech01Triggered == false)
			{
				StartCoroutine (PlayerText01 ());
			}

			if (name == "Speech02" && GameControllerScript.instance.speech02Triggered == false)
			{
				StartCoroutine (PlayerText02 ());
			}

			if (name == "Speech03" && GameControllerScript.instance.speech03Triggered == false)
			{
				StartCoroutine (PlayerText03 ());
			}

			if (name == "Speech04" && GameControllerScript.instance.speech04Triggered == false)
			{
				StartCoroutine (PlayerText04 ());
			}

			if (name == "Speech05" && GameControllerScript.instance.speech05Triggered == false)
			{
				StartCoroutine (PlayerText05 ());
			}
		}
	}


	IEnumerator PlayerText01()
	{
		playerCanvas.enabled = true;
		playerText.text = "I guess I'll go this way then!";
		GameControllerScript.instance.speech01Triggered = true;
		yield return new WaitForSeconds (3);
		playerCanvas.enabled = false;
	}


	IEnumerator PlayerText02()
	{
		playerCanvas.enabled = true;
		playerText.text = "How am I going to get up there?";
		GameControllerScript.instance.speech02Triggered = true;
		yield return new WaitForSeconds (3);
		playerCanvas.enabled = false;
	}


	IEnumerator PlayerText03()
	{
		playerCanvas.enabled = true;
		playerText.text = "That one has horns, how am I going to fight it?";
		GameControllerScript.instance.speech03Triggered = true;
		yield return new WaitForSeconds (3);
		playerCanvas.enabled = false;
	}


	IEnumerator PlayerText04()
	{
		playerCanvas.enabled = true;
		playerText.text = "Now I can attack with my vine!";
		GameControllerScript.instance.speech04Triggered = true;
		yield return new WaitForSeconds (3);
		playerCanvas.enabled = false;
	}


	IEnumerator PlayerText05()
	{
		playerCanvas.enabled = true;
		playerText.text = "Yes, I can double jump!";
		GameControllerScript.instance.speech05Triggered = true;
		yield return new WaitForSeconds (3);
		playerCanvas.enabled = false;
	}
}