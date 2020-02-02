using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    public static CardHandler Instance;

    private bool isPlacing;
    public GameObject prefabToPlace;
    public LayerMask layerMask;
    private int StoredLayer;

    public int gridSize;

    public bool IsPlacing
    {
        get => isPlacing;
        set
        {
            isPlacing = value;
            if (value == true)
            {
                StoredLayer = prefabToPlace.layer;

                prefabToPlace.layer = (int)Mathf.Log(layerMask.value, 2);

                Debug.Log(prefabToPlace.layer);
            }
            else
            {
                prefabToPlace.layer = StoredLayer;

                Debug.Log(prefabToPlace.layer);
            };

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Only one CardHandler should exist at a given time");
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            IsPlacing = !IsPlacing;
        }

        if (IsPlacing)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                prefabToPlace.transform.position = new Vector3(
                Mathf.Round(hit.point.x / gridSize * gridSize),
                Mathf.Round(hit.point.y / gridSize * gridSize),
                Mathf.Round(hit.point.z / gridSize * gridSize)
                                                              );
            }

            prefabToPlace.transform.Rotate(0, Input.mouseScrollDelta.y, 0);
        }
    }
}
