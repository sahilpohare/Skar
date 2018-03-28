using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skar{
public class StatesManager : MonoBehaviour {
	InputManager ih;
    public bool CanMove;
	public bool isAttacking;
	public bool isGrounded;
    public Vector3 GroundcheckOffset = Vector3.zero;
    public RaycastHit GroundCheckhitInfo;
    public GroundCheckSettings gs = new GroundCheckSettings();
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
	  if(Physics.Raycast(transform.position + GroundcheckOffset ,Vector3.down,out GroundCheckhitInfo, gs.groundCheckDistance , ~gs.ignoredGrounds)){
		  g = true;
	  }else{
		  g = false;
	  }
	  Debug.DrawRay(transform.position + GroundcheckOffset,Vector3.down,Color.blue);
      return g;
	}
 } 	
    [System.Serializable]
    public class GroundCheckSettings
    {
        public float shellOffset = .1f;
        public float groundCheckDistance = .06f;
        public LayerMask ignoredGrounds;
    }


}
