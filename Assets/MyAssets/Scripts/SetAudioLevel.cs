using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetAudioLevel : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void AudioLevel(float level)
    {
        audioMixer.SetFloat("Volume", SetLevel(level));
    }
    
    public float SetLevel(float level)
    {
        return Mathf.Log10(level) * 20;
    }
}
