using System.Collections;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    private int             wayPointCount;    //이동 경로 개수
    private Transform[]     wayPoints;        //이동 경로 정보
    private int             currentIndex = 0; //현재 목표지점 인덱스
    private EnemyMovement   movement2D;       //오브젝트 이동 제어
    private EnemySpawner    enemyspawner;     //적의 삭제를 본인이 아닌 EnemySpawner가 알려서 삭제.

    public void Setup(EnemySpawner enemySpawner,Transform[] _wayPoints)
    {
        movement2D = GetComponent<EnemyMovement>();
        this.enemyspawner = enemySpawner;

        //적 이동경로  WayPoints 정보 설정.
        wayPointCount  = _wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = _wayPoints;

        //적의 위치롤 첫번째 WayPoint 위치로 설정.
        transform.position = _wayPoints[currentIndex].position;

        //적 이동,목표지점 설정 코루틴 함수 시작.
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        //첫 다음 이동 방향 설정.
        NextMoveTo();

        while(true)
        {
            //적 오브젝트 회전
            transform.Rotate(Vector3.forward * 10.0f);

            //조건 : 적의 현재 위치와 목표 위치가 거리가 0.02 * _movement2D.Movespeed(이동속도)보다 작을때
            //만약  0.02 * _movement2D.Movespeed(이동속도)가 너무 빨라지면 조건문에 안걸릴 수 있음 그때는 (0.02)수치 조절
            if(Vector3.Distance(transform.position, wayPoints[currentIndex].position)< 0.02 * movement2D.Movespeed)
            {
                //다음 이동 방향 설정
                NextMoveTo();
            }
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        //아직 이동할 WayPoint가 남았다면
        if(currentIndex<wayPointCount-1)
        {
            //적의 위치를 정확하게 목표 위치로 설정.
            transform.position = wayPoints[currentIndex].position;
            //이동 방향 설정 =>다음 목표 지점(waypoints)
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized; // 방향 벡터
            movement2D.MoveTo(direction);
        }
        //현재 위치가 마지막 WayPont라면
        else
        {
            OnDie();
            //여기에 생명력 깍이는 코드와 이펙트 추가해야함.
        }
    }

    public void OnDie()
    {
        enemyspawner.DestroyEnemy(this);
    }
}
