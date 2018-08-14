using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMIColor : VirtualMenuItem {
	public Color m_color;
	private GameObject ink_l;

	protected override void Start ()
	{
		base.Start ();

		ink_l = GameObject.Find ("Ink_l");
	}

	protected override void Update() {
		base.Update ();

		GetComponent<Renderer> ().material.color = m_color;

		this.GetComponent<Renderer> ().material.SetFloat ("_UseBodyColor", 1);
		this.GetComponent<Renderer> ().material.SetColor ("_BodyColor", m_color);
	}

	public override void onHandGrab ()
	{
		hand_l.GetComponent<HandManager> ().contextSwitch ("paint");

		ink_l.GetComponent<Renderer> ().material.SetColor ("_Color", m_color);
		ink_l.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", m_color);

		m_menu.close ();

		Debug.Log("3D Color selected from VMIColor Script");
	}

}
