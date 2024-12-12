using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureSliderScript : MonoBehaviour
{
    [SerializeField] private Slider CreatureSlider;

    public AudioSource SliderSound;
    [SerializeField] private AudioClip Sound;

    // Start is called before the first frame update
    void Start()
    {
        SliderSound.clip = Sound;

        CreatureSlider.onValueChanged.AddListener((v) =>
        {SliderSound.volume = v; 
        SliderSound.Play(); }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
