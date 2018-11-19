using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData {

    // Use this for initialization
    private static float water_cycle_speed;
    private static int seed, sea_level;

    public static float Water_cycle_speed
    {
        get
        {
            return water_cycle_speed;
        }
        set
        {
            water_cycle_speed = value;
        }
    }

    public static int Seed
    {
        get
        {
            return seed;
        }
        set
        {
            seed = value;
        }
    }

    public static int Sea_level
    {
        get
        {
            return sea_level;
        }
        set
        {
            sea_level = value;
        }
    }
}
