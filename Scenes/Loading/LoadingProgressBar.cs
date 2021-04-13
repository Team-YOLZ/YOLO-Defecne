using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class LoadingProgressBar : Loading
{
    [SerializeField]
    private Image Loading_bar;
    [SerializeField]
    private float ProgressSpeed= 0.8f;

    private float curFillAmount = 0;

    protected override void Init()  // 부모클래스가 실행 해준다.
    {
        base.Init();
    }

    protected override void UpdateController() // 부모클래스가 실행 해준다.
    {
        switch (IsLoading)
        {
            case LoadingState.Loading:
                Loading_bar.fillAmount = curFillAmount;
                curFillAmount += Time.deltaTime * ProgressSpeed;
                if (curFillAmount >= 0.998) curFillAmount = 0;  //프로그레스바 회전
                break;
            case LoadingState.LoadingSuccess:
                break;
            case LoadingState.LoadingFail:
                break;

        }
    }

}
