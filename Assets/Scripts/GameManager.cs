using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound.GameManager
{
    public class GameManager : MonoBehaviour
    {
        public List<int> numberOfPlayers;
        public int currentPlayerTurn;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private IEnumerator StartDicing()
        {
            currentPlayerTurn += 1;
            yield return null;
        }
    }
}
