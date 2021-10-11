using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }
}
