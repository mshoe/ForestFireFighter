using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreCalculation : MonoBehaviour {
    public int deadCell;
    public GameObject procGenEngine;
    private float startTime;
    private int init_null;
    private float gameDur;

    private int world_width;
    private int world_length;
    public int num_grass_blocks;
    public int num_wood_blocks;
    public int num_water_blocks;
    public int num_leaf_blocks;

    public Text scoreDisplay;
    private double score;

    // Use this for initialization
    void Start () {
        world_width = procGenEngine.GetComponent<ProcGen>().world_width;
        world_length = procGenEngine.GetComponent<ProcGen>().world_length;

        init_null = world_width * world_length - num_grass_blocks
            - num_wood_blocks - num_water_blocks - num_leaf_blocks;
        score = 0;
        setScoreDisplay();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameDur = Time.realtimeSinceStartup;
        score = Mathf.RoundToInt(gameDur);
        setScoreDisplay();
    }

    void UpdateNullCounts(){
        deadCell -= 1;
    }

    void setScoreDisplay(){
        scoreDisplay.text = "Time: " + score.ToString();
    }
}
