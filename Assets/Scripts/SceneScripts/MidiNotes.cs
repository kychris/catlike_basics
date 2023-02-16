using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

using MidiPlayerTK;

public class MidiNotes : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;
    public List<int> notes = new List<int>();
    public InputActionReference TriggerAmount;
    XRGrabInteractable grabInteractable;

    private void Awake() { grabInteractable = GetComponent<XRGrabInteractable>(); }

    void Start()
    {
        midiStreamPlayer = FindObjectOfType<MidiStreamPlayer>();
    }

    void OnEnable() { grabInteractable.hoverEntered.AddListener(PlayNotes); }
    //void OnDisable() { grabInteractable.hoverEntered.RemoveListener(PlayNotes); }

    private void Update()
    {
        //Debug.Log((int)(TriggerAmount.action.ReadValue<float>() * 127));
    }

    public void PlayNotes(HoverEnterEventArgs hoverEnterEventArgs)
    {
        Debug.Log(hoverEnterEventArgs.interactorObject.transform.gameObject.GetComponent<Rigidbody>().velocity.ToString());
        foreach (int note in notes)
        {
            var mptkEvent = new MPTKEvent()
            {
                Channel = 0, // Between 0 and 15
                Duration = -1, // Infinite
                Value = note, // Between 0 and 127, with 60 plays a C4
                Velocity = (int)(TriggerAmount.action.ReadValue<float>() * 127), // Max 127
            };
            midiStreamPlayer.MPTK_PlayEvent(mptkEvent);
        }
    }
}
