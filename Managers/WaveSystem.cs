using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves; //현재 스테이지 모든 웨이브 정보.
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1; //현재 웨이브 인덱스.

    private GameObject Enemy_EnemySpawner;
    private EnemySpawner enemy_enemySpawner;
    //웨이브 출력 정보를 위한 Get 프로퍼티.(현재 웨이브)
    public int CurrentWave => currentWaveIndex + 1; // 시작이 0이기에 +1

    public void Start()
    {
        //Enemy_EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner(Client)");
        //enemy_enemySpawner = Enemy_EnemySpawner.GetComponent<EnemySpawner>();
        StartWave();
    }
    public void StartWave()
    {
        //현재 맵에 적이 없고 웨이브가 남아있다면
        if(enemySpawner.EnemyList.Count==0 && currentWaveIndex < waves.Length-1)
        {
            //인덱스의 시작이 -1로 해둔 이유 웨이브 인덱스 증가 코드가 먼저 와야하기 때문
            currentWaveIndex++;
            //EnemySpawner의 startWave() 호출.
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
    public void NextWave()
    {
        if(enemySpawner.EnemyList.Count == 0)
        {
            StartWave();
        }
    }
    public void FixedUpdate()
    {
        NextWave();
    }
}
[System.Serializable]
public struct Wave
{
    public float spawnTime;           //현재 웨이브 생성주기.
    public int maxEnemyCount;         //현재 웨이브 적 등장 카운트.
    public GameObject[] enemyPrefabs; //현재 웨이브 적 등장 종류.
}

