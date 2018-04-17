﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform Target;
	[Range(0,01)]
	public float speed = 0.125f;
	public Vector3 Offset;
	private Vector3 currentpos;
	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Follow();
	}

	void Init(){
		if(Target == null){
			Target = GameObject.FindGameObjectWithTag("Player").transform;
		}
	}
    
	void Follow()
    {
		transform.position = Vector3.SmoothDamp(transform.position,Target.position + Offset,ref currentpos,speed);
        //transform.position = Vector3.Lerp(transform.position,Target.position,speed);	
	}
}
