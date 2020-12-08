using System.Collections;
using UnityEngine;

public enum WeaponState
{
    SearchTarget =0,
    AttackToTarget,
}

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject _projecttilePrefab;                        //발사체 프리팹.
    [SerializeField]
    private Transform _spawnPoint;                                //발사체 생성 위치.
    [SerializeField]
    private float _attackRate = 0.5f;                             //공격 속도(Default : 0.5f)
    [SerializeField]
    private float _attackRange = 2.0f;                            //공격 범위(Default : 2.0f)

    private WeaponState weaponState = WeaponState.SearchTarget;   //타워 상태(Default : Search)
    private Transform attackTarget = null;                        //공격 대상(Default : null)
    private EnemySpawner enemySpawner;                            //게임에 존재하는 적 정보 획득용.

    private void Awake()
    {
    }

    public void SetUp(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        //Default 상태는 Search
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //이전 상태 종료.
        StopCoroutine(weaponState.ToString());
        //상태 변경.
        weaponState = newState;
        //새로운 상태 시작.
        StartCoroutine(weaponState.ToString());
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            //제일 가까이 있는 적 찾기 위해 최초 거리를 크게 설정.
            float closestDistSqr = Mathf.Infinity;
            //EnemySpawner 안의 EnemyList에 있는 모든 적 검사.
            for (int i = 0; i < enemySpawner.EnemyList.Count; i++)
            {
                //적 리스트와 타워간 거리 검사.
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                //공격 사거리 안에 들어왔고 && 가장 가까운 적 즉 distance가 가장 짧은 적.
                if (distance <= _attackRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }
            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }
            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while(true)
        {
            // 1.target이 있는지 검사 (다른 타워에 의해 죽었거나 , Goal 지점에 도달해 이미 사라진 target 존재 가능함.)
            if(attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget); //다시 Default로 돌아 간후 break;
                break;
            }

            //2. target이 공격범위 안에 있는지 검사. (공격 범위를 벗어나면 새로운 타겟 찾아야함.)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance > _attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRate만큼 대기(연사 속도 고려.)
            yield return new WaitForSeconds(_attackRate);

            //4. 공격(발사체 지정.)
            SpawnProjecttile();
        }
    }

    private void SpawnProjecttile()
    {
        GameObject go = Instantiate(_projecttilePrefab, _spawnPoint.position, Quaternion.identity);
        go.name = $"{transform.name}_Bullet";
        if (GameObject.Find("@BulletGroup") != null)
        {
            go.transform.parent = GameObject.Find("@BulletGroup").transform;
        }
        else
        {
            GameObject root = new GameObject(name: "@BulletGroup");
            go.transform.parent = root.transform;
        }
        //생성된 발사체에게 타겟 정보 전달.
        go.GetComponent<ProjectTile>().SetUp(attackTarget);
    }
}
