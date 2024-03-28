using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace NightKeepers
{
    public class TimeManager : MonoBehaviour
    {
        public static TimeManager Instance { get; private set; }

        private float _globalTime;
        private bool _isDay = true; 

        public float GlobalTime // Use Access the time from other scripts
        {
            get { return _globalTime; }
        }

        public bool IsDay // Use Access the day events from other scripts
        {
            get { return _isDay; }
        }
        
        void Awake()
        {
            ;
            if (Instance == null)  
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            _globalTime += Time.deltaTime;

            
            if (_globalTime >= 10f)
            {
                
                _globalTime = 0f;
                _isDay = !_isDay;
            }
            Debug.Log(IsDay);
            Debug.LogError(_globalTime);
        }
    }

}
