using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentTracking : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] float distance;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ship.transform.localPosition = new Vector3(0, 2450 - Hand.Instance.Movement * distance, 0);
    }
}