using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public Image logo;
    public Image prompt1, credits;
    public Image fade_panel;
    float f = 0.0f;
    bool start_game = false;

    private void Start()
    {
        StartCoroutine("LogoCoroutine");
        StartCoroutine("StartCoroutine");
    }

    private void Update()
    {
        if(prompt1.color == Color.white)
        {
            f += 1*Time.deltaTime;
            prompt1.rectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f) * (Mathf.Abs((Mathf.Cos(f*5)*0.1f) + 1));

            // Check for input from touch now
            if(Input.touchCount > 0)
            {
                if (!start_game)
                {
                    start_game = true;
                    StartCoroutine("StartGame");
                }
            }
        }
    }

    private IEnumerator LogoCoroutine()
    {
        while(logo.rectTransform.anchoredPosition != new Vector2(0, -200))
        {
            logo.rectTransform.anchoredPosition = Vector2.Lerp(logo.rectTransform.anchoredPosition, new Vector2(0, -200), 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    private IEnumerator StartCoroutine()
    {
        yield return new WaitForSeconds(2);

        while(prompt1.color.a <= 0.925f)
        {
            prompt1.color = Color.Lerp(prompt1.color, Color.white, 1.5f * Time.deltaTime);
            credits.color = Color.Lerp(prompt1.color, Color.white, 1.5f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        prompt1.color = Color.white;
        credits.color = Color.white;
    }

    private IEnumerator StartGame()
    {
        while(fade_panel.color.a <= 0.95f)
        {
            fade_panel.color = Color.Lerp(fade_panel.color, Color.black, 1.5f * Time.deltaTime);
            Camera.main.transform.position -= Vector3.down * 10 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        fade_panel.color = Color.black;

        SceneManager.LoadScene("main");
    }
}
