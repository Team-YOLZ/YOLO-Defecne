using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private bool IsVictory; //승리했는가 패배했는가?
    private int MyTrophy;
    private int MyClass;
    private int AddTrophy; //기본 더해지는 트로프 ..추후에) 버프트로피를 사면 더 얻을수 있게..
    private NetworkManager _networkManager;

    public void Win()
    {
        IsVictory = true;
        Init();
    }
    public void Lose()
    {
        IsVictory = false;
        Init();
    }

    void Init()
    {
        gameManager.Pause(); //게임 일시 정지
        gameManager.MydacksForResultWindow(); //나의 덱들 화면에 보여주기 

        _networkManager = GameObject.Find("@Managers").GetComponent<NetworkManager>();  //FindObjectOfType 사용시 너무 느리니...
        MyTrophy = _networkManager.trophy;
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
            MyNickname_Text.text = _networkManager.GetUserNickname();

            AddTrophy= Random.Range(30, 40); //30~40 트로피 랜덤으로 주기
            StartCoroutine("AddMyTrophy");  //현재 트로피 갯수에서 더해진 트로피 만큼 짜르륵 올라감
            
            AddTrophy_Text.text = $"+ {AddTrophy}";
            TotalTrophy_Text.text = $"+ {AddTrophy}";

            _networkManager.GetAndUpdateUserScore(AddTrophy, IsVictory); //랭킹 갱신 
        }
        else //패배 시 
        {
            Result_Image.sprite = Resources.Load<Sprite>("GameResultUI/lose");
            MyRank_Image.sprite = Resources.Load<Sprite>($"Rank/rank_{MyClass}");
            MyNickname_Text.text = _networkManager.GetUserNickname();

            AddTrophy = Random.Range(20, 30); //-30~ -20 트로피 랜덤으로 주기
            StartCoroutine(SubMyTrophy());  //현재 트로피 갯수에서 더해진 트로피 만큼 짜르륵 올라감

            AddTrophy_Text.text = $"- {AddTrophy}";
            TotalTrophy_Text.text = $"- {AddTrophy}";

            _networkManager.GetAndUpdateUserScore(AddTrophy, IsVictory); //랭킹 갱신 
        }
    }

    IEnumerator AddMyTrophy()  //이건 왜 안돼 ㅡ.ㅡ;;
    {
        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < AddTrophy + 1; i++)
        {
            int s = MyTrophy + i;
            MyTrophy_Text.text = s.ToString();
            yield return new WaitForSeconds(0.07f);
        }
    }

    IEnumerator SubMyTrophy()
    {
        yield return new WaitForSeconds(0.4f);

        for (int i = 0; i < AddTrophy + 1; i++)
        {
            int s = MyTrophy - i;
            MyTrophy_Text.text = s.ToString();
            yield return new WaitForSeconds(0.08f);
        }
    }

    public void GameToMain()  //메인화면으로 이동 
    {
        gameresult_Canvas.SetActive(false);
        Managers.Photon.OnDisconnect();
        //Managers.Scene.LoadScene(Define.Scene.Main);
        Managers.Photon.LoadLevelMain();  //마스터가 나가야 나갈 수 있나...?
        //클라이언트는 왜 다시 게임화면으로 넘어가 지는데 ...... 방장은 잘 나갔자나...?
        //아니면 로비매니저에 있는 etc변수들 static으로 선언해서 살아남도록????

    }
}
