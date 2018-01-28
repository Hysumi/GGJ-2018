using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    public enum AudioChannel {Master, Sfx, Music};
    public int musicQtd = 3;

    [Range(0,1)]
    public float masterVolumePercent = .2f;
    [Range(0, 1)]
    public float sfxVolumePercent = 1;
    [Range(0, 1)]
    public float musicVolumePercent = 1;

    AudioSource sfx2DSource;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;
    Transform audioListener;
    Transform playerT;

    SoundLibrary library;

    private void Awake()
    { 
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(gameObject); //Não apaga quando troca de scene

            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[musicQtd];
            for (int i = 0; i < musicQtd; i++)
            {
                GameObject newMusicSource = new GameObject("Music Source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            GameObject newSfx2Dsource = new GameObject("2D sfx source");
            sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
            newSfx2Dsource.transform.parent = transform;

            audioListener = FindObjectOfType<AudioListener>().transform;
            playerT = FindObjectOfType<Player>().transform;

            //Carrega as preferências do player salvas
            masterVolumePercent = PlayerPrefs.GetFloat("master vol", masterVolumePercent);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol", sfxVolumePercent);
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", musicVolumePercent);
        }
    }

    private void Update()
    {
        if(playerT != null)
        {
            audioListener.position = playerT.position;
        }
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
            default:
                break;
        }

        for (int i = 0; i < musicQtd; i++)
        {
            musicSources[i].volume = musicVolumePercent * masterVolumePercent;
        }

        //Salva as preferências do player para que ele não tenha que trocar quando abrir o jogo de novo
        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);

    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();

        StartCoroutine(AnimateMusicCrossfade(fadeDuration));
    }

    #region Play 3D Sounds

    //Precisa determinar uma posição no plano 3D
    public void PlaySound(AudioClip clip, Vector3 pos) 
    {
        if(clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent); //Bom para efeitos sonoros, mas não para música
        }
    }

    public void PlaySound(string groupName, Vector3 pos) //Escolhe o som de um grupo de forma aleatória
    {
        PlaySound(library.GetClipFromName(groupName), pos); 
    }

    public void PlaySound(string groupName, string name, Vector3 pos) //Escolhe o som de um grupo a partir do nome do som
    {
        PlaySound(library.GetClipFromName(groupName, name), pos);
    }

    public void PlaySound(string groupName, int id, Vector3 pos) //Escolhe o som de um grupo a partir do id da posição do grupo
    {
        PlaySound(library.GetClipFromName(groupName, id), pos);
    }

    #endregion

    #region Play 2D Sounds

    //Não precisa determinar uma posição

    public void PlaySound2D(string soundGroup)  //Escolhe o som de um grupo de forma aleatória
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundGroup), sfxVolumePercent * masterVolumePercent);
    }

    public void PlaySound2D(string soundName, string name)  //Escolhe o som de um grupo a partir do nome do som
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName, name), sfxVolumePercent * masterVolumePercent);
    }

    public void PlaySound2D(string soundName, int id)  //Escolhe o som de um grupo a partir do id da posição do grupo
    {
        sfx2DSource.PlayOneShot(library.GetClipFromName(soundName, id), sfxVolumePercent * masterVolumePercent);
    }

    #endregion 

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

/* Obs:
 * PlayClipAtPoint: Não pode ser usado para som 2D pois ele toca em um ponto específico no espaço 3D
 * PlayOneShot: Som 2D. Não passa posição
 */
