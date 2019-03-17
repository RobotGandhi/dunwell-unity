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
    SpikeSystem spike_system;
    MapManager m_manager;
    public Map current_map;
    private int step_count;

    public Text step_count_text;

    public Image fade_panel;

    [Header("Game Over")]
    public Image panel;
    public Image prompt;

    bool game_can_reset;

    void Awake()
    {
        Application.targetFrameRate = 60;

        // Get spike system
        spike_system = FindObjectOfType<SpikeSystem>();

        // Get map going
        m_manager = GetComponent<MapManager>();
        current_map = m_manager.SpawnMap();
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu");
        }
    }

    public void PlayerDie()
    {
        StartCoroutine("GameOver");
    }

    private IEnumerator GameStart()
    {
        // Center camera
        Vector3 cameraGoalPos = new Vector3(Constants.CameraX * MapManager.GroundTileSize, (Constants.MapHeight / 2) * MapManager.GroundTileSize, -10);
        Camera.main.transform.position = cameraGoalPos;

        // Play player intro
        FindObjectOfType<Player>().PlayIntroAt(new Vector2(1, 1));

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

        while(prompt.rectTransform.anchoredPosition != new Vector2(0, 750))
        {
            prompt.rectTransform.anchoredPosition = Vector2.Lerp(prompt.rectTransform.anchoredPosition, new Vector2(0, 760), 3f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    public void Step()
    {
        step_count++;
        step_count_text.text = step_count.ToString();

        // Message all the enemies
        foreach(var e in current_map.enemy_map)
        {
            e.Value.Step();
        }

        // Message spikes
        spike_system.Step();

        StopCoroutine("FlashStepText");
        StartCoroutine("FlashStepText");
    }

    public void NewMap()
    {
        StartCoroutine("LevelEnd");
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

        current_map = GetComponent<MapManager>().SpawnMap();
        spike_system.NewLevel(current_map);
        StartCoroutine("GameStart");

        while(fade_panel.color.a >= 0.05f)
        {
            fade_panel.color = Vector4.MoveTowards(fade_panel.color, Color.clear, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        fade_panel.color = Color.clear;     
    }

    private IEnumerator FlashStepText()
    {
        yield return null;
    }
}
