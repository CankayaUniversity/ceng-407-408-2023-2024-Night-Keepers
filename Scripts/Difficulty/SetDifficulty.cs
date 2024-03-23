using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NightKeepers.Difficulty
{
    public class SetDifficulty : MonoBehaviour
    {
       
        public Difficult Difficult;

        public void _SetDifficulty(int difficult)
        {
            switch (difficult)
            {
                case 1:    
                    Difficult.health = 10;
                    Difficult.damage = 5;
                    Difficult.spawnCount = 5;
                    Debug.Log("Easy");
                    break;
                case 2:
                    Difficult.health = 20;
                    Difficult.damage = 10;
                    Difficult.spawnCount = 10;
                    Debug.Log("Normal");
                    break;
                case 3:
                    Difficult.health = 30;
                    Difficult.damage = 15;
                    Difficult.spawnCount = 15;
                    Debug.Log("Hard");
                    break;
            }
        }

    }
}
