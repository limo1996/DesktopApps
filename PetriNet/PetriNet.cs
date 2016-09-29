using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUS
{
    //class that holds all the data parsed from XML
    public class PetriNet
    {
        private List<Place> _places;
        private List<Arc> _arcs;
        private List<Transition> _transitions;

        public List<Place> Places 
        {
            get
            {
                return _places;
            }
            set
            {
                _places = value;
            }
        }

        public List<Arc> Arcs
        {
            get 
            {
                return _arcs;
            }
            set
            {
                _arcs = value;
            }
        }

        public List<Transition> Transitions
        {
            get
            {
                return _transitions;
            }
            set
            {
                _transitions = value;
            }
        }

        public int? X { get; set; }
        public int? Y { get; set; }
        public string Label { get; set; }
        public int? ID { get; set; }


    }
}
