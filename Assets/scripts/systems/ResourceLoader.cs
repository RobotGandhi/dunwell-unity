using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{

    public static Dictionary<string, Sprite> sprite_map = new Dictionary<string, Sprite>();
    public static Dictionary<string, TextAsset> level_text_map = new Dictionary<string, TextAsset>();
    static bool has_loaded_resources = false;


    void Awake()
    {
        // Load sprites
        if (!has_loaded_resources)
            StartCoroutine(LoadSpritesCoroutine());
    }

    private void LoadSprites()
    {       
        Sprite[] all_sprites = Resources.LoadAll<Sprite>("graphics");
        foreach(Sprite x in all_sprites)
        {
            sprite_map.Add(x.name, x);
        }
    }

    private void LoadLevels()
    {
        TextAsset[] all_level_files = Resources.LoadAll<TextAsset>("levels");
        foreach(TextAsset x in all_level_files)
        {
            level_text_map.Add(x.name, x);
        }
    }

    private IEnumerator LoadSpritesCoroutine()
    {
        LoadSprites();
        LoadLevels();
        
        yield return null;

        has_loaded_resources = true;
    }

    /* Returns loaded sprite with given parameter name */
    public static Sprite GetSprite(string name)
    {
        return sprite_map[name];
    }

    public static TextAsset GetLevelTextFile(string name)
    {
        return level_text_map[name];
    }

    public static bool HasLevelTextFile(string name)
    {
        return level_text_map.ContainsKey(name);
    }
}
