using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHp : MonoBehaviour
{
    [SerializeField]
    private float maxHP;
    private float currentHP;
    private bool isDie = false;
    private EnemyCtrl enemy;
    private SpriteRenderer spriteRenderer;

    //외부 클래스에서 참고할때 사용하기 위함.
    public float MaxHP => maxHP; 
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<EnemyCtrl>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        if (isDie == true) return;

        currentHP -= damage;

        StopCoroutine(HitAlphaAnimation());
        StartCoroutine(HitAlphaAnimation());

        //사망 코드
        if(currentHP<=0)
        {
            isDie = true;

            enemy.OnDie(EnemyDestroyType.kill);
        }
    }

    private IEnumerator HitAlphaAnimation()
    {
        //현재 적의 색상.
        Color color = spriteRenderer.color;

        //적의 투명도 40퍼센트.
        color.a = 0.4f;
        spriteRenderer.color = color;

        //0.05초 대기
        yield return new WaitForSeconds(0.05f);

        //적의 투명도 100프로 설정.
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
