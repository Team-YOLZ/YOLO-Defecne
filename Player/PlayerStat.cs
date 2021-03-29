using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerStat : MonoBehaviour
{
    public PhotonView PV;

    void Awake()
    {

        if (!PV.IsMine) this.gameObject.tag = "PlayerStat(Client)";
    }
}
