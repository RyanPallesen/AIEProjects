using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position += new Vector3(
            Random.RandomRange(-4, 4),
            Random.RandomRange(-4, 4),
            Random.RandomRange(-4, 4));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
