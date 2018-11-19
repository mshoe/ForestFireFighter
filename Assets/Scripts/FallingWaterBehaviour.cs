using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWaterBehaviour : MonoBehaviour {
	public GameObject splash;

    List<List<List<GameObject>>> world_map;
    public int world_width;
    public int world_length;
    public int world_height;

    public GameObject fx_engine;

    // Use this for initialization
    void Start () {
        GameObject proc_gen = GameObject.Find("ProcGenEngine");
        world_map = proc_gen.GetComponent<ProcGen>().world_map;
        world_width = proc_gen.GetComponent<ProcGen>().world_width;
        world_length = proc_gen.GetComponent<ProcGen>().world_length;
        world_height = proc_gen.GetComponent<ProcGen>().world_height;
        /*GameObject fx_engine = GameObject.Find("FxEngine");
        if (fx_engine == null) Debug.Log("NULLLLLL");
        else Debug.Log("NOTTTTT");*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision collision)
    {
		//Debug.Log("Destroying falling water");
		Instantiate(splash, gameObject.transform.position, Quaternion.identity);
		Destroy(gameObject);
        ExtinguishFire(gameObject.transform.position);
	}

    void ExtinguishFire(Vector3 position)
    {
        int x = (int)Mathf.Round(position.x);
        int y = (int)Mathf.Round(position.y);
        int z = (int)Mathf.Round(position.z);
        for (int i = x - 1; i <= x + 1; i++)
        {
            for (int j = z - 1; j <= z + 1; j++)
            {
                if (i < 0 || i >= world_width || j < 0 || j >= world_length)
                {
                    continue;
                }
                for (int k = 0; k < y; k++)
                {
                    GameObject block = world_map[i][j][k];
                    if (block != null)
                    {
                        block.GetComponent<BlockAttributes>().fire_level = -100;
                        fx_engine.GetComponent<FxEngine>().UpdateFireFx(block);
                    }
                }
            }
        }
    }
}
