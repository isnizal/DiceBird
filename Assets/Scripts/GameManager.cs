

namespace DuRound.Manager
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DuRound.ThePath;
    using DuRound.Minion;
    using TMPro;
    using Cinemachine;
    
    
    public class GameManager : MonoBehaviour
    {
        public List<int> numberOfPlayers;
        public int currentPlayerTurn;

        private Transform startPos;
        private PathFinding _pathFinding;
        private GameObject _player;
        private Player _playerScripts;
     
        private List<GameObject> listOfPlayers = new List<GameObject>();
        private Animator diceManipulateAnimator,textTurnAnimator;
        private Texture2D dice1, dice2, dice3, dice4, dice5, dice6;
        private Sprite dice1Sprite, dice2Sprite, dice3Sprite, dice4Sprite, dice5Sprite, dice6Sprite;
        private UnityEngine.UI.Image diceManipulate;

        private bool isDicing;
        private bool isPlayerTurn;

        private string playerTurn = "Your Turn";
        private string enemyTurn = "Enemy Turn";

        private CinemachineVirtualCamera _cineMachineVirtual;

        private CanvasGroup _canvasGroup;
        private Player test;

        private TextMeshProUGUI _currentTurnText;
        private Dialog.DialogSystem dialog;
        private List<int> isSkipTurn = new List<int>();

        private void Awake()
        {
            diceManipulate = GameObject.Find("DiceManipulate").GetComponent<UnityEngine.UI.Image>();
            diceManipulateAnimator = GameObject.Find("CanvasOverlay").GetComponent<Animator>();
            textTurnAnimator = GameObject.Find("CanvasCamera").GetComponent<Animator>();
            _currentTurnText = GameObject.Find("CanvasCamera").transform.GetChild(0).transform.GetChild(0)
                .GetComponentInChildren<TextMeshProUGUI>();
            _player = Resources.Load("Player/PlayerPrefabs") as GameObject;
            //Debug.Log(Resources.Load("WhiteDice/dieWhite1") as Texture2D);
            dice1 = Resources.Load("WhiteDice/dieWhite1") as Texture2D;
            dice2 = Resources.Load("WhiteDice/dieWhite2") as Texture2D;
            dice3 = Resources.Load("WhiteDice/dieWhite3") as Texture2D;
            dice4 = Resources.Load("WhiteDice/dieWhite4") as Texture2D;
            dice5 = Resources.Load("WhiteDice/dieWhite5") as Texture2D;
            dice6 = Resources.Load("WhiteDice/dieWhite6") as Texture2D;
            dice1Sprite = Sprite.Create(dice1,new Rect(0,0,dice1.width,dice1.height),new Vector2(0.5f,0.5f));
            dice2Sprite = Sprite.Create(dice2, new Rect(0, 0, dice2.width, dice2.height), new Vector2(0.5f, 0.5f));
            dice3Sprite = Sprite.Create(dice3, new Rect(0, 0, dice3.width, dice3.height), new Vector2(0.5f, 0.5f));
            dice4Sprite = Sprite.Create(dice4, new Rect(0, 0, dice4.width, dice4.height), new Vector2(0.5f, 0.5f));
            dice5Sprite = Sprite.Create(dice5, new Rect(0, 0, dice5.width, dice5.height), new Vector2(0.5f, 0.5f));
            dice6Sprite = Sprite.Create(dice6, new Rect(0, 0, dice6.width, dice6.height), new Vector2(0.5f, 0.5f));
            isPlayerTurn = false;
            dialog = Resources.Load("Dialog/DialogSystem") as Dialog.DialogSystem;
            _canvasGroup = GameObject.Find("Dialogue").GetComponent<CanvasGroup>();
            currentPlayerTurn = 1;
            _pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
            _cineMachineVirtual = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            startPos = _pathFinding.GetListPoints()[0].transform;
            //spawn player
            for (int player = 0; player < numberOfPlayers.Count; player++)
            {
                var obj = Instantiate(_player, new Vector2( startPos.transform.position.x,
                    startPos.transform.position.y),Quaternion.identity);
                obj.name = "player " + player;
                listOfPlayers.Add(obj);
                isSkipTurn.Add(player);
            }
            for (int turn = 0; turn < isSkipTurn.Count; turn++)
            {
                isSkipTurn[turn] = 0;
            }
            _playerScripts = listOfPlayers[0].GetComponent<Player>();
         //   listOfPlayers[0].GetComponent<Player>().StartMove();
        }
        // Start is called before the first frame update
        IEnumerator  Start()
        {

            SetChatPanel(0);
            yield return new WaitForSeconds(0.5f);
            SetChatPanel(1);
            CheckPlayerTurn();
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void CheckForTheSkipTurn()
        {
            if (currentPlayerTurn == 1)
            {
                isSkipTurn[0] = 1;
                isSkipTurn[1] = 0;
            }
            else if (currentPlayerTurn == 2)
            {
                isSkipTurn[1] = 1;
                isSkipTurn[0] = 0;
            }
        }
        public List<GameObject> GetListPlayer() { return listOfPlayers; }
        public bool GetPlayerTurn() { return isPlayerTurn; }
        public Animator GetTextAnimator() { return textTurnAnimator; }
        public CinemachineVirtualCamera GetCineMachineVirtual() { return _cineMachineVirtual; }

        public void ChangeCamera()
        {
            if (isPlayerTurn)
            {
                Debug.Log(listOfPlayers[0].name);
                _cineMachineVirtual.LookAt = listOfPlayers[0].transform;
                _cineMachineVirtual.Follow = listOfPlayers[0].transform;
            }
            else
            {
                _cineMachineVirtual.LookAt = listOfPlayers[1].transform;
                _cineMachineVirtual.Follow = listOfPlayers[1].transform;
            }
        }
        //call from animation
        public void OpeningDice()
        {

            diceManipulateAnimator.SetBool("isReady", true);
        }
        public void ClosingDice()
        {
            StopDicing();
            diceManipulateAnimator.SetBool("isReady", false);
        }
        public void StartAnimationDicing()
        {
            //diceManipulateAnimator.SetTrigger("isPlay");
            diceManipulateAnimator.SetBool("isReady", true);
            StartCoroutine(StartDicing());
        }

        //call from current control player
        public void CheckPlayerTurn()
        {
            if (numberOfPlayers.Count == 2)
            {
                //Debug.Log(currentPlayerTurn);
                //own player
                if (currentPlayerTurn == 1 && isSkipTurn[0] == 0)
                {
                    isPlayerTurn = true;
                    ChangeCamera();

                    _currentTurnText.text = playerTurn;

                    textTurnAnimator.SetTrigger("isOpen");
                   // OpeningDice();
                }
                //not own player
                else if(currentPlayerTurn == 2 && isSkipTurn[1] == 0)
                {
                    isPlayerTurn = false;
                    ChangeCamera();
                    Debug.Log("enemy turn@");
                    _currentTurnText.text = enemyTurn;
                    textTurnAnimator.SetTrigger("isOpen");

                    StartCoroutine(WaitForSeconds());

                    //move player after dicing
                      StartAnimationDicing();
                }
            }
        }
        private IEnumerator WaitForSeconds()
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("finish waiting");
            textTurnAnimator.SetTrigger("isButtonOff");
            OpeningDice();
        }
        public void CoroutineDicing()
        {
            StopCoroutine(StartDicing());
            StartCoroutine(StartDicing());
        }
        private IEnumerator StartDicing()
        {
            yield return new WaitForSeconds(1f);
            isDicing = true;

            while (isDicing)
            {

                var random = Random.Range(1, 7);
                ChangeSpriteDice(random);
                yield return isDicing;


                diceNumber = random;
            }
        }
        private int diceNumber;
        public void StopDicing()
        {
            isDicing = false;
            StopCoroutine(StartDicing());
            StartCoroutine(listOfPlayers[currentPlayerTurn - 1].GetComponent<Player>().MovePosition(diceNumber));

        }
        private void  ChangeSpriteDice(int number)
        {
            switch (number)
            {
                case 1:
                    diceManipulate.sprite = dice1Sprite;
                    break;
                case 2:
                    diceManipulate.sprite = dice2Sprite;
                    break;
                case 3:
                    diceManipulate.sprite = dice3Sprite;
                    break;
                case 4:
                    diceManipulate.sprite = dice4Sprite;
                    break;
                case 5:
                    diceManipulate.sprite = dice5Sprite;
                    break;
                case 6:
                    diceManipulate.sprite = dice6Sprite;
                    break;
            }
        }

        public void CheckThePlayerLastPosition()
        {
            if (Mathf.Approximately(listOfPlayers[0].transform.position.x, _pathFinding.GetListPoints()[_pathFinding.
                GetListPoints().Length].transform.position.x) && Mathf.Approximately(listOfPlayers[0].transform.position.y,
                _pathFinding.transform.position.y))
            {
                Debug.LogWarning("You going to heaven");
            }
            else if (Mathf.Approximately(listOfPlayers[1].transform.position.x, _pathFinding.GetListPoints()[_pathFinding.
            GetListPoints().Length].transform.position.x) && Mathf.Approximately(listOfPlayers[1].transform.position.y,
            _pathFinding.transform.position.y))
            {
                Debug.LogWarning("You going to hell");
            }
        }
        public void SetChatPanel(int textNumber)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            Time.timeScale = 0;
            switch (textNumber)
            {
                case 0:
                    _canvasGroup.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.IntroductionText;
                    break;
                case 1:
                    _canvasGroup.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.BeginningText;
                    break;
                case 2:
                    _canvasGroup.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.EndingText;
                    break;
                case 3:
                    _canvasGroup.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.StayTurnText ;
                    break;
                case 4:
                    _canvasGroup.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.GainTurnText;
                    break;
                case 5:
                    _canvasGroup.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.LoseTurnText;
                    break;
            }
        }
        public void CloseChatPanel()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            Time.timeScale = 1;
        }
    }
}
