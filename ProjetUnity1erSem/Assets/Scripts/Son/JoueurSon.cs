using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class JoueurSon : MonoBehaviour
{

    [SerializeField] private AudioSource sourceAudio;
    
    
    [Serializable]
    public struct BibliothequeSons
    {
        public string nom;
        public List<AudioClip> bibliotheque;
    }
    [Space(10)]
    [SerializeField] private List<BibliothequeSons> bibliothequesSons;
    
    // Start is called before the first frame update
    private void OnValidate()
    {
        if (!sourceAudio) TryGetComponent(out sourceAudio);
    }

    private void JouerSon(AudioClip clip)
    {
        sourceAudio.clip = clip;
        sourceAudio.Play();
    }

    private void JouerSon(List<AudioClip> clips, bool peutRepeterSon = true)
    {
        if (clips.Count == 0)
        {
            Debug.LogError("Aucun son à jouer");
            return;
        }
        AudioClip clipAJouer = clips[Random.Range(0, clips.Count)];
        if(!peutRepeterSon)
            while (clipAJouer == sourceAudio.clip)
            {
                clipAJouer = clips[Random.Range(0, clips.Count)];
            }
        JouerSon(clipAJouer);
    }

    public void JouerBilbiotheque(string nomBiblio, bool peutRepeterSon = true)
    {
        foreach (BibliothequeSons biblio in bibliothequesSons)
        {
            if (biblio.nom == nomBiblio)
            {
                JouerSon(biblio.bibliotheque, peutRepeterSon);
                return;
            }
        }
        throw new ArgumentException("Le nom de la bibliothèque est pas le bon");
    }
}
