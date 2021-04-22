using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable; //CustomProperties를 사용하기 위함

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public GameObject MyField;

    public GameObject[] Mydacks;
    public GameObject[] MydacksForResult;

    public string[] MydacksString;
    public Sprite[] MyDacks_Image;
    
    public TowerTemplete[] towerTempletes;
    public Hashtable ht = new Hashtable();
    public string htstring;

    private bool bPause = false;

    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        if (instance == null) instance = this;
        else Destroy(gameObject);

        ht.Add("쇠", "Tower_Iron");
        ht.Add("불", "Tower_Fire");
        ht.Add("얼음", "Tower_Ice");
        ht.Add("바람", "Tower_Wind");
        ht.Add("활", "Tower_Arrow");
        ht.Add("에너지", "Tower_Energy");
        ht.Add("잠김", "Tower_Lock");
        ht.Add("가시", "Tower_Thorn");
        ht.Add("빛", "Tower_Light");
        ht.Add("제물", "Tower_Offering");
    }

    private void Start()
    {
        // MydackList 업데이트
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log(cp["DackList"]);
        MydacksString = cp["DackList"].ToString().Split(',');
        for (int i = 0; i < MydacksString.Length; i++)
        {
            Debug.Log(MydacksString[i]);
        }

        for (int i = 0; i < Mydacks.Length; i++)
        {
            Debug.Log(Mydacks[i] + MydacksString[i]);
            Mydacks[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(MydacksString[i]);
            htstring += ht[Mydacks[i].GetComponent<Image>().sprite.name];
            towerTempletes[i] = Resources.Load<TowerTemplete>($"Tower_Template/{ht[Mydacks[i].GetComponent<Image>().sprite.name]}");
        }
    }

    public void Pause() // 게임 일시 정ㅈㅣ
    {
        bPause = !bPause;

        if (bPause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void MydacksForResultWindow()
    {
        // MydackList 업데이트
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        MydacksString = cp["DackList"].ToString().Split(',');

        for (int i = 0; i < MydacksForResult.Length; i++)
        {
            Debug.Log(MydacksForResult[i] + MydacksString[i]);
            MydacksForResult[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(MydacksString[i]);
        }
    }
}


