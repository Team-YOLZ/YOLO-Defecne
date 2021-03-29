using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScene : BaseScene
{
    [SerializeField]
    private GameObject _inventroyCanvas=null;
    [SerializeField]
    private GameObject _mainCanvas=null;
    [SerializeField]
    private GameObject _emptyCanvas=null;
    [SerializeField]
    private GameObject _lobbyPanel = null;

   Vector3 _main;
    Vector3 _inventory;
    Vector3 _empty;
    public override void Clear()
    {
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Main;
    }
    void Start()
    {
        Init();
        //두번 클릭될 경우 방지.
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        //초기위치값 설정 (핸드폰 해상도 별로 UI 픽셀을 고정시키기 위해)
        _main = _mainCanvas.transform.position;
        _inventory = _inventroyCanvas.transform.position;
        _empty = _emptyCanvas.transform.position;

        //로비 비활성
        _lobbyPanel.SetActive(false);
    }
    public void GoLobby()
    {
        //Managers.Scene.LoadScene(Define.Scene.Lobby);
        //로비씬 x -> 로비UI
        _lobbyPanel.SetActive(true);
        
    }

    public void GoInventory()
    {
        iTween.MoveTo(_inventroyCanvas, iTween.Hash("position", _main,
            "time", 0.5f, "easeType", iTween.EaseType.linear));
        iTween.MoveTo(_mainCanvas, iTween.Hash("position", _empty,
            "time", 0.5f, "easeType", iTween.EaseType.linear));
    }
    public void GoMain()
    {
        iTween.MoveTo(_inventroyCanvas, iTween.Hash("position", _inventory,
            "time", 0.5f, "easeType", iTween.EaseType.linear));
        iTween.MoveTo(_mainCanvas, iTween.Hash("position", _main,
            "time", 0.5f, "easeType", iTween.EaseType.linear));
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100f, Color.red, 1.0f);


    }
}