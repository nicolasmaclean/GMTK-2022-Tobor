using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeAttacker : Enemy
{
    [SerializeField] Transform _player;
    private NavMeshAgent _agent;
    [SerializeField] GameObject _attackCollider;

    private float _timeStamp = 0f;
    private float _timeDelay = 0.2f;

    private float _nextAttack = 0f;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        _attackCollider.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //chase player
        if (Time.time >= _timeStamp + _timeDelay)
        {
            _agent.SetDestination(_player.position);
            _timeStamp = Time.time;
            DetectPlayer();
        }

        //check health
        
        if (_health <= 0)
        {
            EnemyDied();
        }
        

        //check damage dealer
        /*
        float currentTargetDistance = Vector3.Distance(transform.position, _player.position);
        if (currentTargetDistance >= _rangeOfAttack)
        {
            EnemyDamaged(5);
        }
        */
    }

    private void DetectPlayer()
    {
        float currentTargetDistance = Vector3.Distance(transform.position, _player.position);
        if (currentTargetDistance <= _rangeOfAttack)
        {
            
            _agent.isStopped = true;
            if (Time.time > _nextAttack)
            {
                _nextAttack = Time.time + _damageRate;
                EnemyAttack();
            }
        }
        else
        {
            _agent.isStopped = false;
        }
    }

    private void EnemyAttack()
    {
        Collider[] hitPlayers = Physics.OverlapBox(_attackCollider.transform.position, _attackCollider.transform.position);
        foreach(Collider player in hitPlayers)
        {
            Debug.Log("Player attacked");
        }
    }

}
