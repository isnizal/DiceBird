using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuRound.ThePath;

namespace DuRound.Minion {
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private PathFinding _pathFinding;
        private Transform[] listPosition;
        private Rigidbody2D _rigidBody2D;

        private int amountOfTurn { get; set; }
        private int incrementPosition { get; set; }

        private void Awake()
        {
            _pathFinding = GameObject.Find("PathFinding").GetComponent<PathFinding>();
            incrementPosition = 1;
        }
        // Start is called before the first frame update
        void Start()
        {
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

        private void MovePlayer(int currentPointPosition)
        {
            if (incrementPosition < currentPointPosition)
            {
                _rigidBody2D.MovePosition(Vector2.MoveTowards(_rigidBody2D.transform.position,
                    listPosition[incrementPosition].transform.position, 5f));
            }
        }
        //private IEnumerator MovePosition(int currentPosition)
        //{
        //    if (incrementPosition < currentPosition)
        //    {
        //        while (incrementPosition < listPosition[incrementPosition])
        //        {
        //            
        //        }
        //    }
        //}
        
    }
}
