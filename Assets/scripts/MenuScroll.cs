using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScroll : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Scrollbar scrollBar;
    public Image image;

    void OnEnable()
    {
        //Subscribe to the ScrollRect event
        scrollRect.onValueChanged.AddListener(scrollRectCallBack);
        scrollBar.value = 1f;
    }

    //Will be called when ScrollRect changes
    void scrollRectCallBack(Vector2 value)
    {
        Debug.Log("ScrollRect Changed: " + value);
        
        image.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, image.sprite.rect.height * (value.y - 1) * -1, 0);
        Debug.Log(image.gameObject.transform.position);
    }

    void OnDisable()
    {
        //Un-Subscribe To ScrollRect Event
        scrollRect.onValueChanged.RemoveListener(scrollRectCallBack);
    }
}
