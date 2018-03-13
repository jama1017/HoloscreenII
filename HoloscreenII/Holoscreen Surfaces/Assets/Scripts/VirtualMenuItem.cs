using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMenuItem : MonoBehaviour {

	protected VirtualMenu m_menu = null;

	protected GameObject hand_l, thumb_l, indexfinger_l, middlefinger_l, ringfinger_l, palm_l;
	protected bool m_collidingWithHand = false;

	// Use this for initialization
	protected virtual void Start () {
		hand_l = GameObject.Find ("Hand_l").gameObject;

		thumb_l = hand_l.transform.GetChild (0).gameObject;
		indexfinger_l = hand_l.transform.GetChild (1).gameObject;
		middlefinger_l = hand_l.transform.GetChild (2).gameObject;
		ringfinger_l = hand_l.transform.GetChild (4).gameObject;
		palm_l = hand_l.transform.GetChild (5).gameObject;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if (this.transform.parent != null) {
			m_menu = this.transform.parent.gameObject.GetComponent<VirtualMenu>();
		}

		if (m_menu != null) {
			if (!m_collidingWithHand) {
				m_collidingWithHand = collidesWithHand ();

				if (m_collidingWithHand) {
					onHandIn ();
				}
			} else {
				m_collidingWithHand = collidesWithHand ();

				if (!m_collidingWithHand) {
					onHandOut ();
				}
			}
		}
	}

	bool collidesWithHand() {
		return collidesWithFinger (thumb_l) ||
			collidesWithFinger (indexfinger_l) ||
			collidesWithFinger (middlefinger_l) ||
			collidesWithFinger (ringfinger_l) ||
			collidesWithObject (palm_l);
	}

	bool collidesWithFinger(GameObject finger) {
		for (int i = 0; i < 3; i++) {
			if (collidesWithObject(finger.transform.GetChild (i).gameObject)) {
				return true;
			}
		}

		return false;
	}

	bool collidesWithObject(GameObject o) {
		Collider c = this.GetComponent<Collider>();

		return c.bounds.Intersects (o.GetComponent<Collider> ().bounds);
	}

	public virtual void onHandIn() {
		
	}

	public virtual void onHandOut() {

	}
}
