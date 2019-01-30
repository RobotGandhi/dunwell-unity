using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Map
{
    public int[,] tile_map;
    public Dictionary<Vector2, GameObject> item_map;
    public Dictionary<Vector2, enemy> enemy_map;
    public Dictionary<Vector2, GameObject> ice_map;

    public Map() 
    {
        item_map = new Dictionary<Vector2, GameObject>();
        enemy_map = new Dictionary<Vector2, enemy>();
        ice_map = new Dictionary<Vector2, GameObject>();
    }
}

public class game_master : MonoBehaviour
{
    map_manager m_manager;
    public Map current_map;
    private int step_count;

    public Text step_count_text;

    [Header("Game Over")]
    public Image panel;
    public Image prompt;

    bool game_can_reset;

    void Awake()
    {
        Application.targetFrameRate = 60;

        // Get map going
        m_manager = GetComponent<map_manager>();
        current_map = m_manager.SpawnMap();
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
        Vector3 cameraGoalPos = new Vector3((Constants.MapWidth / 2) * m_manager.ground_sprite.bounds.size.x, (Constants.MapHeight / 2) * m_manager.ground_sprite.bounds.size.y, -10);
        Camera.main.transform.position = new Vector3(cameraGoalPos.x, cameraGoalPos.y + m_manager.ground_sprite.bounds.size.y * Constants.MapHeight*2, -10);
        
        // Center camera on map!
        while(Camera.main.transform.position != cameraGoalPos)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, cameraGoalPos, player.fall_speed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        // Play player intro
        FindObjectOfType<player>().PlayIntroAt(new Vector2(1, 1));
    }

    private IEnumerator GameOver()
    {
        yield return null;
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

        StopCoroutine("FlashStepText");
        StartCoroutine("FlashStepText");
    }

    public void NewMap()
    {
        StartCoroutine("LevelEnd");
    }

    private IEnumerator FlashStepText()
    {
        step_count_text.rectTransform.localScale = Vector3.zero;
        while(step_count_text.rectTransform.localScale.x < 1.0f)
        {
            step_count_text.rectTransform.localScale = Vector3.MoveTowards(step_count_text.rectTransform.localScale, Vector3.one, 5.0f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        while(step_count_text.rectTransform.localScale.x > 0.0f)
        {
            step_count_text.rectTransform.localScale = Vector3.MoveTowards(step_count_text.rectTransform.localScale, Vector3.zero, 5.0f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        step_count_text.rectTransform.localScale = Vector3.zero;
    }

    private IEnumerator LevelEnd()
    {
        Vector3 cameraGoalPos = new Vector3((Constants.MapWidth / 2) * m_manager.ground_sprite.bounds.size.x, m_manager.ground_sprite.bounds.size.y * Constants.MapHeight * -2, -10);
        while(Camera.main.transform.position != cameraGoalPos)
        {
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, cameraGoalPos, player.fall_speed*1.1f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        current_map = GetComponent<map_manager>().SpawnMap();
        StartCoroutine("GameStart");
    }
}
