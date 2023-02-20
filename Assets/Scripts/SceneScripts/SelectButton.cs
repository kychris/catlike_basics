using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;

public enum ButtonType
{
    Note = 0,
    Octave = 1,
    Accidental = 2
}

public class SelectButton : MonoBehaviour
{
    public ButtonType buttonType;
    Keyboard keyboard;
    TextMeshProUGUI buttonText;

    

    private void Start()
    {
        keyboard = GetComponentInParent<Keyboard>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        NameToButtonText();
        GetComponentInChildren<ButtonVR>().onRelease.AddListener(testFunc);
    }

    public void NameToButtonText()
    {
        buttonText.text = gameObject.name;
    }

    void testFunc()
    {
        Debug.Log("hello");
        keyboard.ChangeNote(buttonType, buttonText.text);
    }
}
