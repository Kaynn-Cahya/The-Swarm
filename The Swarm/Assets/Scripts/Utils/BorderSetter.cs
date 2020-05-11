﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MyBox;

namespace Border {
    public class BorderSetter : MonoBehaviour {
        [MustBeAssigned, SerializeField, Tooltip("The top border")]
        private GameObject topBorder;

        [MustBeAssigned, SerializeField, Tooltip("The bottom border")]
        private GameObject bottomBorder;

        [MustBeAssigned, SerializeField, Tooltip("The left border")]
        private GameObject leftBorder;

        [MustBeAssigned, SerializeField, Tooltip("The right border")]
        private GameObject rightBorder;

        [SerializeField, Tooltip("Offset for the heigth and width of the border respectively.")]
        private float xOffset, yOffset;

        // Use this for initialization
        void Start() {
            SetBorderPos();
        }

        private void SetBorderPos() {
            Camera mainCamera = Camera.main;

            // Get the top middle position of the camera.
            Vector2 temp = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height));
            // Find the offset (border size) of the border.
            var borderOffset = topBorder.GetComponent<BoxCollider2D>().bounds.size.y;
            // Move the top border to the top-middle position of the camera, adjusting with the offset.
            topBorder.transform.position = (temp + new Vector2(0, borderOffset / 2f + yOffset));

            // Bottom
            temp = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width / 2f, 0));
            borderOffset = bottomBorder.GetComponent<BoxCollider2D>().bounds.size.y;
            bottomBorder.transform.position = (temp - new Vector2(0, borderOffset / 2f + yOffset));

            //Left
            temp = mainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height / 2f));
            borderOffset = leftBorder.GetComponent<BoxCollider2D>().bounds.size.x;
            leftBorder.transform.position = (temp - new Vector2(borderOffset / 2f + xOffset, 0));

            temp = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height / 2f));
            borderOffset = rightBorder.GetComponent<BoxCollider2D>().bounds.size.x;
            rightBorder.transform.position = (temp + new Vector2(borderOffset / 2f + xOffset, 0));
        }
    }
}