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

    private IEnumerator coolDownFatigue;
    
    private bool peutManger = true;
    public bool PeutManger => peutManger;
    
    // Start is called before the first frame update
    private void Start()
    {
        coolDownFatigue = CoolDownFatigue();
        StartCoroutine(coolDownFatigue);
    }

    // Update is called once per frame
    private void Update()
    {
           Courir();
    }

    private void Courir()
    {
        GameManager.Singleton.AjouterScore(nivoFatigue);
    }

    private IEnumerator CoolDownFatigue()
    {
        peutManger = (int)nivoFatigue < 2;
        
        yield return new WaitForSeconds(tmpsFatigue);

        nivoFatigue = (int) nivoFatigue > 0 ? nivoFatigue - 1 : Fatigue.extenue;

        coolDownFatigue = CoolDownFatigue();
        StartCoroutine(coolDownFatigue);
    }

    public void Nourrir()
    {
        if (peutManger)
        {
            StopCoroutine(coolDownFatigue);
            nivoFatigue += 1;
            coolDownFatigue = CoolDownFatigue();
            StartCoroutine(coolDownFatigue);
        }
    }
}
