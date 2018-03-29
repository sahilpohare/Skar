using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skar
{
    public class InputManager : MonoBehaviour {

        public float Horizontal;
	    public float Vertical;
	    public bool M1;
	    public bool M2;
	    public float mouseX;
	    public float mouseY;
        public bool Jump;
        public bool Dash;
	    // Use this for initialization
	    void Start () {
            CursorSettings(false, CursorLockMode.Locked);
	    }
	
	    // Update is called once per frame
	    void FixedUpdate () {
		    Horizontal = Input.GetAxisRaw("Horizontal");
		    Vertical = Input.GetAxisRaw("Vertical");
		    mouseX = Input.GetAxis("Mouse X");
		    mouseY = Input.GetAxis("Mouse Y");
		    M1 = Input.GetMouseButton(0);
		    M2 = Input.GetMouseButton(1);
            Jump = Input.GetButtonDown("Jump");
            Dash = Input.GetButtonDown("Dash");
	    }

        public void CursorSettings(bool visibility, CursorLockMode lockState)
        {
            Cursor.lockState = lockState;
            Cursor.visible = visibility;
        }

        public Vector2 GetInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }

        public Vector3 GetMoveDirection(Vector2 input, Transform camAnchor)
        {
            Vector3 v;
            Vector3 h;
            v = camAnchor.forward;
            h = camAnchor.right;
            v.y = 0;
            h.y = 0;
            return (v * input.y + h * input.x).normalized;
        }

        public static InputManager Init()
        {
            if (FindObjectOfType<InputManager>() == null)
            {
                return new GameObject().GetComponent<InputManager>();
            }
            else
            {
                return FindObjectOfType<InputManager>();
            }
        }
    }
}