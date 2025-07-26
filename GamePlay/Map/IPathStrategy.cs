using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay {

    public interface IPathStrategy {
        List<Vector2Int> CreatePathPoints(int x, int y);
    }

    
}
