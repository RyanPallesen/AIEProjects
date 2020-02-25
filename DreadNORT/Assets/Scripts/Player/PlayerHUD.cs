using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

    public Gun gun;

    public TextMeshProUGUI currentClip;
    public TextMeshProUGUI clipSizeText;

    public Image clipImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gun)
        {
            currentClip.text = gun.bulletsInClip.ToString();
            clipSizeText.text = gun.ClipSize.ToString();

            clipImage.fillAmount = (float)gun.bulletsInClip / (float)gun.ClipSize;
        }

        if(gun.isReloading)
        {
            clipImage.fillAmount = gun.cooldownTimer / gun.ReloadSpeed;
            if(gun.cooldownTimer >= gun.ReloadSpeed)
            {
                Debug.LogError("CAUGHT");
            }
        }
    }
}
