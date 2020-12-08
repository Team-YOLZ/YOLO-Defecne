using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool _isBuildTower { set; get; }

    private void Awake()
    {
        _isBuildTower = false;
    }
}
