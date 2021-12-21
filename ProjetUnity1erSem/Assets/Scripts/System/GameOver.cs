using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private static GameOver cela;

    public static GameOver Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<GameOver>();
            return cela;
        }
         
    }

    [SerializeField] private TextMeshProUGUI texteScore;
    [SerializeField] private TextMeshProUGUI texteMort;
    [SerializeField] private TextMeshProUGUI textCoxiTuees;
    [SerializeField] private GameObject Menu;

    private void Awake()
    {
        Menu.SetActive(false);
    }

    private void CompterCoxiTuees()
    {
        int statCoxi = Coxinelle.coxiMorte;
        textCoxiTuees.text = statCoxi + " Coccinelles tuees";
    }
    public void AfficherGameOver(int score, bool estMeilleurScore, GameManager.TypeMort typeMort)
    {
        Menu.SetActive(true);
        texteScore.text = "";
        if (estMeilleurScore)
        {
            texteScore.text = "MEILLEURE DISTANCE !!!    ";
        }

        texteScore.text += "Distance Parcourue : " + score + "M";

        texteMort.text = typeMort switch
        {
            GameManager.TypeMort.tomber => "Tu es tombee du bateau ! Les fourmis ne savent pas nager...",
            GameManager.TypeMort.pluPuceron => "Tu n'as plus de puceron. Tu es condamnee a errer sur les flots",
            _ => throw new ArgumentOutOfRangeException(nameof(typeMort), typeMort, null)
        };
        CompterCoxiTuees();
    }
}
