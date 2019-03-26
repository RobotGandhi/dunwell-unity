using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    public RectTransform won;

    private void Start()
    {
        won.position = Camera.main.WorldToViewportPoint(new Vector3(0, Screen.height, 0));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("main");
        }
#endif
        if(Input.touchCount > 0)
        {
            SceneManager.LoadScene("main");
        }
    }

}
