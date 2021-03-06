using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Random = UnityEngine.Random;

public class ListePucerons : MonoBehaviour
{
    private static ListePucerons cela;

    public static ListePucerons Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<ListePucerons>();
            return cela;
        }
    }
    
    [SerializeField] private List<Puceron> listePucerons;

    public List<Puceron> LesPucerons => listePucerons;

    public List<Puceron> LesPuceronsLibres
    {
        get
        {
            List<Puceron> puceronsLibres = listePucerons.Where(puceron => !puceron.estAttaque).ToList();
            return puceronsLibres.Count == 0 ? null : puceronsLibres;
        }
    }

    public static Puceron RecupPuceronAleatoire()
    {
        List<Puceron> puceronsLibres = Singleton.LesPuceronsLibres;
        int alea = Random.Range(0, puceronsLibres.Count);
        return puceronsLibres[alea];
    }

    public void RetirerPuceron(Puceron puceronARetirer)
    {
        if(listePucerons.Contains(puceronARetirer)) listePucerons.Remove(puceronARetirer);
    }

    private void OnValidate()
    {
        ActualiserListePuceron();
    }

    private void ActualiserListePuceron()
    {
        listePucerons.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out Puceron puceron)) listePucerons.Add(puceron);
        }
    }
}
