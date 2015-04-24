using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;


public class MenuButton : Button
{
    public Image LeftSelectorImage;
    public Image RightSelectorImage;
    
    protected override void Awake()
    {
        base.Awake();

        LeftSelectorImage.gameObject.SetActive(false);
        RightSelectorImage.gameObject.SetActive(false);
    }

    public override void OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        base.OnSelect(eventData);

        LeftSelectorImage.gameObject.SetActive(true);
        RightSelectorImage.gameObject.SetActive(true);
    }

    public override void OnDeselect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        base.OnDeselect(eventData);

        LeftSelectorImage.gameObject.SetActive(false);
        RightSelectorImage.gameObject.SetActive(false);
    }
}
