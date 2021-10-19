using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMan : MonoBehaviour
{
    
    [SerializeField] private GameObject objetASuivre;
    [SerializeField] private float tmpsPrAttendreObjet;
    [SerializeField] private float vitesseMax;
    [SerializeField] private float decalageVertical;
    [SerializeField] private float decalageHorizontal;
    [SerializeField] private Camera camera;
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
}
