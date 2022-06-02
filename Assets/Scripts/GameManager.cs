

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
        private Animator textTurnAnimator,openingAnimator;
        private Texture2D playerSprite1,playerSprite2;
        private Sprite dice1Sprite, dice2Sprite, dice3Sprite, dice4Sprite, dice5Sprite, dice6Sprite;
        private Sprite _playerSprite1, _playerSprite2;
        private UnityEngine.UI.Image diceManipulate;

        private bool isDicing;
        private bool isPlayerTurn;

        private string playerTurn = "Your Turn";
        private string enemyTurn = "Enemy Turn";

        private CinemachineVirtualCamera _cineMachineVirtual;

        private CanvasGroup _canvasGroupDialog;
        private CanvasGroup _canvasGroupAnyText;

        private TextMeshProUGUI _currentTurnText;
        private TextMeshProUGUI _anyText;
        private Dialog.DialogSystem dialog;
        private List<int> isSkipTurn = new List<int>();
        private UnityEngine.UI.Button _closeButton;

        //sounds
        private Sounds.SoundManager _soundManager;

        private void Awake()
        {
            
            var CanvasCamera = GameObject.Find("CanvasCamera");
            textTurnAnimator = CanvasCamera.GetComponent<Animator>();
            _currentTurnText = CanvasCamera.transform.GetChild(0).transform.GetChild(0)
                .GetComponentInChildren<TextMeshProUGUI>();
            openingAnimator = CanvasCamera.transform.GetChild(3).GetComponent<Animator>();
            openingAnimator.SetBool("isOpen", true);
            Time.timeScale = 1;
            playerSprite1 = Resources.Load("Player/Sprites/pieceBlack_border00") as Texture2D;
            playerSprite2 = Resources.Load("Player/Sprites/pieceBlue_border01") as Texture2D;
            _player = Resources.Load("Player/PlayerPrefabs") as GameObject;
            _playerSprite1 = Sprite.Create(playerSprite1, new Rect(0, 0, playerSprite1.width, playerSprite1.height),
                new Vector2(0.5f, 0.5f));
            _playerSprite2 = Sprite.Create(playerSprite2, new Rect(0, 0, playerSprite2.width, playerSprite2.height),
                new Vector2(0.5f, 0.5f));
            isPlayerTurn = false;
            _soundManager = GameObject.Find("SoundManager").GetComponent<Sounds.SoundManager>();
            dialog = Resources.Load("Dialog/DialogSystem") as Dialog.DialogSystem;
            _canvasGroupDialog = GameObject.Find("Dialogue").GetComponent<CanvasGroup>();
            _canvasGroupAnyText = GameObject.Find("OpenText").GetComponent<CanvasGroup>();
            _anyText = _canvasGroupAnyText.GetComponentInChildren<TextMeshProUGUI>();
            _closeButton = GameObject.Find("Dialogue").transform.GetChild(0).transform.GetChild(1)
                .GetComponent<UnityEngine.UI.Button>();
           // textAnimator = GameObject.Find("OpenText").GetComponent<Animator>();
            currentPlayerTurn = 1;
            _pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
            _cineMachineVirtual = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
            startPos = GameObject.Find("SpawnPosition").transform;
            //spawn player
            for (int player = 0; player < numberOfPlayers.Count; player++)
            {
                var obj = Instantiate(_player, new Vector2( startPos.position.x,
                    startPos.position.y),Quaternion.identity);
                if (player == 0)
                {
                    obj.GetComponent<Player>().SetPlayer(true);
                }
                else
                {
                    obj.GetComponent<Player>().SetPlayer(false);
                }
                obj.name = "player " + player;
                listOfPlayers.Add(obj);
                isSkipTurn.Add(player);
            }
            for (int turn = 0; turn < isSkipTurn.Count; turn++)
            {
                isSkipTurn[turn] = 0;
            }
            listOfPlayers[0].GetComponent<SpriteRenderer>().sprite = _playerSprite1;
            listOfPlayers[1].GetComponent<SpriteRenderer>().sprite = _playerSprite2;
        }
        // Start is called before the first frame update
        IEnumerator  Start()
        {

            SetChatPanel(0);
            yield return new WaitForSeconds(1f);
            SetChatPanel(1);
            yield return new WaitForSeconds(1f);
            StartCoroutine(CheckPlayerTurn());
        }

        // Update is called once per frame
        void Update()
        {
        }

        public Sounds.SoundManager GetSoundManager() { return _soundManager; }
        public bool IsPlayerTurn() { return isPlayerTurn; }
        public void SetPlayerTurn()
        {
             Debug.Log(currentPlayerTurn + "player turn");
            if (currentPlayerTurn == 1)
            {
                isSkipTurn[0] = 1;
            }
            else if (currentPlayerTurn == 2)
            {
                isSkipTurn[1] = 1;
            }

           // Debug.Log(isSkipTurn[0] + "player 0");
          //  Debug.Log(isSkipTurn[1] + "player 1");
            
        }
        public List<GameObject> GetListPlayer() { return listOfPlayers; }
        public bool GetPlayerTurn() { return isPlayerTurn; }
        public Animator GetTextAnimator() { return textTurnAnimator; }
        public CinemachineVirtualCamera GetCineMachineVirtual() { return _cineMachineVirtual; }

        public void ChangeCamera()
        {
            if (isPlayerTurn)
            {
                _cineMachineVirtual.LookAt = listOfPlayers[0].transform;
                _cineMachineVirtual.Follow = listOfPlayers[0].transform;
            }
            else
            {
                _cineMachineVirtual.LookAt = listOfPlayers[1].transform;
                _cineMachineVirtual.Follow = listOfPlayers[1].transform;
            }
        }
        //call from current control player
        public IEnumerator CheckPlayerTurn()
        {
            _soundManager.StopSounds();
            if (numberOfPlayers.Count == 2)
            {
               // Debug.Log(currentPlayerTurn + "player turn");
                //own player
                if (currentPlayerTurn == 1)
                 {

                    if (isSkipTurn[0] == 1)
                    {
                      //  Debug.LogWarning("player 1 skip turn");
                        isSkipTurn[0] = 0;
                        //currentPlayerTurn++;
                        _canvasGroupAnyText.alpha = 1;
                        _canvasGroupAnyText.interactable = true;
                        _canvasGroupAnyText.blocksRaycasts = true;
                        _anyText.text = " Player 1 Skip Turn";
                        yield return new WaitForSeconds(5f);
                        _canvasGroupAnyText.alpha = 0;
                        _canvasGroupAnyText.interactable = false;
                        _canvasGroupAnyText.blocksRaycasts = false;
                         StopCoroutine(CheckPlayerTurn());
                        StartCoroutine(CheckPlayerTurn());

                    }
                    else
                    {
                        //Debug.Log("player turn");
                        isPlayerTurn = true;
                        ChangeCamera();

                        _currentTurnText.text = playerTurn;

                        textTurnAnimator.SetTrigger("isOpen");
                        _soundManager.PlayTurnSounds();
                        yield return new WaitForSeconds(1f);
                        SetChatPanel(6);
                        yield return new WaitForSeconds(1f);
                        listOfPlayers[1].GetComponent<Player>().SetTurnCountOn();
                        yield return null;
                    }
                 }
                 //not own player
                 else if (currentPlayerTurn == 2)
                 {
                    if (isSkipTurn[1] == 1)
                    {
                       // Debug.LogWarning("player 2 skip turn");
                        isSkipTurn[1] = 0;
                        currentPlayerTurn = 1;
                       // Debug.Log("set player turn to 1 " + currentPlayerTurn);
                        _canvasGroupAnyText.alpha = 1;
                        _canvasGroupAnyText.interactable = true;
                        _canvasGroupAnyText.blocksRaycasts = true;
                        _anyText.text = " Player 2 Skip Turn";
                        yield return new WaitForSeconds(5f);
                        _canvasGroupAnyText.alpha = 0;
                        _canvasGroupAnyText.interactable = false;
                        _canvasGroupAnyText.blocksRaycasts = false;
                         StopCoroutine(CheckPlayerTurn());
                        StartCoroutine(CheckPlayerTurn());
                        //yield return null;
                    }
                    else
                    {
                        isPlayerTurn = false;
                        ChangeCamera();
                        //Debug.Log("enemy turn@");
                        _currentTurnText.text = enemyTurn;



                        textTurnAnimator.SetTrigger("isOpen");
                        _soundManager.PlayTurnSounds();
                        yield return new WaitForSeconds(5f);
                        SetChatPanel(7);
                        yield return new WaitForSeconds(1f);
                        // StartCoroutine(WaitForSeconds());

                        //move player after dicing
                        // StartAnimationDicing();
                        var noChance = Random.Range(1, 6);
                        _canvasGroupAnyText.alpha = 1;
                        _canvasGroupAnyText.interactable = true;
                        _canvasGroupAnyText.blocksRaycasts = true;
                        _anyText.text = " You got " + noChance + " Position to move";
                        yield return new WaitForSeconds(5f);
                        _canvasGroupAnyText.alpha = 0;
                        _canvasGroupAnyText.interactable = false;
                        _canvasGroupAnyText.blocksRaycasts = false;
                        StartCoroutine(listOfPlayers[0].GetComponent<Player>().MovePosition(noChance));
                        yield return null;
                    }
                    // break;
                 }
                
            }
        }
        private IEnumerator WaitForSeconds()
        {
            yield return new WaitForSeconds(2f);
           // Debug.Log("finish waiting");
            textTurnAnimator.SetTrigger("isButtonOff");
           //. OpeningDice();
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
                GetListPoints().Length-1].transform.position.x) && Mathf.Approximately(listOfPlayers[0].transform.position.y,
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
            _canvasGroupDialog.alpha = 1;
            _canvasGroupDialog.interactable = true;
            _canvasGroupDialog.blocksRaycasts = true;
            Time.timeScale = 0;
            switch (textNumber)
            {
                case 0:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.IntroductionText;
                    break;
                case 1:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.BeginningText;
                    break;
                case 2:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.EndingText;
                    _closeButton.onClick.AddListener(()=>UnityEngine.SceneManagement.SceneManager.LoadScene
                        ("MainLevel"));
                    break;
                case 3:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.WaterText ;
                    break;
                case 4:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.StayText;
                    break;
                case 5:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                        = dialog.MountainText;
                    break;
                case 6:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = dialog.moveNumberText;
                    break;
                case 7:
                    _canvasGroupDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>().text
                    = dialog.playerNumberText;
                    break;
            }
        }
        public void CloseChatPanel()
        {
            _canvasGroupDialog.alpha = 0;
            _canvasGroupDialog.interactable = false;
            _canvasGroupDialog.blocksRaycasts = false;
            Time.timeScale = 1;
        }
    }
}
