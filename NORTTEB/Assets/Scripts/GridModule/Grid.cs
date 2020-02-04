using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    /*//modifiable value 
    [SerializeField] public float gridSize = 1f;
    
    public Vector3 NearestGridPoint(Vector3 pos)
    {
       pos -= transform.position;

        int xPoint = Mathf.RoundToInt(pos.x / gridSize);
        int yPoint = Mathf.RoundToInt(pos.y / gridSize);
        int zPoint = Mathf.RoundToInt(pos.z / gridSize);

        Vector3 result = new Vector3((float)xPoint * gridSize, (float)yPoint * gridSize, (float)zPoint * gridSize);

        result += transform.position;

        return result;
    }

    //draws cubes at each grid point
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        for(float x = 0; x < 10; x += gridSize)
        {
            for (float z = 0; z < 4; z += gridSize)
            {
                var point = NearestGridPoint(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }*/

    private int rows = 4;
    private int cols = 10;


    void Start()
    {
        InitializeGrid();
    }
       
    private void InitializeGrid()
    {
        for(int row = 0; row < 4; row++ )
        {
            for (int col = 0; col < 10; col++)
            {

            }
        }
    }
}