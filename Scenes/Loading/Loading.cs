using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System.Diagnostics; //시간 측정
using Debug = UnityEngine.Debug;  //Diagnostics 사용시 디버그에 충동 생길 수 있으므로...

// 시간측정하는 이유 : 마스터가 서버로 커넥되는 순간이 너무 빠르면 로딩화면이 순식간에 지나가는 이물감이 생기므로
public class Loading : MonoBehaviour
{
    private Stopwatch sw = new Stopwatch(); //최소 경과시간을 알기 위해 
    private TimeSpan ts; //스탑워치 시간을 담을 변수 -> 시 :분 :초 :밀리초 형식

    [SerializeField]
    protected static LoadingState IsLoading = LoadingState.Loading; //로딩 상태

    private void Start()
    {
        Init(); //자식클래스의 Init()도 실행 됨
    }
    private void Update()
    {
        UpdateController();  //자식 클래스의 UpdateController()도 실행 됨
    }

    protected virtual void Init()
    {
        IsLoading = LoadingState.Loading;
        sw.Start();  //시간측정 시작
    }

    protected virtual void UpdateController()
    {
        switch (IsLoading)   //상태 별로 나누기 위함 .. 근데 이 함수에는 switch문 필요 없을거같은데,, 
        {
            case LoadingState.Loading:
                break;
            case LoadingState.LoadingSuccess:
                break;
            case LoadingState.LoadingFail:
                break;
        }
    }

    //LobbyManager.cs에서 pun콜백 함수 OnConnectedToMaster()실행 시 IsConnectedToMaster()호출됨
    //(마스터가 서버에 접속 ,로컬플레이어닉네임 등록이 완료되면 호출된다.)
    public void IsConnectedToMaster(bool ise) 
    {
        if (ise)
        {
            sw.Stop();  //시간 측정 정지
            ts = sw.Elapsed; // TimeSpan클래스의 멤버변수 ts에 측정된 시간 저장
            if (ts.Milliseconds < 2500) // 최소 2.5 초가 지나지 않았으면..
                StartCoroutine(WaitForIt());  //조금 더 기다리도록
            else
            {
                IsLoading = LoadingState.LoadingSuccess;
            }
                
        }
        else
        {
            IsLoading = LoadingState.LoadingFail;
        }
    }

    IEnumerator WaitForIt()
    {
        if (ts.Milliseconds < 1000) //1초채 지나지 않았다면..
            yield return new WaitForSeconds(2.5f);
        else
            yield return new WaitForSeconds(1.8f);

        IsLoading = LoadingState.LoadingSuccess;
  
    }

}
