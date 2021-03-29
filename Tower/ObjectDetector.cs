using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class ObjectDetector : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    [SerializeField]
    private TowerSpawner _towerSpawner;


    //private Camera mainCamera;
    //private Ray ray;
    //private RaycastHit hit;

    [SerializeField]
    private GameObject[] tiles; //타일

    [SerializeField]
    private GameObject[] Enemytiles; // 적 타일.

    private List<Transform> emptytiles = new List<Transform>(); //빈 타일 리스트

    private void Awake()
    {
        //mainCamera 할당
        //mainCamera = Camera.main;
        if(PV.IsMine) tiles = GameObject.FindGameObjectsWithTag("Tile");
        else tiles = GameObject.FindGameObjectsWithTag("Tile(Client)");
    }

    //private void Update()
    //{
        //타일에 직접 소환하는 코드 (예전 코드)

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //ray  (origin : maincamera , direction : 클릭된 마우스 좌표)
        //    ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //    //raycast : ray에 검출된 object return 
        //    if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // 일단 무한대로 쏨.
        //    {
        //        if (hit.transform.CompareTag("Tile"))
        //        {
        //            _towerSpawner.SpawnTower(hit.transform);
        //        }
        //    }
        //}
    //}

    public void SpawnTower()
    {
        emptytiles.Clear();
        for (int i = 0; i < tiles.Length; i++)   //빈 타일 서치
        {
            if (tiles[i].GetComponent<Tile>()._isBuildTower == false) //빈 타일이면
            {
                emptytiles.Add(tiles[i].transform);
            }
        }

        if (emptytiles.Count == 0) return; //남은 칸 없으면 return

        int emptyList = Random.Range(0, emptytiles.Count); //빈 타일 중 랜덤.

        _towerSpawner.SpawnTower(emptytiles[emptyList]);
    }
}
