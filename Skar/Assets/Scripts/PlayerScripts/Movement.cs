using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skar{
public class Movement : MonoBehaviour {
    public bool _canMove;
	public Rigidbody rb;
	public float _moveSpeed = 10f;
	[Range(0,1)]
	public float _rotSpeed = .5f;
	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		if(InputVector() != Vector3.zero){
		  Rotate();
		}
		Move();
	}
	void Init (){
		rb = GetComponent<Rigidbody>();
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.drag = 5;
	}
	void Move(){
		if(InputVector() != Vector3.zero){
            rb.velocity = InputVector() * _moveSpeed;
		}
	}
	void Rotate(){
        RotateTowardsSmooth(InputVector());
	}
    public void RotateTowardsSmooth(Vector3 Target){
		Quaternion desiredRot = Quaternion.LookRotation(Target);
		transform.rotation = Quaternion.Slerp(transform.rotation,desiredRot,_rotSpeed);
	}

	Vector3 InputVector(){
		Vector3 v = Camera.main.transform.forward * ih.Vertical;
		Vector3 h = Camera.main.transform.right * ih.Horizontal;
		v.y = 0;
		h.y = 0;

		return (v+h).normalized;
	}
  }
}