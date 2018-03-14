﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
	//Left hand has priority
	//private DataManager dataManager;
	private GestureControl gestureManager;
	private PaintManager paintManager;
	private GameObject grabHolder, palm;
	private bool is_grabbing = false;

	//Context: objct, paint, menu
	private string context = "object";
	private int context_buff_len = 30;
	private int context_buff_idx;
	private int[] context_buff;
	Dictionary<int, string> context_dict = new Dictionary<int, string>();

	// These methods will be called on the object it hits.
	const string OnRaycastExitMessage = "OnRaycastExit";
	const string OnRaycastEnterMessage = "OnRaycastEnter";
	private GameObject prev_hit;

	// Use this for initialization
	void Start () {
		//dataManager = GameObject.Find ("gDataManager").GetComponent<DataManager> ();
		gestureManager = this.GetComponent<GestureControl> ();
		paintManager = this.GetComponent<PaintManager> ();
		palm = this.transform.GetChild (5).gameObject;
		grabHolder = this.transform.GetChild (5).GetChild (0).gameObject;
		context_buff = new int[context_buff_len];
		context_dict.Add (0, "object");
		context_dict.Add (1, "paint");
		context_dict.Add (2, "menu");
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (palm.GetComponent<Rigidbody> ().angularVelocity);
		//if (gestureManager.bufferedGesture () == "palm" && palm.transform.forward.y > 0.9f) {
		//	contextBuffUpdate (1);
		//} else if (gestureManager.bufferedGesture () == "palm" && palm.transform.forward.y < -0.9f){
		//	contextBuffUpdate (0);
		//}
		Debug.Log(gestureManager.bufferedGesture());

		switch (bufferedContext()){
		case "menu":
			break;
		case "paint":
			break;
		default:
			GameObject interact_obj = getHandObject ();
			if (interact_obj != null) {
				//SendMessageTo (OnRaycastExitMessage, prev_hit);
				prev_hit = null;
				//guideToObject ();
				//Grab a object if hand gesture is grabbing
				if (gestureManager.bufferedGesture () == "pinch") {
					if (!is_grabbing) {
						Debug.Log ("Grabbing object");
						grabObject (interact_obj);
					}
				} else {
					if (is_grabbing) {
						Debug.Log ("Releasing object");
						//releaseObject (interact_obj);
					}
				}
			} else {
				//hitObject ();
			}
				break;
		}
	}

	/* 	hit
	*	Input: String set_to_context
	*	Output: None
	*	Summary: Switch context: 1. 'object' to 'paint' 2. 'paint' to 'object'
	*/
	private void hitObject(){
		RaycastHit hit;
		//guideToObject ();
		if (Physics.Raycast (palm.transform.position, palm.transform.forward, out hit, 10f) ) {
			GameObject cur_hit = hit.collider.gameObject;
			if (prev_hit != cur_hit && cur_hit.tag == "InteractableObj") {
				SendMessageTo (OnRaycastExitMessage, prev_hit);
				SendMessageTo (OnRaycastEnterMessage, cur_hit);
				prev_hit = cur_hit;
			}
		} else {
			SendMessageTo (OnRaycastExitMessage, prev_hit);
			prev_hit = null;
		}
	}

	private void guideToObject(){
		if (prev_hit) {
			GameObject arrow = GameObject.Find ("arrow");
			arrow.transform.GetChild (0).gameObject.SetActive(true);
			arrow.transform.position = palm.transform.position + 0.01f * palm.transform.forward;
			arrow.transform.up = palm.transform.forward;
		} else {
			GameObject arrow = GameObject.Find ("arrow");
			arrow.transform.GetChild (0).gameObject.SetActive(false);
		}
	}

	private void SendMessageTo(string msg, GameObject tar){
		if (tar)
			tar.SendMessage (msg, this.gameObject, SendMessageOptions.DontRequireReceiver);
	}

	/* 	contextSwtitch
	*	Input: String set_to_context
	*	Output: None
	*	Summary: Switch context: 1. 'object' to 'paint' 2. 'paint' to 'object'
	*/
	public void contextSwitch(string set_to_context){
		if (context != set_to_context){
			if (set_to_context == "paint") {
				//closeMenu ();
				paintManager.turnOnPaint();
				removeHandObject ();
				context = set_to_context;
			} else if(set_to_context == "object") {
				//closeMenu ();
				paintManager.turnOffPaint ();
				context = set_to_context;
			} else if (set_to_context == "menu"){
				paintManager.turnOffPaint ();
				removeHandObject ();
				//call menu func
				context = set_to_context;
			}
		}
		return;
	}

	/* 	bufferedContext
	*	Input: None
	*	Output: Output mode context in the array
	*	Summary: Build a histogram of context buffer array
	*/
	private string bufferedContext(){
		int[] context_hist = new int[context_dict.Count];
		for (int i = 0; i < context_buff_len; i++) 
			context_hist [context_buff [i]] += 1;

		int modeContext = 0;
		for (int i = 0; i < context_hist.Length; i++) {
			if (context_hist [i] > context_hist [modeContext])
				modeContext = i;
		}

		contextSwitch (context_dict [modeContext]);
		return context_dict [modeContext];
	}

	/* 	contextSwtitch
	*	Input: String set_to_context
	*	Output: None
	*	Summary: Switch context: 1. 'object' to 'paint' 2. 'paint' to 'object'
	*/
	private void contextBuffUpdate(int cur_context){
		context_buff [context_buff_idx++] = cur_context;
		context_buff_idx = context_buff_idx % context_buff_len;
	}

	/* 
	//Method aborted
	private bool isGrabGesture(){
		GameObject thumb_2 = this.transform.GetChild (0).GetChild (2).gameObject;
		GameObject indexfinger_2 = this.transform.GetChild (1).GetChild (2).gameObject;
		float dist_thumb_index = Vector3.Distance(thumb_2.transform.position, indexfinger_2.transform.position);
		if (dist_thumb_index < 0.065){
			return true;
		}
		return false;
	}
	*/

	/* 	grabObject
	*	Input: GameObject obj
	*	Output: None
	*	Summary: 1. Reset and inactivate rigidbody of current obj. Otherwise obj could "magically" move in your hand :) 2. Set obj to move with hand
	*/
	private void grabObject(GameObject obj){
		obj.GetComponent<Collider> ().isTrigger = true;
		obj.GetComponent<Rigidbody> ().useGravity = false;
		obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		obj.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		obj.GetComponent<Rigidbody> ().Sleep ();
		obj.transform.SetParent(grabHolder.transform);
		is_grabbing = true;
		//Debug.Log (obj.name + " is grabbed");
	}

	/* 	releaseObject
	*	Input: GameObject obj
	*	Output: None
	*	Summary: 1. Free current obj from hand 2. Activate rigidbody of current obj
	*/
	private void releaseObject(GameObject obj){
		obj.transform.parent = null;
		obj.GetComponent<Rigidbody>().useGravity = true;
		obj.GetComponent<Collider> ().isTrigger = false;
		obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		obj.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		is_grabbing = false;
		//Debug.Log (obj.name + " is dropped");
	}

	/* 	collectHandSpeed
	*	Input: None
	*	Output: None
	*	Summary: TODO
	*/
	private void collectHandSpeed(){
		return;
	}

	private bool hand_busy = false;
	private GameObject hand_obj;

	/* 	setHandObject
	*	Input: GameObject obj
	*	Output: None
	*	Summary: 1. Set current interactable object 2. Set busy flag as true
	*/
	public bool setHandObject(GameObject obj){
		if (context != "object") {
			return false;
		}

		hand_obj = obj;
		hand_busy = true;
		return true;
	}

	public bool checkHandBusy(){
		return hand_busy;
	}

	/* 	removeHandObject
	*	Input: None
	*	Output: None
	*	Summary: 1. Remove current obj
	*/
	public void removeHandObject(){
		if (is_grabbing)
			releaseObject (hand_obj);
		hand_obj = null;
		hand_busy = false;
		return;
	}

	public GameObject getHandObject(){
		return hand_obj;
	}
}
