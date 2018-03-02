using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintFeature : MonoBehaviour {

	//hand objects declared
	private GameObject palm, indexfinger;

	//Painter feature global variables
	private LineRenderer ink;
	private bool isPainting = false;
	private float dist_thumb_index_initial;
	private bool canPaint = false;

	// Use this for initialization
	void Start () {
		indexfinger = this.transform.GetChild (1).GetChild (2).gameObject;
		palm = this.transform.GetChild (5).gameObject;
		dist_thumb_index_initial = Vector3.Distance(indexfinger.transform.position, palm.transform.position);
		ink = GameObject.Find ("Ink").GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!canPaint) {
			return;
		}

		GestureControl gesture = this.GetComponent<GestureControl> ();

		//Paint feature
		if (gesture.Pose) {
			if (!isPainting) {
				isPainting = true;
				ink.numCornerVertices = 3;
				ink.numCapVertices = 3;
			} else {
				Vector3 newPoint = new Vector3 ();
				newPoint = indexfinger.transform.position;
				ink.positionCount++;
				ink.SetPosition (ink.positionCount - 1, newPoint);
			}
		} else if (isPainting){
			isPainting = false;
			ink = Instantiate (ink);
			// clear all drawing points
			ink.positionCount = 0;
			//ink.SetPosition (0, new Vector3(0,0,0));
		}
		/*}else{
			isPainting = false;
			ink.positionCount = 0;
		}*/
	}

	public void setCanPaint(bool cp) {
		canPaint = cp;
	}

	/*	Check pose pointing
 	*	Input: GameObject
	*	Output: Boolean 
	*/
	private bool checkPosePointing(){
		float dist_thumb_index_current = Vector3.Distance(indexfinger.transform.position, palm.transform.position);
		if (dist_thumb_index_current > (dist_thumb_index_initial - 0.023f))
			return true;

		return false;
	}
}
