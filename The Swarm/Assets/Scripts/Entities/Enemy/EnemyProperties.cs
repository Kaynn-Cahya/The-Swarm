using MyBox;
using UnityEngine;

[System.Serializable]
public class EnemyProperties {
    [SerializeField, Tooltip("Movespeed of the enemies"), PositiveValueOnly]
    private float enemyMoveSpeed;

    [SerializeField, Tooltip("How fast the enemies rotate to face the player"), PositiveValueOnly]
    private float enemyRotateSpeed;

    public float EnemyMoveSpeed { get => enemyMoveSpeed; set => enemyMoveSpeed = value; }
    public float EnemyRotateSpeed { get => enemyRotateSpeed; set => enemyRotateSpeed = value; }
    public Transform EnemyTarget { get; set; }
}
