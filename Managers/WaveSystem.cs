using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves; //현재 스테이지 모든 웨이브 정보.
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1; //현재 웨이브 인덱스.
    //웨이브 출력 정보를 위한 Get 프로퍼티.(현재 웨이브)
    public int CurrentWave => currentWaveIndex + 1; // 시작이 0이기에 +1

    public GameObject[] EnemySpawn;
    public EnemySpawner[] enemy_enemySpawner;

    public PhotonView PV;

    private PhotonView SpawnerPV;

    public void Start()
    {
        //EnemySpawn = GameObject.FindGameObjectsWithTag("EnemySpawner");
        //for (int i = 0; i < EnemySpawn.Length; i++)
        //{
        //    enemy_enemySpawner[i] = EnemySpawn[i].GetComponent<EnemySpawner>();
        //}
    }

    [PunRPC]
    public void StartWave()
    {
        //현재 맵에 적이 없고 웨이브가 남아있다면
        if (enemy_enemySpawner[1].EnemyList.Count == 0 && enemy_enemySpawner[0].EnemyList.Count == 0 && currentWaveIndex < waves.Length - 1)
        {
            //인덱스의 시작이 -1로 해둔 이유 웨이브 인덱스 증가 코드가 먼저 와야하기 때문
            currentWaveIndex++;
            //EnemySpawner의 startWave() 호출.
            enemy_enemySpawner[1].StartWave(waves[currentWaveIndex]);
            enemy_enemySpawner[0].StartWave(waves[currentWaveIndex]);
        }

    }

    public void FixedUpdate()
    {
        EnemySpawn = GameObject.FindGameObjectsWithTag("EnemySpawner");
        for (int i = 0; i < EnemySpawn.Length; i++)
        {
            enemy_enemySpawner[i] = EnemySpawn[i].GetComponent<EnemySpawner>();
        }
        if (enemy_enemySpawner[0].EnemyList.Count==0)
        {
            enemy_enemySpawner[0].canWave = true;
        }
        else
        {
            enemy_enemySpawner[0].canWave = false;
        }

        if (enemy_enemySpawner[1].EnemyList.Count == 0)
        {
            enemy_enemySpawner[1].canWave = true;
        }
        else
        {
            enemy_enemySpawner[1].canWave = false;
        }


        if (enemy_enemySpawner[0].canWave==true && enemy_enemySpawner[1].canWave == true)
        {
            PV.RPC("StartWave", RpcTarget.AllBuffered);
        }
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;           //현재 웨이브 생성주기.
    public int maxEnemyCount;         //현재 웨이브 적 등장 카운트.
    public GameObject[] enemyPrefabs; //현재 웨이브 적 등장 종류.
}

