using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{

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
