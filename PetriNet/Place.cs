using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUS
{
    public class Place
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Tokens { get; set; }
        public string Label { get; set; }
        public bool IsStatic { get; set; }

       /* public static bool operator ==(Place p1, Place p2)
        {
            if (p1 != null && p2 != null)
            {
                return p1.ID == p2.ID && p1.IsStatic == p2.IsStatic && p1.Label == p2.Label
                    && p1.Tokens == p2.Tokens && p1.X == p2.X && p1.Y == p2.Y;
            }
            else
            {
                throw new ArgumentNullException("argument can not be null");
            }
        }

        public static bool operator !=(Place p1, Place p2)
        {
            if (p1 != null && p2 != null)
            {
                return p1.ID != p2.ID || p1.IsStatic != p2.IsStatic || p1.Label != p2.Label
                    || p1.Tokens != p2.Tokens && p1.X != p2.X && p1.Y != p2.Y;
            }
            else
            {
                throw new ArgumentNullException("argument can not be null");
            }
        }

        */
    }
}
