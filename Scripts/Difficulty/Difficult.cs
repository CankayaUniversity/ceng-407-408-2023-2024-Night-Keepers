using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NightKeepers.Difficulty
{
    [CreateAssetMenu(fileName = "New Difficulty", menuName = "Difficulty")]
    
    public class Difficult : ScriptableObject
    {
        
        public int health;
        public int damage;
        public int spawnCount;
    
    }

}
