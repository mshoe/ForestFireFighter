using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatus : MonoBehaviour {
    public GameObject proc_gen;
    private List<List<List<GameObject>>> world_map;
    private int world_width;
    private int world_length;
    private int world_height;
    private int num_wood_blocks;
    private int num_leaf_blocks;

    public GameObject fire_engine;

    public int score;
    public float elapsed_time;
    private float max_time;

    public int status;
    private int NOTDONE = 0;
    private int LOST = 1;
    private int WON = 2;

    public GameObject game_done_panel;

    // Use this for initialization
    void Start () {
        world_map = proc_gen.GetComponent<ProcGen>().world_map;
        world_width = proc_gen.GetComponent<ProcGen>().world_width;
        world_height = proc_gen.GetComponent<ProcGen>().world_height;
        world_length = proc_gen.GetComponent<ProcGen>().world_length;
        num_wood_blocks = proc_gen.GetComponent<ProcGen>().num_wood_blocks;
        num_leaf_blocks = proc_gen.GetComponent<ProcGen>().num_leaf_blocks;

        score = 0;
        elapsed_time = 0f;
        max_time = 60f;

        status = NOTDONE;
	}
	
	// Update is called once per frame
	void Update () {
        if (status == NOTDONE)
        {
            elapsed_time += Time.deltaTime;
            bool trees_remain = TreesRemain();
            int num_fire = fire_engine.GetComponent<fireEngineScript>().GetFireObjectsCount();
            string new_text = "";
            //if (elapsed_time > max_time || !trees_remain)
            if (!trees_remain)
            {
                status = LOST;
                new_text = "You Lost!";
            }
            else if (num_fire == 0)
            {
                status = WON;
                new_text = "You Won!";
            }
            else
            {
                return;
            }
            game_done_panel.transform.Find("WinLoseText").gameObject.GetComponent<Text>().text = new_text;
            game_done_panel.SetActive(true);
        }
        
	}

    bool TreesRemain()
    {
        for (int k = 0; k < world_height; k++)
        {
            for (int i = 0; i < world_width; i++)
            {
                for (int j = 0; j < world_length; j++)
                {
                    GameObject block = world_map[i][j][k];
                    if (block != null && (block.name == "WoodBlock(Clone)" || block.name == "LeafBlock(Clone)"))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
