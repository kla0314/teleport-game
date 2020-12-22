using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StageUtils
{
    public class Directions
    {
        public enum DirectionFacing : int
        {
            LEFT = -1,
            RIGHT = 1,
        }

        // Helper function to convert direction to a Vector3
        public Vector3 GetVectorDirection(DirectionFacing directionFacing)
        {
            return new Vector3((int) directionFacing, 0, 0);
        }
    }
}
