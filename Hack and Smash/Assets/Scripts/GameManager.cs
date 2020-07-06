using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    

    [System.Serializable]
    public struct CapturePoint
    {
        public string name;

        public Transform spawnPosition;

        public Transform CapturePosition;
        public float captureRadius;
    }

    [System.Serializable]
    public struct ThroughLine
    {
        public string name;

        public CapturePoint pointFrom;
        public CapturePoint pointTo;
        public List<Transform> FromToTransforms;
    }

    public List<CapturePoint> capturePoints = new List<CapturePoint>();
    public List<ThroughLine> throughLines = new List<ThroughLine>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void chooseGame()
    {
        CapturePoint currentCapturePoint = capturePoints[Random.Range(0, capturePoints.Count)];
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
