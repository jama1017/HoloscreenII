using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour {
	private WSManager ws;
	private DataManager dataManager;
	private GameObject l_palm;

	private GameObject l_finger0;
	private GameObject l_finger0_bone0;
	private GameObject l_finger0_bone1;
	private GameObject l_finger0_bone2;

	private GameObject l_finger1;
	private GameObject l_finger1_bone0;
	private GameObject l_finger1_bone1;
	private GameObject l_finger1_bone2;

	private GameObject l_finger2;
	private GameObject l_finger2_bone0;
	private GameObject l_finger2_bone1;
	private GameObject l_finger2_bone2;

	private GameObject l_finger3;
	private GameObject l_finger3_bone0;
	private GameObject l_finger3_bone1;
	private GameObject l_finger3_bone2;

	private GameObject l_finger4;
	private GameObject l_finger4_bone0;
	private GameObject l_finger4_bone1;
	private GameObject l_finger4_bone2;

	private GameObject finger;
	private GameObject bone;
	// Use this for initialization
	void Start () {
		ws = GameObject.Find ("WebsocketManager").GetComponent<WSManager>();
		dataManager = GameObject.Find ("gDataManager").GetComponent<DataManager> ();
		l_palm = this.transform.GetChild (5).gameObject;
		l_finger0 = this.transform.GetChild (0).gameObject;
		l_finger0_bone0 = l_finger0.transform.GetChild (0).gameObject;
		l_finger0_bone1 = l_finger0.transform.GetChild (1).gameObject;
		l_finger0_bone2 = l_finger0.transform.GetChild (2).gameObject;

		l_finger1 = this.transform.GetChild (1).gameObject;
		l_finger1_bone0 = l_finger1.transform.GetChild (0).gameObject;
		l_finger1_bone1 = l_finger1.transform.GetChild (1).gameObject;
		l_finger1_bone2 = l_finger1.transform.GetChild (2).gameObject;

		l_finger2 = this.transform.GetChild (2).gameObject;
		l_finger2_bone0 = l_finger2.transform.GetChild (0).gameObject;
		l_finger2_bone1 = l_finger2.transform.GetChild (1).gameObject;
		l_finger2_bone2 = l_finger2.transform.GetChild (2).gameObject;

		l_finger3 = this.transform.GetChild (3).gameObject;
		l_finger3_bone0 = l_finger3.transform.GetChild (0).gameObject;
		l_finger3_bone1 = l_finger3.transform.GetChild (1).gameObject;
		l_finger3_bone2 = l_finger3.transform.GetChild (2).gameObject;

		l_finger4 = this.transform.GetChild (4).gameObject;
		l_finger4_bone0 = l_finger4.transform.GetChild (0).gameObject;
		l_finger4_bone1 = l_finger4.transform.GetChild (1).gameObject;
		l_finger4_bone2 = l_finger4.transform.GetChild (2).gameObject;
	}

	// Update is called once per frame
	void Update () {
		string msg = "";
		if (this.name.Contains("_l"))
			msg = ws.getHandInfoLeft ();
		else if (this.name.Contains("_r"))
			msg = ws.getHandInfoRight ();
		//Debug.Log (msg);
		//if (msg.Equals("")) 
		//	this.gameObject.SetActive (false);

		var hand_info = msg.Split (new char[] {',', ':', ';'});

		int i = 2; //skip hand type
		Vector3 palm_norm = new Vector3();
		Vector3 palm_dir = new Vector3();

		while (i<hand_info.Length){
			string type = hand_info[i++];

			if (type.Contains ("palm")) {
				if (type.Contains ("pos")) {
					Vector3 palm_pos = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
					palm_pos = palm_pos * 0.001f;
					//palm_pos [1] += 0.2f;
					l_palm.transform.localPosition = palm_pos;
					dataManager.setLeftHandPosition (palm_pos);
				} else if (type.Contains ("vel")) {
					i += 3;
				} else if (type.Contains ("norm")) {
					palm_norm = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
					Quaternion palm_rot_byNorm = Quaternion.FromToRotation (Vector3.forward, palm_norm);

					//l_palm.transform.localRotation = palm_rot_byNorm;
				} else {
					palm_dir = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
					Quaternion palm_rot_byDir = Quaternion.FromToRotation (Vector3.up, palm_dir);

					//l_palm.transform.localRotation = palm_rot_byDir * l_palm.transform.localRotation;
					l_palm.transform.localRotation = Quaternion.LookRotation(palm_norm, palm_dir);
				}

			} else if (type.Contains ("finger")) {
				//Debug.Log (hand_info [i]);
				int finger_i = int.Parse (hand_info [i++]);
				//GameObject finger = this.transform.GetChild (finger_i).gameObject;
				//Debug.Log (hand_info [i]);
				for (int bone_i = 0; bone_i < 3; bone_i++) {
					for (int vec3_i = 0; vec3_i < 2; vec3_i++) {
						//Debug.Log ( hand_info [i]);
						string vec3_type = hand_info [i++];
						finger = getFinger (finger_i);
						bone = getBoneFromFinger (finger_i, bone_i);
						if (vec3_type.Contains ("pos")) {
							Vector3 bone_pos = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), -float.Parse (hand_info [i++]));
							bone_pos = bone_pos * 0.001f;
							//bone_pos [1] += 0.2f;
							bone.transform.localPosition = bone_pos;
						} else {
							//Quaternion palm_rot_byNorm = Quaternion.FromToRotation (Vector3.forward, palm_norm);
							Vector3 finger_dir = new Vector3 (float.Parse (hand_info [i++]), float.Parse (hand_info [i++]), float.Parse (hand_info [i++]));
							Quaternion palm_rot_byDir = Quaternion.FromToRotation (Vector3.up, finger_dir);
							bone.transform.localRotation = palm_rot_byDir;
						}
					}
				}
			} else {
				i++;
			}
		}

		transform.position = Camera.main.transform.position + Camera.main.transform.rotation * new Vector3(0.02f, -0.09f, -0.01f);
		transform.rotation = Camera.main.transform.rotation;
	}

	/* Bone Mapping functions, do not modify 
	   unless you are sure what it is */
	GameObject getBoneFromFinger(int fingerIndex, int bone){
		GameObject b = l_finger0_bone0;
		switch (fingerIndex) {
		/* finger 0 */
		case 0:
			switch (bone) {
			case 0:
				b = l_finger0_bone0;
				break;
			case 1:
				b = l_finger0_bone1;
				break;
			case 2:
				b = l_finger0_bone2;
				break;
			}
			break;

			/* finger 0 */
		case 1:
			switch (bone) {
			case 0:
				b = l_finger1_bone0;
				break;
			case 1:
				b = l_finger1_bone1;
				break;
			case 2:
				b = l_finger1_bone2;
				break;
			}
			break;

			/* finger 0 */
		case 2:
			switch (bone) {
			case 0:
				b = l_finger2_bone0;
				break;
			case 1:
				b = l_finger2_bone1;
				break;
			case 2:
				b = l_finger2_bone2;
				break;
			}
			break;

			/* finger 0 */
		case 3:
			switch (bone) {
			case 0:
				b = l_finger3_bone0;
				break;
			case 1:
				b = l_finger3_bone1;
				break;
			case 2:
				b = l_finger3_bone2;
				break;
			}
			break;

			/* finger 0 */
		case 4:
			switch (bone) {
			case 0:
				b = l_finger4_bone0;
				break;
			case 1:
				b = l_finger4_bone1;
				break;
			case 2:
				b = l_finger4_bone2;
				break;
			}
			break;

			/* default */
		default:
			b = l_finger0_bone0;
			break;
		}

		return b;
	}

	GameObject getFinger(int index){
		GameObject f;
		switch (index) {
		case 0:
			f = l_finger0;
			break;
		case 1:
			f =  l_finger1;
			break;
		case 2:
			f = l_finger2;
			break;
		case 3:
			f =  l_finger3;
			break;
		case 4:
			f =  l_finger4;
			break;
		default:
			f = l_finger0;
			break;
		}
		return f;
	}
}