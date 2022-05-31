
namespace DuRound.Minion {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using DuRound.ThePath;
    using DuRound.Manager;
    public class Player : MonoBehaviour
    {
      //  [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _groundLayerMask,_forestLayerMask,
            _mountainCollider2D,_waterCollider2D,_castleCollider2D;
        private PathFinding _pathFinding;
        private Transform[] listPosition;
        private Rigidbody2D _rigidBody2D;
        private GameManager _gameManager;
        private int lastPosition { get; set; }
      //  private bool firstTurn = false;
     //   private int amountOfTurn { get; set; }
      //  private int incrementPosition { get; set; }

        private void Awake()
        {


        }
        // Start is called before the first frame update
        void Start()
        {

            _pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _mountainCollider2D = GameObject.Find("Mountain").GetComponent<Collider2D>();
            _waterCollider2D = GameObject.Find("Water").GetComponent<Collider2D>();
            _castleCollider2D = GameObject.Find("Castle").GetComponent<Collider2D>();
           // incrementPosition = 1;
            increment = 0;
            lastPosition = 0;
            listPosition = _pathFinding.GetListPoints();
            _rigidBody2D = GetComponent<Rigidbody2D>();
           // firstTurn = false;
        }

        // Update is called once per frame
        void Update()
        {
           // Debug.LogWarning(lastPosition + "lastpos");
        }
        private void Reset()
        {
            _animator = GetComponent<Animator>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
           // incrementPosition = 1;
        }
        private int increment;
        public IEnumerator MovePosition(int currentPosition)
        {
            _gameManager.GetSoundManager().PlayPawnSound();
           //Debug.Log(currentPosition + "currentPosition");
            lastPosition += currentPosition;
            //Debug.Log(lastPosition + "last pos");
          //  Debug.Log(listPosition[lastPosition]);
            while (true)
            {
                if (Mathf.Approximately(_rigidBody2D.transform.position.x, listPosition[listPosition.Length-1].transform.position.x)
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[listPosition.Length-1].transform.position.y))
                {
                    //_gameManager.CheckThePlayerLastPosition();
                  //  Debug.Log("NEXT LEVEL");
                    _gameManager.SetChatPanel(2);
                    StopAllCoroutines();
                    break;
                }
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[increment + 1].transform.position, 3 * Time.deltaTime));
                if (Mathf.Approximately(_rigidBody2D.transform.position.x , listPosition[increment + 1].transform.position.x) 
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[increment + 1].transform.position.y))
                {
                    increment++;
                    if (increment == lastPosition)
                    {
                        //_gameManager.currentPlayerTurn++;
                      //  Debug.Log("player turn" + _gameManager.currentPlayerTurn);
                        if (_gameManager.currentPlayerTurn > 2)
                            _gameManager.currentPlayerTurn = 1;
                        yield return new WaitForSeconds(1f);
                        CheckForLayerMask(increment);

                        //
                        break;
                    }
                  //  }
                    //break;
                }
                yield return null;
            }
        }
        public void StartMove()
        {
            _gameManager.CheckPlayerTurn();
        }

        private void CheckForLayerMask(int position)
        {
            if (_rigidBody2D.IsTouching(_mountainCollider2D))
            {
                StartCoroutine(MountainMove(position));
            }
            else if (_rigidBody2D.IsTouching(_waterCollider2D))
            {
                StartCoroutine(WaterMove(position));
            }
            else if (_rigidBody2D.IsTouching(_castleCollider2D))
            {
                StartCoroutine(CastleMove(position));
            }
            else
            {
                //Debug.Log("no collision");
                _gameManager.currentPlayerTurn++;
                if (_gameManager.currentPlayerTurn > 2)
                    _gameManager.currentPlayerTurn = 1;
                increment = position;
                StartCoroutine(_gameManager.CheckPlayerTurn());

            }
        }
        private IEnumerator WaterMove(int position)
        {
            
            while (true)
            {
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[position-1].transform.position, 3 * Time.deltaTime));
                if (Mathf.Approximately(_rigidBody2D.transform.position.x, listPosition[position - 1].transform.position.x)
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[position - 1].transform.position.y))
                {

                   _gameManager.SetChatPanel(3);
                   yield return new WaitForSeconds(1f);
                     increment = position-1;
                    lastPosition -= 1;
                    CheckForLayerMask(lastPosition);
                        break;
                    

                }
                yield return null;
            }
            //_gameManager.CheckPlayerTurn();
        }
        private IEnumerator MountainMove(int position)
        {
           var minusThree = position - 3;
            while (true)
            {
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[position-1].transform.position, 3 * Time.deltaTime));
                if (Mathf.Approximately(_rigidBody2D.transform.position.x, listPosition[position-1].transform.position.x)
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[position-1].transform.position.y))
                {
                    position--;
                    if (minusThree == position)
                    {
                        _gameManager.SetChatPanel(5);
                        yield return new WaitForSeconds(1f);
                        increment = minusThree;
                        lastPosition -= 3;
                        CheckForLayerMask(lastPosition);
                        break;
                    }

                }
                yield return null;
            }
        }
        private IEnumerator CastleMove(int position)
        {
            _gameManager.SetChatPanel(4);
            _gameManager.SetPlayerTurn();
            yield return new WaitForSeconds(0.5f);
            _gameManager.currentPlayerTurn++;
            increment = position;
            if (_gameManager.currentPlayerTurn > 2)
                _gameManager.currentPlayerTurn = 1;
            StartCoroutine(_gameManager.CheckPlayerTurn());
        }

    }
}
