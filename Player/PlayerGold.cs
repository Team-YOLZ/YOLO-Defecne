using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    private float currentGold = 100;

    public float CurrentGold
    {
        set => currentGold = Mathf.Max(0, value);

        get => currentGold;
    }
}
