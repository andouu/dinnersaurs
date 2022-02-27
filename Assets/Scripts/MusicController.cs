using System;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource _titleMusic;
    [SerializeField] private AudioSource _gameMusic;

    private void Start()
    {
        playTitleMusic();
    }
    
    public void playTitleMusic()
    {
        _gameMusic.Stop();
        _titleMusic.Play();
    }
    
    public void playGameMusic()
    {
        _titleMusic.Stop();
        _gameMusic.Play();
    }
}
