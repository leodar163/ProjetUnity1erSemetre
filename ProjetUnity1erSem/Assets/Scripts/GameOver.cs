using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
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
    
    public void AfficherGameOver(int score, bool estMeilleurScore, GameManager.TypeMort typeMort)
    {
        gameObject.SetActive(true);
        texteScore.text = "";
        if (estMeilleurScore)
        {
            texteScore.text = "MEILLEUR SCORE !!!\n";
        }

        texteScore.text += score.ToString();

        switch (typeMort)
        {
            case GameManager.TypeMort.tomber :
                break;
                texteMort.text = "Tu es tombé du bateau ! Les fourmis ne savent pas nager...";
            case GameManager.TypeMort.pluPuceron :
                texteMort.text = "Tu n'as plus de puceron. Tu est condamné à errer sur les fots";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(typeMort), typeMort, null);
        }
    }
}
