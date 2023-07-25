using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CW.Common;
using Lean.Transition;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private LeanPlayer introUIAnimation;
    [SerializeField]
    private GameObject respawnGO;
    [SerializeField]
    private LeanPlayer respawnAnimation;

    [SerializeField]
    private GameObject gameOverGO;
    [SerializeField]
    private LeanPlayer gameOverAnimation;

    [SerializeField]
    private TMP_Text currentLives;
    // Start is called before the first frame update
    void Start()
    {
        ResetRespawnTransition();
        ResetGameOverTransition();
    }



    public void PlayIntroTransition()
    {
        introUIAnimation.Begin();
    }

    //respawn trainsition 
    public void PlayRespawnTransition()
    {
        respawnGO.SetActive(true);

        respawnAnimation.Begin();
    }

    public void ResetRespawnTransition()
    {
        respawnGO.transform.localScale = new Vector3(0, 0, 0);
        respawnGO.transform.eulerAngles = new Vector3(0, 0, 0);
        respawnGO.SetActive(false);
    }

    public void PlayGameOverTransition()
    {
        gameOverGO.SetActive(true);

        gameOverAnimation.Begin();
    }

    public void ResetGameOverTransition()
    {
        gameOverGO.transform.localScale = new Vector3(0, 0, 0);
        gameOverGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -350);
        gameOverGO.SetActive(false);
    }

    public void UpdateCurrentLives(int currentLife)
    {
        currentLives.text = "Current Lives:" + currentLife.ToString();
    }
}
