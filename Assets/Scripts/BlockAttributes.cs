using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAttributes : MonoBehaviour
{

    public int fire_level;
    public int death_cycles;


    public Vector3Int grid_ind;

    // Use this for initialization
    void Start()
    {
        // convert transform to grid index
        grid_ind = new Vector3Int(Mathf.RoundToInt(transform.position.x),
            Mathf.RoundToInt(transform.position.z),
            Mathf.RoundToInt(transform.position.y));
    }

    // Update is called once per frame
    void Update()
    {

    }
}