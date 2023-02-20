using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public TMP_InputField NotesField;
    public TMP_InputField OctavesField;
    public TMP_InputField AccidentalsField;



    public void ChangeNotes(string c)
    {
        NotesField.text = c;
    }

    public void ChangeOctaves(string c)
    {
        OctavesField.text = c;
    }

    public void ChangeAccidentals(string c)
    {
        AccidentalsField.text = c;
    }
}
