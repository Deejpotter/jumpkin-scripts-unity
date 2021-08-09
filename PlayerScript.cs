using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
	public static PlayerScript instance;

	public float moveSpeed, jumpSpeed;
	public Transform rayEnd;
	public Vector3 spawnPos;
	public Image fadeImage;
	public Text playerText;
	public JoystickScript joystick;
	public AudioClip jumpSound, candySound, vineSound, upgradeSound;
	public bool attacking = false, dead = false, canDropDown = false;

	Rigidbody2D rb2d;
	Animator ani;
	RaycastHit2D hit;
	public Canvas playerCanvas;

	// v stores the current velocity, oldPos stores the old position at the end of each frame.
	Vector2 v, oldPos;

	// horAxis stores the input from the horizontal axis, drag is the percentage of speed that is retained each frame when the player is grounded.
	float horAxis, verAxis, groundDrag = .9f, airDrag = .98f, fadeTime = .5f, waitTime = 1f;

	// jumps is the ammount of jumps left until grounded, maxJumps is the number that jumps is set to after being grounded.
	int jumps, maxJumps = 2;

	bool grounded = false, canJumpAgain = true;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != null && instance != this)
		{
			Destroy (gameObject);
		}
	
		rb2d = GetComponent <Rigidbody2D> ();
		ani = GetComponent <Animator> ();
		playerCanvas = GameObject.Find ("PlayerCanvas").GetComponent<Canvas> ();
		playerText = GameObject.Find ("PlayerText").GetComponent<Text> ();
		playerCanvas.enabled = false;
		spawnPos = transform.position;

		if (PlayerPrefs.GetString ("Orientation") == "Landscape")
		{
			joystick = GameObject.Find ("LandscapeJoystickContainer").GetComponent<JoystickScript>();
		}

		if (PlayerPrefs.GetString ("Orientation") == "Portrait")
		{
			joystick = GameObject.Find ("PortraitJoystickContainer").GetComponent<JoystickScript>();
		}
	}


	void Update()
	{
		//grounded = Physics2D.Linecast (transform.position, rayEnd.position, 1 << LayerMask.NameToLayer ("Ground"));

		#if UNITY_ANDROID
		if (GameControllerScript.instance.oldOrientation != PlayerPrefs.GetString ("Orientation"))
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
		#endif

		if (dead)
		{
			rb2d.velocity = Vector2.zero;
		}

		Behavior ();

		Animation ();

		Movement ();

		Jump ();
	}


	void FixedUpdate()
	{
		
	}


	void LateUpdate()
	{
		/*
		 * All of this was for keeping the player within the bounds of the game but it started glitching and it isn't neccessary anyway
		 * 
		 
		float transX;

		if (transform.position.x < CameraScript.instance.topLeft.x)
		{
			transX = CameraScript.instance.topLeft.x;
		}
		else if (transform.position.x > CameraScript.instance.bottomRight.x)
		{
			transX = CameraScript.instance.bottomRight.x;
		}
		else
		{
			transX = transform.position.x;
		}

		transform.position = new Vector3 (transX, transform.position.y, transform.position.z);
		*/

		// This stores the current position for use next frame
		oldPos = transform.position;

		rb2d.velocity = v;
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Candy")
		{
			GameControllerScript.instance.PlaySoundEffect (candySound);

			if (col.name.Contains ("Pile"))
			{
				Destroy (col.gameObject);
				PlayerPrefs.SetInt ("Candy", PlayerPrefs.GetInt ("Candy") + 10);
			}
			else
			{
				Destroy (col.gameObject);
				PlayerPrefs.SetInt ("Candy", PlayerPrefs.GetInt ("Candy") + 1);
			}
		}

		if (col.name == "BossStart")
		{
			GameControllerScript.instance.limboBoss.SetActive (true);
			CameraScript.instance.PlayMusic (CameraScript.instance.scaryMusic);
		}

		if (col.tag == "DeathZone")
		{
			StartCoroutine (Death ());
		}

		if (col.name == "VineUpgrade")
		{
			GameControllerScript.instance.vineUnlocked = true;
			Destroy (col.gameObject);
			GameControllerScript.instance.PlaySoundEffect (upgradeSound);
		}

		if (col.name == "DoubleJumpUpgrade")
		{
			GameControllerScript.instance.doubleJumpUnlocked = true;
			Destroy (col.gameObject);
			GameControllerScript.instance.PlaySoundEffect (upgradeSound);
		}

		if (col.name == "EndGamePortal")
		{
			SceneManager.LoadScene ("EndGameScreen");
		}
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if (!dead)
		{
			if (col.gameObject.name.Contains ("LimboSpiked"))
			{
				StartCoroutine (Death ());
			}
			else if (col.gameObject.name.Contains ("LimboNormal"))
			{
				if (col.transform.position.y < transform.position.y - .6f)
				{
					if (col.transform.position.x > transform.position.x - .6f && col.transform.position.x < transform.position.x + .6f)
					{
						col.gameObject.GetComponent<EnemyScript> ().Death ();
						rb2d.velocity = Vector2.zero;
						rb2d.AddForce (Vector2.up * jumpSpeed, ForceMode2D.Force);
					}
				}
				else
				{
					StartCoroutine (Death ());
				}
			}
			else if (col.gameObject.name.Contains("Boss"))
			{
				StartCoroutine (Death ());
			}
		}

		if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (col.gameObject.transform.position.y < transform.position.y)
			{
				grounded = true;

				if(col.gameObject.name.Contains("Platform"))
				{
					canDropDown = true;
				}
			}
		}

		if (col.gameObject.tag == "Candy")
		{
			GameControllerScript.instance.PlaySoundEffect (candySound);

			if (col.gameObject.name.Contains ("Pile"))
			{
				Destroy (col.gameObject);
				PlayerPrefs.SetInt ("Candy", PlayerPrefs.GetInt ("Candy") + 10);
			}
			else
			{
				Destroy (col.gameObject);
				PlayerPrefs.SetInt ("Candy", PlayerPrefs.GetInt ("Candy") + 1);
			}
		}
	}


	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			if (col.gameObject.transform.position.y < transform.position.y)
			{
				grounded = false;
				canDropDown = false;
				jumps -= 1;
			}
		}
	}
		

	void Movement()
	{
		#if UNITY_ANDROID
		horAxis = joystick.Horizontal();
		verAxis = joystick.Vertical();
		#else
		horAxis = Input.GetAxis("Horizontal");
		verAxis = Input.GetAxis("Vertical");
		#endif

		v = rb2d.velocity;

		if (horAxis == 0 && grounded)
		{
			v = new Vector2 (v.x * groundDrag, v.y);
		}
		else if(horAxis == 0 && !grounded)
		{
			v = new Vector2 (v.x * airDrag, v.y);
		}
		else if(horAxis < -.3f || horAxis > .3f)
		{
			v = new Vector2 (Mathf.Sign(horAxis) * moveSpeed, v.y);
		}

		if (transform.position.y < oldPos.y)
		{
			v = new Vector2 (v.x, v.y - .2f);
		}

		v = new Vector2 (v.x + (GameControllerScript.instance.movingPlatformVelocity.x * .1f), v.y);
	}


	void Behavior()
	{
		if (grounded == true)
		{
			jumps = maxJumps;
			ani.SetBool ("Grounded", true);
		}

		if (GameControllerScript.instance.doubleJumpUnlocked)
		{
			maxJumps = 2;
		}
		else
		{
			maxJumps = 1;
		}

		if (GameControllerScript.instance.vineUnlocked)
		{
			if (Input.GetButtonDown ("Attack"))
			{
				if (transform.localScale.x == -1)
				{
					StartCoroutine (Attack ("Left"));
				}
				else
				{
					StartCoroutine (Attack ("Right"));
				}

			}
		}

		if (!attacking)
		{
			if (transform.position.x != oldPos.x)
			{
				if (transform.position.x < oldPos.x)
				{
					transform.localScale = new Vector3 (-1, 1, 1);
					playerCanvas.transform.localScale = new Vector3 (-1, 1, 1);
				}
				else if (transform.position.x > oldPos.x)
				{
					transform.localScale = new Vector3 (1, 1, 1);
					playerCanvas.transform.localScale = new Vector3 (1, 1, 1);
				}
			}
		}

		if (canDropDown && verAxis < -.9f)
		{
			StartCoroutine (DropDown());
		}
	}


	void Jump()
	{
		if (jumps > 0)
		{
			if (Input.GetButtonDown ("Jump"))
			{
				jumps -= 1;
				rb2d.velocity = new Vector2 (rb2d.velocity.x, 0);
				rb2d.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Force);
				ani.SetTrigger ("Jump");
				Debug.Log (jumps);
				GameControllerScript.instance.PlaySoundEffect (jumpSound);
			}


			if (verAxis > .5f && canJumpAgain)
			{
				canJumpAgain = false;
				jumps -= 1;
				rb2d.velocity = new Vector2 (rb2d.velocity.x, 0);
				rb2d.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Force);
				ani.SetTrigger ("Jump");
				Debug.Log (jumps);
				GameControllerScript.instance.PlaySoundEffect (jumpSound);
			}

			if (verAxis < .5f)
			{
				canJumpAgain = true;
			}
		}
	}


	public void JumpWithButton ()
	{
		if (jumps > 0)
		{
			jumps -= 1;
			rb2d.velocity = new Vector2 (rb2d.velocity.x, 0);
			rb2d.AddForce (new Vector2 (0, jumpSpeed), ForceMode2D.Force);
			ani.SetTrigger ("Jump");
			GameControllerScript.instance.PlaySoundEffect (jumpSound);
		}
	}


	public IEnumerator Attack (string side)
	{
		if (!attacking)
		{
			if (side == "Left")
			{
				GameControllerScript.instance.PlaySoundEffect (vineSound);
				transform.localScale = new Vector3 (-1, 1, 1);
				attacking = true;
				ani.SetTrigger ("Attack");
				yield return new WaitForSeconds (.5f);
				attacking = false;
			}
			else if (side == "Right")
			{
				GameControllerScript.instance.PlaySoundEffect (vineSound);
				transform.localScale = new Vector3 (1, 1, 1);
				attacking = true;
				ani.SetTrigger ("Attack");
				yield return new WaitForSeconds (.5f);
				attacking = false;
			}
		}
	}


	void Animation()
	{
		if (rb2d.velocity.x < 0)
		{
			ani.SetFloat ("VelocityX", -rb2d.velocity.x);
		}
		else
		{
			ani.SetFloat ("VelocityX", rb2d.velocity.x);
		}

		ani.SetFloat ("VelocityY", rb2d.velocity.y);
	}


	IEnumerator Death()
	{
		GameControllerScript.instance.FadeScreen (true);
		dead = true;
		gameObject.GetComponent<Collider2D> ().enabled = false;
		PlayerPrefs.SetInt ("Lives", PlayerPrefs.GetInt("Lives") - 1);
		yield return new WaitForSecondsRealtime (waitTime);
		transform.position = spawnPos;
		Camera.main.transform.position = spawnPos;
		gameObject.GetComponent<Collider2D> ().enabled = true;
		dead = false;
		GameControllerScript.instance.FadeScreen (false);
	}


	/*
	void FadeScreen(bool fade)
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
	*/


	IEnumerator DropDown()
	{
		gameObject.GetComponent<Collider2D> ().enabled = false;
		yield return new WaitForSeconds (.3f);
		gameObject.GetComponent<Collider2D> ().enabled = true;
	}


	public IEnumerator EnemyAliveText()
	{
		playerCanvas.enabled = true;
		playerText.text = "I can't go until I take care of all the enemies!";
		yield return new WaitForSeconds (3);
		playerCanvas.enabled = false;
	}


	void EnterPortraitMode()
	{
		joystick = GameObject.Find ("PortraitJoystickContainer").GetComponent<JoystickScript>();
	}


	void EnterLandscapeMode()
	{
		joystick = GameObject.Find ("LandscapeJoystickContainer").GetComponent<JoystickScript>();
	}
}