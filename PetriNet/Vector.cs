using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUS
{
    //class that represents vector of N dimenstions of type int
    public class VectorInt 
    {
        public int N 
        {
            get
            {
                return n;
            }
            set 
            {
                n = value;
                _vector = new int[value];
                for(int i = 0; i < value; i++)
                    _vector[i] = 0;
            }
        }
        private int n = 0;

        public int[] Items
        {
            get
            {
                return _vector;
            }
            set
            {
                if (value.Length == N)
                {
                    _vector = value;
                }
            }        
        }
        
        //enabled operator []
        public int this[int key]
        {
            get 
            {
                if (key < N)
                {
                    return _vector[key];
                }
                else
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array !");
                }
            }
            set
            {
                if (key < N)
                {
                    _vector[key] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array !");
                }
            }
        }
        
        private int[] _vector;

        public VectorInt(int n)
        {
            this.N = n;
        }

        public VectorInt(params int[]items)
        {
            this.N = items.Length;
            for (int i = 0; i < N; i++)
            {
                this[i] = items[i];
            }
        }

        // + operator enabled
        public static VectorInt operator +(VectorInt v1, VectorInt v2)
        {
            if (v1.N == v2.N)
            {
                VectorInt vector = new VectorInt(v1.N);
                for (int i = 0; i < vector.N; i++)
                {
                    dynamic a = v1[i];
                    dynamic b = v2[i];
                    vector[i] = a + b;
                }
                return vector;
            }
            else
            {
                throw new ArgumentException("Given vectors can not be added");
            }
        }

        /*public static bool operator ==(VectorInt v1, VectorInt v2)
        {
            if (v1 == null)
                return false;
            if (v2 == null)
                return false;
            if (v1.Equals(v2))
                return true;

            if (v1.N != v2.N)
                return false;
            for (int i = 0; i < v1.N; i++)
            {
                if(v1[i] != v2[i])
                    return false;
            }
            return true;
        }

        public static bool operator !=(VectorInt v1, VectorInt v2)
        {
            if (v1 == v2)
                return true;
            if (v1.N != v2.N)
                return true;
            for (int i = 0; i < v1.N; i++)
            {
                if (v1[i] != v2[i])
                    return true;
            }
            return false;
        }*/

        //checks if are 2 vectors equal
        public bool EqualTo(VectorInt other)
        {
            if (other == null)
                return false;
            if (this.N != other.N)
                return false;
            for (int i = 0; i < N; i++)
            {
                if (this[i] != other[i])
                    return false;
            }
            return true;
        }

        // returns something like that 1,5,3,6, for vector with items {1,5,3,6}
        public override string ToString()
        {
            string returned = "";
            foreach (var item in Items)
            {
                returned += item + ",";
            }
            return returned;
        }

        public bool IsNullVector()
        {
            foreach(int i in _vector)
            {
                if (i != 0)
                    return false;
            }
            return true;
        }
    }
}