using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;
using System.Collections.Generic;


public class WSManager : MonoBehaviour, WebSocketUnityDelegate {

	// Web Socket for Unity
	private WebSocketUnity webSocket;
	public string websocketServer = "192.168.43.95";
	public string websocketPort = "9999";
	private string handinfo_l = "";
	private string handinfo_r = "";
	private string gestureinfo = "";

//	public Vector2 faceTrackingScreenDims = new Vector2 (480, 320);
//	private float eyeDistance = -1.0f;
//	private float eyeScale = -1.0f;
//	private Vector2 eyeCentroid = new Vector2(0, 0);
//
	private GameObject InputHolder;
	private InputField websocketInputField;
	// Use this for initialization
	void Start () {
		/*
		InputHolder = GameObject.Find ("NodeServer");
		websocketInputField = InputHolder.GetComponentInChildren<UnityEngine.UI.InputField> ();
		websocketInputField.text = websocketServer;
		websocketInputField.onEndEdit.AddListener(delegate { updateWebSocketServerInfo();});
		*/
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log ("aaaaa");
		Debug.Log(websocketServer);
	}


	/// <summary>
	/// This function is called when the object becomes enabled and active.
	/// </summary>
	public void OnEnable()
	{
		// Create web socket
		Debug.Log ("Connecting");
		string url = "ws://"+websocketServer + ":" + websocketPort;
		webSocket = new WebSocketUnity(url, this);

		// Open the connection
		webSocket.Open();
	}
		
	private void updateWebSocketServerInfo(){
		websocketServer = websocketInputField.text;
		webSocket.Close ();
		OnEnable ();
	}

	#region WebSocketUnityDelegate implementation

	// These callbacks come from WebSocketUnityDelegate
	// You will need them to manage websocket events
	public string getHandInfoLeft(){
		//Debug.Log("Hand_l" );
		return handinfo_l;	
	}
	public string getHandInfoRight(){
		//Debug.Log("Hand_r" );
		return handinfo_r;	
	}

	public string getGestureInfoRight(){
		//Debug.Log("gestureinfo" );
		return gestureinfo;	
	}

	// This event happens when the websocket is opened
	public void OnWebSocketUnityOpen (string sender)
	{
		Debug.Log("WebSocket connected, "+sender);
		//GameObject.Find("NotificationText").GetComponent<TextMesh>().text = "WebSocket connected, "+sender;
	}

	// This event happens when the websocket is closed
	public void OnWebSocketUnityClose (string reason)
	{
		Debug.Log("WebSocket Close : "+reason);
		//GameObject.Find("NotificationText").GetComponent<TextMesh>().text = "WebSocket Close : "+reason;
	}

	// This event happens when the websocket received a message
	public void OnWebSocketUnityReceiveMessage (string message)
	{
		//Debug.Log("Received from server : " );
		var hand_list = message.Split (new string[] { "#OneMore#" }, System.StringSplitOptions.None);
		var gesture_list = message.Split (new string[] { "#GestureDetected#" }, System.StringSplitOptions.None);

		/* Assign partial message to left/hand variable */
		//var List = message.Split (new char[] {',', ':', ';'});
		string handinfo_l_temp = "";
		string handinfo_r_temp = "";
		for (int hand_i = 0; hand_i < hand_list.Length; hand_i++) {
			var hand_info = hand_list[hand_i].Split (new char[] {',', ':', ';'});
			if (hand_info [0].Contains ("hand_type")) {
				//Debug.Log (hand_info [i]);
				if (hand_info [1].Contains ("left"))
					handinfo_l_temp = hand_list [hand_i]; 
				else
					handinfo_r_temp = hand_list [hand_i];
			}				
		}
		handinfo_l = handinfo_l_temp;
		handinfo_r = handinfo_r_temp;

		/* Find if there are gestures detected */
		if (gesture_list.Length > 1) {
			gestureinfo = gesture_list [1];
		}
	}

	// This event happens when the websocket received data (on mobile : ios and android)
	// you need to decode it and call after the same callback than PC
	public void OnWebSocketUnityReceiveDataOnMobile(string base64EncodedData)
	{
		// it's a limitation when we communicate between plugin and C# scripts, we need to use string
		byte[] decodedData = webSocket.decodeBase64String(base64EncodedData);
		OnWebSocketUnityReceiveData(decodedData);
	}

	// This event happens when the websocket did receive data
	public void OnWebSocketUnityReceiveData(byte[] data)
	{	
		int testInt1 = System.BitConverter.ToInt32(data,0);
		int testInt2 = System.BitConverter.ToInt32(data,4);;

		//Debug.Log("Received data from server : " + testInt1+", "+testInt2);
		//GameObject.Find("NotificationText").GetComponent<TextMesh>().text = "Received data from server : " + testInt1+", "+testInt2;
	}

	// This event happens when you get an error@
	public void OnWebSocketUnityError (string error)
	{
		Debug.LogError("WebSocket Error : "+ error);
		//GameObject.Find("NotificationText").GetComponent<TextMesh>().text = "WebSocket Error : "+ error;
	}

	#endregion
}
