using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skar{
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(StatesManager))]
public class AbilitesManager : MonoBehaviour {
    PlayerMovement movl;
	InputManager ih;
	StatesManager st;
    [Header("Dash")]
	public bool canDash = true;
	public float dashtime = .5f;
	public float dashspeed = 10;
	public bool isInDash = false;
	// Use this for initialization
	void Start () {
		movl = GetComponent<PlayerMovement>();
		ih = InputManager.Init();
		st = GetComponent<StatesManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if(ih.Dash){
			StartCoroutine(Dash());
		}
		if(isInDash){
			if(!st.isGrounded){
			    movl.rigidbody.useGravity = false;
			}
			movl.rigidbody.drag = 10f;	
		}
		else
		{
		   movl.rigidbody.useGravity = true;
		}
	}
	IEnumerator Dash(){
		Vector3 dir;
		if(ih.moveDirection != Vector3.zero){
			dir = ih.moveDirection;
		}else{
			dir = transform.forward;
		}

		st.CanMove = false;
		isInDash = true;
		movl.rigidbody.AddForce(dir * dashspeed,ForceMode.VelocityChange);
		yield return new WaitForSeconds(dashtime);
		isInDash = false;
		st.CanMove = true;
	}
  }
}
