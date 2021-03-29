using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;

    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        //Slider가 쫒아다닐 타겟 설정.
        targetTransform = target;
        //RectTransform 컴포넌트 정보 가져오기.
        rectTransform = GetComponent<RectTransform>();
    }

    //오브젝트 위치정보를 실시간으로 받아와야해서 Update문 사용 불가피 대신 LateUpdate문 사용으로 과부하 최소화시킴.
    private void LateUpdate()
    {
        // 적이 죽으면 Slider UI도 삭제.
        if(targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }
        //오브젝트의 월드 좌표 기준 화면에서 좌표 값 구함.
        Vector3 screenposition = Camera.main.WorldToScreenPoint(targetTransform.position);

        //화면 내에서 좌표 + distance 만큼 떨어진 위치에 Slider UI 띄움.
        rectTransform.position = screenposition + distance;
    }
}
