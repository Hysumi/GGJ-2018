using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour {

    public SoundGroup[] soundGroups;
    Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

    private void Awake()
    {
        foreach(SoundGroup soundGroup in soundGroups)
        {
            groupDictionary.Add(soundGroup.groupName, soundGroup.group);
        }
    }

    public AudioClip GetClipFromName(string groupName)
    {
        if (groupDictionary.ContainsKey(groupName))
        {
            AudioClip[] sounds = groupDictionary[groupName];
            return sounds[Random.Range(0, sounds.Length)]; //Dado o nome do grupo, retorna um som aleatório do grupo
        }
        return null;
    }

    //Dado o nome do grupo e o do som, retorna o som escolhido pelo nome
    public AudioClip GetClipFromName(string groupName, string name)
    {
        if (groupDictionary.ContainsKey(groupName))
        {
            print("oi");

            AudioClip[] sounds = groupDictionary[groupName];
            
            for (int i = 0; i < sounds.Length; i++)
            {
                print(sounds[i].name);
                if(sounds[i].name == name)
                {
                    return sounds[i];
                }
            }
        }
        return null;
    }

    //Dado o nome do grupo e o id da posição, retorna o som pelo id
    public AudioClip GetClipFromName(string groupName, int id)
    {
        if (groupDictionary.ContainsKey(groupName))
        {
            AudioClip[] sounds = groupDictionary[groupName];
            return sounds[id];
        }
        return null;
    }

    [System.Serializable]
    public class SoundGroup
    {
        public string groupName;
        public AudioClip[] group;
    }
}
