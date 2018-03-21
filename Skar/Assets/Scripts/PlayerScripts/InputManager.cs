using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public float Horizontal;
	public float Vertical;
	public bool M1;
	public bool M2;
	public float mouseX;
	public float mouseY;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Horizontal = Input.GetAxisRaw("Horizontal");
		Vertical = Input.GetAxisRaw("Vertical");
		mouseX = Input.GetAxis("Mouse X");
		mouseY = Input.GetAxis("Mouse Y");
		M1 = Input.GetMouseButton(0);
		M2 = Input.GetMouseButton(1);
	}

}
