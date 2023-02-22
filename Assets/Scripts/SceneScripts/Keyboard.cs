using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using Newtonsoft;
using Newtonsoft.Json;
using System;
using MidiPlayerTK;
using System.ComponentModel;
using Unity.VisualScripting;

public class Keyboard : MonoBehaviour
{
    public TMP_Text textField;
    public TMP_Text allSelectionText;
    public TextAsset midiConversionText;
    Dictionary<string, int> noteToMidi;
    SoundManager soundManager;

    //Note, Accidental, Octave (C#4)
    string[] components = new string[] { "", "", "" };
    List<string> notes = new List<string>();

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

    public void AddNote()
    {
        if (components[0] != "" && components[2] != "")
        {
            notes.Add(string.Join("", components));
        }
        UpdateAllSelectionText();
    }

    public void DeleteNote()
    {
        if (notes.Count > 0)
        {
            notes.RemoveAt(notes.Count - 1);
        }
        UpdateAllSelectionText();
    }

    public void ClearCurrentNote()
    {
        components = new string[] { "", "", "" };
        updateText();
    }

    public void UpdateAllSelectionText() => allSelectionText.text = notes.ToCommaSeparatedString();

    public void CreateNoteObject()
    {

    }
}