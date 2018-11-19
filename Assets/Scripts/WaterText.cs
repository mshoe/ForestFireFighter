using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterText : MonoBehaviour {
    public Text water_text;
    public GameObject heli;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        water_text.text = "Water Tank: " + heli.GetComponent<helicopter_controller>().current_water_level.ToString();
	}
}
