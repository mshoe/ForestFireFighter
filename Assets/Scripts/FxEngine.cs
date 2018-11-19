using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxEngine : MonoBehaviour {

    public GameObject fireFX;
    private int fire_counter;

    public void UpdateFireFx(GameObject block)
    {
        int fire_level = block.GetComponent<BlockAttributes>().fire_level;
        if (fire_level <= 0)
        {
            // remove any fire fx on block
            if (block.transform.Find("Fire(Clone)") != null)
            {
                Destroy(block.transform.Find("Fire(Clone)").gameObject);
            }
        }
        else if (fire_level >= 1)
        {
            // remove any fire fx on block
            // add level 1 fire fx to block
            if (block.transform.Find("Fire(Clone)") == null)
            {
                GameObject new_fire = Instantiate(fireFX, block.transform.position, Quaternion.identity);
                new_fire.transform.parent = block.transform;
                fire_counter++;
                if (fire_counter >= 3)
                {
                    fire_counter = 0;
                    new_fire.transform.Find("FireParticles").gameObject.SetActive(true);
                }
            }
        }
        
    }

    // Use this for initialization
    void Start () {
        fire_counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
