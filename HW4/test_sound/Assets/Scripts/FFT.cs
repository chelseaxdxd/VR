using UnityEngine;
using System;
using System.Collections;

public class FFT : MonoBehaviour
{

    public AudioSource Source;
    public string CurrentAudioInput = "none";
    int deviceNum = 0;
    public int WINDOW_SIZE = 256;
    public float[] spectrum;
    public float loudness = 0;
    public const float freq = 24000f;
    public float rr = 0.0f;
    public float volumn30avg = 0;
    public float[] volumn30 = new float[30];
    int i = 0;
    public bool clap= false;



    IEnumerator Start()
    {

        Source = GetComponent<AudioSource>();
        spectrum = new float[WINDOW_SIZE];
        yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        if (Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            Debug.Log("mic found!!");
        }
        else
        {
            Debug.Log("mic not found!!");
        }


        string[] inputDevices = new string[Microphone.devices.Length];
        deviceNum = 0;

        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            inputDevices[i] = Microphone.devices[i].ToString();
            Debug.Log("Device"+i+":"+ inputDevices[i]);
        }
        CurrentAudioInput = Microphone.devices[deviceNum].ToString();
        StartMic();
        Source.mute = false;


    }




    public void StartMic()
    {
        Source.clip = Microphone.Start(CurrentAudioInput, true, 5, (int)freq);
    }

    void Update()
    {
        rr = (rr + 1.0f) % 360.0f;
        loudness = GetAveragedVolume() * 100.0f + 1.0f;
        volumn30[i % 30] = loudness;
        i++;
        float sum = 0;
        for(int j=0;j<30;j++)
        {
            sum += volumn30[j];
        }
        volumn30avg = sum / 30.0f;
        if(i>30)
        {
            if (clap == false && volumn30[(i - 15) % 30] > 10 && volumn30avg < 5)
            {
                Debug.Log("clap");
                clap = true;
            }
            else if (clap == false && volumn30[(i - 15) % 30] > 10 && volumn30avg >= 5)
                Debug.Log("blow");
            else clap = false;
        }
        if(loudness>15)
            Debug.Log(loudness);

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            Microphone.End(CurrentAudioInput);
            deviceNum += 1;
            if (deviceNum > Microphone.devices.Length - 1)
                deviceNum = 0;
            CurrentAudioInput = Microphone.devices[deviceNum].ToString();

            StartMic();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Source.Play();
        }

        float delay = 0.010f;
        int microphoneSamples = Microphone.GetPosition(CurrentAudioInput);

        if (microphoneSamples / freq > delay)
        {
            if (!Source.isPlaying)
            {
                Debug.Log("Starting thing");
                Source.timeSamples = (int)(microphoneSamples - (delay * freq));
                Source.Play();
            }
        }
        Source.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);
        transform.localScale = new Vector3(loudness, loudness, loudness);
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
  






    }


    float GetAveragedVolume()
    {
        float[] data = new float[WINDOW_SIZE];
        float a = 0;
        Source.GetOutputData(data, 0);
        foreach (float s in data)
        {
            a += Mathf.Abs(s);
        }
        return a / WINDOW_SIZE;
    }
}