using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class TowerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TowerTemplete[] towerTemplete; //타워 정보.

    [SerializeField]
    private GameObject _towerPrefab; //리소스매니저 사용 안할 시 사용.

    public int towerBuildGold = 10; // 타워건설에 필요한 골드.
    [SerializeField]
    public EnemySpawner _enemySpawner; //현재 맵에 존재하는 적 리스트 정보 얻기위해.

    //[SerializeField]
    //private GameObject[] _towerprefab;

    [SerializeField]
    private PlayerGold playerGold; // 타워 건설 시 골드 감소를 위해.

    [SerializeField]
    private PhotonView PV;

    private static GameObject go;
    private static GameObject go_Client;

    Transform Root;

    public void Start()
    {
        //타워 선택 리스트 할당 코드 들어가야함. 디비 연동 후.
        //_towerprefab = GameObject.FindGameObjectsWithTag();
        if (!PV.IsMine) this.gameObject.tag = "TowerSpawner(Client)";
    }
    public void SpawnTower(Transform tileTransform)
    {
        if(towerBuildGold > playerGold.CurrentGold) //타워 건설 비용이 부족하면 
        {
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        //우선 타워건설 가능한 타일인지 판단.
        if (tile._isBuildTower == true) //빈타일이 아니면 
        {
            return; //이미 타워가 있는곳이면 생성되지 않고 return
        }

        //선택한 타일에 타워 생성.
        tile._isBuildTower = true;

        //타워 건설에 필요한 골드 감소.
        playerGold.CurrentGold -= towerBuildGold;

        //타워 설치시 필요 골드 증가.
        towerBuildGold += 10;
        //선택한 타일의 위치에 타워 오브젝트 생성.
        int RandomTower = Random.Range(0, 5);
        if (PV.IsMine)
        {
            // go = PhotonNetwork.Instantiate(_towerprefab[RandomTower].name, tileTransform.transform.position, Quaternion.identity) as GameObject;
            go = PhotonNetwork.Instantiate(towerTemplete[RandomTower].name, tileTransform.transform.position, Quaternion.identity) as GameObject;
            TowerParenting(go);
            //타워 무기에 enemyspawner List 전달.
            go.GetComponent<TowerWeapon>().SetUp(_enemySpawner,playerGold);
        }
      
    }

    public void TowerParenting(GameObject game)
    {
        game.name = "@Tower01";
        if (Root == null)
        {
            Root = new GameObject().transform;
            Root.name = $"{game.name}_Group";
        }
        game.transform.parent = Root.transform;
    }

}
