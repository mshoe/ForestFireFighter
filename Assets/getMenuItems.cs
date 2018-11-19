using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System;

public class getMenuItems : MonoBehaviour {
    public Button easy_b, medium_b, hard_b, help_b, quit_b;
    public InputField seed_field;
    public bool start_game;
    
    public int WATER_CYCLE_TIME;

	// Use this for initialization
	void Start () {
        start_game = false;
        easy_b.onClick.AddListener(delegate { modifyGame(1.5f, 3); });
        medium_b.onClick.AddListener(delegate { modifyGame(1f, 2); });
        hard_b.onClick.AddListener(delegate { modifyGame(0.3f, 1); });
        quit_b.onClick.AddListener(Application.Quit);
        help_b.onClick.AddListener(helpScene);
        seed_field.onValueChanged.AddListener(delegate { modifySeed(Int32.Parse(seed_field.GetComponent<InputField>().text)); });
	}

    void modifyGame(float cycle, int sea_level){
        StaticData.Water_cycle_speed = cycle;
        StaticData.Sea_level = sea_level;
        SceneManager.LoadScene("SampleScene");
        //fireEngine.GetComponent<FireEngine>().water_cycle_time = cycle
    }

    void helpScene(){
        SceneManager.LoadScene("Help");
    }

    void modifySeed(int seed)
    {
        StaticData.Seed = seed;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
