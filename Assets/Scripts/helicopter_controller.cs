using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class helicopter_controller : MonoBehaviour
{
    public GameObject procGenEngine;
    public GameObject falling_water_engine;
    public float movement_speed;
    public float current_water_level;
    public float water_tank_size;

    public int wideness; //lat width
    public int lengthness; //long length
    public int heightness; //alt height

    public string up_key;
    public string down_key;
    public string left_key;
    public string right_key;
    public string water_adding_key;
    public string water_dispensing_key;
    public Vector4 initial_RGBA;
    public int adding_water_mode_on;
    public int adding_concrete_mode_on;

    public Vector3 starting_position;

    // Use this for initialization
    void Start()
    {

        //starting position information
        int world_width = procGenEngine.GetComponent<ProcGen>().world_width;
        int world_length = procGenEngine.GetComponent<ProcGen>().world_length;
        int world_height = procGenEngine.GetComponent<ProcGen>().world_height;
        //transform.position = new Vector3(world_length / 2, world_height / 2, world_width / 2);
        transform.position = starting_position;
        initial_RGBA = GetComponent<Renderer>().material.color;
        //moveemnt and water content
        movement_speed = 25f;
        current_water_level = 20f;
        water_tank_size = 100f;
        adding_water_mode_on = 0;
        adding_concrete_mode_on = 0;

    }



    // Update is called once per frame
    void Update()
    {
    
        //new plan: n
        int world_width = procGenEngine.GetComponent<ProcGen>().world_width;
        int world_length = procGenEngine.GetComponent<ProcGen>().world_length;
        int world_height = procGenEngine.GetComponent<ProcGen>().world_height;


        //move in x direction
        int vertical_direction;
        if (Input.GetKey(up_key)){
            //up is pressed
            vertical_direction = 1;
        }
        else if (Input.GetKey(down_key)){
            //down is pressed
            vertical_direction = -1;
        }
        else{
            vertical_direction = 0;
        }

        //move in z direction
        int horizontal_direction;
        if (Input.GetKey(right_key))
        {
            //right is pressed
            horizontal_direction = 1;
        }
        else if (Input.GetKey(left_key))
        {
            //left is pressed
            horizontal_direction = -1;
        }
        else{
            horizontal_direction = 0;
        }

        //calculate move
        float dx = movement_speed * vertical_direction * -1f * Time.deltaTime + movement_speed * horizontal_direction * -1f * Time.deltaTime;
        float dz = movement_speed * vertical_direction * -1f * Time.deltaTime + movement_speed * horizontal_direction * 1f * Time.deltaTime;



        //check if out of bound
        if (transform.position.x + dx < 0 | transform.position.x + dx >= world_width - 1)
        {
            dx = 0f;
        }
        if (transform.position.z + dz < 0 | transform.position.z + dz >= world_length - 1)
        {
            dz = 0f;
        }
        transform.Translate(dx, 0f, dz);

        //step 2: check camera vector stuff
        //step 3: combine with water

        //new plan:
        //no need to check height, using only x and y for helicopter.
        //TO DO:
        //check camera vector and dispense water

        // adding water: check coordinates, sound effect, lower tube
        // dispensing water: sound effect, trigger bool for water release animation and fire being put off

        wideness = (int)(Math.Round((transform.position.x), MidpointRounding.AwayFromZero));
        //print("This is widness: ");
        //print(wideness);


        lengthness = (int)(Math.Round((transform.position.z), MidpointRounding.AwayFromZero));
        //print("This is widness: ");
        //print(lengthness);


        heightness = (int)(Math.Round((transform.position.y), MidpointRounding.AwayFromZero));
        //print("This is widness: ");
        //print(heightness);


        // Add water
        if (Input.GetKey(water_adding_key))
        {
            if (current_water_level < water_tank_size)
            {
                //check if can add water by checking the blocks underneath
                if (can_add_water(wideness,lengthness) == 1){
                    current_water_level += 1;
                    adding_water_mode_on = 1;
                    //GetComponent<Renderer>().material.color = new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f);
                    print(current_water_level);
                }
                else{
                    // BS FEATURE
                    adding_concrete_mode_on = 1;
                    //if try to add water when no water underneath
                    print("Cannot add water at current location");
                    //System.Threading.Thread.Sleep(3000);
                   
                }
            }
            else
            {
                print("Current tank is full, cannot add more water");
            }
        }


        //check which color to change the helicopters into
        if(adding_water_mode_on == 1){
            GetComponent<Renderer>().material.color = new Color(0f, 0f, 1f, 1f);
            adding_water_mode_on = 0;
        }
        else if(adding_concrete_mode_on == 1){
            GetComponent<Renderer>().material.color = new Color(128f / 255f, 128f / 255f, 128f / 255f, 1f);
            adding_concrete_mode_on = 0;
        }
        else{
            GetComponent<Renderer>().material.color = initial_RGBA;
        }

        // Dispense water
        if (Input.GetKeyDown(water_dispensing_key))
        {
            if (current_water_level > 0f)
            {
                current_water_level -= 1;
                falling_water_engine.GetComponent<FallingWaterEngine>().DropWater(transform.position.x, transform.position.y - 1, transform.position.z);
                print(current_water_level);
            }
            else
            {
                print("Current tank is empty, cannot dispense more water");
            }
        }
    }

    //if water block is right underneath and can add water
    //return 1 if water can be added
    //return 0 if water cannot be added
    int can_add_water(int how_wide, int how_long)
    {
        List<GameObject> vertical_points = procGenEngine.GetComponent<ProcGen>().world_map[how_wide][how_long];
        int max_height = 0;


        for (int i = 0; i < vertical_points.Count; i++)
        { //assuming helicopters are higher than trees
            if (vertical_points[i])
            {
                if (i > max_height)
                {
                    max_height = i;
                }
                print(vertical_points[i]);
            }
            else
            {
                //print("this is NU")
            }

        }
        if (vertical_points[max_height].name == "WaterBlock(Clone)")
        {
            //current_water_level += 1;
            return 1;
        }
        else
        {
            //print("Cannot add water");
            return 0;
        }
    }
}
