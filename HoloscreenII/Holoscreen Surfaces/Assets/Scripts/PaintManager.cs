using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour {

	//hand objects declared
	private GameObject palm, indexfinger_tip;
	private GestureControl gestureManager;

	//Painter feature global variables
	private LineRenderer ink, new_ink;
	private bool is_painting, paint_mode = false;
	private GameObject ink_group;

	// Use this for initialization
	void Start () {
		indexfinger_tip = this.transform.GetChild (1).GetChild (2).gameObject;
		palm = this.transform.GetChild (5).gameObject;
		if (this.name == "Hand_l")
			ink = GameObject.FindGameObjectWithTag ("Ink_l").GetComponent<LineRenderer>();
		else
			ink = GameObject.FindGameObjectWithTag ("Ink_r").GetComponent<LineRenderer>();
		ink_group = GameObject.Find ("Ink_group");
		gestureManager = this.GetComponent<GestureControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (paint_mode);

		//Paint feature
		if (paint_mode) {
			if (gestureManager.bufferedGesture () == "paint") {
				if (!is_painting) {
					new_ink = Instantiate (ink);
					new_ink.GetComponent<Renderer> ().material = Instantiate (new_ink.GetComponent<Renderer> ().material) as Material;
					new_ink.gameObject.transform.SetParent (ink_group.gameObject.transform);
					new_ink.positionCount = 0;
					new_ink.numCornerVertices = 5;
					new_ink.numCapVertices = 5;
					is_painting = true;
				} else {
					Vector3 newPoint = new Vector3 ();
					newPoint = indexfinger_tip.transform.position;
					new_ink.positionCount++;
					new_ink.SetPosition (new_ink.positionCount - 1, newPoint);
				}
			} else {
				is_painting = false;
			}
		}
	}

	public void cleanInk(){
		if (ink_group.transform.childCount > 0) {
			foreach (Transform child in ink_group.transform) {
				GameObject.Destroy (child.gameObject);
			}
		}
	}

	public void turnOnPaint(){
		paint_mode = true;
	}

	public void turnOffPaint(){
		cleanInk ();
		paint_mode = false;
	}
}
