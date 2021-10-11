using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditorInternal;
using UnityEngine;

public class Coxinelle : MonoBehaviour
{
    [SerializeField] private float vitesse;
    [SerializeField] private float distanceAttaque;
    [SerializeField] private float tmpsPrTuerPuceron;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 pointSpawn;
    private Puceron puceronANiquer;
    private bool peutAttaquer;
    private bool sEnfuit;
    
    // Start is called before the first frame update
    void Start()
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
            if (Vector2.Distance(transform.position, puceronANiquer.transform.position) > distanceAttaque)
            {
                Vector2 direction = (puceronANiquer.transform.position - transform.position).normalized;
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
        puceronANiquer.estAttaque = true;
    }

    public void Mourir()
    {
        puceronANiquer.estAttaque = false;
    }

    private void SEnfuire()
    {
        Vector2 direction = (pointSpawn - (Vector2)transform.position).normalized;
        rb.velocity = direction * vitesse;

        if (Vector2.Distance(pointSpawn, transform.position) <= 0.05f)
        {
            Destroy(gameObject);
        }
    }
}
