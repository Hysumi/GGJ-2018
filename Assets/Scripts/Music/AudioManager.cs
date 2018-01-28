using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public int musicQtd = 3;

    float masterVolumePercent = .2f;
    float sfxVolumePercent = 1;
    float musicVolumePercent = 1;

    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;
    Transform audioListener;
    Transform playerT;

    private void Awake()
    { 
        instance = this;

        musicSources = new AudioSource[musicQtd];
        for (int i = 0; i < musicQtd; i++)
        {
            GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
            musicSources[i] = newMusicSource.AddComponent<AudioSource>();
            newMusicSource.transform.parent = transform;
        }

        audioListener = FindObjectOfType<AudioListener>().transform;
        playerT = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if(playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if(clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent); //Bom para efeitos sonoros, mas não para música
        }
    }

    IEnumerator AnimateMusicCrossfade(float duration) //Troca de música
    {
        float percent = 0;
        while(percent < 1)
        {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            musicSources[1- activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent,  0, percent);
            yield return null;
        }
    }
}

/*      Configurando o Ambiente:
 *  1. Criar um Empty GameObject e Centralizar.
 *  2. Colocar o Script de AudioManager.cs e MusicManager.cs
 *  3. Criar um objeto vazio filho e colocar no filho o component Audio Listener
 */

/*      Como usar para efeitos sonoros:
 *      
 *  1. Criar os AudioSources no script em que vai ser usado o som.
 *  2. Na função em que for ativar o som, por o código:
 *      - AudioManager.instance.PlaySound(clip, position);
 *  3. No inspector colocar os audios dentro do objeto que tem os AudioSources criados anteriormente
 *  4. Remover o componente Audio Listener da Main Camera
 */  