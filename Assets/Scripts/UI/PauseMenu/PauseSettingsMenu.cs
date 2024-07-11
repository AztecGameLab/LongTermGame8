using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseSettingsMenu : MonoBehaviour
{
    // root of the UI doc and parents
    // private VisualElement root;
    private VisualElement _dialogueContainer;
    private VisualElement _SFXVolume;
    private VisualElement _MusicVolume;
    private VisualElement _AutosavesContainer;

    // Dialogue Radio
    private RadioButtonGroup _dialogueRadio;

    // Sliders
    public VisualElement AudioSlider;
    public VisualElement MusicSlider;
    public VisualElement AutoSaveSlider;

    // Slider percentages
    private Label _SFXVolumeDisplay;
    private Label _MusicVolumeDisplay;
    private Label _ASFrequencyDisplay;

    /*
     * QUICK EXPLANATION for why there is so many references for anyone that wants to know:
     *      In the UI doc, I can't change the name of some elements
     * (the sliders have the same name, and if I change it, they stop
     * being sliders. Why that happens is not important for this explanation).
     * So, instead of referencing them by the root, I reference them by their parents.
     * All elements in the UI doc are like tree nodes, so if two nodes have the same name,
     * you can instead search for them by their parent. This doesn't work if they are sibligs though.
     */
    void Start()
    {
        // Getting root 
        var root = GetComponent<UIDocument>().rootVisualElement;

        //Getting parents to get references with same names
        _dialogueContainer = root.Q<VisualElement>("DialogueContainer");
        _SFXVolume = root.Q<VisualElement>("SFXVolume");
        _MusicVolume = root.Q<VisualElement>("MusicVolume");
        _AutosavesContainer = root.Q<VisualElement>("AutosavesContainer");

        // Getting references...
        // For Dialogue
        _dialogueRadio = _dialogueContainer.Q<RadioButtonGroup>("SFQRadio");
        SetDialogueSpeed(_dialogueRadio.value);

        // For Audio
        AudioSlider = _SFXVolume.Q("SFQSlider");
        _SFXVolumeDisplay = root.Q<Label>("SFXVolumeDisplay");

        // For Musix
        MusicSlider = _MusicVolume.Q("SFQSlider");
        _MusicVolumeDisplay = root.Q<Label>("MusicVolumeDisplay");

        // For AutoSave
        AutoSaveSlider = _AutosavesContainer.Q("SFQSlider");
        _ASFrequencyDisplay = root.Q<Label>("ASFrequencyDisplay");


        // Method to call when the a slider moves
        _dialogueRadio.RegisterValueChangedCallback(evt => SetDialogueSpeed(evt.newValue));
        AudioSlider.RegisterCallback<ChangeEvent<float>>(SliderValueChangedAudio);
        MusicSlider.RegisterCallback<ChangeEvent<float>>(SliderValueChangedMusic);
        AutoSaveSlider.RegisterCallback<ChangeEvent<float>>(SliderValueChangedAutoSave);
    }

    private static void SetDialogueSpeed(int value)
    {
        Ltg8.Ltg8.Settings.dialogueSettings.selected = value;
    }

    // These bottom methods do the same thing. They get the value of the slider, and update the text to display that value
    void SliderValueChangedAudio(ChangeEvent<float> value)
    {
        float v = Mathf.Round(value.newValue);
        _SFXVolumeDisplay.text = v.ToString() + "%";
    }

    void SliderValueChangedMusic(ChangeEvent<float> value)
    {
        float v = Mathf.Round(value.newValue);
        _MusicVolumeDisplay.text = v.ToString() + "%";
    }

    void SliderValueChangedAutoSave(ChangeEvent<float> value)
    {
        float v = Mathf.Round(value.newValue);

        if (v == 0)
        {
            _ASFrequencyDisplay.text = "Never";
        }
        else
        {
            // Do switch cases to make this number match whatever frequency it should match
            _ASFrequencyDisplay.text = "Every " + v.ToString() + "min";
        }
    }
}