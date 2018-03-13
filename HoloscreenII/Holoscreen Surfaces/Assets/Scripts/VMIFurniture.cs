using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMIFurniture : VirtualMenuItem {
	public GameObject m_furniturePrefab;

	void Update() {

	}
	
	public override void onHandIn ()
	{
		Camera cam = Camera.main;

		GameObject newFurnitureObject = Instantiate(m_furniturePrefab,
			cam.transform.position + cam.transform.forward,
			Quaternion.LookRotation(-cam.transform.forward, new Vector3(0, 1, 0))) as GameObject;
	}

}
