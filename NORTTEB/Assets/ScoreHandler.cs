using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    public Image AirImage;
    public Image MetalImage;
    public Image FuelImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        GetComponent<TextMeshProUGUI>().text = "Movement : " + (Hand.Instance.Movement * 100) + "k KM";
    }
}
