using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool estGameOver;

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
        if (menuPause) menuPause.SetActive(false);
        
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Pause();
        }
        if(ListePucerons.Singleton.LesPucerons.Count <= 0 && !estGameOver) GameOver(TypeMort.pluPuceron);
    }

    public void GameOver(TypeMort typeMort)
    {
        if(!global::GameOver.Singleton) return;
        Time.timeScale = 0;
        int score = ScoreDistance;
        EnregistrerScore();
        bool estMeilleurScore = score == PlayerPrefs.GetInt("MeilleurScore");
        global::GameOver.Singleton.AfficherGameOver(score,estMeilleurScore,typeMort);
        estGameOver = true;
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

    public void ChargerScene(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }

    public void OuvrirURL(string url)
    {
        Application.OpenURL(url);
    }

    private void EnregistrerScore()
    {
        int ancienMeilleurScore = PlayerPrefs.GetInt("MeilleurScore");
        int nvMeilleurScore = ScoreDistance > ancienMeilleurScore ? ScoreDistance : ancienMeilleurScore;
        
        PlayerPrefs.SetInt("MeilleurScore", nvMeilleurScore);
    }
}
