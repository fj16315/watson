using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycasting : MonoBehaviour {

	public float raycastDistance; 
	RaycastHit objectHit;
	bool display = false;	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.DrawRay(this.transform.position, this.transform.forward * raycastDistance, Color.yellow);

		//if(Physics.Raycast(this.transform.position, this.transform.forward, out objectHit, raycastDistance)){
		//	if(Equals(objectHit.collider.gameObject.name, "NPC_Test")){
		//		Debug.Log("true");
		//		display = true;
		//	}
		//}
		//else{
		//	display = false;
		//}

	}

	void OnGUI(){
		//if(display == true){
		//	Debug.Log("textbox activated");
		//	GUI.Box(new Rect((Screen.width-150)/2,(Screen.height-50)/2,150,50 ), "Press T to talk to NPC");
			
		//}
	}
}
