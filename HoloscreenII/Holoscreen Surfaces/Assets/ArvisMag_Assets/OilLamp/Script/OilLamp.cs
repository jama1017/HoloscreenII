using UnityEngine;
using System.Collections;

public class OilLamp : MonoBehaviour {
	//=================================================
	public bool Active = false;
	public bool Visible = true;
	public GameObject litheObj;
	//=================================================
	private Light OilLampLight;
	private ParticleSystem flamePart;
	//=================================================

	// Use this for initialization
	void Start () {

		OilLampLight = litheObj.GetComponent<Light>();
		flamePart = litheObj.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

		foreach(Transform lampPart in transform){
			if(lampPart.GetComponent<MeshRenderer>()){
				lampPart.GetComponent<MeshRenderer>().enabled = Visible;
			}
		}

		if(Visible){
			OilLampLight.enabled = Active;
			flamePart.enableEmission = Active;
		}else{
			OilLampLight.enabled = false;
			flamePart.enableEmission = false;
		}
	}
}
