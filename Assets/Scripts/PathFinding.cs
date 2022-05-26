using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound.ThePath
{
    public class PathFinding : MonoBehaviour
    {
        private Transform[] _listOfWayPoints { get; set; }

        private void Awake()
        {
            var numOfPoints = transform.childCount;
            _listOfWayPoints = new Transform[numOfPoints];
            for (int points = 0; points < _listOfWayPoints.Length; points++)
            {
                _listOfWayPoints[points] = transform.GetChild(points).transform;
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
        public Transform[] GetListPoints() { return _listOfWayPoints; }
    }
}
