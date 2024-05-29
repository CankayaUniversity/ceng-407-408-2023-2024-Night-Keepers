using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NightKeepers
{
    public class WallManager : Singleton<WallManager>
    {
        [SerializeField] private List<Building> _parentWallsList;
        public List<MeshFilter> _wallMeshFilters;
    }
}
