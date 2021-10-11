using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fourmi : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Transform pied;


    [SerializeField] private KeyCode droite;
    [SerializeField] private KeyCode gauche;
    [SerializeField] private KeyCode saut;

    [SerializeField] private float forceSaut = 50;
    [SerializeField] private float vitesse = 1.5f;
    [SerializeField] private float distance = 1.5f;

    [SerializeField] private bool auSol;

    [SerializeField] private LayerMask DetectionSol;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouvementLateral();
        MouvementSaut();
        DetectSol();

    }

    void MouvementLateral()
    {
        if (Input.GetKey(droite))
        {
            rb.velocity = new Vector2(vitesse, rb.velocity.y);
        }
        /*else
        {
            vitesse = 0;
        }*/

        if (Input.GetKey(gauche))
        {
            rb.velocity = new Vector2(-vitesse, rb.velocity.y);
        }
        /*else
        {
            vitesse = 0;
        }*/
    }

    void MouvementSaut()
    {
        if (Input.GetKeyDown(saut))
        {
            if (auSol == true)
            {
                rb.AddForce(new Vector2(0, forceSaut));
            }
        }
    }
    
    void DetectSol()
    {
        Vector2 origine = pied.position;
        Vector2 direction = Vector2.down;
        Vector2 decalage = new Vector2(0.13f, 0);

        RaycastHit2D hit = Physics2D.Raycast(origine - decalage, direction, distance, DetectionSol);
        RaycastHit2D hit1 = Physics2D.Raycast(origine, direction, distance, DetectionSol);
        RaycastHit2D hit2 = Physics2D.Raycast(origine + decalage, direction, distance, DetectionSol);
        
        if (hit || hit1 || hit2)
        {
            auSol = true;
        }
        else
        {
            auSol = false;
            
        }

        if(hit)
        {
            Debug.DrawRay(origine - decalage, direction * distance, Color.green);
        }
        else
        {
            Debug.DrawRay(origine - decalage, direction * distance, Color.red);
        }

        if(hit1)
        {
            Debug.DrawRay(origine, direction * distance, Color.green);
        }
        else
        {
            Debug.DrawRay(origine, direction * distance, Color.red);
        }

        if(hit2)
        {
            Debug.DrawRay(origine + decalage, direction * distance, Color.green);
        }
        else
        {
            Debug.DrawRay(origine + decalage, direction * distance, Color.red);
        }

    }
    
}
