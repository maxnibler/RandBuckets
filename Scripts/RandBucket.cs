using System.Collections.Generic;
using System;

namespace mnibler.Randomization
{
    public class RandBucket<T>
    {
        private int TotalWeight;
        private List<SelectorNode> Pool;
        private System.Random Rand;

        public RandBucket()
        {
            Rand = new System.Random();
            TotalWeight = 0;
            Pool = new List<SelectorNode>();
        }

        public void Add(T item, int weight)
        {
            SelectorNode newNode = new SelectorNode(TotalWeight, TotalWeight+weight, item);
            Pool.Add(newNode);
            TotalWeight += weight;
        }

        public T Random()
        {
            SelectorNode node = GetEnabled();
            return node.Value;
        }

        private SelectorNode GetEnabled()
        {
            int safety = 9999;
            do
            {
                double i = Rand.NextDouble() * TotalWeight; 
                SelectorNode node = Get(i);
                safety--;
                if (safety < 0) return node;
            } while (node.Disabled);
            return node;
        }

        private SelectorNode Get(double i)
        {
            //todo : improve this with a binary search
            foreach (SelectorNode node in Pool)
            {
                // Debug.Log(string.Format("{0}, {1} - {2}", node.Low, node.High, i));
                if (i > node.Low && i <= node.High)
                {
                    return node;
                }
            }
            throw new Exception("Random value not within pool");
        }

        private class SelectorNode
        {
            public int Low;
            public int High;
            public T Value;
            public bool Disabled;
            public SelectorNode(int low, int high, T value)
            {
                Low = low;
                High = high;
                Value = value;
                Disabled = false;
            }
        }
    }
}