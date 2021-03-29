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
    public string[] MydacksString;
    public Sprite[] MyDacks_Image;
    public PhotonView PV;
    private void Awake()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log(cp["DackList"]);
        MydacksString = cp["DackList"].ToString().Split(',');
        for(int i=0; i< MydacksString.Length; i++)
        {
            Debug.Log(MydacksString[i]);
        }
        Mydacks = GameObject.FindGameObjectsWithTag("Mydack");
    }

    private void Start()
    {
        // MydackList 업데이트
        for (int i = 0; i < Mydacks.Length; i++)
        {
            Debug.Log(Mydacks[i] + MydacksString[i]);
            Mydacks[i].GetComponent<Image>().sprite = Resources.Load<Sprite>(MydacksString[i]);
        }

        //if (!PV.IsMine)
        //    PhotonNetwork.Instantiate("MyField", new Vector3(1.3f, 0.18f, 0), Quaternion.Euler(0, 0, 180));
    }
}
