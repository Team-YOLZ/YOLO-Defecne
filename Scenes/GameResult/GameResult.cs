using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameResult : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject gameresult_Canvas;

    [SerializeField]
    private Image Result_Image; // Win or Lose
    [SerializeField]
    private Image MyRank_Image;  // My Rank Image
    [SerializeField]
    private Text MyNickname_Text; //My Nickname
    [SerializeField]
    private Text MyTrophy_Text; // Current Trophy + GameResult Trophy
    [SerializeField]
    private Text AddTrophy_Text; // GameResult Add Trophy 1  ... To do) Using buff Item( Add Trophy 2)
    [SerializeField]
    private Text TotalTrophy_Text; // GameResult Total Trophy
    //[SerializeField]
    //private PhotonView PV;

    private bool IsVictory; //승리했는가 패배했는가?
    private int MyTrophy;
    private int MyClass;
    private int AddTrophy; //기본 더해지는 트로프 ..추후에) 버프트로피를 사면 더 얻을수 있게..
    private NetworkManager _networkManager;

    public void Result(bool isVictory)
    {
        IsVictory = isVictory;
        Init();
    }

    //public void Win()
    //{
    //    IsVictory = true;
    //    Init();
    //}
    //public void Lose()
    //{
    //    IsVictory = false;
    //    Init();
    //}

    void Init()
    {
        gameManager.Pause(); //게임 일시 정지
        gameManager.MydacksForResultWindow(); //나의 덱들 화면에 보여주기 

        _networkManager = GameObject.Find("@Managers").GetComponent<NetworkManager>();

        Hashtable cp = PhotonNetwork.LocalPlayer.CustomProperties;
        MyTrophy = (int)cp["MyTrophy"];
        MyClass = MyTrophy / 200;

        ResultScene();
    }

    void ResultScene()
    {
        gameresult_Canvas.SetActive(true);

        if (IsVictory) //승리 시 
        {
            Result_Image.sprite = Resources.Load<Sprite>("GameResultUI/win");
            MyRank_Image.sprite = Resources.Load<Sprite>($"Rank/rank_{MyClass}");
            MyNickname_Text.text = PhotonNetwork.LocalPlayer.NickName;

            AddTrophy = Random.Range(30, 40); //30~40 트로피 랜덤으로 주기
            StartCoroutine(AddMyTrophy());  //현재 트로피 갯수에서 더해진 트로피 만큼 짜르륵 올라감

            AddTrophy_Text.text = $"+ {AddTrophy}";
            TotalTrophy_Text.text = $"+ {AddTrophy}";

            _networkManager.GetAndUpdateUserScore(AddTrophy, IsVictory); //랭킹 갱신 
        }
        else //패배 시 
        {
            Result_Image.sprite = Resources.Load<Sprite>("GameResultUI/lose");
            MyRank_Image.sprite = Resources.Load<Sprite>($"Rank/rank_{MyClass}");
            MyNickname_Text.text = PhotonNetwork.LocalPlayer.NickName;

            AddTrophy = Random.Range(20, 30); //-30~ -20 트로피 랜덤으로 주기
            StartCoroutine(SubMyTrophy());  //현재 트로피 갯수에서 더해진 트로피 만큼 짜르륵 올라감

            AddTrophy_Text.text = $"- {AddTrophy}";
            TotalTrophy_Text.text = $"- {AddTrophy}";

            _networkManager.GetAndUpdateUserScore(AddTrophy, IsVictory); //랭킹 갱신 
        }

        //Invoke("GameToMain", 5); //5초 후에 메인씬으로 이동
        StartCoroutine(WaitResult()); //5초 후에 Rpc함수 실행
    }

    IEnumerator AddMyTrophy()  //이건 타임스케일로 멈춰서 코루틴문이 안돌아감..!
    {
        yield return new WaitForSecondsRealtime(0.4f);

        for (int i = 0; i < AddTrophy + 1; i++)
        {
            int s = MyTrophy + i;
            MyTrophy_Text.text = s.ToString();
            yield return new WaitForSecondsRealtime(0.07f);
        }
    }

    IEnumerator SubMyTrophy()
    {
        yield return new WaitForSecondsRealtime(0.4f);

        for (int i = 0; i < AddTrophy + 1; i++)
        {
            int s = MyTrophy - i;
            MyTrophy_Text.text = s.ToString();
            yield return new WaitForSecondsRealtime(0.08f);
        }
    }

    IEnumerator WaitResult()  //5초 후에 
    {
        yield return new WaitForSecondsRealtime(5.0f);
        gameManager.Pause();
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
            Debug.Log("---Main Scene!!----");
        }
    }
}
