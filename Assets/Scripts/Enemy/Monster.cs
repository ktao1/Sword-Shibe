using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
	public float speed;
	
	public float shotDelay;
	
	public GameObject bullet;
	
	public bool canShot;
	
	public void Shot (Transform origin)
	{
		Instantiate (bullet, origin.position, origin.rotation);
	}
	
	public void Move (Vector2 direction)
	{

	}
}
