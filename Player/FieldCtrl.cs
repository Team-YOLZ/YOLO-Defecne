using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FieldCtrl : MonoBehaviour
{

    [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
    public static GameObject LocalPlayerInstance;

    public PhotonView PV;

    void Awake()
    {
        if (PV.IsMine)
        {
            FieldCtrl.LocalPlayerInstance = this.gameObject;
        }
        //else
        //{
        //    this.gameObject.transform.position = new Vector3(-1.3f, 0.18f, 77.0f);
        //    this.gameObject.transform.rotation = Quaternion.Euler(0, 180.0f, 180.0f);
        //}

        //DontDestroyOnLoad(this.gameObject);
    }

    //void Start()
    //{
    //    if (!PV.IsMine)
    //    {
       
    //        this.gameObject.transform.position = new Vector3(-1.3f, 0.18f, 77.0f);
    //        this.gameObject.transform.rotation = Quaternion.Euler(0, 180.0f, 180.0f);
    //    }
    //}


}
