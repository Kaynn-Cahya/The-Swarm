using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {
    [SerializeField, MustBeAssigned]
    private List<Vector2> patrolPositions;

    [SerializeField, PositiveValueOnly]
    private float patrolSpeed;

    private int currentIndex;

    private Vector2 currentTarget;

    private void Awake() {
        currentIndex = 0;
        currentTarget = patrolPositions[0];
    }

    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, currentTarget, patrolSpeed * Time.deltaTime);

        if (transform.position.Approximately(currentTarget)) {
            NextPath();
        }

        #region Local_Function

        void NextPath() {
            ++currentIndex;

            if (currentIndex >= patrolPositions.Count) {
                currentIndex = 0;
            }

            currentTarget = patrolPositions[currentIndex];
        }

        #endregion
    }
}
