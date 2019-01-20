using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sfx_system : MonoBehaviour
{
    private AudioClip[] all_sfx;

    public Dictionary<string, AudioClip> sfx_map = new Dictionary<string, AudioClip>();

    void Start()
    {
        all_sfx = Resources.LoadAll<AudioClip>("sfx");

        foreach (AudioClip x in all_sfx)
        {
            sfx_map.Add(x.name, x);
        }
    }

    public void PlaySFX(string name)
    {
        AudioClip clip = sfx_map[name];
        GameObject temp = new GameObject();
        AudioSource source = temp.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(temp, clip.length);
    }
}
