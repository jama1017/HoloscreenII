using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {


	private Vector3 hand_l_position;
	private bool hand_l_isgrab;
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

	public void setLeftHandGrab(bool t){
		hand_l_isgrab = t;
	}

	public Vector3 getLeftHandPosition (){
		return hand_l_position;
	}

	public bool getLeftHandGrab(){
		return hand_l_isgrab;
	}
}
