using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LoadingImage : Loading
{
    [SerializeField]
    private GameObject Loading_image;

    protected override void Init() //부모클래스가 실행 시켜줌 
    {
        base.Init();  
    }

    protected override void UpdateController() //부모클래스가 실행 시켜줌 
    {
        switch (IsLoading)
        {
            case LoadingState.Loading:
                break;
            case LoadingState.LoadingSuccess:
                Loading_image.SetActive(false);  //로딩이 완료되면 로딩화면이 꺼지도록
                break;
            case LoadingState.LoadingFail:
                break;
           
        }
    }

}
