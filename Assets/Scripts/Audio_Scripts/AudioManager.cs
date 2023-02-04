using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager Instance { get; private set; }
    public bool StopAllWhenSceneLoads;
    // This is Brackeys Audio Manager system with some small tweaks. 
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(transform.gameObject);
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.pitch == 0) s.source.pitch = 1f;
        }
    }

    private void Start()
    {
        Play("music");
    }

    public void StopAll()
    {
        foreach(Sound s in sounds)
        {
            s.source.Stop();
        }
    }

    // Update is called once per frame
    public void Play(string name)
    {
     
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("\"" +name + "\" was called to Play but no audio file was found in manager with this name.");
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Pause(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.UnPause();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //this is a filthy way to code but here we go
        foreach (Sound s in sounds)
        {
            if (s.name.Contains(scene.name))
            {
                if (s.name.Contains("Music")) Play(s.name);
            }
            else if (s.name.Contains("Music")) Stop(s.name);
        }
    }
}
