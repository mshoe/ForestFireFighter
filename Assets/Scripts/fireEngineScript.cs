using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireEngineScript : MonoBehaviour
{
    public GameObject procGenEngine;
    public GameObject fxEngine;

    private float cycle_timer;
    public float water_cycle_time;

    public int max_death_cycles;
    public int max_fire_level;
    private Dictionary<string, float> type_chance;
    private LinkedList<Vector3Int> fireObjects;

    private int world_width;
    private int world_length;
    private int world_height;

    // Use this for initialization
    void Start()
    {
        type_chance = new Dictionary<string, float>();
        type_chance.Add("GrassBlock(Clone)", 1.0f);
        type_chance.Add("WaterBlock(Clone)", -1.0f);
        type_chance.Add("WoodBlock(Clone)", 1.0f);
        type_chance.Add("LeafBlock(Clone)", 1.0f);


        water_cycle_time = StaticData.Water_cycle_speed;

        fireObjects = new LinkedList<Vector3Int>();

        world_width = procGenEngine.GetComponent<ProcGen>().world_width;
        world_length = procGenEngine.GetComponent<ProcGen>().world_length;
        world_height = procGenEngine.GetComponent<ProcGen>().world_height;

        //Initialize the init fire linked list
        foreach (Vector3Int initFire in procGenEngine.GetComponent<ProcGen>().init_fire_map) {
            fireObjects.AddLast(initFire);
        }
    }

    // Update is called once per cycle
    void Update()
    {
        cycle_timer += Time.deltaTime;
        if (cycle_timer >= water_cycle_time)
        {
            

            LinkedList<Vector3Int> new_fire_objects = new LinkedList<Vector3Int>();

            foreach (Vector3Int fire_obj in fireObjects)
            {
                int ii = fire_obj.x;
                int jj = fire_obj.y;
                int kk = fire_obj.z;
                if (procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk] != null &&
                    procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk].GetComponent<BlockAttributes>().fire_level != 0)
                {
                    UpdateAdjacentBlocks(fire_obj, new_fire_objects);
                }
            }

            // remove the no longer fire objects
            LinkedListNode<Vector3Int> fireObjNode = fireObjects.First;
            while (fireObjNode != null)
            {

                var next = fireObjNode.Next;
                int ii = fireObjNode.Value.x;
                int jj = fireObjNode.Value.y;
                int kk = fireObjNode.Value.z;
                if (procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk] != null &&
                    procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk].GetComponent<BlockAttributes>().death_cycles >= max_death_cycles)
                {
                    //Debug.Log(procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk].GetComponent<BlockAttributes>().fire_level);
                    //Debug.Log(procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk].GetComponent<BlockAttributes>().death_cycles);
                    fireObjects.Remove(fireObjNode);
                    if (procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk].name != "GrassBlock(Clone)")
                    {
                        Destroy(procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk]);
                        procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk] = null;
                    }
                }
                else if (procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk] == null || 
                    procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk].GetComponent<BlockAttributes>().fire_level <= 0)
                {
                    fireObjects.Remove(fireObjNode);
                }
                fireObjNode = next;
                
                
            }

            // add the new fire objects
            foreach (Vector3Int new_fire_obj in new_fire_objects)
            {
                fireObjects.AddLast(new_fire_obj);
            }

            // DEBUG TEST, print all fire objects

            /*foreach (GameObject fire_obj in fireObjects)
            {
                Debug.Log(fire_obj.GetComponent<BlockAttributes>().grid_ind);
            }*/
            
            cycle_timer = 0f;
        }
    }

    void UpdateAdjacentBlocks(Vector3Int fire_block, 
        LinkedList<Vector3Int> new_fire_objects)
    {
        // look at all adjacent blocks
        int i = fire_block.x;
        int j = fire_block.y;
        int k = fire_block.z;

        for (int ii = i-1; ii <= i+1; ii++)
        {
        for (int jj = j-1; jj <= j+1; jj++)
        {
        for (int kk = k-1; kk <= k+1; kk++)
        {
            // check if in bounds
            if (ii < 0 || ii >= world_width || jj < 0 || jj >= world_length || kk < 0 || kk >= world_height)
            {
                continue;
            }

            GameObject block = procGenEngine.GetComponent<ProcGen>().world_map[ii][jj][kk];

            // check if block is even there
            if (block == null || block.name == "WaterBlock(Clone)")
                continue;

            // See if fire can spread
            //Debug.Log(type_chance[block.name]);
            if (true)//Random.value <= type_chance[block.name])
            {
                if (block.GetComponent<BlockAttributes>().fire_level == 3)
                {
                    if (block.GetComponent<BlockAttributes>().death_cycles < max_death_cycles)
                        block.GetComponent<BlockAttributes>().death_cycles++;
                }
                else
                {
                    block.GetComponent<BlockAttributes>().fire_level++;
                    // block just became on fire
                    // ignores grass blocks that are perma fire
                    if (block.GetComponent<BlockAttributes>().fire_level == 1)
                    {
                        new_fire_objects.AddLast(new Vector3Int(ii, jj, kk));
                    }
                }
                fxEngine.GetComponent<FxEngine>().UpdateFireFx(block);
            }
        }
        }

            
        }
    }

    public int GetFireObjectsCount()
    {
        return fireObjects.Count;
    }
}