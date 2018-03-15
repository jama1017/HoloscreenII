using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMIFurniture : VirtualMenuItem {
	public GameObject m_furniturePrefab;
	
	public override void onHandGrab ()
	{
		Camera cam = Camera.main;

		// Add object
		GameObject fo = Instantiate(m_furniturePrefab,
			cam.transform.position + cam.transform.forward,
			Quaternion.LookRotation(cam.transform.forward, new Vector3(0, 1, 0))) as GameObject;


		Debug.Log ("Creating new object");
		fo.tag = "InteractableObj";

		InteractionScriptObject iso = fo.AddComponent<InteractionScriptObject> ();
		iso.secondaryMaterial = new Material(Shader.Find ("ModelEffect/VerticsOutline_Always"));
		iso.secondaryMaterial.SetColor ("_OutlineColor", new Color (0, 248.0f / 256.0f, 63.0f / 256.0f, 143.0f / 256.0f));
		iso.secondaryMaterial.mainTexture = fo.GetComponent<Renderer> ().material.mainTexture;

		fo.GetComponent<Renderer> ().material.shader = Shader.Find ("Custom/ZTestBlur");
		fo.GetComponent<Renderer> ().material.SetTexture ("_CameraDepthTexture", GameObject.Find ("DepthCamera").GetComponent<Camera> ().targetTexture);

		BoxCollider c = fo.AddComponent<BoxCollider> ();
		c.isTrigger = false;

		Rigidbody r = fo.AddComponent<Rigidbody> ();
		r.collisionDetectionMode = CollisionDetectionMode.Discrete;
		r.mass = 20;
		r.useGravity = true;
		r.velocity = Vector3.zero;
		r.angularVelocity = Vector3.zero;

		// Change context
		hand_l.GetComponent<HandManager> ().contextSwitch ("object");
	}

}
