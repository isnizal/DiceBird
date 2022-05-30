namespace DuRound.Sounds
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private AudioClip _diceClip, _pawnMovement;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        // Start is called before the first frame update
        void Start()
        {
            _diceClip = Resources.Load("Sounds/dieShuffle1") as AudioClip;
            _pawnMovement = Resources.Load("Sounds/pawnMove") as AudioClip;
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
        public void PlayDiceSound()
        {
            _audioSource.PlayOneShot(_diceClip);
        }
        public void PlayPawnSound()
        {
            _audioSource.PlayOneShot(_pawnMovement);
        }

        public void StopSounds()
        {
            _audioSource.Stop();

        }
    }
}
