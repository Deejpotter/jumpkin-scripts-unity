using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidCandyScript : MonoBehaviour 
{
	public float force = 10f;

	Rigidbody2D rb2d;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();

		rb2d.AddForce (new Vector2(Random.Range(-force * .5f, force * .5f), force));
	}
}