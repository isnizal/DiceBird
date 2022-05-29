
namespace DuRound.Minion {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using DuRound.ThePath;
    using DuRound.Manager;
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Collider2D _groundLayerMask,_forestLayerMask,
            _mountainCollider2D,_waterCollider2D,_castleCollider2D;
        private PathFinding _pathFinding;
        private Transform[] listPosition;
        private Rigidbody2D _rigidBody2D;
        private GameManager _gameManager;
        private int lastPosition { get; set; }

        private int amountOfTurn { get; set; }
        private int incrementPosition { get; set; }

        private void Awake()
        {


        }
        // Start is called before the first frame update
        void Start()
        {
            lastPosition = 0;
            _pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            _mountainCollider2D = GameObject.Find("Mountain").GetComponent<Collider2D>();
            _waterCollider2D = GameObject.Find("Water").GetComponent<Collider2D>();
            _castleCollider2D = GameObject.Find("Castle").GetComponent<Collider2D>();
            incrementPosition = 1;
            increment = 0;
            listPosition = _pathFinding.GetListPoints();
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
        }
        private void Reset()
        {
            _animator = GetComponent<Animator>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            incrementPosition = 1;
        }
        private int increment;
        public IEnumerator MovePosition(int currentPosition)
        {
            Debug.Log(currentPosition + "currentPosition");
            lastPosition += currentPosition;

            while (_rigidBody2D.transform.position != listPosition[29].transform.position)
            {
                //if (Mathf.Approximately(_rigidBody2D.transform.position.x, listPosition[listPosition.Length-1].transform.position.x)
                //    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[listPosition.Length-1].transform.position.y))
                //{
                //    _gameManager.CheckThePlayerLastPosition();
                //    break;
                //}
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[increment].transform.position, 2 * Time.deltaTime));
                if (Mathf.Approximately(_rigidBody2D.transform.position.x , listPosition[increment].transform.position.x) 
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[increment].transform.position.y))
                {
                    increment++;
                    //var minusPos = 0;
                    //Debug.Log("check" + lastPosition);
                    // minusPos++;
                    //   if (minusPos > lastPosition)
                    //    {
                    // Debug.Log(minusPos);
                    if (increment == 29)
                    {
                        yield return new WaitForSeconds(1f);
                        CheckForLayerMask(29);
                        _gameManager.currentPlayerTurn++;
                        if (_gameManager.currentPlayerTurn > 2)
                            _gameManager.currentPlayerTurn = 1;
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
        }
        private IEnumerator WaterMove(int position)
        {
            var minusOne = position - 1;
            while (_rigidBody2D.transform.position != listPosition[minusOne].transform.position)
            {
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[minusOne].transform.position, 2 * Time.deltaTime));
                if (Mathf.Approximately(_rigidBody2D.transform.position.x, listPosition[position].transform.position.x)
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[position].transform.position.y))
                {
                    if (position < minusOne)
                    {
                        yield return new WaitForSeconds(1f);
                        _gameManager.CheckPlayerTurn();
                        increment = position += 1;
                        break;
                    }

                }

            }
            _gameManager.CheckPlayerTurn();
        }

        private IEnumerator MountainMove(int position)
        {
            //stop move
            position--;
          //  Debug.Log("position" + position);
            var minusThree = position - 3;
            while (_rigidBody2D.transform.position != listPosition[minusThree].transform.position)
            {
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[position].transform.position, 2 * Time.deltaTime));
                if (Mathf.Approximately(_rigidBody2D.transform.position.x, listPosition[position].transform.position.x)
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[position].transform.position.y))
                {
                    position--;
                    if (position < minusThree)
                    {
                        yield return new WaitForSeconds(1f);
                        Debug.Log("turn on");
                        _gameManager.CheckPlayerTurn();
                        increment = position +=1;
                        break;
                    }

                }
                yield return null;

            }

        }
        private IEnumerator CastleMove(int position)
        {
            var plusTwo = position + 2;
            while (_rigidBody2D.transform.position != listPosition[position].transform.position)
            {
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[plusTwo].transform.position, 2 * Time.deltaTime));
                if (Mathf.Approximately(_rigidBody2D.transform.position.x, listPosition[position].transform.position.x)
                    && Mathf.Approximately(_rigidBody2D.transform.position.y, listPosition[position].transform.position.y))
                {
                    position++;
                    if (position > plusTwo)
                    {
                        yield return new WaitForSeconds(1f);
                        _gameManager.CheckForTheSkipTurn();
                        break;
                    }
                }
                yield return null;
            }
            _gameManager.CheckPlayerTurn();
        }
        //private IEnumerator ForestMove(int position)
        //{
        //    while (_rigidBody2D.transform.position != listPosition[position].transform.position)
        //    {
        //        _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
        //            listPosition[position].transform.position, 2 * Time.deltaTime));
        //        yield return null;
        //    }
        //}

    }
}
