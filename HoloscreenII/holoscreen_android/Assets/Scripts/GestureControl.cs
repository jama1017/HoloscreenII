using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//new
using System;
using System.IO;
using OpenCVForUnity;

public class GestureControl : MonoBehaviour {
	//Left hand finger declare
	private GameObject palm;

	//poseDetectorMLtrainning
	LogisticRegression logis_reg_model;
	SVM svm_model;
	int gesture_num = 4;
	int mat_n = 15;

	//poseDetector buffer
	int[] gesture_buff;
	int gesture_buff_len = 15;
	int gesture_buff_idx = 0; 

	//Gesture dictionary
	Dictionary<int, string> gesture_dict = new Dictionary<int, string>();

	// Use this for initialization
	void Start () {
		palm = this.transform.GetChild (5).gameObject;
		gesture_buff = new int[gesture_buff_len];

		//Gesture dicitonary establishes
		gesture_dict.Add(0, "palm");
		gesture_dict.Add(1, "pinch");
		gesture_dict.Add(2, "paint");
		gesture_dict.Add(3, "fist");
		//gesture_dict.Add(4, "undefined");

		//Train svm model when svm model does not exist
		//if (File.Exists ("Assets/Data/svm.xml"))
		//	svm_model = OpenCVForUnity.SVM.load ("Assets/Data/svm.xml");
		//else {
			gestureDetectorMLtrain ();
			svm_model.save ("Assets/Data/svm.xml");
		//}

	}
	
	// Update is called once per frame
	void Update () {
		//Update gesture buffer array
		gesture_buff[gesture_buff_idx++] = gestureDetectorMLpredict ();
		gesture_buff_idx = (gesture_buff_idx) % gesture_buff_len;
		//handDataGenerator ();
	}


	/* 	
	//Method aborted
	private bool checkPosePointing(){
		Vector3 palm_plane_norm, palm_plane_up, palm_plane_right, vec_bone1, vec_bone2, proj_bone1, proj_bone2;
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
	*/

	/* 	gestureDetectorMLtrain
	*	Input: GameObject obj
	*	Output: None
	*	Summary: 1. Process text file to OpenCV mat 2.train the model and store model in disk
	*/
	private bool gestureDetectorMLtrain(){
		//Data process
		string data_fname_basic = "Assets/Data/handDataG";
		Mat all_data = new Mat ();
		Mat all_label = new Mat ();
		for (int i = 0; i < gesture_num; i++) {
			string data_fname = data_fname_basic + i.ToString () + "_new.txt";
			int mat_m = File.ReadAllLines (data_fname).Length;
			Mat cur_data = Mat.zeros (mat_m, mat_n, CvType.CV_32F);
			Mat cur_label = Mat.ones (mat_m, 1, CvType.CV_32S);
			/* assign labels for cur_label */
			Core.multiply (cur_label, new Scalar ((double)(i)), cur_label);

			StreamReader reader = new StreamReader (data_fname);
			int row = 0;
			while (reader.Peek () >= 0) {
				string cur_line = reader.ReadLine ();
				float[] cur_line_data = Array.ConvertAll (cur_line.Split (','), float.Parse);
				cur_data.put (row, 0, cur_line_data);
				row++;
			}
			all_data.push_back (cur_data);
			all_label.push_back (cur_label);
			reader.Close ();
		}
		TrainData data_lable = OpenCVForUnity.TrainData.create (all_data, OpenCVForUnity.Ml.ROW_SAMPLE, all_label);
		data_lable.setTrainTestSplitRatio (0.8, true);
		TrainData train_data_lable = OpenCVForUnity.TrainData.create (data_lable.getTrainSamples (), OpenCVForUnity.Ml.ROW_SAMPLE, data_lable.getTrainResponses ());
		TrainData test_data_lable = OpenCVForUnity.TrainData.create (data_lable.getTestSamples (), OpenCVForUnity.Ml.ROW_SAMPLE, data_lable.getTestResponses ());

		//Debug usage
		//Debug.Log("Debug print(GestureControl.poseDetectorMLtrainning()):----------");

		//logistic reg model starts
		/*
		logis_reg_model = OpenCVForUnity.LogisticRegression.create ();
		ret_val = logis_reg_model.train (data_lable);
		Debug.Log ("Logistic train success : " + ret_val);
		logis_reg_model = OpenCVForUnity.LogisticRegression.create ();
		logis_reg_model.setLearningRate (0.0001);
		logis_reg_model.setRegularization(OpenCVForUnity.LogisticRegression.REG_L2);
		logis_reg_model.setIterations (100);
		logis_reg_model.setTrainMethod (OpenCVForUnity.LogisticRegression.BATCH);
		logis_reg_model.setMiniBatchSize (8);
		Debug.Log("Train success: " + logis_reg_model.train(all_data, OpenCVForUnity.Ml.ROW_SAMPLE,all_label));
		//Debug.Log("Train error: " + logis_reg_model.calcError(data_lable,true,all_label));
		
		//test
		Mat result = Mat.zeros(1,1,CvType.CV_32S);
		int idx = 1500;
		logis_reg_model.predict (all_data.row (idx), result, 0);
		Debug.Log ("Data is: " + all_data.get(idx,0)[0]);
		Debug.Log ("Theta is: " + logis_reg_model.get_learnt_thetas().get(0,0)[0]);
		Debug.Log ("Theta is: " + logis_reg_model.get_learnt_thetas().get(0,1)[0]);

		Debug.Log ("Label should be: " + all_label.get(idx,0)[0]);
		Debug.Log ("Predicted label is: " + result.get (0, 0) [0]);


		*/

		//logistic reg model ends


		//svm model starts (13.5 ~ 14.5)
		//Optimize STEP 1: kerneltype: RBF, C=3; CHI2 (8.5, 8.5); INTER (6.5, 6.5)
		//Optimize STEP 2: C = 1 to C = 20. (6.5, 6.5) to (2, 2.5)
		svm_model = OpenCVForUnity.SVM.create();
		//Debug.Log (svm_model.getType(SVM));
		svm_model.setKernel (SVM.INTER);
		svm_model.setC (20);
		bool ret_val = svm_model.train (train_data_lable);
		Debug.Log ("SVM train success : " + ret_val);
		/*
		//Debug usage
		Mat res = Mat.ones(1,1,CvType.CV_32S);
		int idx = 2000;
		svm_model.predict (all_data.row (idx), res, 0);
		Debug.Log ("Label should be: " + all_label.get(idx,0)[0]);
		Debug.Log("SVM predicted: " + res.get (0, 0) [0]);
		*/

		//Trainning error printed out
		Debug.Log("SVM trainning error: " + svm_model.calcError (train_data_lable,true,train_data_lable.getResponses()));

		//Test err printed out
		Debug.Log("SVM test error: " + svm_model.calcError (test_data_lable,true,test_data_lable.getResponses()));

		//Predicton speed printed out
		Mat temp_row = test_data_lable.getSamples();
		Mat result = Mat.ones(1,1,CvType.CV_32S);
		float start_time= Time.realtimeSinceStartup;
		svm_model.predict (temp_row.row(0), result, 0);
		float time_elapsed = Time.realtimeSinceStartup - start_time;
		Debug.Log ("Prediction time is " + time_elapsed.ToString("F6") + "s");
		//svm model ends
		/*
		//DTree model
		DTrees model_DTree = OpenCVForUnity.DTrees.create();
		bool ret_val = model_DTree.train (train_data_lable);
		Debug.Log ("DTree train success : " + ret_val);
		//Trainning error printed out
		Debug.Log("DTree trainning error: " + model_DTree.calcError (train_data_lable,true,train_data_lable.getResponses()));

		//Test err printed out
		Debug.Log("DTree test error: " + model_DTree.calcError (test_data_lable,true,test_data_lable.getResponses()));
		*/
		return ret_val;
	}

	/* 	gestureDetectorMLpredict
	*	Input: None
	*	Output: None
	*	Summary: 1. Collect current hand data 2. Generate an instant prediction for current gesture
	*/
	private int gestureDetectorMLpredict(){
		//initialize/declare necessary normals and vector array
		Vector3[] vec_bone2 = new Vector3[5];
		float[] cur_data_array = new float[15];
		Mat cur_data_mat = Mat.zeros(1,mat_n,CvType.CV_32F);

		Vector3 palm_plane_norm = palm.transform.forward;
		Vector3 palm_plane_up = palm.transform.up;
		Vector3 palm_plane_right = palm.transform.right;

		//collect current hand data
		for (int i = 0; i < 5; i++) {
			Vector3 vec_palm_bone2 = this.transform.GetChild (i).GetChild (2).position - palm.transform.position;
			vec_bone2 [i].x = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_right).magnitude;
			vec_bone2 [i].y = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_norm).magnitude;
			vec_bone2 [i].z = Vector3.ProjectOnPlane (vec_palm_bone2, palm_plane_up).magnitude;
			cur_data_array [i * 3] = vec_bone2 [i].x;
			cur_data_array [i * 3 + 1] = vec_bone2 [i].y;
			cur_data_array [i * 3 + 2] = vec_bone2 [i].z;
		}
		cur_data_mat.put (0, 0, cur_data_array);

		//predict
		Mat result = Mat.ones(1,1,CvType.CV_32S);
		svm_model.predict (cur_data_mat, result, 0);

		//Debug usage
//		string cur_gesture = gesture_dict[((int)result.get (0, 0) [0])];
//		Debug.Log ("Predicted label is: " + cur_gesture);

		return ((int)result.get (0, 0) [0]);
	}

	/* 	bufferedGesture
	*	Input: None
	*	Output: None
	*	Summary: Output mode gesture in last detector_buff_len frames to reduce noise
	*/
	public string bufferedGesture(){
		int[] gesture_hist = new int[gesture_dict.Count];
		for (int i = 0; i < gesture_buff_len; i++) {
			gesture_hist [gesture_buff [i]] += 1;
		}

		int modeGesture = 0;
		for (int i = 0; i < gesture_hist.Length; i++){
			if (gesture_hist[i] >= gesture_hist[modeGesture])
				modeGesture = i;
		}
		return gesture_dict [modeGesture];
	}


	/* 	handDataGenerator
	*	Input: None
	*	Output: None
	*	Summary: Output current gesture data into a .txt file
	*/
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
			temp += vec_bone2 [i].x.ToString ("F10") + "," + vec_bone2 [i].y.ToString ("F10") + "," + vec_bone2 [i].z.ToString ("F10") ;
			if (i < 4)
				temp += ",";
			else
				temp += "\n";
		}

		//System.IO.File.AppendAllText("handDataG_new.txt", temp);
	}
}
