using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {


	private Vector3 hand_l_position;
	private bool hand_l_isgrab, hand_r_isgrab;
	private GameObject hand_l_obj, hand_r_obj;
	// Use this for initialization
	void Start () {
		hand_l_position = new Vector3 (0, 0, 0);	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setLeftHandPosition(Vector3 v){
		hand_l_position = v;
	}

	public void setLeftHandVelocity(Vector3 v){
		//hand_l_position = v;
	}

	public void setLeftHandBusy(bool t){
		if (!t){
			hand_l_obj.GetComponent<InteractionScriptObject>().resetIsGrabbed();
			hand_l_obj = null;
		}
		hand_l_isgrab = t;
	}

	public void setRightHandBusy(bool t){
		if (!t){
			hand_r_obj.GetComponent<InteractionScriptObject>().resetIsGrabbed();
			hand_r_obj = null;
		}
		hand_r_isgrab = t;
	}

	public bool checkLeftHandBusy(){
		return hand_l_isgrab;
	}

	
	public bool checkRightHandBusy(){
		return hand_r_isgrab;
	}

	public void setLeftHandObject(GameObject obj){
		hand_l_obj = obj;
		Debug.Log("setLeftHandObject:" + hand_l_obj.name);
		return;
	}

	public void setRightHandObject(GameObject obj){
		hand_r_obj = obj;
		return;
	}

	public GameObject getLeftHandObject(){
		return hand_l_obj;
	}

	public Vector3 getLeftHandPosition (){
		return hand_l_position;
	}

}
