
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skar{
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(StatesManager))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour {
    
    [Serializable]
    public class AdvancedSettings
    {
        public float shellOffset = .1f;
        public float groundCheckDistance = .06f;
        public LayerMask ignoredGrounds;
    }
    

    public AdvancedSettings advancedSettings_Access = new AdvancedSettings();
    public StatesManager states;
    public InputManager ih;
    public float speed = 5;
    public float jumpForce = 10;
    [Range(0,1)]
    public float _rotSpeed = .4f;
    public int midAirJumps = 1;
    public float rotationSpeed = 15;
    public float groundedDrag = 10;
    [SerializeField] float timeTillNextJump = .5f;
    private float jumpTime;
    private float currentNumberOfJumps;

    public Vector3 moveDirection;

    public Rigidbody rigidbody;

    CapsuleCollider playerCollider;
    private bool onGround;
    public bool midairControl;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = groundedDrag;
        playerCollider = GetComponentInChildren<CapsuleCollider>();
        states = GetComponent<StatesManager>();
        ih = InputManager.Init();
    }

    private void FixedUpdate()
    {
        CheckGround();
        AirborneCheck();
      if(states.CanMove){
        if(midairControl)
        {
            ExecuteMovement();
        }
        else
        {
            if(onGround)
            {
                ExecuteMovement();
            } 
        }
      }
    }


    void RotateToMoveDirection(Vector3 direction)
    {
        Quaternion desiredRot = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation,desiredRot,_rotSpeed);
    }

    void Move(Vector3 direction)
    {
        if (rigidbody.velocity.magnitude < speed)
        {
            rigidbody.AddForce(direction * speed, ForceMode.Impulse);
        }
        else
        {
            rigidbody.velocity = new Vector3(direction.x * speed, rigidbody.velocity.y, direction.z * speed);
        }
    }

    void ExecuteMovement()
    {
        if (ih.Vertical != 0 || ih.Horizontal != 0)
        {
            if (ih.Horizontal != 0 && ih.Vertical != 0)
            {
                RotateToMoveDirection(ih.moveDirection);
                Move(ih.moveDirection * Mathf.Sin(Mathf.Deg2Rad * 45));
            }
            else
            {
                RotateToMoveDirection(ih.moveDirection);
                Move(ih.moveDirection);
            }
        }
    }

    void CheckGround()
    {
      onGround = states.isGrounded;
    }

    void AirborneCheck()
    {
        if(onGround)
        {   if(states.CanMove){
            rigidbody.drag = groundedDrag;
            }
            currentNumberOfJumps = 0;
            Jump();
        }
        else
        {  
            if(states.CanMove){
            rigidbody.drag = 0;
            }
            if (currentNumberOfJumps < midAirJumps)
            {
                if(Time.time > jumpTime + timeTillNextJump)
                {
                    Jump();
                }
            }
        }
    }

    void Jump()
    {
        if (ih.Jump)
        {
            rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            jumpTime = Time.time;
            currentNumberOfJumps++;
        }
    }
 } 
}