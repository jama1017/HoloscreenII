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

			if (item.GetComponent<Renderer> () != null) {
				item.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
				item.AddComponent<BoxCollider> ();
			} else {
				item.transform.localScale = new Vector3 (0.05f, 0.05f, 0.05f);

				foreach(Transform child in item.transform) {
					child.gameObject.AddComponent<BoxCollider> ();
				}
			}

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
