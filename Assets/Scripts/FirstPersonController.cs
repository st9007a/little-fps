using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;
		dir.Normalize();

		GetComponent<Rigidbody>().velocity = new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime + Vector3.up * GetComponent<Rigidbody>().velocity.y;
	}
}
