using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used on collider of an object which can be grabbed by user.
/// </summary>
public class GrabCollider : MonoBehaviour {
	[SerializeField]
	private Transform BindObject;		// The object bind to this collider.
	[SerializeField]
	[Range(1.0f, 3.0f)]
	private float ExpandScale = 1.2f;			// The scale that the collider will expand if it's entered.

	private int LeftHandFingerIn = 0;		// How many left hand's fingers are in the collider right now.

	private const int CHandFingerThreshold = 3;		// How many fingers in it can trigger a state switch.

	private enum GRABCOLLIDER_STATE {
		TO_ENTER,
		TO_EXIT
	}

	GRABCOLLIDER_STATE State;

	// Use this for initialization
	void Start () {
		// make sure this object has a collider
		Collider cd = GetComponent<Collider>();
		if (cd == null) {
			gameObject.SetActive (false);
			return;
		}

		// check if the collider is triggered.
		if (cd.isTrigger == false) {
			cd.isTrigger = true;
		}

		// if band object is null, defaultly set it to its parent
		if (BindObject == null) {
			BindObject = transform.parent;
		}

		State = GRABCOLLIDER_STATE.TO_ENTER;
		LeftHandFingerIn = 0;
	}

	// Receive Trigger Message
	void OnTriggerEnter(Collider other) {
		// Don't want palm
		if (other.name == "palm")
			return;
		
		if (other.transform.parent.parent.name == "Hand_l") {
			LeftHandFingerIn++;
		}

		// If it's not waiting for enter, just ignore it.
		if (State != GRABCOLLIDER_STATE.TO_ENTER)
			return;

		if (LeftHandFingerIn >= CHandFingerThreshold) {
			// Tell it to be grabbed
			if (BindObject != null) {
				// Try get hand manager
				HandManager hm = other.transform.parent.parent.GetComponent<HandManager>();
				if (hm != null && !hm.checkHandBusy()) {
					// High light it
					BindObject.GetComponent<Renderer>().material.color = Color.blue;
					hm.setHandObject (BindObject.gameObject);
				}
			}

			SwitchToReadyExit ();
		}
	}

	// Receive Trigger Message
	void OnTriggerExit (Collider other) {
		if (other.name == "palm")
			return;
		
		if (other.transform.parent.parent.name == "Hand_l") {
			LeftHandFingerIn--;
		}
		if (State == GRABCOLLIDER_STATE.TO_EXIT && LeftHandFingerIn < CHandFingerThreshold) {
			HandManager hm = GameObject.Find ("Hand_l").GetComponent<HandManager> ();
			if (hm != null && !hm.IsGrabbing) {
				BindObject.GetComponent<Renderer> ().material.color = Color.red;
				ReleaseSelf ();
				SwitchToReadyEnter ();
			}
		}
	}

	// Exit Grab State
	public void ReleaseSelf() {
		HandManager hm = GameObject.Find ("Hand_l").GetComponent<HandManager> ();
		if (hm != null) {
			hm.removeHandObject();
		}
	}

	// Called when User grab this and then release this
	public void OnGrabFinished() {
		ReleaseSelf ();
		if (State == GRABCOLLIDER_STATE.TO_EXIT && LeftHandFingerIn < CHandFingerThreshold) {
			BindObject.GetComponent<Renderer> ().material.color = Color.red;
			SwitchToReadyEnter ();
		}
		else {
			// TODO: if you like, you could add hm.setHandobject here, so that user can release an object then grab it again immediately.
		}
	}

	private void SwitchToReadyExit() {
		State = GRABCOLLIDER_STATE.TO_EXIT;
		transform.localScale = transform.localScale * ExpandScale;
	}

	private void SwitchToReadyEnter() {
		State = GRABCOLLIDER_STATE.TO_ENTER;
		transform.localScale = transform.localScale / ExpandScale;
	}
}
