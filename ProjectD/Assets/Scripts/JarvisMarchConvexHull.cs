using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JarvisMarchConvexHull 
{

    public static HashSet<Vector3> GetHull(Vector3[] points)
    {
        HashSet<Vector3> result = new HashSet<Vector3>();

        int leftMostIndex = 0;
        for (int i = 1; i < points.Length; i++)
        {
            if (points[leftMostIndex].x > points[i].x)
                leftMostIndex = i;

            else if( (points[leftMostIndex].x == points[i].x))
            {
                if (points[leftMostIndex].y > points[i].y)
                    leftMostIndex = i;
            }
        }
        result.Add(points[leftMostIndex]);
        List<Vector3> collinearPoints = new List<Vector3>();
        Vector3 current = points[leftMostIndex];

        while (true)
        {
            Vector3 nextTarget = points[0];
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i] == current)
                    continue;
                float x1, x2, y1, y2;
                x1 = current.x - nextTarget.x;
                x2 = current.x - points[i].x;

                y1 = current.z - nextTarget.z;
                y2 = current.z - points[i].z;

                float val = (y2 * x1) - (y1 * x2);
                if (val > 0)
                {
                    nextTarget = points[i];
                    collinearPoints = new List<Vector3>();
                }
                else if (val == 0)
                {
                    if (Vector2.Distance(current, nextTarget) < Vector2.Distance(current, points[i]))
                    {
                        collinearPoints.Add(nextTarget);
                        nextTarget = points[i];
                    }
                    else
                        collinearPoints.Add(points[i]);                    
                }
            }

            foreach (Vector3 t in collinearPoints)
                result.Add(t);            
            if (nextTarget == points[leftMostIndex])
                break;
            result.Add(nextTarget);
            current = nextTarget;
        }

        return result;
    }

}
