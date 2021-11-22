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
    [SerializeField] private Transform origineTete;
    [SerializeField] private Transform origineGauche;
    
    [Header ("Collision")]
    [SerializeField] private LayerMask detectionSol;
    [SerializeField] private LayerMask maskCoxi;
    [SerializeField] private float distanceAttaque = 1;
    [SerializeField] private Vector2 tailleBoite;
    [SerializeField] private Vector2 boiteDetectSol;
   // [SerializeField] private Vector2 offsetBoite/*= new Vector2(origineTete.position.x, origineTete.position.y)*/;
    [SerializeField] private float porteeAttaque;

    [Header ("Commandes")]
    [SerializeField] private KeyCode toucheDroite;
    [SerializeField] private KeyCode toucheGauche;
    [SerializeField] private KeyCode toucheSaut;
    [SerializeField] private KeyCode toucheAttaque;
    [SerializeField] private KeyCode toucheInteraction;
   
    [Header ("Variables")]
    [SerializeField] private float forceSaut = 50;
    [SerializeField] private float vitesse = 1.5f;
    //[SerializeField] private float distance = 1.5f;
    
    private bool regardeDroite;
    
    [Header ("Triggers")]
    [SerializeField] private bool auSol;
    [SerializeField] private bool auMur;
    [SerializeField] private bool enMouvement;
    //[SerializeField] private bool aPortee;


    private void OnDrawGizmos()
    {
        Vector2 direction1 = new Vector2(0.5f, 0);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(origineTete.position, tailleBoite);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pied.position, boiteDetectSol);
        Gizmos.DrawWireCube(origineTete.position, direction1);
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

        Vector2 origine1 = origineTete.position;
        Vector2 direction1 = new Vector2(0.5f, 0);

        Collider2D hit3 = Physics2D.OverlapBox(origine1, direction1, 0, detectionSol);

        if (hit3 && enMouvement)
        {
            auMur = true;
        }

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
                //enMouvement = true;
            }
        }
    }
    
    void DetectSol()
    {
        Vector2 origine = pied.position;
        Vector2 direction = Vector2.down;
        Vector2 decalage = new Vector2(0.13f, 0);

        Collider2D hit = Physics2D.OverlapBox(origine, direction, 0, detectionSol);

        if (hit)
        {
            auSol = true;
            
        }
        else
        {
            auSol = false;
            
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
       // aPortee = false;
        Vector2 origine1 = origineTete.position;
        int direction = regardeDroite ? 1 : -1;

        Debug.DrawRay(origine1, Vector2.right * direction * distanceAttaque, Color.yellow);
        //RaycastHit2D hitCoxi = Physics2D.Raycast(origine1, Vector2.right * direction, distanceAttaque, maskCoxi);

        Collider2D[] hitCoxi = Physics2D.OverlapCircleAll(origine1, tailleBoite.x, maskCoxi);

        for (int i = 0; i < hitCoxi.Length; i++)
        {
            if(hitCoxi[i].TryGetComponent(out Coxinelle coxi))
            {
                coxi.Mourir();
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

       Vector3 nvellePosition = origineTete.localPosition;
        nvellePosition.x *= (regardeDroite && nvellePosition.x < 0) || (!regardeDroite && nvellePosition.x > 0) ? -1 :  1;
        origineTete.localPosition = nvellePosition;
    }

    #endregion
}
