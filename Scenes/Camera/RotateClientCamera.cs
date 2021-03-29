using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RotateClientCamera : MonoBehaviour
{
   
    void Start()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            this.gameObject.transform.position = new Vector3(0, 0, 50.0f);
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180.0f, 180.0f);

        }

    }


}
