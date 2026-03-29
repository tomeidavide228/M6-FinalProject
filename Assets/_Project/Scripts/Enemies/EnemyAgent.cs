using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol,
    Chase
}

public class EnemyAgent : MonoBehaviour
{
    [Header("Enemy States Settings")]
    [SerializeField] private WaypointManager _waypointsManager;
    [SerializeField] private EnemyState _defaultState;
    [SerializeField] private int pathIndex;

    [Header("Patrol Settings")]

    [SerializeField] private float _waitTime = 3f;


    [SerializeField] private float _loseSightTime = 1f;

    [Header("Chase Settings")]

    [SerializeField] private float _chaseInterval;

    private EnemyState _currentState;
    private NavMeshAgent _agent;
    private PlayerDetection _sight;
    private WaypointPaths _waypoints;
    private int currentIndex = 0;
    bool _isWaiting;
    private Coroutine _waitCoroutine;
    private float _lastChase;
    private float _lastTimeSeenPlayer=-10f;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _sight = GetComponent<PlayerDetection>();
    }

    private void Start()
    {
        _currentState = _defaultState;
        _waypoints = _waypointsManager.GetWaypoints(pathIndex);
        _agent.SetDestination(_waypoints.Waypoints[currentIndex].position);

    }

    private void Update()
    {
        if (_sight.CanSeePlayer())
        {
            _lastTimeSeenPlayer = Time.time;
        }

        EnemyState newState;

        if (Time.time - _lastTimeSeenPlayer < _loseSightTime)
        {
            newState = EnemyState.Chase;
        }
        else
        {
            newState = _defaultState;
        }
      
        if (newState != _currentState)
        {
            ChangeState(newState);
        }
        switch (_currentState)
        {
            case EnemyState.Patrol:
                Patrolling();
                break;
            case EnemyState.Chase:
                Chasing();
                break;
        }
    }

    private void Patrolling()
    {
        if (_isWaiting)
        {
            return;
        }
        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            _waitCoroutine = StartCoroutine(WaitInterval());
        }
    }

    private void Chasing()
    {
        if (Time.time - _lastChase > _chaseInterval)
        {
            _agent.SetDestination(_sight.Player.position);
            _lastChase = Time.time;
        }
    }


    IEnumerator WaitInterval()
    {
        _isWaiting = true;
        _agent.isStopped = true;
        yield return new WaitForSeconds(_waitTime);

        currentIndex = (currentIndex + 1) % _waypoints.Waypoints.Length;
        _agent.SetDestination(_waypoints.Waypoints[currentIndex].position);

        _agent.isStopped = false;
        _isWaiting = false;
        _waitCoroutine = null;
    }

    private void ChangeState(EnemyState newState)
    {
        _currentState = newState;
        OnEnterState(newState);
    }
    private void OnEnterState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Patrol:
                _agent.isStopped = false;
                _agent.updateRotation = true;
                break;
            case EnemyState.Chase:
                SetupChase();
                break;
        }
    }

    private void SetupChase()
    {
        _agent.updateRotation = true;
        _agent.isStopped = false;

        if (_waitCoroutine != null)
        {
            StopCoroutine(_waitCoroutine);
            _waitCoroutine = null;
            _isWaiting = false;
        }

        _lastChase = 0f;
    }

}