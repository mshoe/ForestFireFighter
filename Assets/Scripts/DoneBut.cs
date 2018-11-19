using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoneBut : MonoBehaviour {
    public Button but;
	// Use this for initialization
	void Start () {
        but.onClick.AddListener(ButClick);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void ButClick()
    {
        SceneManager.LoadScene("Menus");
    }
}
