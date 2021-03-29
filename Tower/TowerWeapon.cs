using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public enum WeaponState
{
    SearchTarget =0,
    AttackToTarget,
    AttackToTarget_Client,
}

public class TowerWeapon : MonoBehaviourPun //, IPunObservable
{
    public PhotonView PV;

    [SerializeField]
    private TowerTemplete towerTemplete;                          //타워 정보.
    [SerializeField]
    private GameObject _projecttilePrefab;                        //발사체 프리팹.
    [SerializeField]
    private Transform _spawnPoint;                                //발사체 생성 위치.
    //[SerializeField]
    //private float _attackRate = 0.5f;                             //공격 속도(Default : 0.5f)
    //[SerializeField]
    //private float _attackRange = 2.0f;                            //공격 범위(Default : 2.0f)
    //[SerializeField]
    //private float attackdamage = 1f;                              //공격력.

    private WeaponState weaponState = WeaponState.SearchTarget;   //타워 상태(Default : Search)
    private Transform attackTarget = null;                        //공격 대상(Default : null)
    private EnemySpawner enemySpawner;                            //게임에 존재하는 적 정보 획득용.
    private EnemySpawner _enemySpawner;
    private GameObject go;
    private GameObject EnemySpawner_Client;
    private int level = 0;
    private SpriteRenderer spriteRenderer;
    private PlayerGold playerGold;

    public Sprite Towersprite => towerTemplete.weapon[level].sprite;
    public float Damage => towerTemplete.weapon[level].damage;
    public float Rate => towerTemplete.weapon[level].rate;
    public float Range => towerTemplete.weapon[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplete.weapon.Length;


    void Start()
    {
        EnemySpawner_Client = GameObject.FindWithTag("TowerSpawner(Client)");
        if (!PV.IsMine)
        {
            _enemySpawner = EnemySpawner_Client.GetComponent<TowerSpawner>()._enemySpawner; //클라이언트도 게임에 존재하는 적 정보 획득
            SetUp(_enemySpawner,playerGold);

            EnemySpawner_Client.GetComponent<TowerSpawner>().TowerParenting(gameObject); //클라타워 그룹화
            ChangeState(WeaponState.SearchTarget);
        }

    }

    public void SetUp(EnemySpawner enemySpawner,PlayerGold playerGold)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerGold = playerGold;
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
                if (distance <= towerTemplete.weapon[level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }

            if (attackTarget != null )
            {
                ChangeState(WeaponState.AttackToTarget);
            }
            yield return null;
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            // 1.target이 있는지 검사 (다른 타워에 의해 죽었거나 , Goal 지점에 도달해 이미 사라진 target 존재 가능함.)
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget); //다시 Default로 돌아 간후 break;
                break;
            }

            //2. target이 공격범위 안에 있는지 검사. (공격 범위를 벗어나면 새로운 타겟 찾아야함.)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > towerTemplete.weapon[level].range)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRate만큼 대기(연사 속도 고려.)
            yield return new WaitForSeconds(towerTemplete.weapon[level].rate);

            //4. 공격(발사체 지정.)
            SpawnProjecttile();
        }
    }
    public bool Upgrade()
    {
        //타워 업그레이드에 필요한 골드가 충분한지 검사.
        if(playerGold.CurrentGold < towerTemplete.weapon[level+1].cost)
        {
            return false;
        }
        //타워 레벨 증가.
        level++;
        //타워 외형 변경.
        spriteRenderer.sprite = towerTemplete.weapon[level].sprite;
        //골드 차감.
        playerGold.CurrentGold -= towerTemplete.weapon[level].cost;

        return true;
    }
    private void SpawnProjecttile()
    {
    
        if (PV.IsMine)
        {
            PV.RPC("Fire", RpcTarget.All); //같은 pv를 가진 오브젝트에 Frie()함수를 호출해라! 
        }
  
    }

    void BulletParenting(GameObject game)
    {
        game.name = $"{_projecttilePrefab.name}_Bullet";
        if (GameObject.Find("@BulletGroup") != null)
        {
            game.transform.parent = GameObject.Find("@BulletGroup").transform;
        }
        else
        {
            GameObject root = new GameObject(name: "@BulletGroup");
            game.transform.parent = root.transform;
        }
    }
    void BulletParenting_Client(GameObject game) 
    {
        game.name = $"{_projecttilePrefab.name}_Bullet(Client)";
        if (GameObject.Find("@BulletGroup(Client)") != null)
        {
            game.transform.parent = GameObject.Find("@BulletGroup(Client)").transform;
        }
        else
        {
            GameObject root = new GameObject(name: "@BulletGroup(Client)");
            game.transform.parent = root.transform;
        }
    }

    [PunRPC]
    void Fire()  //총알에  타겟정보와 공격력을 전해줄건데....
    {
        go = Instantiate(_projecttilePrefab, _spawnPoint.position, Quaternion.identity);

        if (PV.IsMine)
        {
            BulletParenting(go);
            go.GetComponent<ProjectTile>().SetUp(attackTarget, towerTemplete.weapon[level].damage);
        }
        else
        {
            BulletParenting_Client(go);
            go.GetComponent<ProjectTile>().SetUp(attackTarget, towerTemplete.weapon[level].damage);

            //go.GetComponent<ProjectTile>().SetUp(_receiveAttackTarget, _receiveAttackDamage);
        }
    }

    //private Transform _receiveAttackTarget;  //실시간으로 넘겨준 정보를 받아올 변수
    //private float _receiveAttackDamage;  //실시간으로 넘겨준 정보를 받아올 변수
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.IsWriting)
    //    {
    //        stream.SendNext(attackTarget);
    //        stream.SendNext(attackdamage);
    //    }
    //    else
    //    {
    //        _receiveAttackTarget = (Transform)stream.ReceiveNext();
    //        _receiveAttackDamage = (float)stream.ReceiveNext();
    //    }

    //}
}
