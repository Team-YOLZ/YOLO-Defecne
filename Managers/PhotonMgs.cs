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
    }

    public void ShowLogConutOfRooms()  //현재 방의 수 보여주기
    {
        Debug.Log("현재 방 수 : " + PhotonNetwork.CountOfRooms);
    }

    

    //#region Pun Callbacks

    //public override void OnConnectedToMaster()
    //{
    //    Debug.Log("connect To Master");

    //    PhotonNetwork.LocalPlayer.NickName = _userId;
    //    Debug.Log("유저닉네임 : " + PhotonNetwork.LocalPlayer.NickName);

    //}

    //public override void OnDisconnected(DisconnectCause cause)
    //{
    //    Debug.Log("disconnect To Master");

    //    PhotonNetwork.ConnectUsingSettings();  // 재 접속
    //}

    //public override void OnJoinedLobby() // 로비 입장시
    //{
    //    Debug.Log("joined Lobby");
    //}

    //public override void OnLeftLobby() //로비 떠날ㅅㅣ
    //{
    //    Debug.Log("Leave Lobby");
    //    //마스터서버로 
    //}

    //public override void OnCreatedRoom()
    //{
    //    Debug.Log("번호 생성 후, 대기중..");
    //    Debug.Log("create현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
    //}
    //public override void OnJoinedRoom()
    //{
    //    Debug.Log("방 참가 완료");
    //    Debug.Log("join현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
    //    Debug.Log("join현재 방 인원쉐 : " + PhotonNetwork.CurrentRoom.PlayerCount);
    //    //주사위 정보들 SetCustom프로퍼티 여기서 진행.
    //    if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
    //    {
    //        //PV.RPC("EnterGame", RpcTarget.MasterClient);
    //        _enterGame = true;
    //        Debug.Log("_enterGame은 트루!!");
    //    }
    //}
    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
      
    //    Debug.Log("방 참가 실패: " + message);
    //}

    //public override void OnLeftRoom()
    //{
    //    Debug.Log("방 나가기");
    //    PhotonNetwork.JoinLobby(); //로비로 다시 들어감
    //}

    //#endregion
}
