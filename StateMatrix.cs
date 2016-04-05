using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TSP
{
    class StateMatrix
    {
        //initial matrix that children span from
        private double[,] initMatrix;
        private double[,] matrix;

        //best solution so far
        private double bssf;
        private double lowerbound;

        private City[] Cities;
        private int numberCities;
        
        ArrayList route;
        ArrayList row;
        ArrayList col;

        //init constructor
        public StateMatrix(City[] Cities)
        {
            this.Cities = Cities;

            //the number of cities or vertexes in the graph
            numberCities = Cities.Length;

            initMatrix = new double[numberCities, numberCities];
            initMatrix = createMatrix();

            //init bssf
            bssf = 0;
            getBSSF(initMatrix);

            reduction(initMatrix);

           // route = new ArrayList();
            row = new ArrayList();
            col = new ArrayList();
            
            //route.Add(0);
            row.Add(0);
            col.Add(0);
            
        }

        //constructor for each child
        public StateMatrix(int numberCities)
        {
            matrix = new double[numberCities, numberCities];

            //route = new ArrayList();
            row = new ArrayList();
            col = new ArrayList();

            this.numberCities = numberCities;
        }

        //initialize a 2 dimesional matrix. O(n^2)
        private double[,] createMatrix()
        {
            //min = new double[numberCities];
            for (int i = 0; i < numberCities; i++)//rows O(n)
            {
                //min[i] = double.PositiveInfinity;
                for (int j = 0; j < numberCities; j++)//columns O(n)
                {
                    if (i == j)
                        initMatrix[i, j] = double.PositiveInfinity;
                    else
                        initMatrix[i, j] = Cities[i].costToGetTo(Cities[j]);
                }
            }
            return initMatrix;
        }

        // A Greedy approach that picks the closest vertex distance
        private void getBSSF(double[,] matrix)
        {
            HashSet<int> cityVisited = new HashSet<int>();

            int tempFrom = 0;
            int connectFrom = 0;
            int connectTo = 0;

            for (int i = 0; i < Cities.Length; i++)//rows O(n)
            {
                //start at city 0
                cityVisited.Add(connectFrom);

                if (i == Cities.Length - 1)
                {
                    cityVisited.Remove(0);
                }
                double cost = double.PositiveInfinity;
                for (int j = 0; j < Cities.Length; j++)//columns O(n)
                {
                    if (!cityVisited.Contains(j))
                    {
                        connectTo = j;
                        double connectionCost = matrix[connectFrom, connectTo];
                        if (connectionCost < cost)
                        {
                            cost = connectionCost;
                            tempFrom = j;
                        }
                    }
                }
                connectFrom = tempFrom;
                bssf += cost;
            }
        }

        private void reduction(double[,] matrix)
        {
            double rowMin;
            double colMin;
      
            for (int i = 0; i < numberCities; i++)
            {
                rowMin = double.PositiveInfinity;
                for (int j = 0; j < numberCities; j++)
                {
                    if (matrix[i, j] < rowMin)
                        rowMin = matrix[i, j];
                }

             
                //skip the additional loop if min is 0
                if (rowMin == 0)
                    continue;
                if (rowMin == double.PositiveInfinity)
                    continue;
                lowerbound += rowMin;
                /*
                if (lowerbound == double.PositiveInfinity)
                    return;*/

                for (int j = 0; j < numberCities; j++)
                {
                    matrix[i, j] = matrix[i, j] - rowMin;
                }
            }

            for (int i = 0; i < numberCities; i++)
            {
                colMin = double.PositiveInfinity;
                for (int j = 0; j < numberCities; j++)
                {
                    if (matrix[j, i] < colMin)
                        colMin = matrix[j, i];
                }

              
                if (colMin == 0)
                    continue;
                if (colMin == double.PositiveInfinity)
                    continue;
                lowerbound += colMin;
                /*
                if (lowerbound == double.PositiveInfinity)
                    return;*/

                for (int j = 0; j < numberCities; j++)
                {
                    matrix[j, i] = matrix[j, i] - colMin;
                }  
            }
            this.matrix = matrix;
        }

        //Creates a new StateMatrix that copies lowerbound, matrix, and route
        private StateMatrix deepcopy(StateMatrix parent)
        {
            StateMatrix state = new StateMatrix(numberCities);
            state.setLowerBound(parent.lowerbound);

            for (int i = 0; i < numberCities; i++)//rows O(n)
            {
                if (row.Count > i)
                {
                    
                    //state.route.Add((int)route[i]);
                    state.row.Add( parent.row[i]);
                    state.col.Add( parent.col[i]);
                }
                for (int j = 0; j < numberCities; j++)//columns O(n)
                {
                    state.matrix[i, j] = parent.matrix[i, j];
                }
            }
            return state;
        }

         public StateMatrix createChildren(StateMatrix parent)
        {
                StateMatrix state = deepcopy(parent);
                printMatrix(state.matrix);

                int row = (int)state.row[state.row.Count - 1];

                int nextCol = (int)col[col.Count - 1] + 1;
                col.Add(nextCol);
                int column = nextCol;

                state.lowerbound += state.matrix[row,column];

                state.matrix[column, row] = double.PositiveInfinity;
                for (int i = row; i < numberCities; ++i)
                {
                    state.matrix[row, i] = double.PositiveInfinity;
                    state.matrix[i, column] = double.PositiveInfinity;
                }
                printMatrix(state.matrix);
                reduction(state.matrix);

                printMatrix(state.matrix);
            return state; 
        }


        public ArrayList getRoute()
        {
            return route;
        }

        /*
        public ArrayList setRoute()
        {
            route = new ArrayList();

            int index = 0;
            for (int i = 0; i < Cities.Length; ++i)//O(n)
            {
                route.Add(i);
            }
        }*/





        /*
    public ArrayList route()
    {
        ArrayList route = new ArrayList();

        int index = 0;
        for (int i = 0; i < Cities.Length; ++i)//O(n)
        {
            route.Add(Cities[index]);
        }
        return route;
    }*/

        public void printMatrix(double[,] matrix)
        {
            for (int i = 0; i < numberCities; ++i)
            {
                for (int j = 0; j < numberCities; ++j)
                {
                    String cost = matrix[i, j].ToString().PadLeft(5, ' ');
                    if (matrix[i, j] == double.PositiveInfinity)
                        cost = "   --";
                    Console.Out.Write(cost);
                }
                Console.Out.WriteLine();
            }

            Console.Out.WriteLine("Reduced Cost Matrix {0}x{0}, Lower Bound: {1}", numberCities, lowerbound);

            //Console.Out.Write("Solution({0}): [{1}]", level, string.Join(", ", exited.ToArray()));

            Console.Out.WriteLine("\r\n");
         }

        public void setLowerBound(double lowerbound)
        {
            this.lowerbound = lowerbound;
        }

        public double getLowerBound()
        {
            return lowerbound;
        }

        public double[,] getMatrix()
        {
            return matrix;
        }

        public double[,] getInitMatrix()
        {
            return initMatrix;
        }

        public double getBSSF()
        {
            return bssf;
        }
    }
}
