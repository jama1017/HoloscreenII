using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScriptHand : MonoBehaviour {
	//Left hand has priority
	private DataManager dataManager;
	private GameObject grabHolder;

	// Use this for initialization
	void Start () {
		dataManager = GameObject.Find ("gDataManager").GetComponent<DataManager> ();
		grabHolder = this.transform.GetChild (5).GetChild (0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		GameObject interact_obj = dataManager.getLeftHandObject();
		if (interact_obj != null){
			//Debug.Log("has object to interact: " + interact_obj.name);
			//hightligh interact obj
			dataManager.setLeftHandBusy(true);

			//Grab a object if hand gesture is grabbing
			if (isGrabGesture()){
				//Debug.Log("Grabbing: " + interact_obj.name);
				grabObject(interact_obj);
			}else{
				releaseObject(interact_obj);
				dataManager.setLeftHandBusy(false);
			}
		}
	}

	private bool isGrabGesture(){
		GameObject thumb_2 = this.transform.GetChild (0).GetChild (2).gameObject;
		GameObject indexfinger_2 = this.transform.GetChild (1).GetChild (2).gameObject;
		float dist_thumb_index = Vector3.Distance(thumb_2.transform.position, indexfinger_2.transform.position);
		if (dist_thumb_index < 0.070){
			return true;
		}
		return false;
	}
	private void grabObject(GameObject obj){
		obj.GetComponent<Collider> ().isTrigger = true;
		obj.GetComponent<Rigidbody> ().useGravity = false;
		obj.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		obj.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		obj.GetComponent<Rigidbody> ().Sleep ();
		obj.transform.SetParent(grabHolder.transform);
		
	}
	private void releaseObject(GameObject obj){
		obj.transform.parent = null;
		obj.GetComponent<Rigidbody>().useGravity = true;
		obj.GetComponent<Collider> ().isTrigger = false;
		obj.GetComponent<Rigidbody> ().velocity = new Vector3 (0, -2, 0);
	}

	private void collectHandSpeed(){
		return;
	}

}
