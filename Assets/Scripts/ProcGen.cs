using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcGen : MonoBehaviour {

    public int world_width;
    public int world_length;
    public int world_height;
    public float height_scale;
    public float tree_chance;
    public float leaf_chance;
    public int max_tree_height;
    public int max_init_fires;

    public int SEA_LEVEL;

    public int seed;
    public float noisyness;

    Vector3 world_center;

    public GameObject grass_block;
    public GameObject water_block;
    public GameObject wood_block;
    public GameObject leaf_block;

    public GameObject fxEngine;

    public List<List<List<GameObject>>> world_map;
    public List<Vector3Int> leaf_map; // for storing location of leaf block, useful for fire engine
    public List<Vector3Int> init_fire_map;

    public int num_grass_blocks;
    public int num_wood_blocks;
    public int num_water_blocks;
    public int num_leaf_blocks;

    List<List<float>> height_map;
    List<List<int>> height_int_map;

    void InitHeightMap()
    {
        // Initiliazes the height map to be used with the noise world gen
        //hmap_width = world_width / world_to_hmap;
        //hmap_length = world_length / world_to_hmap;

        height_map = new List<List<float>>();
        height_map.Capacity = world_width;
        height_int_map = new List<List<int>>();
        height_int_map.Capacity = world_width;
        for (int i = 0; i < world_width; i++)
        {
            height_map.Add(new List<float>());
            height_map[i].Capacity = world_length;
            height_int_map.Add(new List<int>());
            height_int_map[i].Capacity = world_length;
            for (int j = 0; j < world_length; j++)
            {
                height_map[i].Add(0.0f);
                height_map[i][j] = Mathf.PerlinNoise(
                    seed + (float)i / world_width * noisyness,
                    seed + (float)j / world_height * noisyness
                );
                //Debug.Log(i.ToString() + "," + j.ToString());
                //Debug.Log(height_map[i][j]);
                height_int_map[i].Add(0);
                int height = (int)(height_map[i][j] * height_scale);
                if (height >= world_height)
                {
                    height = world_height - 1;
                }
                height_int_map[i][j] = height;
            }
        }
    }

    void InitSurface()
    {
        world_map = new List<List<List<GameObject>>>();
        world_map.Capacity = world_width;
        for (int i = 0; i < world_width; i++)
        {
            world_map.Add(new List<List<GameObject>>());
            world_map[i].Capacity = world_length;
            for (int j = 0; j < world_length; j++)
            {
                world_map[i].Add(new List<GameObject>());
                world_map[i][j].Capacity = world_height;
                for (int k = 0; k < world_height; k++)
                {
                    world_map[i][j].Add(null);
                }

                int height = height_int_map[i][j];
                GameObject block;

                if (height > SEA_LEVEL)
                {
                    block = grass_block;
                } 
                else // (height <= SEA_LEVEL)
                {
                    block = water_block;
                }

                // hide interior
                if (i == 0 || i == world_width-1 || j == 0 || j == world_width - 1)
                {
                    for (int k = height-1; k >= 0; k--)
                    {
                        world_map[i][j][k] = Instantiate(block, new Vector3(i, k, j), Quaternion.identity);
                        
                        // spaghetti code
                        if (block == grass_block)
                        {
                            num_grass_blocks++;
                        } else if (block == water_block)
                        {
                            num_water_blocks++;
                        }
                    }
                }
                world_map[i][j][height] = Instantiate(block, new Vector3(i, height, j), Quaternion.identity);
                // spaghetti code
                if (block == grass_block)
                {
                    num_grass_blocks++;
                }
                else if (block == water_block)
                {
                    num_water_blocks++;
                }
            }
        }
    }

    void InitTrees()
    {
        leaf_map = new List<Vector3Int>();

        for (int i = 0; i < world_width; i++)
        {
            for (int j = 0; j < world_length; j++)
            {
                int height = height_int_map[i][j];
                // if grass block, chance to put tree block
                // also if not on edge
                if (height > SEA_LEVEL && i != 0 && i != world_width - 1 && j != 0 && j != world_length - 1)
                {
                    float rand_val = Random.value;
                    bool tree_here = rand_val <= tree_chance;
                    //Debug.Log(rand_val);
                    if (! tree_here)
                    {
                        continue;
                    }
                    int tree_top = (int)Random.Range((float)(height+1), (float) (height + max_tree_height));
                    tree_top = Mathf.Min(tree_top, world_height - 1);

                    if (tree_top < world_height - 1)
                    {
                        world_map[i][j][tree_top + 1] = Instantiate(leaf_block, new Vector3(i, tree_top + 1, j), Quaternion.identity);
                        num_leaf_blocks++;
                    }
                    for (int k = tree_top; k > height; k--)
                    {
                        world_map[i][j][k] = Instantiate(wood_block, new Vector3(i, k, j), Quaternion.identity);
                        num_wood_blocks++;

                        // Add leaves
                        if (k > height + 6)
                        {
                            for (int x = 0; x < 4; x++)
                            {
                                int ii, jj;
                                switch(x)
                                {
                                    case 0:
                                        ii = i - 1;
                                        jj = j;
                                        break;
                                    case 1:
                                        ii = i + 1;
                                        jj = j;
                                        break;
                                    case 2:
                                        ii = i;
                                        jj = j + 1;
                                        break;
                                    case 3:
                                        ii = i;
                                        jj = j - 1;
                                        break;
                                    default:
                                        Debug.Log("error");
                                        ii = i;
                                        jj = j;
                                        // impossible
                                        break;
                                }
                                if (ii >= world_width || ii < 0 || jj >= world_length || jj < 0)
                                    continue;
                                if (world_map[ii][jj][k] != null)
                                    continue;
                                world_map[ii][jj][k] = Instantiate(leaf_block, new Vector3(ii, k, jj), Quaternion.identity);
                                num_leaf_blocks++;
                                leaf_map.Add(new Vector3Int(ii, jj, k));
                            }

                            // check adjacent blocks in the horizontal plane.
                            // if null, chance to put leaf block there
                        }
                    }
                }
            }
        }
    }
    
    void InitFires()
    {
        int last_ind = leaf_map.Count - 1;
        //Debug.Log("number of leaves" + leaf_map.Count.ToString());
        for (int i = 0; i < max_init_fires; i++)
        {
            int ind = (int)Random.Range(0.0f, last_ind);
            //Debug.Log(ind);

            int ii = leaf_map[i].x;
            int jj = leaf_map[i].y;
            int kk = leaf_map[i].z;
            world_map[ii][jj][kk].GetComponent<BlockAttributes>().fire_level = 1;
            fxEngine.GetComponent<FxEngine>().UpdateFireFx(world_map[ii][jj][kk]);
            init_fire_map.Add(leaf_map[i]);
            //Debug.Log(leaf_map[i].name);
        }
    }

	// Use this for initialization
	void Awake () {
        world_center = new Vector3((float)world_width / 2.0f, 0.0f, (float)world_length / 2.0f);
        //Debug.Log(world_center);
        seed = StaticData.Seed;
        SEA_LEVEL = StaticData.Sea_level;
        Random.InitState(seed);
        InitHeightMap();
        InitSurface();
        InitTrees();
        InitFires();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
