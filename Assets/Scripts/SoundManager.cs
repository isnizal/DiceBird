namespace DuRound.Sounds
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void SetSoundEffects()
        {
            if (!_audioSource.mute)
            {
                _audioSource.mute = true;
            }
            else
            {
                _audioSource.mute = false;
            }
        }
    }
}
