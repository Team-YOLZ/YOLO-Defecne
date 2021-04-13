using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _movespeed=0.0f;
    [SerializeField]
    private Vector3 _movedirection = Vector3.zero;

    private float baseMoveSpeed;
    //public float Movespeed => _movespeed; //movespeed 변수의 프로포티 (Get기능)

    public float Movespeed
    {
        set => _movespeed = Mathf.Max(0, value); //이동속도 음수 되지않게 설정.
        get => _movespeed;
    }
    private void Awake()
    {
        baseMoveSpeed = _movespeed;
    }

    private void Update()
    {
        transform.position += _movedirection * _movespeed * Time.deltaTime;
    }
    public void MoveTo(Vector3 direction)
    {
        _movedirection = direction;
    }
    public void ResetMoveSpeed()
    {
        _movespeed = baseMoveSpeed;
    }
}
