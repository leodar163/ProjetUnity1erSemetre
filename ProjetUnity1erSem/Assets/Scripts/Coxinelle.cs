using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;

public class Coxinelle : MonoBehaviour
{
    [SerializeField] private float vitesse;
    [SerializeField] private float distanceAttaque;
    [SerializeField] private float tmpsPrTuerPuceron;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sprRend;

    private Vector2 pointSpawn;
    private Vector2 pointPuceron;
    private Puceron puceronANiquer;
    private bool peutAttaquer;
    private bool sEnfuit;
    private AlerteCoxi alerteCoxi;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (sEnfuit)
        {
             SEnfuire();
        }
        else
        {
            RejoindrePuceronANiquer();
        }
    }

    
    private void RejoindrePuceronANiquer()
    {
        if (puceronANiquer)
        {
            if (Vector2.Distance(transform.position, pointPuceron) > distanceAttaque)
            {
                Vector2 direction = (pointPuceron - (Vector2)transform.position).normalized;
                rb.velocity = direction * vitesse;
            }
            else if (!peutAttaquer)
            {
                peutAttaquer = true;
                StartCoroutine(AttaquerPuceron());
            }
            else
            { 
                rb.velocity = Vector2.zero; 
            }
        }
        
    }

    private IEnumerator AttaquerPuceron()
    {
        alerteCoxi = GenerateurAlerteCoxi.GenererAlerteCoxi(this);
        
        yield return new WaitForSeconds(tmpsPrTuerPuceron);
        
        puceronANiquer.Mourir();
        sEnfuit = true;
    }

    public void AssignerPointSpawn(Vector2 origine)
    {
        pointSpawn = origine;
    }
    
    public void AssignerPuceron(Puceron puceron)
    {
        puceronANiquer = puceron;
        pointPuceron = puceron.RecupPointAttaquePlusProche(transform.position);
        puceronANiquer.estAttaque = true;
    }

    public void Mourir()
    {
        if(alerteCoxi) Destroy(alerteCoxi.gameObject);
        if(puceronANiquer) puceronANiquer.estAttaque = false;
        Destroy(gameObject);
    }

    private void SEnfuire()
    {
        if(alerteCoxi) Destroy(alerteCoxi.gameObject);
        Vector2 direction = (pointSpawn - (Vector2)transform.position).normalized;
        rb.velocity = direction * vitesse;

        if (Vector2.Distance(pointSpawn, transform.position) <= 0.05f)
        {
            Mourir();
        }
    }
}
