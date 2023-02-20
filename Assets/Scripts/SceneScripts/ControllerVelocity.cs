using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVelocity : MonoBehaviour
{
    public InputActionReference referencedController;
    public Vector3 velocity;

    void Update()
    {
        velocity = referencedController.action.ReadValue<Vector3>();
    }

}
