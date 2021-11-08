using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textScore;
    private static GameManager cela;

    public static GameManager Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<GameManager>();
            return cela;
        }
    }
    
    private float scoreDistance;
    public int ScoreDistance => (int)scoreDistance;

    [SerializeField] private float metreParSec = 0.1f;
    [SerializeField] private GameObject menuPause;
    private bool estEnPause;

    public void AjouterScore(Scarabe.Fatigue nivoFatigue)
    {
        scoreDistance += metreParSec * (int)nivoFatigue * Time.deltaTime;
        textScore.text = ScoreDistance + "M";
    }

    public enum  TypeMort
    {
        tomber,
        pluPuceron
    }

    private void Awake()
    {
        if (!menuPause) menuPause = GameObject.FindWithTag("MenuPause");
        if (menuPause)  menuPause.SetActive(false);
    }

    public void GameOver(TypeMort typeMort)
    {
        Quitterjeu();
    }

    public void Pause()
    {
        Time.timeScale = estEnPause ? 1 : 0;
        estEnPause = !estEnPause;
        menuPause.SetActive(estEnPause);
    }
    
    public void Quitterjeu()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
