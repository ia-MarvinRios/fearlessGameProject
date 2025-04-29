using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.Events;

public class TargetLockOn : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask enemyLayer;
    public Transform cameraTransform;
    public GameObject lockOnIndicator; // Prefab del indicador
    public float switchSpeed = 5f; // Velocidad de transición del indicador
    public float rotationSpeed = 5f; // Velocidad de giro del jugador

    public List<Transform> enemiesInRange = new List<Transform>();
    public Transform currentTarget;
    public bool isLockedOn = false;
    private Vector3 indicatorVelocity = Vector3.zero; // Para suavizar el movimiento

    public UnityEvent OnLocked;
    public UnityEvent OnRelease;


    private void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        DetectEnemies();
        if (!isLockedOn) HandleTargetSwitching();
        HandleLockOn();
        SmoothIndicatorMovement();
        RotateTowardsTarget();
    }

    void DetectEnemies()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        enemiesInRange = colliders
            .Select(c => c.transform)
            //.Where(e => IsEnemyInFront(e)) // Filtra enemigos solo al frente
            .ToList();
    }

    bool IsEnemyInFront(Transform enemy)
    {
        Vector3 toEnemy = (enemy.position - cameraTransform.transform.position).normalized;
        return Vector3.Dot(transform.forward, toEnemy) > 0.2f; // Solo si está al frente
    }

    void HandleTargetSwitching()
    {
        if (enemiesInRange.Count == 0) return;

        Transform bestTarget = GetClosestEnemy();
        if (bestTarget != currentTarget)
        {

            currentTarget = bestTarget;

        }
    }

    Transform GetClosestEnemy()
    {
        return enemiesInRange
            .OrderBy(e => Vector3.Angle(cameraTransform.forward, e.position - cameraTransform.position))
            .FirstOrDefault();
    }

    void HandleLockOn()
    {
        if (Input.GetMouseButtonDown(1) && !IsTargetOutOfRange()) // Click derecho
        {

            isLockedOn = !isLockedOn;
            lockOnIndicator.GetComponent<Renderer>().material.color = isLockedOn ? Color.red : Color.white;

            if (isLockedOn)
            {
                if (OnLocked != null) OnLocked.Invoke();
            }
            else
            {
                if (OnRelease != null) OnRelease.Invoke();
            }
        }
    }

    void SmoothIndicatorMovement()
    {
        if (currentTarget != null && !IsTargetOutOfRange())
        {

            
            Vector3 targetPosition = currentTarget.position + Vector3.up * 2;
            lockOnIndicator.transform.position = Vector3.SmoothDamp(lockOnIndicator.transform.position, targetPosition, ref indicatorVelocity, 0.2f);
            lockOnIndicator.SetActive(true);
        }
        else
        {

            lockOnIndicator.SetActive(false);
            lockOnIndicator.transform.position = transform.position;
        }
    }


    void RotateTowardsTarget()
    {
        if (isLockedOn && currentTarget != null)
        {
            Vector3 direction = (currentTarget.position - transform.position).normalized;
            direction.y = 0; // Mantener la rotación solo en el eje Y
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }



    bool IsTargetOutOfRange()
    {
        if (currentTarget == null) return true;
        return Vector3.Distance(transform.position, currentTarget.position) > detectionRadius;
    }

}
