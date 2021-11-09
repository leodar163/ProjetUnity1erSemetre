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
    [SerializeField] private GameObject Menu;

    private void Awake()
    {
        Menu.SetActive(false);
    }

    public void AfficherGameOver(int score, bool estMeilleurScore, GameManager.TypeMort typeMort)
    {
        Menu.SetActive(true);
        texteScore.text = "";
        if (estMeilleurScore)
        {
            texteScore.text = "MEILLEUR DISTANCE !!!    ";
        }

        texteScore.text += "Distance Parcourue : " + score + "M";

        texteMort.text = typeMort switch
        {
            GameManager.TypeMort.tomber => "Tu es tombe du bateau ! Les fourmis ne savent pas nager...",
            GameManager.TypeMort.pluPuceron => "Tu n'as plus de puceron. Tu est condamne à errer sur les flots",
            _ => throw new ArgumentOutOfRangeException(nameof(typeMort), typeMort, null)
        };
    }
}
