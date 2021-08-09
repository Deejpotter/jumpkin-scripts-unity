using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Enemy")
		{
			if (col.name.Contains ("Boss"))
			{
				col.gameObject.GetComponent<EnemyScript> ().Damage (10);
			}
			else
			{
				col.gameObject.GetComponent<EnemyScript> ().Death ();
			}
		}

		if(col.name.Contains("Crate"))
		{
			Destroy (col.gameObject);
		}
	}
}