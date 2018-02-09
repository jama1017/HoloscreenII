using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintFeature : MonoBehaviour {

	//Left hand finger declare
	private GameObject palm, indexfinger;

	//Painter feature global variables
	private LineRenderer ink;
	private bool isPainting = false;
	private float dist_thumb_index_initial;

	// Use this for initialization
	void Start () {
		indexfinger = this.transform.GetChild (1).GetChild (2).gameObject;
		palm = this.transform.GetChild (5).gameObject;
		dist_thumb_index_initial = Vector3.Distance(indexfinger.transform.position, palm.transform.position);
		ink = GameObject.Find ("Ink").GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (indexfinger.transform.position);
		//Paint feature
		if (checkPosePointing()){
			if (!isPainting) {
				isPainting = true;
			} else {
				Vector3 newPoint = new Vector3 ();
				newPoint = indexfinger.transform.position;
				ink.positionCount++;
				ink.SetPosition (ink.positionCount-1, newPoint);
			}
		}else{
			isPainting = false;
			ink.positionCount = 0;
			//ink.SetPosition (0, new Vector3(0,0,0));
		}
	}

	/*	Check pose pointing
 	*	Input: GameObject
	*	Output: Boolean 
	*/
	private bool checkPosePointing(){
		float dist_thumb_index_current = Vector3.Distance(indexfinger.transform.position, palm.transform.position);
		//Debug.Log (dist_thumb_index_initial+ "" + dist_thumb_index_current);
		if (dist_thumb_index_current > (dist_thumb_index_initial - 0.021f))
			return true;

		return false;
	}
}
