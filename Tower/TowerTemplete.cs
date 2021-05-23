using UnityEngine;
using Photon.Pun;


[CreateAssetMenu]
public class TowerTemplete : ScriptableObject
{
    public GameObject towerPrefab;
    public GameObject Projecttile;
    public Weapon[] weapon;

    [System.Serializable]
    public struct Weapon
    {
        public Sprite sprite; // 보여지는 sprite 이미지
        public float damage;  // 공격력
        public float rate;    // 공격속도
        public float range;   // 사거리
        public float cost;    // 비용
        public float slow;    // 감속률
    }
}
