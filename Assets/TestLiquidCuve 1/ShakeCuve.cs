using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShakeCuve : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField] private string SonRecupCuve;
    private FMOD.Studio.EventInstance sonrecupcuve;
    
    [SerializeField] private List<GameObject> ListCuve = new List<GameObject>();
    [SerializeField] private Renderer Liquid;
    [SerializeField] private float Speed;
    [SerializeField] private float TimeBeforeSound = 0.3f;
    private float CuveConteneur = 1.7f;
    private float Compteur = 0;
    private bool SongPlayed = true;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        sonrecupcuve = FMODUnity.RuntimeManager.CreateInstance(SonRecupCuve);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            CuveConteneur += 0.02f;
            Liquid.material.SetFloat("_FillAmount", CuveConteneur);
            Compteur = 0;
            SongPlayed = false;
        }
        else if(Input.mouseScrollDelta.y > 0)
        {
            CuveConteneur -= 0.02f;
            Liquid.material.SetFloat("_FillAmount", CuveConteneur);
            Compteur = 0;
            SongPlayed = false;
        }else if (Input.mouseScrollDelta.y == 0 && !SongPlayed)
        {
            Compteur += Time.deltaTime;
            if (Compteur >= TimeBeforeSound)
            {
                if (Liquid.material.GetFloat("_FillAmount")  > 1.4f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 2);
                    sonrecupcuve.start();
                }else if (Liquid.material.GetFloat("_FillAmount") > 1.1f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 3);
                    sonrecupcuve.start();
                }else if (Liquid.material.GetFloat("_FillAmount") > 0.8f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 4);
                    sonrecupcuve.start();
                }else if (Liquid.material.GetFloat("_FillAmount") > 0.5f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 5);
                    sonrecupcuve.start();
                }else if (Liquid.material.GetFloat("_FillAmount") > 0.2f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 6);
                    sonrecupcuve.start();
                }else if (Liquid.material.GetFloat("_FillAmount") > -0.1f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 7);
                    sonrecupcuve.start();
                }else if (Liquid.material.GetFloat("_FillAmount") > -0.4f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 8);
                    sonrecupcuve.start();
                }else if (Liquid.material.GetFloat("_FillAmount") > -0.7f)
                {
                    sonrecupcuve.setParameterByName("SoundChoice", 9);
                    sonrecupcuve.start();
                }

                SongPlayed = true;
                Compteur = 0;
            }
        }

        foreach (GameObject CuvePart in ListCuve)
        {
            float MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * Speed;
            float MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * Speed;
            CuvePart.transform.Rotate(MouseY,MouseX, 0);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MySceneAntoine");
        }
    }
}
