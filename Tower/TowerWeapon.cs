using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public enum WeaponType
{
    Iron = 0, Laser, Ice,
}
public enum WeaponState
{
    //Tower Add State
    SearchTarget = 0,
    TryAttackIron,
    TryAttackLaser,
}

public class TowerWeapon : MonoBehaviourPunCallbacks //, IPunObservable
{
    private GameManager GM;
    public PhotonView PV;
    [SerializeField]
    TowerTemplete[] randomtemplete;

    private WeaponState weaponState = WeaponState.SearchTarget;   //타워 상태(Default : Search)
    private Transform attackTarget = null;                        //공격 대상(Default : null)
    private EnemySpawner enemySpawner;                            //게임에 존재하는 적 정보 획득용.
    private EnemySpawner _enemySpawner;
    private GameObject go;
    private GameObject EnemySpawner_Client;
    private int level = 0;
    private SpriteRenderer spriteRenderer;
    private PlayerGold playerGold;
    private Tile ownertile;

    [Header("Commons")]     //공통인자.
    [SerializeField]
    private TowerTemplete towerTemplete;                          //타워 정보.
    [SerializeField]
    private Transform _spawnPoint;                                //발사체 생성 위치.
    [SerializeField]
    private WeaponType weaponType;                                //무기 속성 정보.

    [Header("Iron")]
    [SerializeField]
    private GameObject _projecttilePrefab;                        //발사체 프리팹.

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer; //레이저로 사용될 선.
    [SerializeField]
    private LayerMask targetLayer;  //광선에 부딪힐 레이어.

    public bool isselect = false;
    public bool isalpha = false;
    public Sprite Towersprite => towerTemplete.weapon[level].sprite;
    public float Damage => towerTemplete.weapon[level].damage;
    public float Rate => towerTemplete.weapon[level].rate;
    public float Range => towerTemplete.weapon[level].range;
    public int Level => level + 1;
    public int MaxLevel => towerTemplete.weapon.Length;
    public float Slow => towerTemplete.weapon[level].slow;


    void Start()
    {
        EnemySpawner_Client = GameObject.FindWithTag("TowerSpawner(Client)");
        if (!PV.IsMine)
        {
            _enemySpawner = EnemySpawner_Client.GetComponent<TowerSpawner>()._enemySpawner; //클라이언트도 게임에 존재하는 적 정보 획득
            SetUp(_enemySpawner, playerGold, ownertile);

            EnemySpawner_Client.GetComponent<TowerSpawner>().TowerParenting(gameObject); //클라타워 그룹화
            ChangeState(WeaponState.SearchTarget);
        }
        
            GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            for (int i = 0; i < randomtemplete.Length; i++)
            {
                randomtemplete[i] = GM.towerTempletes[i];
            }
        
    }

    public void SetUp(EnemySpawner enemySpawner, PlayerGold playerGold, Tile OwnerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        this.playerGold = playerGold;
        this.enemySpawner = enemySpawner;
        this.ownertile = OwnerTile;
        //Default 상태는 Search
        ChangeState(WeaponState.SearchTarget);
    }
    public void Sell()
    {
        ownertile._isBuildTower = false;
        PhotonNetwork.Destroy(gameObject);
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

    private Transform FindClosetAttackTarget()
    {
        //제일 가까이 있는 적 찾기 위해 최초 거리를 크게 설정.
        float closestDistSqr = Mathf.Infinity;

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
        return attackTarget;
    }

    private bool IsPossibleToAttackTarget()
    {
        // 1.target이 있는지 검사 (다른 타워에 의해 죽었거나 , Goal 지점에 도달해 이미 사라진 target 존재 가능함.)
        if (attackTarget == null)
        {
            return false;
        }

        //2. target이 공격범위 안에 있는지 검사. (공격 범위를 벗어나면 새로운 타겟 찾아야함.)
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if (distance > towerTemplete.weapon[level].range)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            attackTarget = FindClosetAttackTarget();

            if (attackTarget != null)
            { 
                if (weaponType == WeaponType.Iron)
                {
                    ChangeState(WeaponState.TryAttackIron);
                }
                else if (weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
            }
            yield return null;
        }
    }

    private IEnumerator TryAttackIron()
    {
        while (true)
        {
            //target 공격하는게 가능한지 검사.
            if (IsPossibleToAttackTarget() == false)
            {
                ChangeState(WeaponState.SearchTarget);
                break;
            }

            //3. attackRate만큼 대기(연사 속도 고려.)
            yield return new WaitForSeconds(towerTemplete.weapon[level].rate);

            //4. 공격(발사체 지정.)
            SpawnProjecttile();
        }
    }

    private IEnumerator TryAttackLaser()
    {
        //레이저 활성화.
        EnableLaser();
        while (true)
        {
            if (IsPossibleToAttackTarget() == false)
            {
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            //
            SpawnLaser();
            yield return null;
        }
    }
    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        if (!PV.IsMine)
        {
            lineRenderer.sortingOrder = 10;
        }
        else
        {
            lineRenderer.sortingOrder = 10;
        }
    }
    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
    }
    private void SpawnLaser()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 direction = attackTarget.position - _spawnPoint.position;
            RaycastHit2D[] hit = Physics2D.RaycastAll(_spawnPoint.position, direction, towerTemplete.weapon[level].range, targetLayer);

            //같은 방향으로 광선 여러 개 쏴서 그 중 attackTarget과 동일한 오브젝트 검출.
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform == attackTarget)
                {
                    //선 시작점.
                    lineRenderer.SetPosition(0, _spawnPoint.position);
                    //선의 목표지점.
                    lineRenderer.SetPosition(1, new Vector3(attackTarget.position.x, attackTarget.position.y, 1));
                }
            }
        }
        else
        {
            Vector3 direction = attackTarget.position - _spawnPoint.position;
            RaycastHit2D[] hit = Physics2D.RaycastAll(_spawnPoint.position, direction, towerTemplete.weapon[level].range, targetLayer);

            //같은 방향으로 광선 여러 개 쏴서 그 중 attackTarget과 동일한 오브젝트 검출.
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform == attackTarget)
                {
                    //선 시작점.
                    lineRenderer.SetPosition(0, _spawnPoint.position);
                    //선의 목표지점.
                    lineRenderer.SetPosition(1, new Vector3(attackTarget.position.x, attackTarget.position.y, -1));
                }
            }
        }
        //적 체력 감소.
        attackTarget.GetComponent<EnemyHp>().TakeDamage(towerTemplete.weapon[level].damage * Time.deltaTime);
    }
    public bool Upgrade()
    {
        //타워 업그레이드에 필요한 골드가 충분한지 검사.
        if (playerGold.CurrentGold < towerTemplete.weapon[level + 1].cost)
        {
            return false;
        }
        if (PV.IsMine)
        {
            int RandomTemplate = Random.Range(0, 5);
            PV.RPC("spriterender", RpcTarget.AllBuffered, RandomTemplate);
        }

        //골드 차감.
        playerGold.CurrentGold -= towerTemplete.weapon[level].cost;
        if (weaponType == WeaponType.Laser)
        {
            //레벨에 따라 레이저 굵어지게 설정.
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }
        return true;
    }

    [PunRPC]
    private void spriterender(int rand)
    {
        towerTemplete = randomtemplete[rand];
        //타워 레벨 증가.
        level++;
        //타워 외형 변경.
        spriteRenderer.sprite = towerTemplete.weapon[level].sprite;
        _projecttilePrefab = towerTemplete.Projecttile;
    }

    private void SpawnProjecttile()
    {
        if (PV.IsMine)
        {
            PV.RPC("Fire", RpcTarget.AllBuffered); //같은 pv를 가진 오브젝트에 Frie()함수를 호출해라! 
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
        }
    }
}
