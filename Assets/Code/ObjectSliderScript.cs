using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSliderScript : MonoBehaviour
{
    [SerializeField] private Slider ObjectSlider;

    public AudioSource ObjectSound;
    [SerializeField] private AudioClip Sound;

    // Start is called before the first frame update
    void Start()
    {
        ObjectSound.clip = Sound;

        ObjectSlider.onValueChanged.AddListener((v) =>
        {ObjectSound.volume = v; 
        ObjectSound.Play(); }
        );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
