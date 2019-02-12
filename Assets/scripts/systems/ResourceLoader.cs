using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : MonoBehaviour
{

    public static Dictionary<string, Sprite> sprite_map = new Dictionary<string, Sprite>();
    static bool has_loaded_sprites = false;

    void Start()
    {
        // Load sprites
        if(!has_loaded_sprites)
            LoadSprites();
    }

    private void LoadSprites()
    {
        Sprite[] all_sprites = Resources.LoadAll<Sprite>("graphics");
        foreach(Sprite x in all_sprites)
        {
            print(x.name);
            sprite_map.Add(x.name, x);
        }

        print("just loaded sprites!");
        has_loaded_sprites = true;
    }

    /* Returns loaded sprite with given parameter name */
    public static Sprite GetSprite(string name)
    {
        return sprite_map[name];
    }

}
