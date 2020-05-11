using Entities;
using Managers;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBody : MonoBehaviour {

    [SerializeField, AutoProperty]
    private Rigidbody2D rb;

    private EnemyProperties enemyProperties;

    internal void SetProperties(EnemyProperties properties) {
        enemyProperties = properties;
    }

    private void FixedUpdate() {
        if (GameManager.Instance.GameOver) { return; }

        MoveTowardsTarget();
    }

    private void MoveTowardsTarget() {
        Vector2 direction = ((Vector2) enemyProperties.EnemyTarget.position - rb.position).normalized;

        float rotationAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotationAmount * enemyProperties.EnemyRotateSpeed;
        rb.velocity = transform.up * enemyProperties.EnemyMoveSpeed;
    }

    internal void Enable(bool enable) {
        gameObject.SetActive(enable);
    }
}
