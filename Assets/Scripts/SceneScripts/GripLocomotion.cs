using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GripLocomotion : MonoBehaviour
{
    public LocomotionSystem locomotionSystem;
    public Transform leftControllerTransform;
    public InputActionReference LeftGrip;
    public InputActionReference RightGrip;

    public GameObject xrRig;
    private bool isMoving = false;
    private Vector3 originalControllerLocation;
    private Vector3 originalRigLocation;

    // Start is called before the first frame update
    void Start()
    {
        LeftGrip.action.performed += isPressed;
        RightGrip.action.performed += isPressed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMoving) {
            if (LeftGrip.action.inProgress == false && RightGrip.action.inProgress == false) {
                isMoving = false;
            }
            Vector3 curControllerLocation = leftControllerTransform.position;
            Vector3 diff = new Vector3(
                originalControllerLocation.x - curControllerLocation.x ,
                originalControllerLocation.y - curControllerLocation.y,
                originalControllerLocation.z - curControllerLocation.z
            );

            Debug.Log(diff);

            xrRig.GetComponent<Rigidbody>().velocity = diff*5;

            //xrRig.transform.position = originalRigLocation + diff;
        }
    }

    private void isPressed(InputAction.CallbackContext action) {
        isMoving = true;
        originalControllerLocation = leftControllerTransform.position;
        originalRigLocation = xrRig.transform.position;
    }
}
