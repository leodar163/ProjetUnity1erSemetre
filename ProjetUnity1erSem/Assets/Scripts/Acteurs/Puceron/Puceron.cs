using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Puceron : MonoBehaviour
{
    [Header("Parties du corps")] 
    [SerializeField] private Transform ptAttaqueDroite;
    [SerializeField] private Transform ptAttaqueGauche;
    [SerializeField] private Transform cul;
    [SerializeField] private SpriteRenderer sprRend;

    [Header("Sprites Lait")]
    [SerializeField] private List<Sprite> etapesLait;

    [Space(20)]
    [Tooltip("Le temps que met le lait à arriver à 100%")]
    [SerializeField] private float tempsSecretionLait;

    private bool goutteBuvable => tailleGoutte >= 1;

    public bool estAttaque;
    private float tailleGoutte;
    
    private void Start()
    {

    }

    private void Update()
    {
        SecreterLait();
        GererEtapesLait();
    }

    private void SecreterLait()
    {
        tailleGoutte += 1 / tempsSecretionLait * Time.deltaTime;
        tailleGoutte = Mathf.Clamp01(tailleGoutte);
    }

    private void GererEtapesLait()
    {
        if (etapesLait == null || etapesLait.Count == 0) return;
        int index = (int)Mathf.Lerp(0, etapesLait.Count -1, tailleGoutte);
        sprRend.sprite = etapesLait[index];
    }
    
    public void RecolterLait()
    {
        if(goutteBuvable) tailleGoutte = 0;
    }

    public void Mourir()
    {
        ListePucerons.Singleton.RetirerPuceron(this);
        Destroy(gameObject);
    }
    
    public Vector2 RecupPointAttaquePlusProche(Vector2 position)
    {
        return Vector2.Distance(position, ptAttaqueDroite.position) > 
               Vector2.Distance(position, ptAttaqueGauche.position) ? 
            ptAttaqueGauche.position : ptAttaqueDroite.position;
    }
}
