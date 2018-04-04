using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualPaintMenu : VirtualMenu {
	public Color[] m_colors;
	public GameObject m_itemPrefab;
	public GameObject m_itemPrefabPre;
	public GameObject m_itemPrefabNxt;
	public GameObject m_itemPrefabCls;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		// Add body
		foreach (Color c in m_colors) {
			GameObject item = Instantiate (m_itemPrefab) as GameObject;

			item.AddComponent<VMIColor> ();
			item.GetComponent<VMIColor> ().m_color = c;

			item.SetActive (false);
			addBodyItem (item);
		}

		// Add footer
		GameObject prevPage = Instantiate (m_itemPrefabPre) as GameObject;
		prevPage.AddComponent<VMIPrevPage> ();
		prevPage.transform.Rotate (new Vector3 (0, 90, 0));
		addFooterItem (prevPage);

		GameObject nextPage = Instantiate (m_itemPrefabNxt) as GameObject;
		nextPage.AddComponent<VMINextPage> ();
		nextPage.transform.Rotate (new Vector3 (0, 90, 0));
		addFooterItem (nextPage);

		GameObject closeMenu = Instantiate (m_itemPrefabCls) as GameObject;
		closeMenu.AddComponent<VMIClose> ();
		closeMenu.transform.Rotate (new Vector3 (0, 90, 0));
		addFooterItem (closeMenu);
	}
}
