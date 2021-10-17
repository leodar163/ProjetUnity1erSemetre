using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puceron : MonoBehaviour
{
    [SerializeField] private Transform cul;
    [SerializeField] private SpriteRenderer sprRend;
    [Header("Couleurs")]
    [SerializeField] private Color32 couleurLaitbuvable;
    [SerializeField] private Color32 couleurLaitNonBuvable;
    
    [Space(20)]
    [Tooltip("Le temps que met le lait à arriver à 100%")]
    [SerializeField] private float tempsSecretionLait;

    public bool goutteBuvable;

    public bool estAttaque = false;
    
    private void Start()
    {
        cul.localScale = goutteBuvable ? Vector3.one : Vector3.zero;
    }

    private void Update()
    {
        SecreterLait();
    }

    private void SecreterLait()
    {
        if (cul.localScale.x >= 1)
        {
            cul.localScale = Vector3.one;
            goutteBuvable = true;
            sprRend.color = couleurLaitbuvable;
        }
        else
        {
            cul.localScale += Vector3.one * (1 / tempsSecretionLait * Time.deltaTime);
            goutteBuvable = false;
            sprRend.color = couleurLaitNonBuvable;
        }
    }

    public void RecolterLait()
    {
        cul.localScale = Vector3.zero;
    }

    public void Mourir()
    {
        ListePucerons.Singleton.RetirerPuceron(this);
        Destroy(gameObject);
    }
}