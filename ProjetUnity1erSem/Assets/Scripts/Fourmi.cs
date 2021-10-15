using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fourmi : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] private Transform pied;
    [SerializeField] private Transform origineDroite;
    [SerializeField] private Transform origineGauche;


    [SerializeField] private KeyCode toucheDroite;
    [SerializeField] private KeyCode toucheGauche;
    [SerializeField] private KeyCode toucheSaut;

    [SerializeField] private float forceSaut = 50;
    [SerializeField] private float vitesse = 1.5f;
    [SerializeField] private float distance = 1.5f;
    
    [SerializeField] private bool auSol;
    [SerializeField] private bool auMur;
    [SerializeField] private bool enMouvement;

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
        DetectMur();

    }

    void MouvementLateral()
    {
        enMouvement = false;
            
        if (Input.GetKey(toucheDroite))
        {
            if(!auMur)
            {
                rb.velocity = new Vector2(vitesse, rb.velocity.y);
                enMouvement = true;
            }
           
        }

        if (Input.GetKey(toucheGauche))
        {
            if (!auMur)
            {
                rb.velocity = new Vector2(-vitesse, rb.velocity.y);
                enMouvement = true;
            }
            
        }

       if (!enMouvement & auSol)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
       
    }

    void MouvementSaut()
    {
        if (Input.GetKeyDown(toucheSaut))
        {
            if (auSol)
            {
                rb.AddForce(new Vector2(0, forceSaut));
                enMouvement = true;
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

    void DetectMur()
    {
        Vector2 origine1 = origineDroite.position;
       // Vector2 direction1 = Vector2.right;
       Vector2 direction1 = new Vector2(0,0);

        RaycastHit2D hit3 = Physics2D.Raycast(origine1, direction1, distance, DetectionSol);

        if(hit3)
        {
            auMur = true;
        }

        if (hit3)
        {
            Debug.DrawRay(origine1, direction1 * distance, Color.green);
        }
        else
        {
            Debug.DrawRay(origine1, direction1 * distance, Color.red);
        }
    }
    
}
