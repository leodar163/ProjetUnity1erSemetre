using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Musique : MonoBehaviour
{

    [SerializeField] private AudioSource audio;

    private void OnValidate()
    {
        if (!audio) TryGetComponent(out audio);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectsOfType<Musique>().Length > 1) Destroy(gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
