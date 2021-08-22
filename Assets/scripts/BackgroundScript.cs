using Assets.scripts.ViewModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    void Awake()
    {
        var audio = GetComponent<AudioSource>();
        GameViewModel.AwakeAudioSetting(audio);
        GameViewModel.AwakeImageQualitySetting();
    }
}
