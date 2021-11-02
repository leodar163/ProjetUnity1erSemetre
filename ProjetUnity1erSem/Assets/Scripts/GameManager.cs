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

    public void AjouterScore(Scarabe.Fatigue nivoFatigue)
    {
        scoreDistance += metreParSec * (int)nivoFatigue * Time.deltaTime;
        textScore.text = ScoreDistance + " m";
    }

    public enum  TypeMort
    {
        tomber,
        plusMoucheron
    }
    public void GameOver(TypeMort typeMort)
    {
        Quitterjeu();
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
