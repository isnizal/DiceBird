using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuRound.ThePath;

namespace DuRound.Manager
{
    public class GameManager : MonoBehaviour
    {
        public List<int> numberOfPlayers;
        public int currentPlayerTurn;

        private Transform startPos;
        private PathFinding _pathFinding;
        private GameObject _player;
     
        private List<GameObject> listOfPlayers = new List<GameObject>();
        private Animator diceManipulateAnimator,textTurnAnimator;
        private Sprite dice1, dice2, dice3, dice4, dice5, dice6;
        private UnityEngine.UI.Image diceManipulate;
        private bool isDicing;
        private bool isPlayer;


        private void Awake()
        {
            diceManipulate = GameObject.Find("DiceManipulate").GetComponent<UnityEngine.UI.Image>();
            diceManipulateAnimator = GameObject.Find("DiceManipulate").GetComponent<Animator>();
            textTurnAnimator = GameObject.Find("CanvasCamera").GetComponent<Animator>();
            _player = Resources.Load("Player/PlayerPrefabs") as GameObject;
            _pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
            startPos = _pathFinding.GetListPoints()[0].transform;
            //spawn player
            for (int player = 0; player < numberOfPlayers.Count; player++)
            {
                var obj = Instantiate(_player, startPos);
                listOfPlayers.Add(obj);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

            
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void OpeningDice()
        {
            diceManipulateAnimator.SetBool("isReady", true);
        }
        public void StartAnimationDicing()
        {
            diceManipulateAnimator.SetBool("isPlay", true);
            StartCoroutine(StartDicing());
        }

        //call from current control player
        public void CheckPlayerTurn()
        {
            if (numberOfPlayers.Count == 2)
            {
                if (currentPlayerTurn == 1)
                {
                    OpeningDice();
                }
            }
        }
        private IEnumerator StartDicing()
        {
            isDicing = true;
            currentPlayerTurn += 1;
            while (isDicing)
            {

                var random = Random.Range(1, 7);
                ChangeSpriteDice(random);
                yield return isDicing;
            }
        }
        public void StopDicing()
        {
            isDicing = false;
        }
        private void  ChangeSpriteDice(int number)
        {
            switch (number)
            {
                case 1:
                    diceManipulate.sprite = dice1;
                    break;
                case 2:
                    diceManipulate.sprite = dice2;
                    break;
                case 3:
                    diceManipulate.sprite = dice3;
                    break;
                case 4:
                    diceManipulate.sprite = dice4;
                    break;
                case 5:
                    diceManipulate.sprite = dice5;
                    break;
                case 6:
                    diceManipulate.sprite = dice6;
                    break;
            }
        }
    }
}
