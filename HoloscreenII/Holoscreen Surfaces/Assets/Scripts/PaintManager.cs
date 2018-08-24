using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour {

	//hand objects declared (thumb added)
	private GameObject palm, indexfinger_tip, thumb_tip;
	private GestureControl gestureManager;

	//Painter feature global variables
	private LineRenderer ink, new_ink;
	private bool is_painting, paint_mode, clean_trail = false;
	private bool eraser_mode = false;
	private GameObject ink_group;

	//-----JM additions------
	//inkList for erasing and highlighting
	private List<LineRenderer> inkList;
	//normal and highlight material
	private Material[] normalMat;
	private Material[] highlightMat;

	//animation curve for changing width
	private AnimationCurve curve;
	private float curveWidth = 0.3f; //curveWidth multiplier
	private float currWidth; //stroke width mode control

	//width float
	private float thin = 0.015f;
	private float mid = 0.028f;
	private float thick = 0.041f;

	//undo/redo list
	private Stack<PaintCommand> undoStack = new Stack<PaintCommand>();
	private Stack<PaintCommand> redoStack = new Stack<PaintCommand>();

	// Use this for initialization
	void Start () {
		//list of inks
		inkList = new List<LineRenderer> ();
		normalMat = new Material[1];

		//curr width
		currWidth = 0;

		indexfinger_tip = this.transform.GetChild (1).GetChild (2).gameObject;
		//added thumb tip
		thumb_tip = this.transform.GetChild(0).GetChild(2).gameObject;

		palm = this.transform.GetChild (5).gameObject;

		if (this.name == "Hand_l") {
			ink = GameObject.FindGameObjectWithTag ("Ink_l").GetComponent<LineRenderer> ();

			//swap material back to normal
			highlightMat = ink.materials;
			normalMat[0] = highlightMat[0];
			ink.materials = normalMat;

		} else {
			ink = GameObject.FindGameObjectWithTag ("Ink_r").GetComponent<LineRenderer> ();
		}

		ink_group = GameObject.Find ("Ink_group");
		gestureManager = this.GetComponent<GestureControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (paint_mode);

		//Paint feature
		if (paint_mode) {
			//if paint mode and not erasing
			if (!eraser_mode) {
				if (gestureManager.bufferedGesture () == "pinch") {
					if (!is_painting) {
					
						this.createNewInk ();

						new_ink.widthCurve = new AnimationCurve (new Keyframe (0f, thin), new Keyframe (0f, thin));
						new_ink.widthMultiplier = curveWidth;
						currWidth = 0;

						is_painting = true;
						clean_trail = false;
					} else {
						/* user is painting */
						Vector3 newPoint = new Vector3 ();

						newPoint = (indexfinger_tip.transform.position + thumb_tip.transform.position) / 2;

						this.adjustWidth (new_ink, Vector3.Distance (indexfinger_tip.transform.position, thumb_tip.transform.position));

						//add here
						if (new_ink.positionCount > 3) {
							if (Vector3.Distance (newPoint, new_ink.GetPosition (new_ink.positionCount - 2)) > 0.003f) {
								new_ink.positionCount++;
								new_ink.SetPosition (new_ink.positionCount - 1, newPoint);
							}
						} else {
							new_ink.positionCount++;
							new_ink.SetPosition (new_ink.positionCount - 1, newPoint);
						}
						
					}
				} else if (gestureManager.bufferedGesture () == "palm") {

					if (new_ink.positionCount > 10 && clean_trail == false) {
						new_ink.positionCount -= 10;
						clean_trail = true;
					}

					is_painting = false;

					//this.highlight(palm.transform.position);
					//this.erase (palm.transform.position);
				}
			}
		}


		Debug.Log ("eraser is erasing? " + eraser_mode);

		if (eraser_mode) {
			Debug.Log ("eraser erasing " + eraser_mode);

			this.highlight (palm.transform.position);
			this.erase (palm.transform.position);
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
		//added JM for eraser
	}

	public void turnOffPaint(){
		//cleanInk ();
		paint_mode = false;
	}

	// for eraser icon button
	public void turnOnEraser(){
		eraser_mode = true;
		Debug.Log ("eraser on " + eraser_mode);
	}

	public void turnOffEraser() {
		eraser_mode = false;
	}

	private void createNewInk(){
		
		new_ink = Instantiate (ink);
		//new_ink.GetComponent<Renderer> ().material = Instantiate (new_ink.GetComponent<Renderer> ().material) as Material;
		new_ink.gameObject.transform.SetParent (ink_group.gameObject.transform);
		new_ink.positionCount = 0;
		new_ink.numCornerVertices = 20;
		new_ink.numCapVertices = 20;

		inkList.Add (new_ink);

		//add to undo stack
		PaintCommand strokePaint = new StrokePaint (new_ink);
		this.performNewCommand (strokePaint);
	}

	private void erase(Vector3 eraserPos){

		if (inkList.Count > 0) {

			for (int j = 0; j < inkList.Count; j++) {
				//check if eraserPos overlaps with a vertice in a ink object
				if (inkList [j] != null) {
					for (int i = 0; i < inkList [j].positionCount; i++) {
						if (inkList [j].gameObject.activeInHierarchy) {
							
							if (Mathf.Abs (inkList [j].GetPosition (i).x - eraserPos.x) < 0.08f && Mathf.Abs (inkList [j].GetPosition (i).y - eraserPos.y) < 0.08f && Mathf.Abs (inkList [j].GetPosition (i).z - eraserPos.z) < 0.08f) {

								inkList [j].gameObject.SetActive (false);

								//add to undo stack
								PaintCommand strokeErase = new StrokeErase (inkList[j]);
								this.performNewCommand (strokeErase);				

								//inkList.RemoveAt (j);

							}
						}
					}
				}
			}
		}
	}

	private void highlight(Vector3 pos){

		if (inkList.Count > 0) {

			for (int j = 0; j < inkList.Count; j++) {
				//check if eraserPos overlaps with a vertice in a ink object
				if (inkList [j] != null) {
					for (int i = 0; i < inkList [j].positionCount; i++) {
						if (inkList [j].gameObject.activeInHierarchy) {
							if (Mathf.Abs (inkList [j].GetPosition (i).x - pos.x) < 0.16 && Mathf.Abs (inkList [j].GetPosition (i).y - pos.y) < 0.16f && Mathf.Abs (inkList [j].GetPosition (i).z - pos.z) < 0.16f) {

								inkList [j].materials = highlightMat;

							} else {
								//change back to normal mat if too far way and highlighted
								if (inkList [j].materials.Length == 2) {
								
									inkList [j].materials = normalMat;
								}
							}
						}
					}
				}
			}
		}
	}
		
	private void adjustWidth(LineRenderer curInk, float tipsDist){

		//smallest dist and thin
		if (tipsDist < 0.025f) {

			if (currWidth != 0) { 
				this.createNewInk ();
				LineRenderer prevInk = inkList [inkList.Count - 1];
				new_ink.SetPosition (0, prevInk.GetPosition (prevInk.positionCount - 1));

				//transition curve
				if (currWidth == thick) {
					new_ink.widthCurve = new AnimationCurve (new Keyframe (0f, thick), new Keyframe (0.1f, thin), new Keyframe (0.9f, thin), new Keyframe (1.0f, thin));
				} else {
					new_ink.widthCurve = new AnimationCurve (new Keyframe (0f, mid), new Keyframe (0.1f, thin), new Keyframe (0.9f, thin), new Keyframe (1.0f, thin));
				}

				new_ink.widthMultiplier = curveWidth;
				currWidth = 0;
				Debug.Log ("---width--- thin width added " + tipsDist);
			}
		
		//mid dist and mid
		} else if (tipsDist >= 0.025f && tipsDist < 0.05f) {
			
			if (currWidth != 1) { 
				this.createNewInk ();
				LineRenderer prevInk = inkList [inkList.Count - 1];
				new_ink.SetPosition (0, prevInk.GetPosition (prevInk.positionCount - 1));

				//transition curve
				if (currWidth == thin) {
					new_ink.widthCurve = new AnimationCurve (new Keyframe (0f, thin), new Keyframe (0.1f, mid), new Keyframe (0.9f, mid), new Keyframe (1.0f, mid));
				} else {
					new_ink.widthCurve = new AnimationCurve (new Keyframe (0f, thick), new Keyframe (0.1f, mid), new Keyframe (0.9f, mid), new Keyframe (1.0f, mid));
				}

				new_ink.widthMultiplier = curveWidth;
				currWidth = 1;
				Debug.Log ("---width--- mid width added " + tipsDist);
			}

		//greatest dist and thick
		} else {

			if (currWidth != 2) { 
				this.createNewInk ();
				LineRenderer prevInk = inkList [inkList.Count - 1];
				new_ink.SetPosition (0, prevInk.GetPosition (prevInk.positionCount - 1));


				//transition curve
				if (currWidth == thin) {
					new_ink.widthCurve = new AnimationCurve (new Keyframe (0f, thin), new Keyframe (0.1f, thick), new Keyframe (0.9f, thick), new Keyframe (1.0f, thick));
				} else {
					new_ink.widthCurve = new AnimationCurve (new Keyframe (0f, mid), new Keyframe (0.1f, thick), new Keyframe (0.9f, thick), new Keyframe (1.0f, thick));
				}

				new_ink.widthMultiplier = curveWidth;
				currWidth = 2;
				Debug.Log ("---width--- thicker width added " + tipsDist);
			}
		}
	}

	private void performNewCommand(PaintCommand command) {
		if (!undoStack.Contains (command)) {
			redoStack.Clear ();
			undoStack.Push (command);
		}
		Debug.Log ("new command pushed " + "undo "+ undoStack.Count + "redo " + redoStack.Count);
	}

	public void undoCommand() {
		if (undoStack.Count > 0) {
			PaintCommand command = undoStack.Pop ();
			command.undo ();
			redoStack.Push (command);
			Debug.Log ("new command undone " + "undo "+ undoStack.Count + "redo " + redoStack.Count);
		}
	}

	public void redoCommand() {
		if (redoStack.Count > 0) {
			PaintCommand command = redoStack.Pop ();
			command.redo ();
			undoStack.Push (command);
			Debug.Log ("new command redone " + "undo "+ undoStack.Count + "redo " + redoStack.Count);
		}
	}
}
