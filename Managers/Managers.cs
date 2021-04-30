using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance; //유일성이 보장된다.
    static Managers Instance { get { Init(); return s_instance; } }

    //DataManager _data = new DataManager();
    //public static DataManager Data { get { return Instance._data; } }

    InputManager _input = new InputManager();
    public static InputManager Input { get { return Instance._input; } }

    ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource { get { return Instance._resource; } }

    //UIManager _ui = new UIManager();
    //public static UIManager UI { get { return Instance._ui; } }

    SceneManagerEx _scene = new SceneManagerEx();
    public static SceneManagerEx Scene { get { return Instance._scene; } }

    //SoundManager _sound = new SoundManager();
    //public static SoundManager Sound { get { return Instance._sound; } }

    //PoolManager _pool = new PoolManager();
    //public static PoolManager Pool { get { return Instance._pool; } }

    PhotonMgs _photon = new PhotonMgs();
    public static PhotonMgs Photon { get { return Instance._photon; } }

    //LobbyManager _lobbyManager = new LobbyManager();
    //public static LobbyManager Lobby { get { return Instance._lobbyManager; } }

    void Start()
    {
        Init();
        Screen.SetResolution(900, 1600, false); //화면해상도 고정.
    }

    // Update is called once per frame
    void Update()
    {
        _input.OnUpdate();
    }
    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
            }
            go.AddComponent<Managers>();
            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();
        }
    }

    public static void Clear() //날려줘야할 것들
    {
    }

}

