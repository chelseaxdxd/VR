using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioSpectrum : MonoBehaviour
{
    AudioSource _audioSource;
    //mic input
    public AudioClip _audioClip;
    public bool _useMic;
    public string _selectedDevice;

    private void Update()
    {
        // get the data
        AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);

        if (m_audioSpectrum != null && m_audioSpectrum.Length > 0)
        {
            spectrumValue = m_audioSpectrum[0] * 100;
        }
    }

    private void Start()
    {
        /// initialize buffer
        m_audioSpectrum = new float[128];
        //mic input
        if(_useMic)
        {
            if(Microphone.devices.Length>0)
            {
                _selectedDevice = Microphone.devices[0].ToString();
                _audioSource.clip = Microphone.Start(_selectedDevice,true,10,AudioSettings.outputSampleRate);
            }
            else
            {
                _useMic = false;
            }
        }
        if (!_useMic)
        {
            _audioSource.clip = _audioClip;
        }
    }

    // This value served to AudioSyncer for beat extraction
    public static float spectrumValue { get; private set; }

    // Unity fills this up for us
    private float[] m_audioSpectrum;

}
