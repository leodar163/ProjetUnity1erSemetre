using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scarabe : MonoBehaviour
{
    public enum Fatigue
    {
        enForme = 2,
        fatigue = 1,
        extenue = 0
    }

    public Fatigue nivoFatigue = Fatigue.enForme;
    [Tooltip("En seconde")]
    [SerializeField] private float tmpsFatigue ;

    [Header("Roue")]
    [SerializeField] private float vitesseRotationRoue;
    [SerializeField] private Transform roue;
    [Header("Animations")]
    [SerializeField] private Animator animScarab; 
    [SerializeField] private Animator animLianes; 
    [SerializeField] private Animator animHelices; 
    private IEnumerator coolDownFatigue;
    
    private bool peutManger = true;
    private static readonly int vitesseCourseAnim = Animator.StringToHash("VitesseCourse");
    private static readonly int VitesseRoulement = Animator.StringToHash("VitesseRoulement");
    private static readonly int VitesseTournage = Animator.StringToHash("VitesseTournage");
    public bool PeutManger => peutManger;
    
    // Start is called before the first frame update
    private void Start()
    {
        ReglerAnims();
        coolDownFatigue = CoolDownFatigue();
        StartCoroutine(coolDownFatigue);
    }

    // Update is called once per frame
    private void Update()
    {
           Courir();
           FaireRoulerRoue();
    }

    private void Courir()
    {
        if (GameManager.Singleton) GameManager.Singleton.AjouterScore(nivoFatigue);
    }

    private IEnumerator CoolDownFatigue()
    {
        peutManger = (int)nivoFatigue < 2;
        
        yield return new WaitForSeconds(tmpsFatigue);

        nivoFatigue = (int) nivoFatigue > 0 ? nivoFatigue - 1 : Fatigue.extenue;

        ReglerAnims();
        
        coolDownFatigue = CoolDownFatigue();
        StartCoroutine(coolDownFatigue);
    }

    private void ReglerAnims()
    {
        switch (nivoFatigue)
        {
            case Fatigue.enForme :
                animScarab.SetFloat(vitesseCourseAnim, 2);
                animLianes.SetFloat(VitesseRoulement, 2);
                animHelices.SetFloat(VitesseTournage, 2);
                break;
            case Fatigue.fatigue :
                animScarab.SetFloat(vitesseCourseAnim, 1);
                animLianes.SetFloat(VitesseRoulement, 1);
                animHelices.SetFloat(VitesseTournage, 1);
                break;
            case Fatigue.extenue :
                animScarab.SetFloat(vitesseCourseAnim, 0);
                animLianes.SetFloat(VitesseRoulement, 0);
                animHelices.SetFloat(VitesseTournage, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public void Nourrir()
    {
        if (peutManger)
        {
            StopCoroutine(coolDownFatigue);
            nivoFatigue += 1;
            ReglerAnims();
            coolDownFatigue = CoolDownFatigue();
            StartCoroutine(coolDownFatigue);
        }
    }

    private void FaireRoulerRoue()
    {
        roue.localEulerAngles = new Vector3(roue.localEulerAngles.x, roue.localEulerAngles.y,
            roue.localEulerAngles.z - vitesseRotationRoue * Time.deltaTime * (int) nivoFatigue);
      if(Paralaxe.Singleton)  Paralaxe.Singleton.VitesseDefilement = (int) nivoFatigue;
    }
}
