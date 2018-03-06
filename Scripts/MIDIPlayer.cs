﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CSharpSynth.Effects;
using CSharpSynth.Sequencer;
using CSharpSynth.Synthesis;
using CSharpSynth.Midi;

[RequireComponent(typeof(AudioSource))]
public class MIDIPlayer : MonoBehaviour
{
    //Public
    //Check the Midi's file folder for different songs
    public string midiFilePath = "Midis/Groove.mid";
    public bool ShouldPlayFile = true;

    //Try also: "FM Bank/fm" or "Analog Bank/analog" for some different sounds
    public string bankFilePath = "GM Bank/gm";
    public int bufferSize = 1024;
    public int midiNote = 60;
    public int midiNoteVolume = 100;
    [Range(0, 127)] //From Piano to Gunshot
    public int midiInstrument = 0;
    public bool SyncTrack;
    public bool Active = false;
    //Private 
    private float[] sampleBuffer;
    private float gain = 1f;
    private MidiSequencer midiSequencer;
    private StreamSynthesizer midiStreamSynthesizer;

    private Queue<uint> notes = new Queue<uint>();

    private bool state = false;
    private int violinNote = 0;
    private int violinString = 0;
    private string noteString = "";
    private readonly string[] _violinFirstPosNotes =
    {
        "G4", "G#4", "A4", "A#4", "B4", "C5", "C#5",    // Each string without the last note that is the same as
        "D5", "D#5", "E5", "F5", "F#5", "G5", "G#5",    // The starting note on the next note
        "A5", "A#5", "B5", "C6", "C#6", "D6", "D#6",
        "E6", "F6", "F#6", "G6", "G#6", "A6", "A#6", "B7"
    };


    private float sliderValue = 1.0f;
    private float maxSliderValue = 127.0f;

    // Awake is called when the script instance
    // is being loaded.
    void Awake()
    {

            
            midiStreamSynthesizer = new StreamSynthesizer(44100, 2, bufferSize, 40);
            sampleBuffer = new float[midiStreamSynthesizer.BufferSize];

            midiStreamSynthesizer.LoadBank(bankFilePath);
        midiSequencer = new MidiSequencer(midiStreamSynthesizer);

    }

    void LoadSong(string midiPath)
    {
        midiSequencer.LoadMidi(midiPath, false);
        midiSequencer.Play();
    }

    public void SyncStart()
    {
        Active = true;

        //These will be fired by the midiSequencer when a song plays. Check the console for messages if you uncomment these
        midiSequencer.NoteOnEvent += new MidiSequencer.NoteOnEventHandler(MidiNoteOnHandler);
        //midiSequencer.NoteOffEvent += new MidiSequencer.NoteOffEventHandler (MidiNoteOffHandler);	
    }


    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        if (!SyncTrack)
        {
            Active = true;
            //These will be fired by the midiSequencer when a song plays. Check the console for messages if you uncomment these
            midiSequencer.NoteOnEvent += new MidiSequencer.NoteOnEventHandler(MidiNoteOnHandler);
            //midiSequencer.NoteOffEvent += new MidiSequencer.NoteOffEventHandler (MidiNoteOffHandler);
        }
        if (!SyncTrack)
            GameObject.Find("NoteSpawner").transform.GetComponent<NoteSpawner2>().syncNote();
    }
    // Start is called just before any of the
    // Update methods is called the first time.
    void Start()
    {

        if(SyncTrack == false)
        notes = midiSequencer.NoteLengths;
        StartCoroutine(Wait());

        
    }

    // Update is called every frame, if the
    // MonoBehaviour is enabled.
    void Update()
    {

        if (Active == true)
        {
            if (state == true && SyncTrack == false)
            {
                uint len = notes.Dequeue();
                notes.Enqueue(len);
                Debug.Log(len);
                GameObject.Find("NoteSpawner").transform.GetComponent<NoteSpawner2>().spawnNote(violinString, violinNote, len);
                state = false;
            }

            if (!midiSequencer.isPlaying)
            {
                //if (!GetComponent<AudioSource>().isPlaying)
                if (ShouldPlayFile)
                {
                    LoadSong(midiFilePath);
                }
            }
            else if (!ShouldPlayFile)
            {
                midiSequencer.Stop(true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                midiStreamSynthesizer.NoteOn(0, midiNote, midiNoteVolume, midiInstrument);
            }

            if (Input.GetMouseButtonDown(1))
            {
                midiStreamSynthesizer.NoteOff(0, midiNote);
            }
        }

        }

        // See http://unity3d.com/support/documentation/ScriptReference/MonoBehaviour.OnAudioFilterRead.html for reference code
        //	If OnAudioFilterRead is implemented, Unity will insert a custom filter into the audio DSP chain.
        //
        //	The filter is inserted in the same order as the MonoBehaviour script is shown in the inspector. 	
        //	OnAudioFilterRead is called everytime a chunk of audio is routed thru the filter (this happens frequently, every ~20ms depending on the samplerate and platform). 
        //	The audio data is an array of floats ranging from [-1.0f;1.0f] and contains audio from the previous filter in the chain or the AudioClip on the AudioSource. 
        //	If this is the first filter in the chain and a clip isn't attached to the audio source this filter will be 'played'. 
        //	That way you can use the filter as the audio clip, procedurally generating audio.
        //
        //	If OnAudioFilterRead is implemented a VU meter will show up in the inspector showing the outgoing samples level. 
        //	The process time of the filter is also measured and the spent milliseconds will show up next to the VU Meter 
        //	(it turns red if the filter is taking up too much time, so the mixer will starv audio data). 
        //	Also note, that OnAudioFilterRead is called on a different thread from the main thread (namely the audio thread) 
        //	so calling into many Unity functions from this function is not allowed ( a warning will show up ). 	
    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (Active == true)
        {
            //This uses the Unity specific float method we added to get the buffer
            midiStreamSynthesizer.GetNext(sampleBuffer);
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = sampleBuffer[i] * gain;
            }
        }
    }

    public void MidiNoteOnHandler(int channel, int note, int velocity)
    {
      //  Debug.Log("NoteOn: " + note.ToString() + " Velocity: " + velocity.ToString());
      
        int counter = 4;
        int newNote = (note % 55) % 28;
        noteString = _violinFirstPosNotes[newNote];

            do
            {
                newNote = newNote - 7;
                counter--;

            } while (newNote >= 0);

            violinNote = (note % 55) % 7;

            violinString = counter;

        if (note % 55 == 28)
        {
            violinString = 0;
            violinNote = 7;
        }

        state = true;

    }

    public void MidiNoteOffHandler(int channel, int note)
    {
       // Debug.Log("NoteOff: " + note.ToString());
    }
}
