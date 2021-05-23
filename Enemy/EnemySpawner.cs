﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyHpSliderPrefab; //적 체력을 나타내는 slider 프리팹.
    [SerializeField]
    private Transform _cansvasTransform; //UI를 표현해주는 Canvas Transform.
    [SerializeField]
    private Transform[] _wayPoints;   //현재 스테이지 이동 경로.
    [SerializeField]
    private PlayerHP playerHP;        //플레이어의 체력 컴포넌트.
    [SerializeField]
    private PlayerGold playerGold;     //플레이어 골드.
    private Wave currentWave;          //현재 웨이브 정보.
    private List<EnemyCtrl> enemyList; //현재맵에 존재하는 적 정보.
    public bool canWave = true;
    public GameObject[] EnemySpawn;
    public EnemySpawner[] enemy_enemySpawner;

    //적의 생성과 삭제는 EnemySpawner에서 하기 때문에 set은 불필요.
    public List<EnemyCtrl> EnemyList => enemyList; //get

    public PhotonView PV;

    private void Awake()
    {
        //적 리스트 메모리 할당.
        enemyList = new List<EnemyCtrl>();
    }
    private void Start()
    {
        _cansvasTransform = GameObject.FindGameObjectWithTag("GameCanvas").transform;
    }
    public void StartWave(Wave wave)
    {
        currentWave = wave;
        StartCoroutine("SpawnEnemy");
    }

    Transform Root;
    private IEnumerator SpawnEnemy()
    {
            int spawnEnemyCount = 0;
            //현재 웨이브에서 생성해야 하는 만큼의 적 생성 후 코루틴 종료.
            while (spawnEnemyCount < currentWave.maxEnemyCount)
            {
                //웨이브에 등장하는 적의 종류에 따라 적 생성,
                int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
                GameObject go = Instantiate(currentWave.enemyPrefabs[enemyIndex]);

                if (!PV.IsMine)
                {
                    go.name = "@Enemy(Client)";
                }
                else
                {
                    go.name = "@Enemy";
                }
                if (Root == null)
                {
                    Root = new GameObject().transform;
                    Root.name = $"{go.name}_Group";
                }
                go.transform.parent = Root.transform;

                EnemyCtrl enemy = go.GetComponent<EnemyCtrl>();  //방금 생성된 적의 Enemy 스크립트

                //this는 나 자신(자신의 EnemySpawner 정보)
                enemy.Setup(this, _wayPoints);   //Setup 호출.
                enemyList.Add(enemy); //리스트에 할당.

                SpawnEnemySliderHP(go);

                //현재 웨이브에서 생성한 숫자 +1
                spawnEnemyCount++;

                yield return new WaitForSeconds(currentWave.spawnTime);  //스폰 시간 대기
            }
    }

    public void DestroyEnemy(EnemyDestroyType type, EnemyCtrl enemy, int gold)
    {
        //살아서 목표지점 도착했을때.
        if (type == EnemyDestroyType.Arrive)
        {
            //플레이어 hp 감소.
            playerHP.TakeDamage(1);
        }
        else if (type == EnemyDestroyType.kill)
        {
            // 적의 종류에 따라 골드 다르게 획득 하기위해 파둠.
            playerGold.CurrentGold += gold;
        }

        //리스트에서 사망하는 적 정보 삭제.
        enemyList.Remove(enemy);
        //적 오브젝트 삭제.
        Destroy(enemy.gameObject);
    }
    private void SpawnEnemySliderHP(GameObject enemy)
    {
        //적 체력 나타내는 slider UI 생성.
        GameObject sliderclone = Instantiate(_enemyHpSliderPrefab);
        //캔버스 밑으로 이동.
        sliderclone.transform.SetParent(_cansvasTransform);
        //이동하며 바뀐 로컬 스케일 값 1로 고정.
        sliderclone.transform.localScale = Vector3.one;
        //slider ui가 따라다닐 대상 설정.
        sliderclone.GetComponent<SliderPositionAutoSetter>().Setup(enemy.transform);
        //slider ui가 자신의 체력 반영하도록 설정.
        sliderclone.GetComponent<EnemyHpViewer>().Setup(enemy.GetComponent<EnemyHp>());
    }
}
