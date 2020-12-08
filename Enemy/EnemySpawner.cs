using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject  _enemyPrefab; //적 프리팹. 리소스매니저 사용 or 사용안함 후에 결정.

    [SerializeField]
    private float       _spawnTime;   //적 생성 주기.
    [SerializeField]
    private Transform[] _wayPoints;   //현재 스테이지 이동 경로.
    private List<EnemyCtrl> enemyList; //현재맵에 존재하는 적 정보.

    //적의 생성과 삭제는 EnemySpawner에서 하기 때문에 set은 불필요.
    public List<EnemyCtrl> EnemyList => enemyList; //get


    private void Awake()
    {
        //적 리스트 메모리 할당.
        enemyList = new List<EnemyCtrl>();
        //적 생성 코루틴 함수 호출
        StartCoroutine("SpawnEnemy");
    }

    Transform Root;
    private IEnumerator SpawnEnemy()
    {
        while(true)
        {
            GameObject go = Instantiate(_enemyPrefab);  //리소스매니저 사용 x
            //GameObject go =Managers.Resource.Instantiate("Enemy01"); // 리소스매니저 사용.
            go.name = "@Enemy";
            if (Root == null)
            {
                Root = new GameObject().transform;
                Root.name = $"{go.name}_Group";
            }
            go.transform.parent = Root.transform;

            EnemyCtrl enemy = go.GetComponent<EnemyCtrl>();  //방금 생성된 적의 Enemy 스크립트

            //this는 나 자신(자신의 EnemySpawner 정보)
            enemy.Setup(this,_wayPoints);   //Setup 호출.
            enemyList.Add(enemy); //리스트에 할당.

            yield return new WaitForSeconds(_spawnTime);  //스폰 시간 대기
        }
    }

    public void DestroyEnemy(EnemyCtrl enemy)
    {
        //리스트에서 사망하는 적 정보 삭제.
        enemyList.Remove(enemy);
        //적 오브젝트 삭제.
        Destroy(enemy.gameObject);
    }
}
