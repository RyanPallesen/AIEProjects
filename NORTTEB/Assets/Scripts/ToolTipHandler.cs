using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipHandler : MonoBehaviour
{
    public EventSystem eventSystem;
    public TextMeshProUGUI textmesh;

    private static ToolTipHandler _instance;

    public static ToolTipHandler Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Update is called once per frame
    private void Update()
    {
       
    }
}
