using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCube : MonoBehaviour
{
    public bool isDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDestroyed)
        {
            GetComponent<Renderer>().material.SetFloat("_Type", 1);
        }
        else
        {
            GetComponent<Renderer>().material.SetFloat("_Type", 0);

        }
    }
}
