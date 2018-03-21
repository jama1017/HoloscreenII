using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMIClose : VirtualMenuItem {

	public override void onHandGrab ()
	{
		m_menu.close();

		hand_l.GetComponent<HandManager> ().contextSwitch ("null");
	}

}
