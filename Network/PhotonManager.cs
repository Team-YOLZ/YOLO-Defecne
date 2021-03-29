//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using Photon.Realtime;
//using UnityEngine.UI;
//using Hashtable = ExitGames.Client.Photon.Hashtable;

//public class PhotonManager : MonoBehaviourPunCallbacks
//{

//    [Header("Lobby")]
//    [SerializeField]
//    private GameObject lobby_panel;
//    [SerializeField]
//    private Text connectionInfo_text;

//    [Header("Lobby-FriendMatch")]
//    [SerializeField]
//    private GameObject friendMatch_panel;
//    [SerializeField]
//    private GameObject friendMatchCreateRoom_btn;
//    [SerializeField]
//    private GameObject friendMatchJoin_btn;

//    [Header("Lobby-FriendMatchJoin")]
//    [SerializeField]
//    private GameObject friendMatchJoin_panel;
//    [SerializeField]
//    private Text receiveCodeInfo_text;
//    [SerializeField]
//    private InputField receiveCode_inputfield;

//    [Header("Lobby-FriendMatchCreateRoom")]
//    [SerializeField]
//    private GameObject friendMatchCreateRoom_panel;
//    [SerializeField]
//    private Text title_text;
//    [SerializeField]
//    private InputField sendCode_inputfield;  //readonly

//    [Header("ETC.")]
//    public bool isjoin = false;
//    public PhotonView PV;
//    #region Private Fields

//    private readonly string _gameVersion = "1";

//    private string _userId;
//    private NetworkManager _user;

//    private string _roomNumber;
//    #endregion


//    #region MonoBehaviours CallBacks

//    private void Awake()
//    {
//        //Init();
//        //값이 true일 때 MasterClient는 PhotonNetwork.LoadLevel()을 호출 할 수 있고 모든 연결된 플레이어들은 동일한 레벨을 자동적으로 로드 할 것이다.
//        //즉, 마스터클라이언트가 씬을 넘길 시 모든 플레이어가 같은 씬으로 이동. 이동중이면 통신이 버퍼에 기록
//        PhotonNetwork.AutomaticallySyncScene = true;
//    }
   
//    void Start()
//    {
//        //Init();
//        friendMatchCreateRoom_btn.SetActive(false); //서버 접속 전 방만들기 막기

//        _user = FindObjectOfType<NetworkManager>(); //네트워크매니저 스크립트로 유저아이디 가지고 올거ㅇ
//        _userId = _user.GetUserNickname(); //유저아이디를 넣기
        
//        PhotonNetwork.GameVersion = _gameVersion;
//        PhotonNetwork.ConnectUsingSettings(); // 마스터 서버 접속

//        lobby_panel.SetActive(false);
//        friendMatch_panel.SetActive(false);
//        friendMatchJoin_panel.SetActive(false);
//        friendMatchCreateRoom_panel.SetActive(false);
  
        

//    }

//    void Init()
//    {
//        GameObject go = GameObject.Find("@Managers");
//        if (go.GetComponent<PhotonManager>() == null)
//        {
//            go.AddComponent<PhotonManager>();
//            go.AddComponent<PhotonView>();
           
//        }
//    }

//    #endregion

//    #region Pun Callbacks

//    public override void OnConnectedToMaster()
//    {
//        Debug.Log("connect To Master");

//        PhotonNetwork.LocalPlayer.NickName = _userId;
//        Debug.Log("유저닉네임 : "+PhotonNetwork.LocalPlayer.NickName);

//        friendMatchCreateRoom_btn.SetActive(true);
//    }

//    public override void OnDisconnected(DisconnectCause cause)
//    {
//        Debug.Log("disconnect To Master");
//        connectionInfo_text.text = $"Error : Connection Disabled {cause.ToString()}";


//        PhotonNetwork.ConnectUsingSettings();  // 재 접속
//    }

//    public override void OnJoinedLobby() // 로비 입장시
//    {
//        Debug.Log("joined Lobby");

//        connectionInfo_text.text = "Lobby : Connected to Master Server!";

//        friendMatchCreateRoom_btn.SetActive(true);

//    }

//    public override void OnLeftLobby() //로비 떠날ㅅㅣ
//    {
//        Debug.Log("Leave Lobby");
//        friendMatchCreateRoom_btn.SetActive(false);
//        //마스터서버로 
//    }

//    public override void OnCreatedRoom()
//    {
//        Debug.Log("번호 생성 후, 대기중..");
//        Debug.Log("create현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
//    }
//    public override void OnJoinedRoom()
//    {
//        Debug.Log("방 참가 완료");
//        Debug.Log("join현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
//        Debug.Log("join현재 방 인원쉐 : " + PhotonNetwork.CurrentRoom.PlayerCount);
//        //주사위 정보들 SetCustom프로퍼티 여기서 진행.
//        if(PhotonNetwork.CurrentRoom.PlayerCount==2)
//        {
//            PV.RPC("EnterGame", RpcTarget.MasterClient);
//        }
//    }
//    public override void OnJoinRoomFailed(short returnCode, string message)
//    {
//        StartCoroutine(InfoTextShake2());
//        Debug.Log("방 참가 실패: " +message);
//    }

//    public override void OnLeftRoom()
//    {
//        Debug.Log("방 나가기");
//        PhotonNetwork.JoinLobby(); //로비로 다시 들어감
//    }

//    #endregion

//    #region Public Methods

//    public void GoLobby() //로비 들어가기
//    {
//        lobby_panel.SetActive(true);
//        friendMatch_panel.SetActive(false);
//        friendMatchJoin_panel.SetActive(false);
//        friendMatchCreateRoom_panel.SetActive(false);

//        PhotonNetwork.JoinLobby();
//    }
//    public void LeaveLobby()  //로비 떠나기
//    {
//        lobby_panel.SetActive(false);
//        friendMatch_panel.SetActive(false);
//        friendMatchJoin_panel.SetActive(false);
//        friendMatchCreateRoom_panel.SetActive(false);

//        PhotonNetwork.LeaveLobby();
//    }

//    public void GoFriendMatch()  //친구와 플레이 들어가기
//    {
//        lobby_panel.SetActive(false);
//        friendMatch_panel.SetActive(true);
//        friendMatchJoin_panel.SetActive(false);
//        friendMatchCreateRoom_panel.SetActive(false);
//    }


//    public void GoFriendMatchJoin()  //친구와 플레이 참여하기 들어가기
//    {
//        lobby_panel.SetActive(false);
//        friendMatch_panel.SetActive(false);
//        friendMatchJoin_panel.SetActive(true);
//        friendMatchCreateRoom_panel.SetActive(false);

//        Debug.Log("현재 방 수 : "+ PhotonNetwork.CountOfRooms);
//    }


//    public void GoFriendMatchCreateRoom() //친구와 플레이 방 만들기 들어가기
//    {
//        lobby_panel.SetActive(false);
//        friendMatch_panel.SetActive(false);
//        friendMatchJoin_panel.SetActive(false);
//        friendMatchCreateRoom_panel.SetActive(true);
//    }

//    public void LeaveRoom()
//    {
//        PhotonNetwork.LeaveRoom();  //master server로 돌아감
//    }

//    public void FriendMatchCreateRoom()
//    {
//        int a = Random.Range(0, 255);
//        _roomNumber = a.ToString();
        

//        title_text.text = "대기중..";
//        sendCode_inputfield.text = _roomNumber;

//        Debug.Log("방 코드번호 : "+ _roomNumber);
//        PhotonNetwork.CreateRoom(_roomNumber, new RoomOptions { MaxPlayers = 2 });
//    }

//    public void FriendMatchJoinGameStart()
//    {
//        if (receiveCode_inputfield.text.Equals(""))
//             StartCoroutine(InfoTextShake());  
//        else
//            PhotonNetwork.JoinRoom(receiveCode_inputfield.text, null);
//    }
//    [PunRPC]
//    public void EnterGame()
//    {
//        PhotonNetwork.LoadLevel("Game");
//    }

//    IEnumerator InfoTextShake()
//    {
//        for (int i = 0; i < 3; i++)
//        {
//            receiveCodeInfo_text.text = "";
//            yield return new WaitForSeconds(.3f);
//            receiveCodeInfo_text.text = "코드를 입력하세요!";
//            yield return new WaitForSeconds(.3f);
//        }
//    }
//    IEnumerator InfoTextShake2()
//    {
//        for (int i = 0; i < 3; i++)
//        {
//            receiveCodeInfo_text.text = "";
//            yield return new WaitForSeconds(.3f);
//            receiveCodeInfo_text.text = "코드를 정확하게 입력하세요!";
//            yield return new WaitForSeconds(.3f);
//        }
//    }
//    #endregion
//}
