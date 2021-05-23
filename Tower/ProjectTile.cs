using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;

public class ProjectTile : MonoBehaviourPunCallbacks //,IPunObservable
{
    public PhotonView PV;

    private EnemyMovement movement2d;
    private Transform target;
    private float damage;
    private Vector3 direction;
   
    public void SetUp(Transform target,float damage)
    {
        movement2d = GetComponent<EnemyMovement>();
        this.target = target; //타워가 설정해준 타겟을 총알 변수내 타겟 변수에 할당.
        this.damage = damage; //타워가 설정해준 공격력.
    }

    private void Start()
    {
        Destroy(gameObject, 2.0f); //2초 뒤에 오브젝트 삭제 
    }
    private void Update()
    { 
        if (target != null) //타겟 감지시
        {
            //발사체 타겟 위치로 이동.
            direction = (target.position - transform.position).normalized; // 방향벡터.
            movement2d.MoveTo(direction);

        }
        else //여러 이유로 타겟이 사라질 시
        {
            //발사체 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (!collision.CompareTag("Enemy")) return; //적이 아닌 대상과 부딪히면 return; 
        if (collision.transform != target) return; //현재 타겟이 아닌 대상과 부딪혀도 return;
        collision.GetComponent<EnemyHp>().TakeDamage(damage); // 적 피 깍이는 함수 새로 추가.
        if(gameObject.CompareTag("IceBullet"))//ice tower라
        {
            EnemyMovement movement = collision.GetComponent<EnemyMovement>();
            movement.Movespeed -= movement.Movespeed * 0.2f;
            StartCoroutine(ResetSpeed(collision.transform.gameObject));
        }
        Destroy(gameObject);                         //발사체 오브젝트 삭제.
    }

    private IEnumerator ResetSpeed(GameObject go)
    {
        yield return new WaitForSeconds(1f);
        go.GetComponent<EnemyMovement>().ResetMoveSpeed();
    }
}
    