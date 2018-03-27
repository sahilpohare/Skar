using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skar{
public class StatesManager : MonoBehaviour {
	InputManager ih;
    public bool Canmove;
	public bool isAttacking;
	public bool isGrounded;
    public Vector3 GroundcheckOffset = Vector3.zero;
    RaycastHit hit;
        // Use this for initialization
    void Start () {
		ih = FindObjectOfType<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
       isGrounded = Grounded();
      
	}
	bool Grounded(){
	  bool g;
	  if(Physics.Raycast(transform.position + GroundcheckOffset ,Vector3.down,out hit, .2f)){
		  g = true;
	  }else{
		  g = false;
	  }
	  Debug.DrawRay(transform.position + GroundcheckOffset,Vector3.down,Color.blue);
      return g;
	}
 } 	


}
