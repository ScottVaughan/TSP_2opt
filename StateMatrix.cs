

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
