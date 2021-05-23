using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Tile : MonoBehaviour
{
    public PhotonView PV;

    public bool _isBuildTower { set; get; }
    private void Awake()
    {
        _isBuildTower = false;

        if(!PV.IsMine) this.gameObject.tag = "Tile(Client)";
    }
}
