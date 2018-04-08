using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitesManager : MonoBehaviour {
    PlayerMovement movl;
	StatesManager st;
    Rigidbody rb;
    [Header("Dash")]
	public bool canDash = true;
	public float dashtime = .5f;
	public float dashspeed = 10;
	public bool isInDash = false;
	public float timeToDash = .7f;
	[SerializeField]
    private int noOfDash = 2;
	[SerializeField]
	private int dashCounter = 0;
	// Use this for initialization
	void Start () {
		movl = GetComponent<PlayerMovement>();
		st = GetComponent<StatesManager>();
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Dash"))
        {
			if(canDash){
			StartCoroutine(Dash());
			}
		}
		if(isInDash)
        {
          rb.useGravity = false;
          rb.drag = movl.movementSettings.groundedDrag;
		}
		else
		{
            rb.useGravity = true;
		}
		ReenableDash();

	}
	IEnumerator Dash(){
		canDash = false;
		StopCoroutine(DashTimer());
		StopCoroutine(ReEnableDash());
		Vector3 dir;
		if(movl.inputSettings.GetMoveDirection(movl.inputSettings.GetInput(), Camera.main.transform) != Vector3.zero)
        {
		    dir = movl.inputSettings.GetMoveDirection(movl.inputSettings.GetInput(), Camera.main.transform);
			movl.RotateToMoveDirection(dir,8.1f);
		}else
        {
			dir = transform.forward;
		}
        if(dashCounter < noOfDash){
		     dashCounter++;
		     st.CanMove = false;
		     st.moveState = StatesManager.MoveState.isInDash;
		     isInDash = true;
             rb.AddForce(dir * dashspeed,ForceMode.VelocityChange);
		     StartCoroutine(DashTimer());
		     yield return new WaitForSeconds(dashtime);
		     isInDash = false;
		     st.CanMove = true;
		}else{
			canDash = true;
		}
		
		yield return null;
		
	}
	IEnumerator DashTimer(){
	   yield return new WaitForSeconds(0);
	   canDash = true;
	} 

	IEnumerator ReEnableDash(){
		yield return new WaitForSeconds (timeToDash);
		dashCounter = 0;
	} 
	void ReenableDash(){
        if(st.isGrounded && dashCounter == noOfDash){
		 StartCoroutine(ReEnableDash());
		}
	}
}
