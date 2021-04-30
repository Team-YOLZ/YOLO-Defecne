using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonMgs 
{
    public bool _enterGame = false;
    public string _userId="tester";
    public int dackcount=0;

    #region Private Fields

    private readonly string _gameVersion = "1";
      #endregion



    public void OnAwake()
    {
        //값이 true일 때 MasterClient는 PhotonNetwork.LoadLevel()을 호출 할 수 있고 모든 연결된 플레이어들은 동일한 레벨을 자동적으로 로드 할 것이다.
        //즉, 마스터클라이언트가 씬을 넘길 시 모든 플레이어가 같은 씬으로 이동. 이동중이면 통신이 버퍼에 기록
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    public void OnStart() //마스터서버 접속
    {
        PhotonNetwork.GameVersion = _gameVersion;
        PhotonNetwork.ConnectUsingSettings(); // 마스터 서버 접속
        
    }
    public void OnDisconnect()
    {
        PhotonNetwork.Disconnect();
        //PhotonNetwork.GameVersion = _gameVersion;
        //PhotonNetwork.ConnectUsingSettings();  // 재 접속

    }
    public void JoinLobby() //로비 참가
    {
        PhotonNetwork.JoinLobby();
    }

    public void LeaveLobby() //로비 떠나기
    {
        PhotonNetwork.LeaveLobby();

    }
    
    public void CreateRoom(string RoomNumber) //방 만들기
    {
        PhotonNetwork.CreateRoom(RoomNumber, new RoomOptions { MaxPlayers = 2 });

    }

    public void JoinRoom(string ReceiveCode) //방 참가하긱
    {
        PhotonNetwork.JoinRoom(ReceiveCode, null);

    }
    public void LeaveRoom() //방 떠나기
    {
        PhotonNetwork.LeaveRoom();  //master server로 돌아감
    }

    public void LoadLevelGame() //씬 동기화
    {
        PhotonNetwork.LoadLevel("Game");
        Debug.Log("---Game Start!!----");
    }
    public void LoadLevelMain()
    {
        PhotonNetwork.LoadLevel("Main");
        Debug.Log("---Main Scene!!----");
        //OnStart();
    }

    public void ShowLogConutOfRooms()  //현재 방의 수 보여주기
    {
        Debug.Log("현재 방 수 : " + PhotonNetwork.CountOfRooms);
    }
}
