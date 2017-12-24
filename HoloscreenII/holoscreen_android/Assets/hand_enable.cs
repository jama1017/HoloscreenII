using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hand_enable : MonoBehaviour {
	public WSManager ws;
	// Use this for initialization
	void Start () {
		ws = GameObject.Find ("WebsocketManager").GetComponent<WSManager>();
	}
	
	// Update is called once per frame
	void Update () {
		string msg = "";
		msg = ws.getHandInfoLeft ();
		if (!msg.Equals ("") ) {
			if (GameObject.Find ("Hand_l").gameObject.activeInHierarchy)
				GameObject.Find ("Hand_l").gameObject.SetActive (true);
		}
			
		msg = ws.getHandInfoRight ();
		if (!msg.Equals ("")) {
			if (GameObject.Find ("Hand_r").gameObject.activeInHierarchy)
				GameObject.Find ("Hand_r").gameObject.SetActive (true);
		}
	}
}
