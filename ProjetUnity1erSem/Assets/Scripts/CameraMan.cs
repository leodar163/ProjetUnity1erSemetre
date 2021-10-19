using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Camera))]
public class CameraMan : MonoBehaviour
{

    private static CameraMan cela;

    public static CameraMan Singleton
    {
        get
        {
            if (!cela) cela = FindObjectOfType<CameraMan>();
            return cela;
        }
    }
    [Header("Refs")]
    [SerializeField] private Camera camera;
    [SerializeField] private GameObject objetASuivre;
    [Header("Propriétés Caméra")]
    [SerializeField] private float tmpsPrAttendreObjet;
    [SerializeField] private float vitesseMax;
    [SerializeField] private float decalageVertical;
    [SerializeField] private float decalageHorizontal;
    [Header("Rendu")]
    [SerializeField] private float dezoom;
    
    public float Dezoom
    {
        get => dezoom;
        set => camera.orthographicSize = value;
    }

    private enum FileRendu
    {
        Update,
        FixedUpdate
    }

    [SerializeField] private FileRendu fileRendu;

    private void OnValidate()
    {
        if (!camera) TryGetComponent(out camera);
        camera.orthographicSize = dezoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if(objetASuivre && fileRendu == FileRendu.Update) SuivreObjetASuivre();
        
        if(Input.anyKeyDown) ScreenShake(0.05f,0.2f);
    }

    private void FixedUpdate()
    {
        if(objetASuivre && fileRendu == FileRendu.FixedUpdate) SuivreObjetASuivre();
    }

    private void SuivreObjetASuivre()
    {
        Vector3 nvllePos = transform.position;
        Vector3 posAAttendre = objetASuivre.transform.position;
        posAAttendre.x += decalageHorizontal;
        posAAttendre.y += decalageVertical;

        Vector2 velocite = new Vector2();

        float diffTmps = fileRendu == FileRendu.Update ? Time.deltaTime : Time.fixedDeltaTime;
        
        nvllePos = Vector2.SmoothDamp(transform.position,posAAttendre,ref velocite
        ,tmpsPrAttendreObjet, vitesseMax, diffTmps );

        nvllePos.z = transform.position.z;
        transform.position = nvllePos;
    }

    private IEnumerator screenShake;
    public void ScreenShake(float force, float duree)
    {
        if(screenShake != null) StopCoroutine(screenShake);
        screenShake = ScreenShakeRoutine(force, duree);
        StartCoroutine(screenShake);
    }

    private IEnumerator ScreenShakeRoutine(float force, float duree)
    {
        float tmps = duree;
        while (tmps > 0)
        {
            Vector3 nvllePos = transform.position;
            nvllePos.x += Random.Range(-1f, 1f) * force;
            nvllePos.y += Random.Range(-1f, 1f) * force;
            transform.position = nvllePos;
            
            float diffTmps = fileRendu == FileRendu.Update ? Time.deltaTime : Time.fixedDeltaTime;

            if (fileRendu == FileRendu.Update) yield return new WaitForEndOfFrame();
            else yield return new WaitForFixedUpdate();

            tmps -= diffTmps;
        }

        screenShake = null;
    }
}
