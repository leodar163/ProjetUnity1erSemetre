using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fourmi : MonoBehaviour
{
    [Header ("Refs")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spriteRendererAbdomene;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform pied;
    [SerializeField] private Transform origineDroite;
    [SerializeField] private Transform origineGauche;
    [SerializeField] private LayerMask detectionSol;
    [SerializeField] private LayerMask maskCoxi;

    [Header ("Commandes")]
    [SerializeField] private KeyCode toucheDroite;
    [SerializeField] private KeyCode toucheGauche;
    [SerializeField] private KeyCode toucheSaut;
    [SerializeField] private KeyCode toucheAttaque;
    [SerializeField] private KeyCode toucheInteraction;
   
    [Header ("Variables")]
    [SerializeField] private float forceSaut = 50;
    [SerializeField] private float vitesse = 1.5f;
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private float distanceAttaque = 1;
    [SerializeField] private Vector2 tailleBoite;
    private bool regardeDroite;
    
    [Header ("Triggers")]
    [SerializeField] private bool auSol;
    [SerializeField] private bool auMur;
    [SerializeField] private bool enMouvement;
    [SerializeField] private bool aPortee;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(origineDroite.position, tailleBoite);
    }

    void Start()
    {
        
    }

    void Update()
    {
        MouvementLateral();
        MouvementSaut();
        DetectSol();
        DetectMur();
        GererAnims();

        if (Input.GetKeyDown(toucheAttaque))
        {
            animator.SetTrigger("Attaque");
        }

        if (Input.GetKeyDown(toucheInteraction))
        {
            Interaction();
        }

    }

    #region Mouvement
    // L� elle bouge
    #region Lateral
    //Les mouvements Droite Gauche
    private void MouvementLateral()
    {
        enMouvement = false;
            
        if (Input.GetKey(toucheDroite))
        {
            if(!auMur)
            {
                rb.velocity = new Vector2(vitesse, rb.velocity.y);
                enMouvement = true;
            }
            regardeDroite = true;
           
        }

        if (Input.GetKey(toucheGauche))
        {
            if (!auMur)
            {
                rb.velocity = new Vector2(-vitesse, rb.velocity.y);
                enMouvement = true;
            }
            regardeDroite = false;
            
        }

        if (!enMouvement & auSol)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
       
    }

    void DetectMur()
    {
        auMur = false;

        Vector2 origine1 = origineDroite.position;
        Vector2 direction1 = new Vector2(0.5f, 0);
        /**Vector2 origine2 = origineGauche.position;
        Vector2 direction2 = new Vector2(-0.5f, 0);

        RaycastHit2D hit3 = Physics2D.Raycast(origine1, direction1, distance, DetectionSol);
        RaycastHit2D hit4 = Physics2D.Raycast(origine2, direction2, distance, detectionSol);**/
        Collider2D hit3 = Physics2D.OverlapBox(origine1, direction1, 0, detectionSol);

        if (hit3 &&( Input.GetKey(toucheDroite) || Input.GetKey(toucheGauche)))
        {
            auMur = true;
        }

        /**if (hit3 && Input.GetKey(toucheGauche))
        {
            auMur = true;
        }

        if (rb.velocity.x > 0)
        {
            if (!auMur)
            {
                Debug.DrawRay(origine1, direction1 * distance, Color.green);
            }
            else
            {
                Debug.DrawRay(origine1, direction1 * distance, Color.red);
            }
        }

        else if (rb.velocity.x < 0)
        {
            if (!auMur)
            {
                Debug.DrawRay(origine2, direction2 * distance, Color.green);
            }
            else
            {
                Debug.DrawRay(origine1, direction1 * distance, Color.red);
            }
        }**/
    }
    #endregion Lateral
    #region Saut
    // Les sauts
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

        RaycastHit2D hit = Physics2D.Raycast(origine - decalage, direction, distance, detectionSol);
        RaycastHit2D hit1 = Physics2D.Raycast(origine, direction, distance, detectionSol);
        RaycastHit2D hit2 = Physics2D.Raycast(origine + decalage, direction, distance, detectionSol);
        
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
    // Vector2 direction1 = Vector2.right;
    #endregion Saut
    #endregion Mouvement
    
    #region Interaction
    // Là elle agit
    #region Lait
    // Pour stocker et redonne le lait
    void Interaction()
    {

    }
    #endregion Lait

    #region Attaque
    // La fourmi contre-attaque!

    private void Attaque()
    {
        aPortee = false;
        //Vector2 devant = new Vector2(origineDroite.position.x - transform.position.x, 0).normalized;
        Vector2 origine1 = origineDroite.position;


    int direction = regardeDroite ? 1 : -1;

    Debug.DrawRay(origine1, Vector2.right * direction * distanceAttaque, Color.yellow);
        RaycastHit2D hitCoxi = Physics2D.Raycast(origine1, Vector2.right * direction, distanceAttaque, maskCoxi);
        if (hitCoxi.collider)
        {
            aPortee = true;
            if (hitCoxi.collider.TryGetComponent(out Coxinelle coxinelle) && aPortee)
            {
                coxinelle.Mourir();  
            }
        }
    }

    #endregion Attaque
    #endregion Interaction
    
    #region Animations

    private void GererAnims()
    {
        if (!auMur && auSol && enMouvement)
        {
            animator.SetBool("avance", true);
        }
        else
        {
            animator.SetBool("avance", false);
        }

        spriteRenderer.flipX = regardeDroite;
        spriteRendererAbdomene.flipX = regardeDroite;

       Vector3 nvellePosition = origineDroite.localPosition;
        nvellePosition.x *= (regardeDroite && nvellePosition.x < 0) || (!regardeDroite && nvellePosition.x > 0) ? -1 :  1;
        origineDroite.localPosition = nvellePosition;
    }

    #endregion
}
