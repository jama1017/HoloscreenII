using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualFurnitureMenu : VirtualMenu {
	public GameObject[] m_furniturePrefabs;
	public GameObject m_itemPrefab;

	// Use this for initialization
	void Start () {
		// Add body
		foreach (GameObject f in m_furniturePrefabs) {
			GameObject item = Instantiate (m_itemPrefab) as GameObject;

			item.AddComponent<VMIFurniture> ();
			item.GetComponent<VMIFurniture> ().m_furniturePrefab = f;
			item.AddComponent<VMIClose> ();

			addBodyItem (item);
		}

		// Add footer
		GameObject prevPage = Instantiate (m_itemPrefab) as GameObject;
		prevPage.AddComponent<VMIPrevPage> ();
		addFooterItem (prevPage);

		GameObject closeMenu = Instantiate (m_itemPrefab) as GameObject;
		closeMenu.AddComponent<VMIClose> ();
		addFooterItem (closeMenu);

		GameObject nextPage = Instantiate (m_itemPrefab) as GameObject;
		nextPage.AddComponent<VMINextPage> ();
		addFooterItem (nextPage);
	}
}
