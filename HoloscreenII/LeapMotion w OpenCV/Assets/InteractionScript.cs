using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractionScript : MonoBehaviour {
	/**
	 * Child indexes of hand represent:
	 * 0 - thumb
	 * 1 - index
	 * 2 - middle
	 * 3 - pinky
	 * 4 - ring
	 * 5 - palm for another hand
	 * 6- forearm
	 */
	public GameObject hand_l,hand_r;

	//Left hand finger declare
	private GameObject thumb_l, indexfinger_l, middlefinger_l, ringfinger_l, palm_l;
	private GameObject thumb_l_2, indexfinger_l_2, middlefinger_l_2, ringfinger_l_2;
	//Right hand finger declare
	private GameObject thumb_r, indexfinger_r, middlefinger_r, ringfinger_r, palm_r;
	private GameObject thumb_r_2, indexfinger_r_2, middlefinger_r_2, ringfinger_r_2;

	//Boolean var to record current grab motion
	bool grabbed = false;

	private int sizeOfSpeedQ = 10; 
	private Queue<Vector3> speedList = new Queue<Vector3>();
	private float colliderReenableTime = 1.5f;
	private float add;
	static float restoreColliderTimer;

	// Use this for initialization
	void Start () {
		//0.21 finger to palm

		for (int i=0; i < sizeOfSpeedQ; i++)
			speedList.Enqueue(new Vector3(0,0,0));
		
		thumb_l = hand_l.transform.GetChild (0).gameObject;
		indexfinger_l = hand_l.transform.GetChild (1).gameObject;
		middlefinger_l = hand_l.transform.GetChild (2).gameObject;
		ringfinger_l = hand_l.transform.GetChild (4).gameObject;
		palm_l = hand_l.transform.GetChild (5).gameObject;

		thumb_l_2 = thumb_l.transform.GetChild (2).gameObject;
		indexfinger_l_2 = indexfinger_l.transform.GetChild (2).gameObject;
		middlefinger_l_2 = middlefinger_l.transform.GetChild (2).gameObject;
		ringfinger_l_2 = ringfinger_l.transform.GetChild (2).gameObject;

		thumb_r = hand_r.transform.GetChild (0).gameObject;
		indexfinger_r = hand_r.transform.GetChild (1).gameObject;
		middlefinger_r = hand_r.transform.GetChild (2).gameObject;
		ringfinger_r = hand_r.transform.GetChild (4).gameObject;
		palm_r = hand_r.transform.GetChild (5).gameObject;

		thumb_r_2 = thumb_r.transform.GetChild (2).gameObject;
		indexfinger_r_2 = indexfinger_r.transform.GetChild (2).gameObject;
		middlefinger_r_2 = middlefinger_r.transform.GetChild (2).gameObject;
		ringfinger_r_2 = ringfinger_r.transform.GetChild (2).gameObject;
	}

	//Simplified version of code


	// Update is called once per frame
	void Update () {
		//Bounce prevetion mechinism
		this.GetComponent<Rigidbody> ().velocity = new Vector3(Math.Min(this.GetComponent<Rigidbody> ().velocity.x, 1f),Math.Min(this.GetComponent<Rigidbody> ().velocity.y, 1f),Math.Min(this.GetComponent<Rigidbody> ().velocity.z, 1f));
		this.GetComponent<Rigidbody> ().velocity = new Vector3(Math.Max(this.GetComponent<Rigidbody> ().velocity.x, -1f),Math.Max(this.GetComponent<Rigidbody> ().velocity.y, -1f),Math.Max(this.GetComponent<Rigidbody> ().velocity.z, -1f));
		//Debug.Log (this.GetComponent<Rigidbody> ().velocity);

		//Grab Detection: 
		//With rigidbody
		Collider c = this.GetComponent<Collider>();
		float dist_thumb_index_l = Vector3.Distance(thumb_l_2.transform.position, indexfinger_l_2.transform.position);
		float dist_thumb_index_r = Vector3.Distance(thumb_r_2.transform.position, indexfinger_r_2.transform.position);

		//float dist_palm_index_l = Vector3.Distance(palm_l.transform.position, indexfinger_l_2.transform.position);
		//float dist_palm_middle_l = Vector3.Distance(palm_l.transform.position, middlefinger_l_2.transform.position);
		//float dist_palm_ring_l = Vector3.Distance(palm_l.transform.position, ringfinger_l_2.transform.position);
		//float dist_palm_thumb_l = Vector3.Distance(palm_l.transform.position, thumb_l_2.transform.position);


		//Method 3(w/o rigidBoy of object): The fingers' collider interesect with the object

		//use vector to help detect grab
		/*
		Vector3 index_thumb1 = indexfinger_l.transform.position - thumb_l_0.transform.position;
		Vector3 thumb_thumb1 = thumb_l_2.transform.position - thumb_l_0.transform.position;
		float angle = Vector3.Angle(index_thumb1, thumb_thumb1);
		Vector3 index_thumb1_r = indexfinger_r_2.transform.position - thumb1_r.transform.position;
		Vector3 thumb_thumb1_r = thumb_r_2.transform.position - thumb1_r.transform.position;
		float angle_r = Vector3.Angle(index_thumb1_r, thumb_thumb1_r);
		
		*/

		Vector3 indexfinger_0_palm_r = indexfinger_r.transform.GetChild(0).position - palm_r.transform.position;
		Vector3 indexfinger_2_thumb1_r = indexfinger_r.transform.GetChild(2).position - palm_r.transform.position;
		float curve_indexfinger_r = Vector3.Angle(indexfinger_2_thumb1_r, indexfinger_0_palm_r);
		//Debug.Log (curve_indexfinger_r);
		if (c.bounds.Intersects (thumb_r_2.GetComponent<Collider> ().bounds)) {
			for (int i = 0; i < 3; i++) {
				thumb_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
			}
		} else {
			for (int i = 0; i < 1; i++) {
				thumb_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
			}
		}

		if (c.bounds.Intersects (indexfinger_r_2.GetComponent<Collider> ().bounds)) {
			for (int i = 0; i < 3; i++) {
				indexfinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
			}
		} else {
			for (int i = 0; i < 1; i++) {
				indexfinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
			}
		}

		if (c.bounds.Intersects (middlefinger_r_2.GetComponent<Collider> ().bounds)) {
			for (int i = 0; i < 3; i++) {
				middlefinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
			}
		} else {
			for (int i = 0; i < 1; i++) {
				middlefinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
			}
		}

		if (c.bounds.Intersects (ringfinger_r_2.GetComponent<Collider> ().bounds)) {
			for (int i = 0; i < 3; i++) {
				ringfinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
			}
		} else {
			for (int i = 0; i < 1; i++) {
				ringfinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
			}
		}
		

		//Determine whethere a grab happnens. Left hand has priority here.
		if ((dist_thumb_index_l < 0.085) && 
			c.bounds.Intersects (thumb_l_2.GetComponent<Collider> ().bounds) &&
			(c.bounds.Intersects (indexfinger_l_2.GetComponent<Collider> ().bounds) ||
				c.bounds.Intersects (middlefinger_l_2.GetComponent<Collider> ().bounds) ||
				c.bounds.Intersects (ringfinger_l_2.GetComponent<Collider> ().bounds))) {

			//Record current velocity and delete the oldest velocity
			this.transform.parent = palm_l.transform;
			Vector3 initial_v = new Vector3 ();
			initial_v = palm_l.GetComponent<Rigidbody> ().velocity;
			speedList.Dequeue ();
			speedList.Enqueue (initial_v);

			grabbed = true;
			restoreColliderTimer = Time.time;

		} else if (dist_thumb_index_r < 0.060 && (curve_indexfinger_r>15) &&
			(c.bounds.Intersects (thumb_r_2.GetComponent<Collider> ().bounds) || c.bounds.Contains (thumb_r_2.transform.position)) &&
			(/*(c.bounds.Contains (indexfinger_r_2.transform.position) ||
				c.bounds.Contains (middlefinger_r_2.transform.position) ||
				c.bounds.Contains (ringfinger_r_2.transform.position)) ||*/

				(c.bounds.Intersects (indexfinger_r_2.GetComponent<Collider> ().bounds) ||
					c.bounds.Intersects (middlefinger_r_2.GetComponent<Collider> ().bounds) ||
					c.bounds.Intersects (ringfinger_r_2.GetComponent<Collider> ().bounds)))) {
			this.GetComponent<Rigidbody> ().isKinematic = true;
			this.transform.parent = palm_r.transform;
			Vector3 initial_v = new Vector3 ();
			initial_v = palm_r.GetComponent<Rigidbody> ().velocity;
			speedList.Dequeue ();
			speedList.Enqueue (initial_v);
			grabbed = true;
			for (int i = 0; i < 2; i++) {
				//indexfinger_l.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
				//thumb_l.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
				//middlefinger_l.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
				palm_l.GetComponent<Collider> ().isTrigger = true;

				//indexfinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
				//thumb_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
				//middlefinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
				palm_r.GetComponent<Collider> ().isTrigger = true;
			}
			restoreColliderTimer = Time.time;

		} else if (grabbed == true) {
			grabbed = false;
			this.transform.parent = null;
			this.GetComponent<Rigidbody> ().isKinematic = false;
			int num_speed = speedList.Count;
			Vector3 average = new Vector3 (0, 0, 0);
			for (int i = 0; i < sizeOfSpeedQ; i++) {
				average += speedList.Dequeue ();
				speedList.Enqueue (new Vector3 (0, 0, 0));
			}
			this.GetComponent<Rigidbody> ().velocity = 0.9f * (average / num_speed);
			restoreColliderTimer = Time.time;

		} else {
			float diff = Time.time - restoreColliderTimer;

			//Wait a certain interval to re-enable the collider of the hand
			if (diff > colliderReenableTime) {
				//Debug.Log("current restoreTime = " + restoreColliderTimer);
				//Debug.Log("Difference : " + diff);
				for (int i = 0; i < 2; i++) {
					//indexfinger_l.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
					//thumb_l.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
					//middlefinger_l.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
					palm_l.GetComponent<Collider> ().isTrigger = false;

					//indexfinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
					//thumb_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
					//middlefinger_r.transform.GetChild (i).GetComponent<Collider> ().isTrigger = false;
					palm_r.GetComponent<Collider> ().isTrigger = false;
				}
			}
		}

		//Hover feature 1
		/*
		float dist_obj_palm_l = Vector3.Distance(this.transform.position, palm_l.transform.position);
		float dist_obj_palm_r = Vector3.Distance(this.transform.position, palm_r.transform.position);
		float threshold = 0.25f;
		if (dist_obj_palm_l < threshold || dist_obj_palm_r < threshold) {
			this.GetComponent<Renderer> ().material.SetFloat ("_Metallic", (threshold - Mathf.Min (dist_obj_palm_l, dist_obj_palm_r)) / threshold);
		}
		else{
			this.GetComponent<Renderer> ().material.SetFloat ("_Metallic", 0);
		}
		*/

		//Hover feature 2
		float dist_obj_palm_l = Mathf.Abs(Vector3.Distance(this.transform.position, palm_l.transform.position));
		float dist_obj_palm_r = Mathf.Abs(Vector3.Distance(this.transform.position, palm_r.transform.position));
		float threshold = 0.25f;
		if ((dist_obj_palm_l < threshold || dist_obj_palm_r < threshold)) {
			//Map distance to color
			float percent = Mathf.Pow(Mathf.Min((threshold - Mathf.Min (dist_obj_palm_l, dist_obj_palm_r)) / (threshold/2), 1), 2);
			float max_brightness = 0.35f;
			float min_brightness = 0f;
			//Debug.Log (percent);

			if (this.GetComponent<Renderer> ().material.GetColor ("_EmissionColor").r > max_brightness) {
				add = -0.03f;
			} else if (this.GetComponent<Renderer> ().material.GetColor ("_EmissionColor").r <= min_brightness){
				add = 0.03f;
			}
			Debug.Log (add);
			Color add_color = new Vector4 (add, add, add, 0f);
			Color cur_color =  this.GetComponent<Renderer> ().material.GetColor("_EmissionColor") + percent*add_color;
			this.GetComponent<Renderer> ().material.SetColor("_EmissionColor", cur_color);

			//Debug.Log ( this.GetComponent<Renderer> ().material.GetColor("_EmissionColor"));
		}else{
			Color cur_color =  new Vector4 (0f, 0f, 0f, 0f);
			this.GetComponent<Renderer> ().material.SetColor("_EmissionColor", cur_color);
		}
		;

	}

	private void disableCollider(GameObject finger){
		for (int i = 0; i < 2; i++) {
			finger.transform.GetChild (i).GetComponent<Collider> ().isTrigger = true;
			Debug.Log (finger.transform.GetChild (i).name);
		}
		return;
	}

	private void enableCollider(GameObject finger){
		for (int i=0; i<2; i++)
			finger.transform.GetChild (i).GetComponent<Collider>().isTrigger = false;
		return;
	}
}
