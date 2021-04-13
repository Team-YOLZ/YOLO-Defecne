using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Collections;

public class ObjectDetector : MonoBehaviourPunCallbacks
{
    public PhotonView PV;

    [SerializeField]
    private TowerSpawner _towerSpawner;
    [SerializeField]
    private TowerData towerData;


    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    [SerializeField]
    private GameObject[] tiles; //타일

    [SerializeField]
    private GameObject[] Enemytiles; // 적 타일.

    private List<Transform> emptytiles = new List<Transform>(); //빈 타일 리스트

    private string UpgradeList; //업그레이드 리스트.
    private GameObject DestroyTower; //없어질 타워.
    private GameObject[] AllTower;  //전체 타워리스트.
    private GameObject[] SameTower; //같은 타워리스트
    //private PhotonView TowerPV;

    private void Awake()
    {
        //mainCamera 할당
        mainCamera = Camera.main;
        if (PV.IsMine) tiles = GameObject.FindGameObjectsWithTag("Tile");
        else tiles = GameObject.FindGameObjectsWithTag("Tile(Client)");
    }
    private void Start()
    {
        towerData = GameObject.FindGameObjectWithTag("MyInfo").GetComponent<TowerData>();
    }
    private void Update()
    {
        //타일에 직접 소환하는 코드(예전 코드)
        if (Input.GetMouseButtonDown(0) && PV.IsMine)
        {
            //ray  (origin : maincamera , direction : 클릭된 마우스 좌표)

            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //raycast : ray에 검출된 object return 
            if (Physics.Raycast(ray, out hit, Mathf.Infinity)) // 일단 무한대로 쏨.
            {
                if (hit.transform.CompareTag("Tower"))
                {
                    //TowerPV = hit.transform.GetComponent<PhotonView>();
                    towerData.OnPanel(hit.transform);
                    //hit.transform.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    AllTower = GameObject.FindGameObjectsWithTag("Tower");
                    //타워 선택.
                    for (int i = 0; i < AllTower.Length; i++)
                    {
                        if (AllTower[i] == hit.transform.gameObject && AllTower[i].GetComponent<PhotonView>().IsMine
                            && AllTower[i].GetComponent<TowerWeapon>().Level < AllTower[i].GetComponent<TowerWeapon>().MaxLevel)
                        {
                            hit.transform.gameObject.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                            hit.transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                            hit.transform.gameObject.GetComponent<TowerWeapon>().isselect = true;
                        }
                        else
                        {
                            AllTower[i].transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
                            AllTower[i].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                            AllTower[i].transform.gameObject.GetComponent<TowerWeapon>().isselect = false;
                        }
                    }
                    for (int i = 0; i < AllTower.Length; i++)
                    {
                        if (AllTower[i].GetComponent<PhotonView>().IsMine && hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite.name ==
                                AllTower[i].GetComponent<SpriteRenderer>().sprite.name && AllTower[i].transform.gameObject.GetComponent<TowerWeapon>().isselect == false
                                && AllTower[i].GetComponent<TowerWeapon>().Level < AllTower[i].GetComponent<TowerWeapon>().MaxLevel)
                        {
                            //깜빡거리는 코드 추가.
                            AllTower[i].GetComponent<TowerWeapon>().isalpha = true;
                            StartCoroutine(HitAlphaAnimation(AllTower[i]));
                        }
                        else
                        {
                            AllTower[i].GetComponent<TowerWeapon>().isalpha = false;
                            StopCoroutine(HitAlphaAnimation(AllTower[i]));
                        }
                    }
                    //업그레이드.
                    if (UpgradeList != null && hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite.name == UpgradeList
                        && hit.transform.gameObject != DestroyTower && DestroyTower.GetComponent<TowerWeapon>().Level < DestroyTower.GetComponent<TowerWeapon>().MaxLevel)
                    {
                        //모든 타워 셀렉트 종료.
                        for (int i = 0; i < AllTower.Length; i++)
                        {
                            if (AllTower[i].GetComponent<PhotonView>().IsMine)
                            {
                                AllTower[i].GetComponent<TowerWeapon>().isalpha = false;
                                AllTower[i].GetComponent<TowerWeapon>().isselect = false;
                                AllTower[i].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                                Color color = AllTower[i].transform.gameObject.GetComponent<SpriteRenderer>().color;
                                color.a = 1.0f;
                                AllTower[i].transform.gameObject.GetComponent<SpriteRenderer>().color = color;
                            }
                        }
                        StopAllCoroutines();
                        DestroyTower.GetComponent<TowerWeapon>().Sell();
                        hit.transform.gameObject.GetComponent<TowerWeapon>().Upgrade();
                        hit.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
                        hit.transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        UpgradeList = null;
                        towerData.OffPanel();
                        return;
                    }
                    UpgradeList = hit.transform.gameObject.GetComponent<SpriteRenderer>().sprite.name;
                    DestroyTower = hit.transform.gameObject;
                }
                else return;
            }
        }
        //esc 누를시 선택 취소.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpgradeList = null;
            DestroyTower = null;
            AllTower = GameObject.FindGameObjectsWithTag("Tower");
            for (int i = 0; i < AllTower.Length; i++)
            {
                if (AllTower[i].GetComponent<PhotonView>().IsMine)
                {
                    StopAllCoroutines();
                    AllTower[i].GetComponent<TowerWeapon>().isalpha = false;
                    AllTower[i].transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
                    AllTower[i].transform.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    Color color = AllTower[i].transform.gameObject.GetComponent<SpriteRenderer>().color;
                    color.a = 1.0f;
                    AllTower[i].transform.gameObject.GetComponent<SpriteRenderer>().color = color;

                }
            }
        }
    }

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

    private IEnumerator HitAlphaAnimation(GameObject go)
    {
        //현재 적의 색상.
        if (go.GetComponent<TowerWeapon>().isalpha == true)
        {
            SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;

            //적의 투명도 40퍼센트.
            color.a = 0.4f;
            spriteRenderer.color = color;

            //0.05초 대기
            yield return new WaitForSeconds(0.2f);

            //적의 투명도 100프로 설정.
            color.a = 1.0f;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(1.0f);
            StartCoroutine(HitAlphaAnimation(go));
        }
    }
}
