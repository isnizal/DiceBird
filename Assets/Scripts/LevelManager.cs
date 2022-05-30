namespace DuRound.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class LevelManager : MonoBehaviour
    {
        private Sounds.SoundManager _soundManager;
        private void Awake()
        {
            _soundManager = GameObject.Find("SoundManager").GetComponent<Sounds.SoundManager>();
           transform.GetChild(1).GetComponent<Button>().onClick.AddListener(StartGame);
            transform.GetChild(2).GetComponent<Button>().onClick.AddListener(QuitGame);
            transform.GetChild(4).GetComponent<Button>().onClick.AddListener(_soundManager.SetSoundEffects);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void StartGame()
        {
            GetComponent<Animator>().SetBool("isOpen", false);
        }
        public void QuitGame()
        {
            Application.Quit();
        }
        public void AdjustPlayer(int player)
        {
            
        }

    }
}
