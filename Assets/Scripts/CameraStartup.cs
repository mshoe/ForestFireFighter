using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStartup : MonoBehaviour {

    public Vector3 world_center;
    public float camera_distance;

	// Use this for initialization
	void Start () {
        Vector3 camera_pos = new Vector3(camera_distance + world_center.x, 1.0f * camera_distance + world_center.y, camera_distance + world_center.z);
        transform.position = camera_pos;
        transform.LookAt(world_center);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
