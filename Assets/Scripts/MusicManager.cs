using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    [SerializeField]private AudioSource music;
    private Slider slider;

    void Start()
    {
        slider=GetComponent<Slider>();
    }

    
    void Update()
    {
        music.volume=slider.value;
    }
}
