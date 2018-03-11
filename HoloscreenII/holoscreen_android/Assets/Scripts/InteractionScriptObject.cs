using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionScriptObject : MonoBehaviour {
	Dictionary<string, int> hand_nearby = new Dictionary<string, int>();
	int hand_l,hand_r;
	private bool isGrabbed = false;
	private DataManager dataManager;
	public Material primaryMaterial;
	public Material secondaryMaterial;
	// Use this for initialization
	void Start () {
		dataManager = GameObject.Find ("gDataManager").GetComponent<DataManager> ();
		hand_nearby.Add("Hand_l",0);
		hand_nearby.Add("Hand_r",0);
		primaryMaterial = this.GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isGrabbed){
			if (hand_nearby.TryGetValue("Hand_l", out hand_l) && hand_l>0){
				if (!dataManager.checkLeftHandBusy()){
					Debug.Log("set isGrabbed: true");
					dataManager.setLeftHandObject(this.gameObject);
					isGrabbed = true;
					highlightSelf();
				}
			}else if (hand_nearby.TryGetValue("Hand_r", out hand_r) && hand_r>0 && (!dataManager.checkRightHandBusy()) ){
				//finish it if we need right hand
			}
		}
	}
	public void resetIsGrabbed(){
		isGrabbed = false;
		unhighlightSelf();
	}

	private void highlightSelf(){
		secondaryMaterial.mainTexture = primaryMaterial.mainTexture;
		this.GetComponent<Renderer> ().material = secondaryMaterial;
	}

	private void unhighlightSelf(){
		this.GetComponent<Renderer> ().material = primaryMaterial;
	}
	void OnTriggerEnter(Collider other){
		if (other.transform.parent != null){
			if (other.transform.parent.parent != null)
				if (other.transform.parent.parent.name == "Hand_l" || other.transform.parent.parent.name == "Hand_r")
					hand_nearby[other.transform.parent.parent.name] += 1;
				//Debug.Log (other.transform.parent.parent.name);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.transform.parent != null) {
			if (other.transform.parent.parent != null) {
				if (other.transform.parent.parent.name == "Hand_l" || other.transform.parent.parent.name == "Hand_r")
					hand_nearby [other.transform.parent.parent.name] -= 1;
				//Debug.Log (other.transform.parent.parent.name);
			}
		}
	}
}
