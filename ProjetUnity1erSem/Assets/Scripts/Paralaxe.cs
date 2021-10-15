using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxe : MonoBehaviour
{
    private static Paralaxe cela;

    public static Paralaxe Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<Paralaxe>();
            return cela;
        }
    }
    
    [System.Serializable]
    private class PlanParalaxe
    {
        public Transform transformPlan;
        public float vitesseDefilement;
        public float profondeur;
        [HideInInspector] public float largeur;
    }
    
    [Range(-1,1)]
    [SerializeField] private int directionDefilement;
    [SerializeField] private List<PlanParalaxe> plans;

    public float VitesseDefilement;
    
    // Start is called before the first frame update
    private void Start()
    {
        GenererDoublons();
    }

    // Update is called once per frame
    private void Update()
    {
        Defiler();
    }

    private void GenererDoublons()
    {
        foreach (var plan in plans)
        {
            if (plan.transformPlan.TryGetComponent(out SpriteRenderer sprRend))
            {
                plan.largeur = sprRend.bounds.size.x;
            }
            Vector3 positionDoublon = plan.transformPlan.position + new Vector3(-directionDefilement,0) * plan.largeur;
            GameObject nvPlan = Instantiate(plan.transformPlan.gameObject, positionDoublon, new Quaternion(), plan.transformPlan);
            nvPlan.transform.localScale = Vector3.one;
            plan.transformPlan.localPosition = new Vector3(0, plan.transformPlan.localPosition.y, plan.profondeur);
        }
    }
    
    private void Defiler()
    {
        foreach (var plan in plans)
        {
            plan.transformPlan.position += new Vector3(directionDefilement,0) * Time.deltaTime * VitesseDefilement *
                                           plan.vitesseDefilement;
            if (Mathf.Abs(plan.transformPlan.localPosition.x) >= plan.largeur)
            {
                plan.transformPlan.localPosition = new Vector3(0, plan.transformPlan.localPosition.y, plan.profondeur);
            }
        }
    }
}
