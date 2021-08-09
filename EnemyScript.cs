using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour 
{
	public Transform rayEnd;
	public GameObject solidCandy, endGamePortal;

	Rigidbody2D rb2d;
	Vector2 oldPos;
	bool grounded = false, canChangeDirection = false;
	float moveSpeed = 40f;
	int health = 10;

	void Start()
	{
		rb2d = GetComponent <Rigidbody2D> ();

		if (name == "LimboBoss")
		{
			moveSpeed = 80f;
			health = 30;
		}
	}


	void Update()
	{
		Behavior ();
		Movement ();
	}


	void FixedUpdate()
	{
		rb2d.velocity = new Vector2 (moveSpeed, 0) * Time.fixedDeltaTime;
	}


	void LateUpdate()
	{
		oldPos = transform.position;
	}


	void Movement()
	{
		/*
		if (grounded)
		{
			canChangeDirection = true;
		}
		else
		{
			canChangeDirection = false;
		}

		if (!grounded && canChangeDirection)
		{
			moveSpeed *= -1;
			canChangeDirection = false;
		}
		*/

		//transform.position = new Vector2 (Mathf.Lerp (pointA.position.x, pointB.position.x, 2f), transform.position.y);
			

	}


	void Behavior()
	{
		if (transform.position.x > oldPos.x)
		{
			if (name == "LimboBoss")
			{
				transform.localScale = new Vector3 (-2, transform.localScale.y, transform.localScale.z);
			}
			else
			{
				transform.localScale = new Vector3 (-1, transform.localScale.y, transform.localScale.z);
			}
		}
		else if(transform.position.x < oldPos.x)
		{
			if (name == "LimboBoss")
			{
				transform.localScale = new Vector3 (2, transform.localScale.y, transform.localScale.z);
			}
			else
			{
				transform.localScale = new Vector3 (1, transform.localScale.y, transform.localScale.z);
			}
		}

		if (health <= 0)
		{
			Death ();
		}
	}


	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.transform.position.y >= transform.position.y)
		{
			moveSpeed *= -1;
		}
	}


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.transform.position.y >= transform.position.y)
		{
			moveSpeed *= -1;
		}
	}


	public void Damage(int damage)
	{
		health -= damage;
	}


	public void Death()
	{
		if(name.Contains("Boss"))
		{
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			CameraScript.instance.PlayMusic (CameraScript.instance.mainMusic);
			GameControllerScript.instance.endGamePortal.SetActive (true);
			Destroy (gameObject);
		}
		else
		{
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Instantiate (solidCandy, this.gameObject.transform.position, this.gameObject.transform.rotation);
			Destroy (gameObject);
		}
	}
}