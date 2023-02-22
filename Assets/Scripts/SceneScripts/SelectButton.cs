using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public enum ButtonType
{
    Note = 0,
    Accidental = 1,
    Octave = 2,
    Function = 3,
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

    void inputNote() 
    {
        // 0 - 2 input type button
        if ((int)buttonType <= 2)
        {
            keyboard.ChangeNote(buttonType, buttonText.text);
        }
        // function type button
        else
        {
            switch (buttonText.text)
            {
                case "+":
                    keyboard.AddNote();
                    break;
                case "-":
                    keyboard.DeleteNote();
                    break;
                case ".":
                    keyboard.ClearCurrentNote();
                    break;
                case "create":
                    keyboard.CreateNoteObject();
                    break;
            }
        }
    }
}
