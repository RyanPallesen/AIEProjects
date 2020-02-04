using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisPiece
{

    public enum ResourceType //note; randomize between two of the existing resources
    {
        Randomized,
        Air,
        Metal,
        Fuel,
        Flexible
    }

    public enum PieceType
    {
        RandomNonDot,
        Random,
        Dot,
        I,
        S,
        Z,
        O,
        T,
        L,
        J,
    }

    [System.Serializable]
    public class Square
    {
        public int xOffset;
        public int yOffset;

         public ResourceType resourceType;
    }

    public List<Square> squares = new List<Square>();
}
