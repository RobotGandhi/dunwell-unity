using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct LevelScoreData
{
    public int gold, silver, bronze;
}

public class ResourceLoader : MonoBehaviour
{

    public static Dictionary<string, Sprite> sprite_map = new Dictionary<string, Sprite>();
    public static Dictionary<string, TextAsset> level_text_map = new Dictionary<string, TextAsset>();
    public static Dictionary<string, LevelScoreData> level_score_map = new Dictionary<string, LevelScoreData>();
    static bool has_loaded_resources = false;

    void Awake()
    {
        // Load sprites
        if (!has_loaded_resources)
            StartCoroutine(LoadAssets());
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

    private void LoadLevelScoreData()
    {
        int _level = 1;

        // Start with world 1
        string[] content = GetLevelTextFile("W1_score_table").text.Split('\n');
        foreach(string line in content)
        {
            if (line != string.Empty)
            {
                string[] split_line = line.Split(null);
                int gold, silver, bronze;
                int.TryParse(split_line[0], out gold);
                int.TryParse(split_line[1], out silver);
                int.TryParse(split_line[1], out bronze);

                string name = "W1L" + _level.ToString();
                _level++;

                // Create the map object
                LevelScoreData x = new LevelScoreData();
                x.gold = gold; x.silver = silver; x.bronze = bronze;
                level_score_map.Add(name, x);
            }
        }

        _level = 1;

        // TODO @ do this for the other zones later on!

    }

    private IEnumerator LoadAssets()
    {
        LoadSprites();
        LoadLevels();
        LoadLevelScoreData();
        
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

    public static LevelScoreData GetLevelScoreData(string name)
    {
        return level_score_map[name];
    }
}
