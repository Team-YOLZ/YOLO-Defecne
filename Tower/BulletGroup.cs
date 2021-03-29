using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BulletGroup : MonoBehaviour
{
    public PhotonView PV;
    void Start()
    {
        if (!PV.IsMine) gameObject.name = "@BulletGroup(Client)";
    }

}
