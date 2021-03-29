using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DackCtrl : MonoBehaviour, IPointerDownHandler
{
    public GameObject[] Mydacks;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.lastPress.tag == "DackList")
        {
            for (int i = 0; i < Mydacks.Length; i++)
            {
                if (eventData.lastPress.GetComponent<Image>().sprite.name == Mydacks[i].GetComponent<Image>().sprite.name)
                {
                    return;
                }
                if (i == Mydacks.Length - 1) gameObject.GetComponent<Image>().sprite = eventData.lastPress.GetComponent<Image>().sprite;
            }
        }
    }
}
