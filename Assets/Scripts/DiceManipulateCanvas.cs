using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuRound.Manager;

namespace DuRound.UI
{
    public class DiceManipulateCanvas : MonoBehaviour
    {
        private GameManager _gameManager;
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartDicing()
        {
            _gameManager.StartAnimationDicing();
        }
    }

}
