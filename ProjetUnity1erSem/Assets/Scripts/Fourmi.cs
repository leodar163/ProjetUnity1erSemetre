using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fourmi : MonoBehaviour
{
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    [SerializeField] private Transform pied;
    [SerializeField] private Transform origineDroite;
    [SerializeField] private Transform origineGauche;

    [SerializeField] private KeyCode toucheDroite;
    [SerializeField] private KeyCode toucheGauche;
    [SerializeField] private KeyCode toucheSaut;
    [SerializeField] private KeyCode toucheAttaque;
    [SerializeField] private KeyCode toucheInteraction;

    [SerializeField] private float forceSaut = 50;
    [SerializeField] private float vitesse = 1.5f;
    [SerializeField] private float distance = 1.5f;
    [SerializeField] private float distanceAttaque = 1;

    [SerializeField] private bool auSol;
    [SerializeField] private bool auMur;
    [SerializeField] private bool enMouvement;
    [SerializeField] private bool aPortee;

    [SerializeField] private LayerMask DetectionSol;
    [SerializeField] private LayerMask maskCoxi;

    private int direction = 1;

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origineDroite.position, origineDroite.position + Vector3.right * distanceAttaque);
    }*/

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
            Attaque();
        }

        if (Input.GetKeyDown(toucheInteraction))
        {
            Interaction();
        }

    }

    #region Mouvement
    // Là elle bouge
    #region Lateral
    //Les mouvements Droite Gauche
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

    void DetectMur()
    {
        auMur = false;

        Vector2 origine1 = origineDroite.position;
        Vector2 direction1 = new Vector2(0.5f, 0);
        Vector2 origine2 = origineGauche.position;
        Vector2 direction2 = new Vector2(-0.5f, 0);

        RaycastHit2D hit3 = Physics2D.Raycast(origine1, direction1, distance, DetectionSol);
        RaycastHit2D hit4 = Physics2D.Raycast(origine2, direction2, distance, DetectionSol);
        // public static Collider2D hit3 OverlapBox(origine1, 1,);

        if (hit3 && Input.GetKey(toucheDroite))
        {
            auMur = true;
        }

        if (hit4 && Input.GetKey(toucheGauche))
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

    void Attaque()
    {

        //Vector2 devant = new Vector2(origineDroite.position.x - transform.position.x, 0).normalized;
        Vector2 origine1 = origineDroite.position;


        Debug.DrawRay(origine1, Vector2.right * direction * distanceAttaque, Color.yellow, 1.5f);
        RaycastHit2D hitCoxi = Physics2D.Raycast(origine1, Vector2.right * direction, distanceAttaque, maskCoxi);
        if (hitCoxi.collider)
        {
            if (hitCoxi.collider.TryGetComponent(out Coxinelle coxinelle))
            { 
                 Destroy(coxinelle.gameObject);
                
            }
        }
    }

    #endregion Attaque
    #endregion Interaction
    #region Animations

    void GererAnims()
    {
        if(Input.GetKey(toucheDroite))
        {
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(toucheGauche))
        {
            spriteRenderer.flipX = false;
        }

    }

    #endregion
}
