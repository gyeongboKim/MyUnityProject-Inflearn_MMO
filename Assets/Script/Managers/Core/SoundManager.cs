using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager 
{
    // AudioSource
    // AudioClip
    // AudioListener

    //Audio Source 컴포넌트
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject rootSound = GameObject.Find("@Sound");
        if(rootSound == null)
        {
            rootSound = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(rootSound);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = rootSound.transform;
            }

            _audioSources[(int)Define.Sound.Bgm].loop = true;
        }
    }

    public void Clear()
    {
        foreach(AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }

    //path를 매개변수로 넘겨주는 경우
    public void Play(string path, Define.Sound soundType = Define.Sound.Effect, float pitch = 1.0f)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, soundType);
        Play(audioClip, soundType, pitch);
    }

    //audioClip을 매개변수로 넘겨주는 경우
    public void Play(AudioClip audioClip, Define.Sound soundType = Define.Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;
        //Bgm인 경우
        if (soundType == Define.Sound.Bgm)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        //Effect인 경우
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    AudioClip GetOrAddAudioClip(string path, Define.Sound soundType = Define.Sound.Effect)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (soundType == Define.Sound.Bgm)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        //Effect인 경우
        else
        {
            // 한번도 재생이 안된 사운드(딕셔너리에 저장되지 않은 사운드)의 경우
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing : {path}");

        return audioClip;
    }

}
