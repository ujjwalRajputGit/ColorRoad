using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : BallControllerBase
{
    public enum PlayerState
    {
        Moving,
        TargetReached,
        Stop,
        Died
    }

    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private Rigidbody _rigidbody;
    [SerializeField]
    private List<GameObject> _targets;

    private bool _isGrounded;
    private int _lastTargetIndex = -1;
    private Vector3 _currentTargetPos;
    private float _targetReachedOffset = 2;
    public PlayerState PlayerCurrentState { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        UpdateCurrentTargetPos();
        ChangePlayerState(PlayerState.Moving);
    }

    private void FixedUpdate()
    {

        switch (PlayerCurrentState)
        {
            case PlayerState.Moving:
                MoveBallToTarget();
                break;
            case PlayerState.TargetReached:
                UpdateCurrentTargetPos();
                break;
            case PlayerState.Stop:
                StopMovement();
                break;
            default:
                break;
        }
    }




    private void MoveBallToTarget()
    {
        if (!_isGrounded) return;

        var direction = (_currentTargetPos - transform.position).normalized;
        Vector3 vel = direction * forwardSpeed;
        _rigidbody.velocity = new Vector3(vel.x, _rigidbody.velocity.y, vel.z);

        if (Mathf.Abs(transform.position.x - _currentTargetPos.x) < _targetReachedOffset ||
        Mathf.Abs(transform.position.z - _currentTargetPos.z) < _targetReachedOffset)
            ChangePlayerState(PlayerState.TargetReached);

        // _rigidbody.AddForce(Vector3.right * horizontal * sideSpeed, ForceMode.VelocityChange);
    }

    private void UpdateCurrentTargetPos()
    {
        _lastTargetIndex++;
        if (_lastTargetIndex < _targets.Count)
        {
            _currentTargetPos = _targets[_lastTargetIndex].transform.position;
            ChangePlayerState(PlayerState.Moving);
        }
        else
        {
            ChangePlayerState(PlayerState.Stop);
        }
    }

    private void StopMovement()
    {
        _rigidbody.velocity = _rigidbody.angularVelocity = Vector3.zero;
    }

    private void ChangePlayerState(PlayerState state)
    {
        PlayerCurrentState = state;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Platform")
        {
            _isGrounded = true;
        }

        if (other.collider.tag == "Ball")
        {
            var ball = other.gameObject.GetComponent<BallControllerBase>();
            if (ball._property.Color == _property.Color)
            {
                // increment score
                ball.gameObject.SetActive(false);
            }
            else
            {
                ChangePlayerState(PlayerState.Died);
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.collider.tag == "Platform")
        {
            _isGrounded = false;
        }
    }
}
