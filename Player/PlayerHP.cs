using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP = 20;
    private float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    [SerializeField]
    private GameResult gameResult;

    [SerializeField]
    PhotonView PV;

    private bool gameVictory = true;
    private void Awake()
    {
        currentHP = maxHP;
    }

    private void Start()
    {
        gameResult = GameObject.Find("GameManager").GetComponent<GameResult>();
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        
        //체력 0 게임오버.
        if (PV.IsMine)
        {   
            if (currentHP <= 0)
            {
                gameVictory = false;
                PV.RPC("WinGameResult", RpcTarget.AllBuffered); 
            }
        }
    }

    [PunRPC]
    void WinGameResult()  //게임 오버 시 게임 결과 창 나오게 
    {
        gameResult.Result(gameVictory);
    }

}
