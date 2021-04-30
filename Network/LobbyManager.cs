using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable; //CustomProperties를 사용하기 위함

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("BackGround")]
    [SerializeField]
    private GameObject GoLobby_btn;
    [SerializeField]
    private GameObject GoInven_btn;

    [Header("Rank")]
    [SerializeField]
    private Text nickname_text;
    [SerializeField]
    private Image trophy_progressbar;
    [SerializeField]
    private Text trophy_text;
    [SerializeField]
    private Image class_image;
    [SerializeField]
    private Text class_text;
    [SerializeField]
    private int MyTrophy;
    [SerializeField]
    private int MyClass;

    [Header("Lobby")]
    [SerializeField]
    private GameObject lobby_panel;
    [SerializeField]
    private Text connectionInfo_text;

    [Header("Lobby-FriendMatch")]
    [SerializeField]
    private GameObject friendMatch_panel;
    [SerializeField]
    private GameObject friendMatchCreateRoom_btn;
    [SerializeField]
    private GameObject friendMatchJoin_btn;

    [Header("Lobby-FriendMatchJoin")]
    [SerializeField]
    private GameObject friendMatchJoin_panel;
    [SerializeField]
    private Text receiveCodeInfo_text;
    [SerializeField]
    private InputField receiveCode_inputfield;

    [Header("Lobby-FriendMatchCreateRoom")]
    [SerializeField]
    private GameObject friendMatchCreateRoom_panel;
    [SerializeField]
    private Text title_text;
    [SerializeField]
    private InputField sendCode_inputfield;  //readonly

    [Header("ETC.")]
    //public bool isjoin = false;
    public PhotonView PV;
    public GameObject[] Mydacks;
    public Sprite[] Mydacks_Image;
    public string MydacksImage;
    //public bool ismaster = false;

    #region Private Fields

    private string _roomNumber; //방 코드 번호(sender)
    private NetworkManager _networkManager;
    private Loading _loading;
    #endregion


    #region MonoBehaviours CallBacks

    void Awake()
    {
        Managers.Photon.OnAwake();
        Debug.Log("로비 어웨이크");
    }

    void Start()
    {
        Debug.Log("로비 스타트");
        _networkManager = GameObject.Find("@Managers").GetComponent<NetworkManager>();  //FindObjectOfType 사용시 너무 느리니...
        //_networkManager = FindObjectOfType<NetworkManager>(); //네트워크매니저 스크립트로 유저아이디 가지고 올거ㅇ
        _loading = FindObjectOfType<Loading>();

        //if (PhotonNetwork.IsConnected) //이미 연결이 되어있다면
        //{
        //    Debug.Log("이미 연결되어 있었고 다시 재접속한다?");
        //    Managers.Photon.OnDisconnect(); //연결해ㅈㅔ 후 재접속
        //}
        //else
        //{
        Managers.Photon.OnStart(); //서버 접
                                   //}

    }

    #endregion

    #region Public Methods


    public void GoLobby() //로비 들어가기
    {
        Managers.Photon.JoinLobby();

        lobby_panel.SetActive(true);
        friendMatch_panel.SetActive(false);
        friendMatchJoin_panel.SetActive(false);
        friendMatchCreateRoom_panel.SetActive(false);

        connectionInfo_text.text = "Lobby : Connected to Master Server!";
        friendMatchCreateRoom_btn.SetActive(true);
    }
    public void LeaveLobby()  //로비 떠나기
    {
        Managers.Photon.LeaveLobby();

        lobby_panel.SetActive(false);
        friendMatch_panel.SetActive(false);
        friendMatchJoin_panel.SetActive(false);
        friendMatchCreateRoom_panel.SetActive(false);

        friendMatchCreateRoom_btn.SetActive(false);

    }

    public void GoFriendMatch()  //친구와 플레이 들어가기
    {
        lobby_panel.SetActive(false);
        friendMatch_panel.SetActive(true);
        friendMatchJoin_panel.SetActive(false);
        friendMatchCreateRoom_panel.SetActive(false);
    }


    public void GoFriendMatchJoin()  //친구와 플레이 참여하기 들어가기
    {
        lobby_panel.SetActive(false);
        friendMatch_panel.SetActive(false);
        friendMatchJoin_panel.SetActive(true);
        friendMatchCreateRoom_panel.SetActive(false);

        Managers.Photon.ShowLogConutOfRooms();
    }


    public void GoFriendMatchCreateRoom() //친구와 플레이 방 만들기 들어가기
    {
        lobby_panel.SetActive(false);
        friendMatch_panel.SetActive(false);
        friendMatchJoin_panel.SetActive(false);
        friendMatchCreateRoom_panel.SetActive(true);
    }

    public void LeaveRoom()
    {
        Managers.Photon.LeaveRoom();
    }

    public void FriendMatchCreateRoom()
    {
        int a = Random.Range(0, 255);
        _roomNumber = a.ToString();


        title_text.text = "대기중..";
        sendCode_inputfield.text = _roomNumber;

        Debug.Log("방 코드번호 : " + _roomNumber);
        Managers.Photon.CreateRoom(_roomNumber);
    }

    public void FriendMatchJoinGameStart()
    {
        if (receiveCode_inputfield.text.Equals(""))
            StartCoroutine(InfoTextShake());
        else
            Managers.Photon.JoinRoom(receiveCode_inputfield.text);

    }


    [PunRPC]
    public void EnterGame()
    {
        Managers.Photon.LoadLevelGame();
    }

    IEnumerator InfoTextShake()
    {
        for (int i = 0; i < 3; i++)
        {
            receiveCodeInfo_text.text = "";
            yield return new WaitForSeconds(.3f);
            receiveCodeInfo_text.text = "코드를 입력하세요!";
            yield return new WaitForSeconds(.3f);
        }
    }

    IEnumerator InfoTextShake2()
    {
        for (int i = 0; i < 3; i++)
        {
            receiveCodeInfo_text.text = "";
            yield return new WaitForSeconds(.3f);
            receiveCodeInfo_text.text = "코드를 정확하게 입력하세요!";
            yield return new WaitForSeconds(.3f);
        }
    }
    IEnumerator GetMyRank()
    {
        MyTrophy = _networkManager.trophy;
        MyClass = MyTrophy / 200;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "MyTrophy", MyTrophy }, { "MyClass", MyClass } });
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log("MyTrophy : " + cp["MyTrophy"] + ", MyClass : " + cp["MyClass"]);

        yield return MyTrophy;

        RankSystem();
    }

    #endregion

    #region Pun Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("connect To Master");

        PhotonNetwork.LocalPlayer.NickName = _networkManager.GetUserNickname();
        Debug.Log("유저닉네임 : " + PhotonNetwork.LocalPlayer.NickName);

        //내 랭킹 조회
        StartCoroutine("GetMyRank");

        _loading.IsConnectedToMaster(true);

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("disconnect To Master");

        PhotonNetwork.ConnectUsingSettings();  // 재 접속
    }

    public override void OnJoinedLobby() // 로비 입장시
    {
        Debug.Log("joined Lobby");
        MydacksImage = "";
        for (int i = 0; i < Mydacks.Length; i++)
        {
            Mydacks_Image[i] = Mydacks[i].GetComponent<Image>().sprite;
            MydacksImage += Mydacks[i].GetComponent<Image>().sprite.name;
            if (i != Mydacks.Length - 1) MydacksImage += ",";
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "DackList", MydacksImage } });
        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        Debug.Log(cp["DackList"]);
    }

    public override void OnLeftLobby() //로비 떠날ㅅㅣ
    {
        Debug.Log("Leave Lobby");
        //마스터서버로 
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("번호 생성 후, 대기중..");
        Debug.Log("create현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("방 참가 완료");
        Debug.Log("join현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("join현재 방 인원쉐 : " + PhotonNetwork.CurrentRoom.PlayerCount);
        //주사위 정보들 SetCustom프로퍼티 여기서 진행.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PV.RPC("EnterGame", RpcTarget.MasterClient);
            Managers.Photon._enterGame = true;
            Debug.Log("_enterGame은 트루!!");
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        StartCoroutine(InfoTextShake2());
        Debug.Log("방 참가 실패: " + message);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방 나가기");
        PhotonNetwork.JoinLobby(); //로비로 다시 들어감
    }
    #endregion



    #region Rank
    void RankSystem()
    {

        //닉네임 설정
        nickname_text.text = PhotonNetwork.LocalPlayer.NickName;

        //0 200     0
        //200 400   1
        //400 600   2
        //600 800   3
        //800 1000  4
        //1000 1200 5
        //1200 1400 6
        //1400 1600 7
        //1600 1800 8
        //1800 2000 9

        //트로피 숫자 
        trophy_text.text = MyTrophy.ToString();

        //클래스 이미지와 숫자
        class_text.text = $"클래스 {MyClass}";
        class_image.sprite = Resources.Load<Sprite>($"Rank/rank_{MyClass}");

        trophy_progressbar.fillAmount = (MyTrophy % 200.0f) / 200.0f;
    }



    #endregion
}
