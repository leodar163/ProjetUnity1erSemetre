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
    [SerializeField] private List<Sprite> etapesAbdomen;
    
    [Header ("Collision")]
    [SerializeField] private LayerMask detectionSol;
    [SerializeField] private LayerMask maskCoxi;
    [SerializeField] private LayerMask maskPuceron;
    [SerializeField] private float distanceAttaque = 1;
    [SerializeField] private Vector2 boitePortee;
    [SerializeField] private Vector2 boiteDetectSol;
    [SerializeField] private Vector2 boiteDetectMur;

    [Header ("Commandes")]
    [SerializeField] private KeyCode toucheDroite;
    [SerializeField] private KeyCode toucheGauche;
    [SerializeField] private KeyCode toucheSaut;
    [SerializeField] private KeyCode toucheAttaque;
    [SerializeField] private KeyCode toucheInteraction;
   
    [Header ("Variables")]
    [SerializeField] private float forceSaut = 50;
    [SerializeField] private float vitesse = 1.5f;
    [SerializeField] private int tailleAbdomen = 0;
    [SerializeField] private int stockageMaximum = 3;
    //[SerializeField] private float distance = 1.5f;
    
    private bool regardeDroite;
    
    [Header ("Triggers")]
    [SerializeField] private bool auSol;
    [SerializeField] private bool auMur;
    [SerializeField] private bool enMouvement;

    private void OnDrawGizmos()
    {
        Vector2 direction1 = new Vector2(0.3f, 0.5f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(origineTete.position, boitePortee.x);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(pied.position, boiteDetectSol);
        Gizmos.DrawWireCube(origineTete.position, boiteDetectMur);
    }

    void Update()
    {
        MouvementLateral();
        MouvementSaut();
        DetectSol();
        DetectMur();
        GererAnims();
        GererEtapesAbdomen();

        if (Input.GetKeyDown(toucheAttaque))
        {
            animator.SetTrigger("Attaque");
        }

        if (Input.GetKeyDown(toucheInteraction))
        {
            TraireDonnerLait();
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
            enMouvement = true;
            if (!auMur)
            {
                rb.velocity = new Vector2(vitesse, rb.velocity.y);
            }
            regardeDroite = true;
           
        }

        if (Input.GetKey(toucheGauche))
        {
            enMouvement = true;
            if (!auMur)
            {
                rb.velocity = new Vector2(-vitesse, rb.velocity.y);
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

        Collider2D hit3 = Physics2D.OverlapBox(origine1, boiteDetectMur, 0, detectionSol);

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

        Collider2D hit = Physics2D.OverlapBox(origine, boiteDetectSol, 0, detectionSol);

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
    public void TraireDonnerLait()
    {
        Vector2 origine1 = origineTete.position;

        Collider2D[] sucePuceron = Physics2D.OverlapCircleAll(origine1, boitePortee.x, maskPuceron);

        foreach (var autre in sucePuceron)
        {
            if (autre.TryGetComponent(out Puceron puceron) && tailleAbdomen < stockageMaximum)
            {
                if(puceron.goutteBuvable)
                {
                    puceron.RecolterLait();
                    tailleAbdomen += 1;
                }
                
            }
            else if (autre.TryGetComponent(out Scarabe scarabe) && tailleAbdomen > 0 && scarabe.nivoFatigue != Scarabe.Fatigue.enForme)
            {
                scarabe.Nourrir();
                tailleAbdomen -= 1;
            }
        }
    }
    #endregion Lait

    #region Attaque
    // La fourmi contre-attaque!

    private void Attaque()
    {
        Vector2 origine1 = origineTete.position;
        int direction = regardeDroite ? 1 : -1;

        Debug.DrawRay(origine1, Vector2.right * direction * distanceAttaque, Color.yellow);
        //RaycastHit2D hitCoxi = Physics2D.Raycast(origine1, Vector2.right * direction, distanceAttaque, maskCoxi);

        Collider2D[] hitCoxi = Physics2D.OverlapCircleAll(origine1, boitePortee.x, maskCoxi);

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
    
    #region Animations/Graphismes

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

    private void GererEtapesAbdomen()
    {
        if (etapesAbdomen == null || etapesAbdomen.Count == 0) return;
        int index = (int)Mathf.Lerp(0, etapesAbdomen.Count - 1, (float)tailleAbdomen / stockageMaximum);
        spriteRendererAbdomene.sprite = etapesAbdomen[index];
    }

    #endregion
}
