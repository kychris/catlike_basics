using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using System;
using MidiPlayerTK;

public class Keyboard : MonoBehaviour
{
    public TMP_Text textField;
    public TextAsset midiConversionText;
    Dictionary<string, int> noteToMidi;
    SoundManager soundManager;

    //Note, Accidental, Octave (C#4)
    string[] components = new string[] { "", "", "" };

    private void Awake()
    {
        string json = midiConversionText.text;
        noteToMidi = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void ChangeNote(ButtonType type, string c)
    {
        // Remove accidentals if seeing _
        if (c == "_")
        {
            components[1] = "";
        }
        else
        {
            components[(int)type] = c;
        }
        updateText();
    }

    void updateText()
    {
        var newNote = string.Join("", components);
        textField.text = newNote;

        if (noteToMidi.Keys.Contains(newNote))
        {
            soundManager.PlayNote(noteToMidi[newNote]);
        }
    }
}