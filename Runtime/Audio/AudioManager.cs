using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.tj.Audio
{
    public class AudioContainer : MonoBehaviour
    {
        public enum AudioType
        {
            ambient,
            effect
        }

        public delegate void OnAudioCompleteDelegate(AudioContainer container);

        public event OnAudioCompleteDelegate OnAudioComplete;
        private AudioSource source;

        private float volume;
        public float Volume
        {
            get
            {
                return volume;
            }

            set
            {
                volume = value;
                VolumeTo = value;
                if (!AudioManager.Mute)
                    source.volume = volume;
            }
        }

        public AudioClip Clip
        {
            private set;
            get;
        }

        public bool IsPlaying
        {
            get;
            private set;
        }

        private float VolumeTo;
        public AudioType Type
        {
            private set;
            get;
        }

        public void Init(AudioClip clip, AudioType type = AudioType.effect, Transform t = null)
        {
            Clip = clip;
            Type = type;

            transform.SetParent(t);
            transform.localPosition = Vector3.zero;

            source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            if (Type == AudioType.ambient)
                source.loop = true;
        }

        public void Mute(bool value)
        {
            source.volume = value ? 0 : Volume;
        }

        public void StartAudio()
        {
            source.Play();
            IsPlaying = true;
        }

        public void Pause()
        {
            source.Pause();
            IsPlaying = false;
        }

        public void Stop()
        {
            source.Stop();
        }

        public void FadeVolume(float val)
        {
            VolumeTo = val;
        }

        public void Kill()
        {
            Stop();
            GameObject.Destroy(gameObject);
        }

        private void Update()
        {
            if (source.time >= Clip.length && IsPlaying)
                AudioCompleted();

            float d = (VolumeTo - Volume);
            if (Mathf.Abs(d) > 0.001)
                Volume += d * 0.1f;
        }


        private void AudioCompleted()
        {
            IsPlaying = false;
            OnAudioComplete?.Invoke(this);
        }

    }
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager instance;
        public static AudioManager Instance
        {
            get
            {
                if (instance)
                    return instance;

                GameObject go = new GameObject("AudioManager");
                DontDestroyOnLoad(go);

                instance = go.AddComponent<AudioManager>();
                audioContainers = new List<AudioContainer>();

                return instance;
            }
        }

        private static bool mute;
        public static bool Mute
        {
            set
            {
                mute = value;
                foreach (AudioContainer c in audioContainers)
                    c.Mute(mute);
            }
            get
            {
                return mute;
            }
        }

        private static List<AudioContainer> audioContainers;

        public AudioContainer PlayEffect(AudioClip clip)
        {
            return PlayEffect(clip);
        }

        public AudioContainer PlayEffect(AudioClip clip, Transform t)
        {
            return CreateAudioContainer(clip, t, AudioContainer.AudioType.effect);
        }

        public AudioContainer PlayAmbient(AudioClip clip)
        {
            return PlayEffect(clip);
        }

        public AudioContainer PlayAmbient(AudioClip clip, Transform t)
        {
           
            return CreateAudioContainer(clip , t , AudioContainer.AudioType.ambient);
        }

        private AudioContainer CreateAudioContainer(AudioClip clip, Transform t, AudioContainer.AudioType audioType)
        {
            AudioContainer audio = TJUtils.Instantiate<AudioContainer>(clip.name);
            audio.OnAudioComplete += Audio_OnAudioComplete;
            audio.Init(clip, audioType, t);
            audio.StartAudio();
            audioContainers.Add(audio);

            return audio;
        }

        private void Audio_OnAudioComplete(AudioContainer container)
        {
            if (container.Type == AudioContainer.AudioType.effect)
                KillAudio(container);
        }

        public void KillAudio(AudioContainer container)
        {
            container.OnAudioComplete -= Audio_OnAudioComplete;
            audioContainers.Remove(container);
            container.Kill();
        }

    }

}