namespace DuRound.Camera
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DuRound.Manager;
    public class CinemachineCameraChange : MonoBehaviour
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

        public void ChangeCamera()
        {
            if (_gameManager.GetPlayerTurn())
            {
               // _gameManager.GetCineMachineVirtual().
            }
        }
    }
}
