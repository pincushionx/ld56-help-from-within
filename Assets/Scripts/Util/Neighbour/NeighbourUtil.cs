using System;
using System.Collections.Generic;
using UnityEngine;

namespace Oneill
{
    public static class NeighbourUtil
    {
        public static readonly int NumNeighbours = 6;
        public static int GetNeighbourIndex(Neighbour neighbour)
        {
            switch (neighbour)
            {
                case Neighbour.Up:
                    return 0;
                case Neighbour.Down:
                    return 1;
                case Neighbour.Left:
                    return 2;
                case Neighbour.Right:
                    return 3;
                case Neighbour.Front:
                    return 4;
                case Neighbour.Back:
                    return 5;
            }
            return -1;
        }
        public static Neighbour GetNeighbourByIndex(int neighbourIndex)
        {
            switch (neighbourIndex)
            {
                case 0:
                    return Neighbour.Up;
                case 1:
                    return Neighbour.Down;
                case 2:
                    return Neighbour.Left;
                case 3:
                    return Neighbour.Right;
                case 4:
                    return Neighbour.Front;
                case 5:
                    return Neighbour.Back;
            }
            return Neighbour.All;
        }
        /// <summary>
        /// Returns the neighbour by the given offset.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Neighbour GetNeighbourByOffset(Vector3 offset)
        {
            Neighbour neighbour = Neighbour.None;
            if (offset.x < 0)
            {
                neighbour |= Neighbour.Left;
            }
            if (offset.x > 0)
            {
                neighbour |= Neighbour.Right;
            }
            if (offset.y < 0)
            {
                neighbour |= Neighbour.Down;
            }
            if (offset.y > 0)
            {
                neighbour |= Neighbour.Up;
            }
            if (offset.z < 0)
            {
                neighbour |= Neighbour.Front;
            }
            if (offset.z > 0)
            {
                neighbour |= Neighbour.Back;
            }
            return neighbour;
        }
        public static Neighbour GetBestNeighbourByOffset(Vector2 offset)
        {
            if (offset.x < 0)
            {
                return Neighbour.Left;
            }
            if (offset.x > 0)
            {
                return Neighbour.Right;
            }
            if (offset.y < 0)
            {
                return Neighbour.Down;
            }
            if (offset.y > 0)
            {
                return Neighbour.Up;
            }
            return Neighbour.None;
        }
        public static Neighbour[] GetNeighboursFromMask(Neighbour neighbourMask)
        {
            int neighbourCount = IntUtil.CountSetBits((int)neighbourMask);
            Neighbour[] neighbours = new Neighbour[neighbourCount];

            int neighbourIndex = 0;
            for (int i = 0; i < Neighbours.Length; i++)
            {
                Neighbour compareNeighbour = Neighbours[i];
                if ((compareNeighbour & neighbourMask) > 0)
                {
                    neighbours[neighbourIndex++] = compareNeighbour;
                }
            }
            return neighbours;
        }

        public static Neighbour GetOppositeNeighbour(Neighbour neighbour)
        {
            switch (neighbour)
            {
                case Neighbour.Up:
                    return Neighbour.Down;
                case Neighbour.Down:
                    return Neighbour.Up;
                case Neighbour.Left:
                    return Neighbour.Right;
                case Neighbour.Right:
                    return Neighbour.Left;
                case Neighbour.Front:
                    return Neighbour.Back;
                case Neighbour.Back:
                    return Neighbour.Front;
            }
            return Neighbour.None;
        }

        private static readonly Neighbour[] NeighboursY =
        {
            Neighbour.Front,
            Neighbour.Left,
            Neighbour.Back,
            Neighbour.Right
        };
        private static readonly Neighbour[] Neighbours =
        {
            Neighbour.Up,
            Neighbour.Down,
            Neighbour.Front,
            Neighbour.Left,
            Neighbour.Back,
            Neighbour.Right,
        };

        public static string PrintNeighbourNames(Neighbour neighbourMask)
        {
            if (neighbourMask == Neighbour.None)
            {
                return Neighbour.None.ToString();
            }

            Neighbour[] neighbours = GetNeighboursFromMask(neighbourMask);
            string neighbourString = "";
            for (int i = 0; i < neighbours.Length; i++)
            {
                if (i > 0)
                {
                    neighbourString += ", ";
                }
                neighbourString += neighbours[i].ToString();
            }
            return neighbourString;
        }
    }
}