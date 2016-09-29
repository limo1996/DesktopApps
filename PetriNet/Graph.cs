using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUS
{

    public enum Matrix
    {
        Input,
        Output,
        Incidency
    }
    //class that represent PetriNet in matrixes
    public class Graph
    {
        #region class varibales
        private int[,] matrixI = null, matrixO = null, matrixC = null;
        private PetriNet net;
        private Dictionary<Transition, int> transitionToInt = new Dictionary<Transition, int>();
        private Dictionary<int, Transition> intToTransition = new Dictionary<int, Transition>();

        private Dictionary<Place, int> placeToInt = new Dictionary<Place, int>();
        private Dictionary<int, Place> intToPlace = new Dictionary<int, Place>();

        private Queue<Place> placesToVisit = new Queue<Place>();
        private List<Place> visitedPlaces = new List<Place>();

        #endregion
        public Graph(PetriNet net)
        {
            BuildGraph(net);
            GetIncidencyMatrix();
        }

        //builds graph from the given Petri net parsed from the XML file
        void BuildGraph(PetriNet net)
        {
            this.net = net;
            matrixI = new int[net.Places.Count,net.Transitions.Count];
            matrixO = new int[net.Transitions.Count, net.Places.Count];

            for(int i = 0; i < net.Places.Count;i++)
            {
                for(int j = 0;j < net.Transitions.Count;j++)
                {
                    matrixI[i, j] = 0;
                    matrixO[j, i] = 0;
                }
            }

            net.Transitions.Sort((x, y) => x.Label.CompareTo(y.Label));
            net.Places.Sort((x, y) => x.Label.CompareTo(y.Label));

            for (int i = 0; i < net.Transitions.Count;i++ )
            {
                transitionToInt[net.Transitions[i]] = i;
                intToTransition[i] = net.Transitions[i];
            }

            for (int i = 0; i < net.Places.Count; i++)
            {
                placeToInt[net.Places[i]] = i;
                intToPlace[i] = net.Places[i];
            }

                foreach (var arc in net.Arcs)
                {
                    var place = from item in net.Places where item.ID == arc.SourceID select item;
                    if (place == null || place.Count() == 0)
                    {
                        var transition = from item in net.Transitions where item.ID == arc.SourceID select item;
                        var place2 = from item in net.Places where item.ID == arc.DestinationID select item;
                        if (transition != null && place2 != null && place2.Count() != 0)
                        {
                            //this.matrixI[placeToInt[place2.First()], transitionToInt[transition.First()]] = arc.Multiplicity;
                            this.matrixO[transitionToInt[transition.First()], placeToInt[place2.First()]] = arc.Multiplicity;
                        }
                        else
                        {
                            throw new FormatException("Incorrect input net");
                        }
                    }
                    else
                    {
                        var transition2 = from item in net.Transitions where item.ID == arc.DestinationID select item;
                        if (transition2 != null && transition2.Count() != 0)
                        {
                        matrixI[placeToInt[place.First()], transitionToInt[transition2.First()]] = arc.Multiplicity;
                            //this.matrixO[transitionToInt[transition2.First()], placeToInt[place.First()]] = arc.Multiplicity;
                        }
                        else
                        {
                            throw new FormatException("Incorrect input net");
                        }
                    }


                }

        }

        public Graph(Tuple<int[,], int[,], string[], string[], int[]> net)
        {
            this.matrixI = net.Item1;
            this.matrixO = net.Item2;
            for (int i = 0; i < net.Item3.Length; i++)
            {
                intToPlace[i] = new Place()
                {
                    Label = net.Item3[i],
                    ID = i,
                    Tokens = net.Item5[i]
                };
            }

            for (int i = 0; i < net.Item4.Length; i++)
            { 
            
            }
            this.net = new PetriNet() { Label = "WTF" };
        }

        //prints all matrixes 
        public string PrintGraph(Matrix matrix)
        {
            string returned = "";
            switch (matrix)
            {
                case Matrix.Input:
                    {
                        if (matrixI != null)
                        {
                            returned += $"Input Matrix:{Environment.NewLine}";

                            for (int i = 0; i < matrixI.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrixI.GetLength(1); j++)
                                {
                                    returned += string.Format("{0,4}",matrixI[i, j]);
                                }
                                returned += Environment.NewLine;
                            }

                            return returned;
                        }
                        else
                            return null;
                    }

                case Matrix.Output:
                    {
                        if (matrixO != null)
                        {
                            returned += "\nOutput Matrix:";
                            returned += Environment.NewLine;

                            for (int i = 0; i < matrixO.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrixO.GetLength(1); j++)
                                {
                                    returned += string.Format("{0,4}", matrixO[i, j]);
                                }
                                returned += Environment.NewLine;
                            }
                            return returned;
                        }
                        else
                            return null;
                    }

                case Matrix.Incidency:
                    {
                        if (matrixC != null)
                        {
                            returned += "\nIncidency Matrix:";
                            returned += Environment.NewLine;

                            for (int i = 0; i < matrixC.GetLength(0); i++)
                            {
                                for (int j = 0; j < matrixC.GetLength(1); j++)
                                {
                                    returned += string.Format("{0,4}",matrixC[i, j],-3);
                                }
                                returned += Environment.NewLine;
                            }
                            return returned;
                        }
                        else
                            return returned;
                    }
                default:
                    return returned;
        }

            /*Console.WriteLine("\n\nPerforming Reachability algorithm...\n\n" + Reachability());

            Console.WriteLine("\n\nPerforming Searching algorithm...\n\nExist way from {0} to {1} => {2}\n\n\n", intToPlace[0].Label,
                intToPlace[2].Label, ExistWay(intToPlace[0], intToPlace[2]));*/

        }

//functions needed in getting incidency matrixes
#region IncidencyMatrix
        public void GetIncidencyMatrix()
        {
            matrixC = Graph.SubstractMatrixes(Graph.TransposeMatrix(this.matrixO), matrixI);
        }

        //trannspose matrix
        public static int[,] TransposeMatrix(int[,] matrix)
        {
            int[,] transposedMatrix = new int[matrix.GetLength(1), matrix.GetLength(0)];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    transposedMatrix[j, i] = matrix[i, j];
                }
            }
            return transposedMatrix;
        }

        //substract two matrixes 
        public static int[,] SubstractMatrixes(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1.GetLength(0) != matrix2.GetLength(0) || matrix1.GetLength(1) != matrix2.GetLength(1))
            {
                throw new ArgumentException("Matrixes are not in the same format");
            }

            int[,] finalMatrix = new int[matrix1.GetLength(0), matrix1.GetLength(1)];

            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(1); j++)
                {
                    finalMatrix[i,j] = matrix1[i, j] - matrix2[i,j];
                }
            }
            return finalMatrix;
        }
#endregion

//functions that performs different algorithms on petri net
#region PetriNetOperations

        //chcecks wether exist way between 2 places in graph
        bool ExistWay(Place from, Place to)
        {
            int[] Tokens = new int[placeToInt.Count];
            foreach (var item in placeToInt)
            { 
                Tokens[item.Value] = item.Key.Tokens;
            }
            placesToVisit.Enqueue(from);
            Place currentNode = null;
            while (placesToVisit.Count > 0 && visitedPlaces.Count < placeToInt.Count)
            {
                currentNode = placesToVisit.Dequeue();
                if (currentNode == to)
                {
                    visitedPlaces.Add(currentNode);
                    break;
                }
                else if(visitedPlaces.Contains(currentNode))
                {
                    continue;
                }
                else
                {
                    visitedPlaces.Add(currentNode);
                    for (int i = 0; i < matrixI.GetLength(1); i++)
                    {
                        if (matrixI[placeToInt[currentNode], i] != 0)
                        {
                            if (currentNode.Tokens - matrixI[placeToInt[currentNode], i] >= 0)
                            {
                                int tokens = currentNode.Tokens / matrixI[placeToInt[currentNode], i];
                                currentNode.Tokens %= matrixI[placeToInt[currentNode], i];
                                for (int j = 0; j < matrixO.GetLength(1); j++)
                                {
                                    if (matrixO[i, j] != 0)
                                    {
                                        intToPlace[j].Tokens += tokens * matrixO[i, j];
                                            Tokens[j] = intToPlace[j].Tokens;
                                        placesToVisit.Enqueue(intToPlace[j]);
                                    }
                                }
                            }
                        }
                        currentNode.Tokens = Tokens[placeToInt[currentNode]];
                    }                    
                }
            }
            return visitedPlaces.Contains(to);
        }

        //algoritm that performs searching on petri net and finds all reachability states
        public string Reachability()
        {
            string returned = "";
            Queue<Tuple<VectorInt, VectorInt>> toBeVisited = new Queue<Tuple<VectorInt, VectorInt>>();
            List<Tuple<VectorInt, VectorInt>> visited = new List<Tuple<VectorInt, VectorInt>>();
            Tuple<VectorInt,VectorInt> actualPos = Tuple.Create(new VectorInt(intToPlace.Count) { Items = GetTokens() },
                new VectorInt(intToPlace.Count) { Items = GetTokens() });

            returned += actualPos.Item1.ToString() + "\n";
            visited.Add(actualPos);

            for (int i = 0; i < intToTransition.Count; i++)
            {
                VectorInt tmp = CheckBootability(actualPos.Item1, GetTransitionVector(i));
                if (tmp != null && CheckVector(tmp))
                {
                    returned += actualPos.Item2.ToString() + " ";
                    returned += intToTransition[i].Label + "\n";
                    returned += tmp.ToString() + "\n";
                    toBeVisited.Enqueue(Tuple.Create(tmp,actualPos.Item1));
                    //visited.Add(tmp);
                }
            }

            while (toBeVisited.Count > 0)
            { 
                actualPos = toBeVisited.Dequeue();
                if((from item in visited where item.Item1.EqualTo(actualPos.Item1) select item).Count() > 0)
                {
                    continue;
                }

                visited.Add(actualPos);
                for (int i = 0; i < intToTransition.Count; i++)
                {
                    VectorInt tmp = CheckBootability(actualPos.Item1, GetTransitionVector(i));
                    if (tmp != null && CheckVector(tmp))
                    {
                        returned += actualPos.Item1.ToString() + " ";
                        returned += intToTransition[i].Label + "\n";
                        returned += tmp.ToString() + "\n";
                        toBeVisited.Enqueue(Tuple.Create(tmp, actualPos.Item1));
                    }
                }
            }
            return returned;
        }

        //gets tokens at the beginning of the making reachability algorithm
        private int[] GetTokens()
        {
            int[] tokens = new int[intToPlace.Count];
            foreach (var item in intToPlace)
            {
                tokens[item.Key] = item.Value.Tokens;
            }
            return tokens;
        }

        //gets vector that will have on all positions 0 except 'position' where will be 1
        private VectorInt GetTransitionVector(int position)
        {
            if (position >= intToTransition.Count)
                throw new ArgumentOutOfRangeException();
            int[] tmp = new int[intToTransition.Count];
            for (int i = 0; i < intToTransition.Count; i++)
            {
                if (i == position)
                    tmp[i] = 1;
                else
                    tmp[i] = 0;
            }

            return new VectorInt(intToTransition.Count) { Items = tmp };
        }

        //checks wether given transitions is bootable with given position of tokens
        private VectorInt CheckBootability(VectorInt tokens, VectorInt transition)
        {
            if (tokens.N == matrixC.GetLength(0) && transition.N == matrixC.GetLength(1))
            {
                VectorInt returned = new VectorInt(matrixC.GetLength(0));

                for (int i = 0; i < matrixC.GetLength(0); i++)
                {
                    int sum = 0;
                    for (int j = 0; j < matrixC.GetLength(1); j++)
                    {
                        sum += matrixC[i, j] * transition[j];
                    }
                    returned[i] = sum + tokens[i];
                }
                return returned;
            }
            return null;
        }
        //checks wether all items in vector are greater tahn zero
        private bool CheckVector(VectorInt v)
        {
            for (int i = 0; i < v.N; i++)
            {
                if (v[i] < 0)
                    return false;
            }
            return true;
        }
#endregion

        public VectorInt PInvariant()
        {
            List<int> vektor = null;
            for(int i = 1; i < Math.Pow(10,this.intToPlace.Count);i++)
            {
                vektor = PridajCislo(intToPlace.Count, i);
                var tmp = Vynasob1(vektor);
                if(tmp != null && tmp.IsNullVector() && NoNulls(vektor))
                {
                    return new VectorInt(vektor.ToArray());
                }
            }
            return null;
        }

        public VectorInt TInvariant1()
        {
            List<int> vektor = null;
            for (int i = 1; i < Math.Pow(10, this.intToTransition.Count); i++)
            {
                vektor = PridajCislo(intToTransition.Count, i);
                var tmp = Vynasob2(vektor);
                if (tmp != null && tmp.IsNullVector()  && NoNulls(vektor))
                {
                    return new VectorInt(vektor.ToArray());
                }
            }
            return null;
        }

        public VectorInt TInvariant2()
        {
            List<int> vektor = null;
            for (int i = 1; i < Math.Pow(10, this.intToTransition.Count); i++)
            {
                vektor = PridajCislo(intToTransition.Count, i);
                var tmp = Vynasob2(vektor);
                if (tmp != null && tmp.IsNullVector())
                {
                    return new VectorInt(vektor.ToArray());
                }
            }
            return null;
        }

        private bool NoNulls(List<int> tmp)
        {
            foreach(int i in tmp)
            {
                if (i == 0)
                    return false;
            }
            return true;
        }

        public VectorInt Vynasob1(List<int> vector)
        {
            List<int> returned = new List<int>();
            int sum;
            for(int i = 0; i < matrixC.GetLength(1);i++)
            {
                sum = 0;
                for(int j =0; j < matrixC.GetLength(0);j++)
                {
                    sum += matrixC[j, i] * vector[j];
                }
                returned.Add(sum);
            }
            return new VectorInt(returned.ToArray());
        }

        public VectorInt Vynasob2(List<int> vector)
        {
            List<int> returned = new List<int>();
            int sum;
            for (int i = 0; i < matrixC.GetLength(0); i++)
            {
                sum = 0;
                for (int j = 0; j < matrixC.GetLength(1); j++)
                {
                    sum += matrixC[i, j] * vector[j];
                }
                returned.Add(sum);
            }
            return new VectorInt(returned.ToArray());
        }
        //789
        //00789
        private List<int> PridajCislo(int dlzka, int cislo)
        {
            List<int> returned = new List<int>();
            int pocetCifier = 1, i = 1;
            int tmp = cislo;

            while ((tmp / 10) != 0)
            {
                tmp /= 10;
                pocetCifier++;
                i++;
            }

            if(pocetCifier > dlzka)
            {
                return null;
            }

            for(int j = 0; j < dlzka - pocetCifier; j++)
            {
                returned.Add(0);
            }

            while((cislo / 10) != 0)
            {
                returned.Add(cislo % 10);
                cislo /= 10;
            }

            returned.Add(cislo % 10);

            //returned.Reverse();
            return returned;
        }
    }

    public class Item
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Tokens { get; set; }
    }
}
