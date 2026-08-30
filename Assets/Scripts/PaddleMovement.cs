using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleMovement : MonoBehaviour
{
    public string controlType = "WASD";
    public float speed = 6f;
    public float yBound = 2.5f;

    void Update()
    {
        float verticalInput = 0f;
        Keyboard currentKeyboard = Keyboard.current;

        if (GameManager.Instance.CurrentPhase != GameManager.GamePhase.Playing) return;
        if (currentKeyboard == null) return;


        if (controlType == "WASD")
        {
            if (currentKeyboard.wKey.isPressed) verticalInput = 1f;
            if (currentKeyboard.sKey.isPressed) verticalInput = -1f;
        }
        else if (controlType == "Arrows")
        {
            if (currentKeyboard.upArrowKey.isPressed) verticalInput = 1f;
            if (currentKeyboard.downArrowKey.isPressed) verticalInput = -1f;
        }

        float newY = transform.position.y + (verticalInput * speed * Time.deltaTime);
        newY = Math.Clamp(newY, -yBound, yBound);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
