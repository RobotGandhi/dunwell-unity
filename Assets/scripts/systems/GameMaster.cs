using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Map
{
    public int[,] tile_map;
    public Dictionary<Vector2, GameObject> item_map;
    public Dictionary<Vector2, Enemy> enemy_map;
    public Dictionary<Vector2, GameObject> spike_map;
    public Dictionary<Vector2, Gate> gate_map;
    public Dictionary<Vector2, PresurePlate> pp_map;
    public GameObject goal;

    public Map() 
    {
        item_map = new Dictionary<Vector2, GameObject>();
        enemy_map = new Dictionary<Vector2, Enemy>();
        spike_map = new Dictionary<Vector2, GameObject>();
        gate_map = new Dictionary<Vector2, Gate>();
        pp_map = new Dictionary<Vector2, PresurePlate>();
    }
}

public class GameMaster : MonoBehaviour
{
    [Header("Level")]
    public int World = 1;
    public int Level = 1;

    SpikeSystem spike_system;
    MapManager m_manager;
    public Map current_map;
    private int step_count;

    public Text step_count_text;

    public Image fade_panel;

    [Header("Game Over")]
    public Image panel;

    [Header("Level Won")]
    public RectTransform level_won;
    public Text level_won_text, level_won_step_text;
    public Image[] medals; // 2 - gold, 1 - silver, 0 - bronze

    bool game_can_reset;

    static bool first_time_flag = true;

    void Awake()
    {
        Application.targetFrameRate = 60;

        // Get spike system
        spike_system = FindObjectOfType<SpikeSystem>();

        // Get map going
        m_manager = GetComponent<MapManager>();
        current_map = m_manager.SpawnMap(ConstructLevelName());
        spike_system.NewLevel(current_map);
    }

    void Start()
    {
        game_can_reset = false;
        step_count = 0;

        StartCoroutine("GameStart");
    }

    void Update()
    {
        if (game_can_reset)
        {
            if (Input.touchCount > 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void PlayerDie()
    {
        StartCoroutine("GameOver");
    }

    private IEnumerator GameStart()
    {
        if (first_time_flag)
        {
            yield return new WaitForSeconds(2);
            first_time_flag = false;
        }

        // Center camera
        Vector3 cameraGoalPos = new Vector3(Constants.CameraX * MapManager.GroundTileSize, (Constants.MapHeight / 2) * MapManager.GroundTileSize, -10);
        Camera.main.transform.position = cameraGoalPos;

        // Play player intro
        FindObjectOfType<Player>().NewLevel(new Vector2(1, 1));

        // Fade out panel
        while (fade_panel.color.a >= 0.05f)
        {
            fade_panel.color = Vector4.MoveTowards(fade_panel.color, Color.clear, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        fade_panel.color = Color.clear;
    }

    private IEnumerator GameOver()
    {
        while(panel.color.a < 0.5f)
        {
            panel.color = Vector4.MoveTowards(panel.color, new Vector4(0, 0, 0, 0.5f), 2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        game_can_reset = true;

        while(panel.rectTransform.anchoredPosition != new Vector2(0, 750))
        {
            panel.rectTransform.anchoredPosition = Vector2.Lerp(panel.rectTransform.anchoredPosition, new Vector2(0, 760), 3f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void PlayerEvent()
    {
        step_count++;
        step_count_text.text = step_count.ToString();

        // Message spikes
        spike_system.Step();

        StopCoroutine("FlashStepText");
        StartCoroutine("FlashStepText");
    }

    public void NewMap()
    {
        // Level Won Panel Things
        level_won_step_text.text = "Score: " + step_count.ToString();
        level_won_text.text = "World " + World.ToString() + " Level " + Level.ToString();

        StartCoroutine("FadeMedals");
        StartCoroutine("FadeInLevelWon");

        // Save the steps
        SaveSteps(World, Level, step_count);

        step_count = 0;
        Level++;
    }

    public void FinishLevel()
    {
        StopCoroutine("FadeInLevelWin");
        StartCoroutine("FadeOutLevelWon");
        StartCoroutine("LevelEnd");
    }

    private IEnumerator FadeMedals()
    {
        // This get reseted! so snapshot it/save it
        int steps = step_count; 
        string level_name = ConstructLevelName();

        yield return new WaitForSeconds(2);

        int i;
        // Check what corresponding medal the player got
        LevelScoreData score_data = ResourceLoader.GetLevelScoreData(level_name);
        if (steps <= score_data.gold)
        {
            i = 2;
        }
        else if (steps <= score_data.silver)
        {
            i = 1;
        }
        else if (steps <= score_data.bronze)
        {
            i = 0;
        }
        else
        {
            i = -1;
        }

        if (i != -1)
        {
            int j = 0;
            while (medals[i].color != Color.white)
            {
                medals[j].color = Color.white;
                Camera.main.GetComponent<CameraShake>().DoShake(Constants.HeavyCamShake, level_won);
                yield return new WaitForSeconds(1.0f);
                j++;
            }
        }
    }

    private IEnumerator LevelEnd()
    {
        while(fade_panel.color.a <= 0.95f)
        {
            fade_panel.color = Vector4.MoveTowards(fade_panel.color, Color.black, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        fade_panel.color = Color.black;

        yield return new WaitForSeconds(2.0f);

        current_map = GetComponent<MapManager>().SpawnMap(ConstructLevelName());
        spike_system.NewLevel(current_map);
        StartCoroutine("GameStart");

        while(fade_panel.color.a >= 0.05f)
        {
            fade_panel.color = Vector4.MoveTowards(fade_panel.color, Color.clear, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        fade_panel.color = Color.clear;     
    }

    private IEnumerator FadeInLevelWon()
    {
        while (level_won.anchoredPosition.y < 750)
        {
            level_won.anchoredPosition = Vector2.Lerp(level_won.anchoredPosition, new Vector2(0, 760), 2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeOutLevelWon()
    {
        while(level_won.anchoredPosition.y > -220)
        {
            level_won.anchoredPosition = Vector2.Lerp(level_won.anchoredPosition, new Vector2(0, -230), 2f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FlashStepText()
    {
        yield return null;
    }

    public string ConstructLevelName()
    {
        return "W" + World.ToString() + "L" + Level.ToString();
    }

    public void SaveSteps(int w, int l, int c)
    {
        string name = "W" + w.ToString() + "L" + l.ToString();
        if (!PlayerPrefs.HasKey(name))
        {
            PlayerPrefs.SetInt(name, c);
        }
        else
        {
            if(c < PlayerPrefs.GetInt(name))
                PlayerPrefs.SetInt(name, PlayerPrefs.GetInt(name) + 1);
        }
    }
}
