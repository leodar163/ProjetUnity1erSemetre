using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurAlerteCoxi : MonoBehaviour
{
    private static GenerateurAlerteCoxi cela;
    
    [SerializeField] private GameObject alerteCoxiBase;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static AlerteCoxi GenererAlerteCoxi(Coxinelle coxi)
    {
        if (!cela) cela = FindObjectOfType<GenerateurAlerteCoxi>();
        if (Instantiate(cela.alerteCoxiBase, cela.transform).TryGetComponent(out AlerteCoxi alerte))
        {
            alerte.Coxinelle = coxi;
            return alerte;
        }

        return null;
    }
}
