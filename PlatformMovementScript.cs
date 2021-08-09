using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementScript : MonoBehaviour 
{
	public float moveSpeed = 40f;

	Rigidbody2D rb2d;

	void Start()
	{
		rb2d = GetComponent <Rigidbody2D> ();
	}


	void FixedUpdate()
	{
		Movement ();
	}


	void Movement()
	{
		rb2d.velocity = new Vector2 (moveSpeed * Time.fixedDeltaTime, 0);
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag != "Player")
		{
			moveSpeed *= -1;
		}

		if (col.gameObject.tag == "Player")
		{
			GameControllerScript.instance.movingPlatformVelocity = rb2d.velocity;
		}
	}


	void OnCollisionStay2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			GameControllerScript.instance.movingPlatformVelocity = rb2d.velocity;
		}
	}


	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			GameControllerScript.instance.movingPlatformVelocity = Vector2.zero;
		}
	}
}