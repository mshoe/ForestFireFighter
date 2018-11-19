using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System;

public class help : MonoBehaviour {
    public Button back_b;

	// Use this for initialization
	void Start () {
        back_b.onClick.AddListener(GoBackToMenu);
    }

    void GoBackToMenu(){
        SceneManager.LoadScene("Menus");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
