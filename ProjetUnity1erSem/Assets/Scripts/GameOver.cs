using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    static private GameOver cela;

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

        switch (typeMort)
        {
            case GameManager.TypeMort.tomber :
                texteMort.text = "Tu es tombe du bateau ! Les fourmis ne savent pas nager...";
                break;
            case GameManager.TypeMort.pluPuceron :
                texteMort.text = "Tu n'as plus de puceron. Tu est condamné à errer sur les fots";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeMort), typeMort, null);
        }
    }
}
