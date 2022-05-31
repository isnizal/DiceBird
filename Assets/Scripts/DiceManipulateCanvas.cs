using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuRound.Manager;

namespace DuRound.UI
{
    public class DiceManipulateCanvas : MonoBehaviour
    {
        private GameManager _gameManager;
        private UnityEngine.UI.Button diceOnBtn;
        // Start is called before the first frame update
        void Start()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            diceOnBtn = GameObject.Find("DiceOnBtn").GetComponent<UnityEngine.UI.Button>();
            diceOnBtn.onClick.AddListener(StartDicing);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void StartDicing()
        {

            _gameManager.GetTextAnimator().SetTrigger("isButtonOff");
            _gameManager.OpeningDice();
        }
        public void StartAnimateDice()
        {
            _gameManager.StartAnimationDicing();
        }
        public void ClosingDice()
        {
           // Debug.Log("closing dice");
            _gameManager.ClosingDice();
        }
    }

}
