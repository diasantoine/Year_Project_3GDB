using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Cursor = UnityEngine.WSA.Cursor;

public class UIRessource : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField] private string SonRecupCuve;
    private FMOD.Studio.EventInstance sonrecupcuve;
    
    private int CountChanged;
    private Vector3 ConteneurScale;
    [SerializeField] private Renderer Liquid;
    [SerializeField] private float TimeBeforeSound = 0.3f;
    private float ValueBaseCuveLiquide;
    private float Compteur = 0;
    private bool SongPlayed = true;
    private float RessourceLastCheck = 0;
    void Start()
    {
        CountChanged = detectDead.ressourceInt;
        ConteneurScale = transform.localScale;
        ValueBaseCuveLiquide = 1.7f;
        sonrecupcuve = FMODUnity.RuntimeManager.CreateInstance(SonRecupCuve);
    }

    // Update is called once per frame
    void Update()
    {

        int ressource = detectDead.ressourceInt;

        if (ressource != CountChanged)
        {
            if (ressource == 0)
            {
                Liquid.material.SetFloat("_FillAmount", ValueBaseCuveLiquide);
                //transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                CountChanged = ressource;
                Compteur = 0;
            }
            else if(ressource > 0)
            {
                if (ressource > RessourceLastCheck)
                {
                    RessourceLastCheck = ressource;
                    SongPlayed = false;
                }
                else
                {
                    RessourceLastCheck = ressource;
                }
                Liquid.material.SetFloat("_FillAmount",ValueBaseCuveLiquide - ressource*0.02f);
                //float ConteneurYScale = Mathf.Clamp(0.01f * ressource, 0, 0.93f);
                //transform.localScale = new Vector3(transform.localScale.x, ConteneurYScale, transform.localScale.z);
                CountChanged = ressource;
                Compteur = 0;
            }
        }
        // if (!SongPlayed)
        // {
        //     Compteur += Time.deltaTime;
        //     if (Compteur >= TimeBeforeSound)
        //     {
        //         if (Liquid.material.GetFloat("_FillAmount")  > 1.4f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 2);
        //             sonrecupcuve.start();
        //         }else if (Liquid.material.GetFloat("_FillAmount") > 1.1f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 3);
        //             sonrecupcuve.start();
        //         }else if (Liquid.material.GetFloat("_FillAmount") > 0.8f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 4);
        //             sonrecupcuve.start();
        //         }else if (Liquid.material.GetFloat("_FillAmount") > 0.5f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 5);
        //             sonrecupcuve.start();
        //         }else if (Liquid.material.GetFloat("_FillAmount") > 0.2f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 6);
        //             sonrecupcuve.start();
        //         }else if (Liquid.material.GetFloat("_FillAmount") > -0.1f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 7);
        //             sonrecupcuve.start();
        //         }else if (Liquid.material.GetFloat("_FillAmount") > -0.4f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 8);
        //             sonrecupcuve.start();
        //         }else if (Liquid.material.GetFloat("_FillAmount") > -0.7f)
        //         {
        //             sonrecupcuve.setParameterByName("SoundChoice", 9);
        //             sonrecupcuve.start();
        //         }
        //         SongPlayed = true;
        //         Compteur = 0;
        //     }
        // }
    }
}
