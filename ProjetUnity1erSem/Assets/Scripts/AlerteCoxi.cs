using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AlerteCoxi : MonoBehaviour
{
    private RectTransform rectTrans;
    [SerializeField] private RectTransform flecheDirection;
    [SerializeField] private RectTransform PanoAlerte;
    private Coxinelle coxi;
    [SerializeField] private float rayonMort;

    public Coxinelle Coxinelle
    {
        set => coxi = value;
    }

    // Start is called before the first frame update
    private void Start()
    {
        rectTrans = (RectTransform) transform;
    }
    
    private void Update()
    {
        if (CoxiEstDansCamera())
        {
            PointerCoxi();
            PositionnerAuBordEcran();
            AfficherAlerte(true);
        }
        else AfficherAlerte(false);
    }

    private bool CoxiEstDansCamera()
    {
        Vector2 posCoxi = Camera.main.WorldToScreenPoint(coxi.transform.position);
        return !(posCoxi.x < 0 || posCoxi.y > 0 ||
                 posCoxi.x > Camera.main.scaledPixelWidth || posCoxi.y > Camera.main.scaledPixelHeight);

    }
    
    private void AfficherAlerte(bool afficher)
    {
        PanoAlerte.gameObject.SetActive(afficher);
        flecheDirection.gameObject.SetActive(afficher);
    }

    private void PointerCoxi()
    {
        flecheDirection.localEulerAngles =
            Quaternion.LookRotation(flecheDirection.forward, coxi.transform.position)
                .eulerAngles;
    }

    private void PositionnerAuBordEcran()
    {
        Vector2 positionCoxi = Camera.main.WorldToScreenPoint(coxi.transform.position);
        Vector2 nvllePosition = new Vector2();
        if (positionCoxi.x < 0)
        {
            nvllePosition.x = 0 + rayonMort;
        }
        else if (positionCoxi.x > Camera.main.scaledPixelWidth)
        {
            nvllePosition.x = Camera.main.scaledPixelWidth - rayonMort;
        }
        else
        {
            nvllePosition.x = positionCoxi.x;
        }
        if (positionCoxi.y < 0)
        {
            nvllePosition.y = 0 + rayonMort;
        }
        else if (positionCoxi.y > Camera.main.scaledPixelHeight)
        {
            nvllePosition.y = Camera.main.scaledPixelHeight - rayonMort;
        }
        else
        {
            nvllePosition.y = positionCoxi.y;
        }
        rectTrans.anchoredPosition = nvllePosition;
    }
    
}
