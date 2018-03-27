
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skar{
[RequireComponent(typeof(StatesManager))]
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

    Vector3 moveDirection;

    Rigidbody rigidbody;

    CapsuleCollider playerCollider;
    private bool onGround;
    public bool midairControl;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.drag = groundedDrag;
        playerCollider = GetComponentInChildren<CapsuleCollider>();
        states = GetComponent<StatesManager>();
        ih = FindObjectOfType<InputManager>();
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

    Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    void GetMoveDirection(Vector2 input, Transform camAnchor)
    {
        Vector3 v;
        Vector3 h;
        v = camAnchor.forward;
        h = camAnchor.right;
        v.y = 0;
        h.y = 0;
        moveDirection = (v * input.y + h * input.x).normalized;
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
            GetMoveDirection(GetInput(), Camera.main.transform);
            if (ih.Horizontal != 0 && ih.Vertical != 0)
            {
                RotateToMoveDirection(moveDirection);
                Move(moveDirection * Mathf.Sin(Mathf.Deg2Rad * 45));
            }
            else
            {
                RotateToMoveDirection(moveDirection);
                Move(moveDirection);
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
        {
            rigidbody.drag = groundedDrag;
            currentNumberOfJumps = 0;
            Jump();
        }
        else
        {
            rigidbody.drag = 0;
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