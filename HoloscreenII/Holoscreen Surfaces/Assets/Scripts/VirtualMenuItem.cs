using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualMenuItem : MonoBehaviour {

	protected VirtualMenu m_menu = null;

	protected GameObject hand_l, thumb_l, indexfinger_l, middlefinger_l, ringfinger_l, palm_l;
	protected bool m_collidingWithHand = false;

	private Shader m_primaryShader;
	private Shader m_secondaryShader;
	private string m_lastGesture = "";

	// Use this for initialization
	protected virtual void Start () {
		// Get shaders
		m_primaryShader = this.GetComponent<Renderer>().material.shader;
		m_secondaryShader = Shader.Find ("ModelEffect/VerticsOutline_Always");

		// Get hand objects
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
			// Detect collision
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

			// Detect grab
			if (m_collidingWithHand) {
				string curGesture = hand_l.GetComponent<GestureControl> ().bufferedGesture ();

				if (curGesture != m_lastGesture && curGesture == "pinch") {
					onHandGrab ();
				}
			}

			// Reset last gesture
			m_lastGesture = hand_l.GetComponent<GestureControl> ().bufferedGesture ();
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

	private void highlightMaterial(GameObject g) {
		g.GetComponent<Renderer> ().material.shader = m_secondaryShader;
		g.GetComponent<Renderer> ().material.SetColor ("_OutlineColor", new Color (0, 248.0f / 256.0f, 63.0f / 256.0f, 143.0f / 256.0f));
		g.GetComponent<Renderer> ().material.SetFloat ("_OutlineWidth", 0.001f);
	}

	private void unhighlightMaterial(GameObject g) {
		g.GetComponent<Renderer> ().material.shader = m_primaryShader;
	}

	public virtual void onHandIn() {
		highlightMaterial (this.gameObject);

		foreach (Transform child in transform) {
			highlightMaterial (child.gameObject);
		}
	}

	public virtual void onHandOut() {
		unhighlightMaterial (this.gameObject);

		foreach (Transform child in transform) {
			unhighlightMaterial (child.gameObject);
		}
	}

	public virtual void onHandGrab() {

	}
}
