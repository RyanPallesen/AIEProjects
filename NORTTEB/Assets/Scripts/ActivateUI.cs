using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateUI : MonoBehaviour
{
    [SerializeField] GameObject gameCredits;
    [SerializeField] Button playButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button quitButton;
    [SerializeField] Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        gameCredits.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCreditsPressed()
    {
        gameCredits.SetActive(true);
        backButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        creditsButton.gameObject.SetActive(false);
    }

    public void OnBackPressed()
    {
        gameCredits.SetActive(false);
        backButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        creditsButton.gameObject.SetActive(true);
    }
}