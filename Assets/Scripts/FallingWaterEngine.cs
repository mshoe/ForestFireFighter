using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWaterEngine : MonoBehaviour {
	public GameObject proc_gen_engine;
	public GameObject falling_water_block;
    public GameObject fx_engine;

	// Use this for initialization
	void Start () {
        falling_water_block.GetComponent<FallingWaterBehaviour>().fx_engine = fx_engine;
	}
	
	// Update is called once per frame
	void Update () {
        //DropWater(4, 30, 1);
    }

    public void DropWater (float x, float y, float z)
    {
        DropWater(new Vector3(x, y, z));
    }

    public void DropWater (Vector3 position)
    {
        position.y -= 0.5f;
		Instantiate(falling_water_block, position, Quaternion.identity);
        position.y -= 1.05f;
        Instantiate(falling_water_block, position, Quaternion.identity);
        position.y -= 1.05f;
        Instantiate(falling_water_block, position, Quaternion.identity);
    }

}
