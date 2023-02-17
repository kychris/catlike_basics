using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerVelocity : MonoBehaviour
{
    // Start is called before the first frame update
    public InputActionReference referencedController;
    public Vector3 velocity;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity = referencedController.action.ReadValue<Vector3>();
        //Debug.Log(velocity);
    }

}
