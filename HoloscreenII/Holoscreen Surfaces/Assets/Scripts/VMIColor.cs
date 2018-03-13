using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMIColor : VirtualMenuItem {
	public Color m_color;

	void Update() {
		GetComponent<Renderer> ().material.color = m_color;
	}
	
	public override void onHandIn ()
	{
		hand_l.GetComponent<PaintFeature> ().setColor (m_color);
	}

}
