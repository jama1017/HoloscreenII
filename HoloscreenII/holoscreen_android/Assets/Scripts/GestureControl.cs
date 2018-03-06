using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//new
using System;
using System.IO;
using OpenCVForUnity;

public class GestureControl : MonoBehaviour {
	//Left hand finger declare
	private GameObject palm, indexfinger, indexfinger_bone1, indexfinger_bone2;
	public bool Pose = false;

	//poseDetectorMLtrainning
	LogisticRegression logis_reg_model;
	int gesture_num = 3;
	int mat_n = 15;


	// Use this for initialization
	void Start () {
		indexfinger = this.transform.GetChild (1).gameObject;
		indexfinger_bone1 = this.transform.GetChild (1).GetChild (1).gameObject;
		indexfinger_bone2 = this.transform.GetChild (1).GetChild (2).gameObject;
		palm = this.transform.GetChild (5).gameObject;

		//new
		poseDetectorMLtrainning();
	}
	
	// Update is called once per frame
	void Update () {
		if (checkPosePointing ())
			Pose = true;
		else
			Pose = false;

		//handDataGenerator ();
	}

	private bool checkPosePointing(){
		Vector3 palm_plane_norm, palm_plane_up, palm_plane_right, vec_bone1, vec_bone2, proj_bone1, proj_bone2, temp;
		palm_plane_norm = palm.transform.forward;
		palm_plane_up = palm.transform.up;
		palm_plane_right = palm.transform.right;

		vec_bone1 = indexfinger_bone1.transform.position - palm.transform.position;
		vec_bone2 = indexfinger_bone2.transform.position - palm.transform.position;
		proj_bone1.y = Vector3.ProjectOnPlane(vec_bone1, palm_plane_norm).magnitude;
		proj_bone2.x = Vector3.ProjectOnPlane(vec_bone2, palm_plane_right).magnitude;
		proj_bone2.y = Vector3.ProjectOnPlane(vec_bone2, palm_plane_norm).magnitude;
		proj_bone2.z = Vector3.ProjectOnPlane(vec_bone2, palm_plane_up).magnitude;

		//Debug.Log ("x: " + proj_bone2.x.ToString("F3") + "  y: " + proj_bone2.y.ToString("F3") + "  z: " + proj_bone2.z.ToString("F3"));
		if ((proj_bone2.y/proj_bone1.y)>1.027) {
			return true;
		}

		return false;
	}

	/* 	poseDetectorMLtrainning
	*	Input: GameObject obj
	*	Output: None
	*	Summary: 1. Process text file to OpenCV mat 2.train the model and store in disk
	*/
	private void poseDetectorMLtrainning(){
		//Data process
		string data_fname_basic = "Assets/Data/handData_G";
		Mat all_data = new Mat();
		Mat all_label = new Mat ();
		for (int i = 0; i < 2; i++) {
			string data_fname = data_fname_basic + i.ToString () + ".txt";
			Debug.Log (data_fname);
			int mat_m = File.ReadAllLines (data_fname).Length;
			Mat cur_data = Mat.zeros(mat_m,mat_n,CvType.CV_32F);
			Mat cur_label = Mat.ones(mat_m, 1, CvType.CV_32F);
			/* assign labels for cur_label */
			Core.multiply(cur_label,new Scalar((double)(i)),cur_label);

			StreamReader reader = new StreamReader (data_fname);
			int row = 0;
			while(reader.Peek() >= 0) {
				string cur_line = reader.ReadLine();
				float[] cur_line_data = Array.ConvertAll (cur_line.Split (','), float.Parse);
				cur_data.put(row,0,cur_line_data);
				row++;
			}
			all_data.push_back (cur_data);
			all_label.push_back (cur_label);
			reader.Close ();
		}
		//
		Debug.Log("----------");

		Debug.Log("----------");

		//

		logis_reg_model = OpenCVForUnity.LogisticRegression.create ();
		logis_reg_model.setLearningRate (0.0001);
		logis_reg_model.setRegularization(OpenCVForUnity.LogisticRegression.REG_L2);
		logis_reg_model.setIterations (100);
		logis_reg_model.setTrainMethod (OpenCVForUnity.LogisticRegression.BATCH);
		logis_reg_model.setMiniBatchSize (8);

		TrainData data_lable = OpenCVForUnity.TrainData.create (all_data, OpenCVForUnity.Ml.ROW_SAMPLE,all_label);
		Debug.Log("Train success: " + logis_reg_model.train(all_data, OpenCVForUnity.Ml.ROW_SAMPLE,all_label));
		Debug.Log("Train error: " + logis_reg_model.calcError(data_lable,true,all_label));
		//temp - test
		Mat result = Mat.zeros(1,1,CvType.CV_32S);
		//it should output 0
		int idx = 1500;
		logis_reg_model.predict (all_data.row (idx), result, 0);
		Debug.Log ("Data is: " + all_data.get(idx,0)[0]);
		Debug.Log ("Data is: " + all_data.get(idx,1)[0]);
		Debug.Log ("Label should be: " + all_label.get(idx,0)[0]);
		Debug.Log ("Predicted label is: " + result.get (0, 0) [0]);
		//test - test
		return;
	}

	private int poseDetectorMLtesting(){
		
		return 0;
	}
	//new

	private void handDataGenerator(){
		Vector3[] vec_bone2 = new Vector3[5];
		Vector3 palm_plane_norm, palm_plane_up, palm_plane_right;
		palm_plane_norm = palm.transform.forward;
		palm_plane_up = palm.transform.up;
		palm_plane_right = palm.transform.right;

		string temp = "";
		for (int i = 0; i < 5; i++) {
			Vector3 vec_palm_bone2 = this.transform.GetChild (i).GetChild (2).position - palm.transform.position;
			vec_bone2 [i].x = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_right).magnitude;
			vec_bone2 [i].y = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_norm).magnitude;
			vec_bone2 [i].z = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_up).magnitude;
			temp += vec_bone2 [i].x.ToString ("F6") + "," + vec_bone2 [i].y.ToString ("F6") + "," + vec_bone2 [i].z.ToString ("F6") ;
			if (i < 4)
				temp += ",";
			else
				temp += "\n";
		}
		System.IO.File.AppendAllText("handData.txt", temp);
	}
}
