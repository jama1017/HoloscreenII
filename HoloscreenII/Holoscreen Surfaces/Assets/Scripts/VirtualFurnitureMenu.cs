using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualFurnitureMenu : VirtualMenu {
	public GameObject[] m_furniturePrefabs;
	public GameObject m_itemPrefab;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		// Add body
		Debug.Log("Creating furniture menu");

		foreach (GameObject f in m_furniturePrefabs) {
			GameObject item = Instantiate (f) as GameObject;

			item.AddComponent<BoxCollider> ();
			Bounds bounds = item.GetComponent<Renderer> ().bounds;
			item.transform.localScale = new Vector3 (0.05f / bounds.size.x, 0.05f / bounds.size.x, 0.05f / bounds.size.x);

			VMIFurniture v = item.AddComponent<VMIFurniture> ();
			v.m_furniturePrefab = f;

			addBodyItem (item);
		}

		Debug.Log ("Creating furniture sub-menu");

		// Add footer
		GameObject prevPage = Instantiate (m_itemPrefab) as GameObject;
		prevPage.AddComponent<VMIPrevPage> ();
		addFooterItem (prevPage);

		GameObject nextPage = Instantiate (m_itemPrefab) as GameObject;
		nextPage.AddComponent<VMINextPage> ();
		addFooterItem (nextPage);

		GameObject closeMenu = Instantiate (m_itemPrefab) as GameObject;
		closeMenu.AddComponent<VMIClose> ();
		addFooterItem (closeMenu);
	}
}
