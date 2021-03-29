using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class LocalPlayerManager : MonoBehaviour
{
    public GameObject MyField;
    public PhotonView PV;

    void Start()
    {


        if (FieldCtrl.LocalPlayerInstance == null)  //필드 생성
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            if (PV.IsMine) PhotonNetwork.Instantiate(this.MyField.name, new Vector3(-1.3f, -0.18f, 0), Quaternion.identity);
            else PhotonNetwork.Instantiate(this.MyField.name, new Vector3(-1.3f, 0.18f,80.0f), Quaternion.Euler(0, 180.0f, 180.0f));
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
        //    PV.RPC("InstantiateField", RpcTarget.AllBuffered);
    }

        //[PunRPC]
        //public void InstantiateField()
        //{
        //    if (PV.IsMine) PhotonNetwork.Instantiate("MyFiled", new Vector3(-1.3f, -0.18f, 0), Quaternion.identity);
        //}


    }
