using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _movespeed=0.0f;
    [SerializeField]
    private Vector3 _movedirection = Vector3.zero;

    public float Movespeed => _movespeed; //movespeed 변수의 프로포티 (Get기능)

    private void Update()
    {
        transform.position += _movedirection * _movespeed * Time.deltaTime;
    }
    public void MoveTo(Vector3 direction)
    {
        _movedirection = direction;
    }
}
