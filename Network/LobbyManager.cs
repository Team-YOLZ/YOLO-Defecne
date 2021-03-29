using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable; //CustomProperties를 사용하기 위함

public class LobbyManager : MonoBehaviourPunCallbacks
{

    [Header("Loding")]
    [SerializeField]
    private GameObject Loding_image;
    [SerializeField]
    private Text Loding_text;
    [SerializeField]
    private Image Loding_bar;
    [SerializeField]
    private GameObject GoLobby_btn;
    [SerializeField]
    private GameObject GoInven_btn;
   


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
    public bool ismaster = false;

    #region Private Fields

    private string _roomNumber; //방 코드 번호(sender)
    private NetworkManager _networkManager;

    #endregion


    #region MonoBehaviours CallBacks

    void Awake()
    {
        Managers.Photon.OnAwake();
    }

    void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>(); //네트워크매니저 스크립트로 유저아이디 가지고 올거ㅇ
                                                              // _userId = _networkManager.GetUserNickname(); //유저아이디를 넣기

        friendMatchCreateRoom_btn.SetActive(false); //서버 접속 전 방만들기 막기
        Managers.Photon.OnStart(); //서버 접

        lobby_panel.SetActive(false);
        friendMatch_panel.SetActive(false);
        friendMatchJoin_panel.SetActive(false);
        friendMatchCreateRoom_panel.SetActive(false);

        friendMatchCreateRoom_btn.SetActive(true);
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
        Managers.Photon.LoadLevel();
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
    #endregion

    #region Pun Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("connect To Master");

        PhotonNetwork.LocalPlayer.NickName = _networkManager.GetUserNickname();
        Debug.Log("유저닉네임 : " + PhotonNetwork.LocalPlayer.NickName);
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
        Mydacks = GameObject.FindGameObjectsWithTag("Mydack");
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
}
