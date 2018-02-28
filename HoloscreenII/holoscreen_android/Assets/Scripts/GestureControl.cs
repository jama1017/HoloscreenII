using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestureControl : MonoBehaviour {
	//Left hand finger declare
	private GameObject palm, indexfinger, indexfinger_bone1, indexfinger_bone2;
	public bool Pose = false;



	// Use this for initialization
	void Start () {
		indexfinger = this.transform.GetChild (1).gameObject;
		indexfinger_bone1 = this.transform.GetChild (1).GetChild (1).gameObject;
		indexfinger_bone2 = this.transform.GetChild (1).GetChild (2).gameObject;
		palm = this.transform.GetChild (5).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (checkPosePointing ())
			Pose = true;
		else
			Pose = false;

		handDataGenerator ();
	}

	private bool checkPosePointing(){
		Vector3 palm_plane_norm, palm_plane_up, palm_plane_right, vec_bone1, vec_bone2, proj_bone1, proj_bone2, temp;
		palm_plane_norm = palm.transform.forward;
		palm_plane_up = palm.transform.up;
		palm_plane_right = palm.transform.right;

		vec_bone1 = indexfinger_bone1.transform.position - palm.transform.position;
		vec_bone2 = indexfinger_bone2.transform.position - palm.transform.position;
		proj_bone1.y = Vector3.ProjectOnPlane(vec_bone1, palm_plane_norm).magnitude;
		proj_bone2.x = Vector3.ProjectOnPlane(vec_bone2, palm_plane_right).magnitude;
		proj_bone2.y = Vector3.ProjectOnPlane(vec_bone2, palm_plane_norm).magnitude;
		proj_bone2.z = Vector3.ProjectOnPlane(vec_bone2, palm_plane_up).magnitude;

		//Debug.Log ("x: " + proj_bone2.x.ToString("F3") + "  y: " + proj_bone2.y.ToString("F3") + "  z: " + proj_bone2.z.ToString("F3"));
		if ((proj_bone2.y/proj_bone1.y)>1.027) {
			return true;
		}

		return false;
	}

	private void handDataGenerator(){
		Vector3[] vec_bone2 = new Vector3[5];
		Vector3 palm_plane_norm, palm_plane_up, palm_plane_right;
		palm_plane_norm = palm.transform.forward;
		palm_plane_up = palm.transform.up;
		palm_plane_right = palm.transform.right;

		string temp = "";
		for (int i = 0; i < 5; i++) {
			Vector3 vec_palm_bone2 = this.transform.GetChild (i).GetChild (2).position - palm.transform.position;
			vec_bone2 [i].x = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_right).magnitude;
			vec_bone2 [i].y = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_norm).magnitude;
			vec_bone2 [i].z = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_up).magnitude;
			temp += vec_bone2 [i].x.ToString ("F6") + "," + vec_bone2 [i].y.ToString ("F6") + "," + vec_bone2 [i].z.ToString ("F6") ;
			if (i < 4)
				temp += ",";
			else
				temp += "\n";
		}
		System.IO.File.AppendAllText("handData.txt", temp);
	}
}
