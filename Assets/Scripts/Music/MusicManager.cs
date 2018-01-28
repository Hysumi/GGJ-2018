using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip mainTheme;
    public AudioClip menuTheme;

    private void Start()
    {
        //AudioManager.instance.PlayMusic(menuTheme, 2);
    }
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //AudioManager.instance.PlayMusic(mainTheme, 3);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioManager.instance.PlaySound("G13D", "fogo", Vector3.zero);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            AudioManager.instance.PlaySound("G13D", 1, Vector3.up);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.instance.PlaySound2D("G12D", "estática");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.instance.PlaySound2D("G12D", 0);
        }

	}
}
