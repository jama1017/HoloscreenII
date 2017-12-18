using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObjectController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GameObject marker = transform.parent.gameObject;

		// Set transform to identity
		Quaternion prevRotation = marker.transform.rotation;
		Vector3 prevScale = marker.transform.localScale;
		Vector3 prevPosition = marker.transform.position;
		marker.transform.rotation = new Quaternion();
		marker.transform.localScale = new Vector3(1, 1, 1);
		marker.transform.position = new Vector3(0, 0, 0);

		// Get bounds
		Bounds bounds = marker.GetComponent<Renderer>().bounds;
		transform.localScale = new Vector3(bounds.size.x, bounds.size.y, 1);

		// Restore transform
		marker.transform.rotation = prevRotation;
		marker.transform.localScale = prevScale;
		marker.transform.position = prevPosition;
	}
}
