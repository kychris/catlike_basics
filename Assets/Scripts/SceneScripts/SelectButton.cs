using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;

public enum ButtonType
{
    Note = 0,
    Accidental = 1,
    Octave = 2,
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
        GetComponentInChildren<ButtonVR>().onRelease.AddListener(inputNote);
    }

    public void NameToButtonText() { buttonText.text = gameObject.name; }

    void inputNote() { keyboard.ChangeNote(buttonType, buttonText.text); }
}
