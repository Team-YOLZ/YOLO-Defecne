using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBtnCtrl : MonoBehaviour
{
    private Button button;
    private GameObject obj;

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
    }

    void Start()
    {
        button.onClick.AddListener(TowerSpawn);
    }

    void TowerSpawn()
    {
        obj = GameObject.Find("ObjectDetector");
        obj.GetComponent<ObjectDetector>().SpawnTower();
      
    }
}