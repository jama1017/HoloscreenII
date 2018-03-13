using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMIColor : VirtualMenuItem {
	public Color m_color;

	protected override void Update() {
		base.Update ();

		GetComponent<Renderer> ().material.color = m_color;
	}
	
	public override void onHandIn ()
	{
		hand_l.GetComponent<PaintFeature> ().setColor (m_color);
	}

}
