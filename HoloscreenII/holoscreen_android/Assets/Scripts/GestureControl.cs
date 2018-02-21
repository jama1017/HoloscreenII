using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureControl : MonoBehaviour {
	//Left hand finger declare
	private GameObject palm, indexfinger, indexfinger_bone1, indexfinger_bone2;

	// Use this for initialization
	void Start () {
		indexfinger = this.transform.GetChild (1).gameObject;
		indexfinger_bone1 = this.transform.GetChild (1).GetChild (1).gameObject;
		indexfinger_bone2 = this.transform.GetChild (1).GetChild (2).gameObject;
		palm = this.transform.GetChild (5).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (checkPosePointing ()) {
			//do nothing
		}
	}

	private bool checkPosePointing(){
		Vector3 palm_plane_norm, proj_bone1, proj_bone2, temp;
		palm_plane_norm = palm.transform.forward;
		temp = indexfinger_bone1.transform.position - palm.transform.position;
		proj_bone1 = Vector3.ProjectOnPlane(indexfinger_bone1.transform.position - palm.transform.position, palm_plane_norm);
		Debug.Log ("dist_bone1: " + temp.magnitude.ToString("F3"));
		Debug.Log ("proj_bone1: " + proj_bone1.magnitude.ToString("F3"));
		proj_bone2 = Vector3.ProjectOnPlane(indexfinger_bone2.transform.position - palm.transform.position, palm_plane_norm);
		Debug.Log ("proj_bone2: " + proj_bone2.magnitude.ToString("F3"));
		return false;
	}
}
