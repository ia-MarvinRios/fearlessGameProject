using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TargetLockOn))]
public class PlayerAttackController : MonoBehaviour
{

    float _damage = 10f;  // Daño por tic
    public float _damageInterval = 0.5f; // Intervalo de daño
    public EnemyController _enemyController;
    PlayerStats _playerStats;
    public Transform _pointFire;
    Animator _animator;

    TargetLockOn _targetLockOn;

    public UnityEvent _OnAttack; 

    public ParticleSystem ChargeParticles;

    private void Start()
    {
        _playerStats =  GetComponent<PlayerStats>();
        _animator = GetComponent<Animator>();
        _targetLockOn = GetComponent<TargetLockOn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        



    }


    void OnAttack()
    {
        if (!_targetLockOn.isLockedOn) return;
        //_animator.SetTrigger("Attack");

        if(_OnAttack != null) _OnAttack.Invoke();

    }

    private void ApplyDamage()
    {
        if (_enemyController != null)
        {
            if (_playerStats) _damage = _playerStats._fuerza;

           // _enemyController.take_dmg(_damage);
            Debug.Log("Aplicando daño "+_damage+" "+_enemyController.name+" Yo: "+gameObject.name);
        }
        
    }

}
