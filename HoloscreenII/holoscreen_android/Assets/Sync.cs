using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour {
	public WSManager ws;
	// Use this for initialization
	void Start () {
		ws = GameObject.Find ("WebsocketManager").GetComponent<WSManager>();
	}

	// Update is called once per frame
	void Update () {
		string msg = "";
		if (this.name.Contains("_l"))
			msg = ws.getHandInfoLeft ();
		else if (this.name.Contains("_r"))
			msg = ws.getHandInfoRight ();
		//Debug.Log (msg);
		if (msg.Equals("")) 
			this.gameObject.SetActive (false);
		//else
		//	this.gameObject.Set;
		
		//Debug.Log (msg);
		var hand_info = msg.Split (new char[] {',', ':', ';'});
		int i = 2; //skip hand type
		Vector3 palm_norm = new Vector3();
		while (i<hand_info.Length){
			string type = hand_info[i++];
			if (type.Contains ("palm")) {
				GameObject palm = this.transform.GetChild (5).gameObject;
				if (type.Contains ("pos")) {
					Vector3 palm_pos = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
					palm_pos = palm_pos * 0.001f;
					palm.transform.position = palm_pos;
				} else if (type.Contains ("vel")){
					i += 3;
				}else if(type.Contains ("norm")){
					palm_norm = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
					Quaternion palm_rot_byNorm = Quaternion.FromToRotation (Vector3.forward, palm_norm);
					palm.transform.rotation = palm_rot_byNorm;
				}else{
					Vector3 palm_dir = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
					Quaternion palm_rot_byDir = Quaternion.FromToRotation (palm.transform.up,palm_dir);
					palm.transform.rotation = palm_rot_byDir*palm.transform.rotation;

				}
					
			}else if(type.Contains ("finger")){
				//Debug.Log (hand_info [i]);
				int finger_i = int.Parse (hand_info [i++]);
				GameObject finger = this.transform.GetChild (finger_i).gameObject;
				//Debug.Log (hand_info [i]);
				for (int bone_i = 0; bone_i < 3; bone_i++) {
					for (int vec3_i = 0; vec3_i < 2; vec3_i++) {
						//Debug.Log ( hand_info [i]);
						string vec3_type = hand_info [i++];
						GameObject bone = finger.transform.GetChild (bone_i).gameObject;
						if (vec3_type.Contains ("pos")) {
							Vector3 bone_pos = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
							bone_pos = bone_pos * 0.001f;
							bone.transform.position = bone_pos;
						} else {
							//Quaternion palm_rot_byNorm = Quaternion.FromToRotation (Vector3.forward, palm_norm);
							Vector3 finger_dir = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
							Quaternion palm_rot_byDir = Quaternion.FromToRotation (Vector3.up, finger_dir);
							bone.transform.rotation = palm_rot_byDir;
						}
					}
				}
			}else
				i++;
		}
	}
}
