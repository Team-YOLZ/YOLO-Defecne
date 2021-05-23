﻿using System.Collections;
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
    }
}
