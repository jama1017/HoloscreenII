using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VMIClose : VirtualMenuItem {

	public override void onHandIn ()
	{
		m_menu.close();
	}

}
