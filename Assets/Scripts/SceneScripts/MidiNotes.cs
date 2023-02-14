using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

public class MidiNotes : MonoBehaviour
{
    public MidiStreamPlayer midiStreamPlayer;
    public List<int> notes = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        midiStreamPlayer = FindObjectOfType<MidiStreamPlayer>();
    }

    public void PlayNotes()
    {
        foreach (int note in notes)
        {
            var mptkEvent = new MPTKEvent()
            {
                Channel = 0, // Between 0 and 15
                Duration = -1, // Infinite
                Value = note, // Between 0 and 127, with 60 plays a C4
                Velocity = 100, // Max 127
            };
            midiStreamPlayer.MPTK_PlayEvent(mptkEvent);
        }
    }
}
