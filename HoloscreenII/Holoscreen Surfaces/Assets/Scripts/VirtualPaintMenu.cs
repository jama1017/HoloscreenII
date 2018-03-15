using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPaintMenu : VirtualMenu {
	public Color[] m_colors;
	public GameObject m_itemPrefab;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		// Add body
		foreach (Color c in m_colors) {
			GameObject item = Instantiate (m_itemPrefab) as GameObject;

			item.AddComponent<VMIColor> ();
			item.GetComponent<VMIColor> ().m_color = c;

			addBodyItem (item);
		}

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
