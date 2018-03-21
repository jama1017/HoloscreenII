using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour {

	//hand objects declared
	private GameObject palm, indexfinger_tip;
	private GestureControl gestureManager;

	//Painter feature global variables
	private LineRenderer ink, new_ink;
	private bool is_painting, paint_mode, clean_trail = false;
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
					new_ink.numCornerVertices = 20;
					new_ink.numCapVertices = 20;
					is_painting = true;
					clean_trail = false;
				} else {
					/* user is painting */
					Vector3 newPoint = new Vector3 ();
					newPoint = indexfinger_tip.transform.position;
		
					//add here
					if (new_ink.positionCount > 3) {
						if (Vector3.Distance (newPoint, new_ink.GetPosition (new_ink.positionCount - 2)) > 0.003f) {
							new_ink.positionCount++;
							new_ink.SetPosition (new_ink.positionCount - 1, newPoint);
						}
					} 
					else
					{
						new_ink.positionCount++;
						new_ink.SetPosition (new_ink.positionCount - 1, newPoint);
					}
				}
			} else {
				if (new_ink.positionCount > 10 && clean_trail == false) {
					new_ink.positionCount -= 10;
					clean_trail = true;
				}
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
