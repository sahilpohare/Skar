using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StatesManager : MonoBehaviour {
    public enum MoveState
{
		 isJumping,
		 isMoving,
		 isInDash,
		 isAttacking
}
    public bool CanMove;
	public bool isAttacking;
	public bool isGrounded;
	public MoveState moveState;
        // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {    
	}
 } 	


