using System;

namespace Project
{
    partial class Environment
    {
        // Declaring variables
        public int Map_Level { get; set; }
        public int Action_Space_N;
        public int Observation_Space_N;

        private int map_n_rows;
        private int map_n_cols;
        private string[,] map;
        private string tile_name;
        private int x_agent_position;
        private int y_agent_position;
        private int temp_x_agent_position;
        private int temp_y_agent_position;
        private int observation;
        private double reward;
        private bool done;
        private double agent_hp;
        private Random random = new Random();

        public Environment(int level = 0)
        {
            // Sets what map to use
            Map_Level = level;

            // Resets environment
            Reset();

            // Initialize variable
            map_n_rows = map.GetLength(0);
            map_n_cols = map.GetLength(1);

            Action_Space_N = action_space.Count;
            Observation_Space_N = map_n_rows * map_n_cols;
        }

        public (int, double, bool, double) Step(int action)
        {
            // Saving agent position
            temp_x_agent_position = x_agent_position;
            temp_y_agent_position = y_agent_position;

            // Making action
            x_agent_position += action_space[action][0];
            y_agent_position += action_space[action][1];
            tile_name = map[y_agent_position, x_agent_position];

            // Wall tile
            if (tile_name == "#")
            {
                x_agent_position = temp_x_agent_position;
                y_agent_position = temp_y_agent_position;
                tile_name = map[y_agent_position, x_agent_position];
            }

            // Environment response
            tile_name_updates();
            get_observation();
            get_done();

            return (observation, reward, done, agent_hp);
        }

        public int Reset()
        {
            // Resets environment
            string[,] copiedMap = (string[,])maps[Map_Level].Clone();
            map = (string[,])copiedMap.Clone();

            // Searching for start tile
            for (int i = 0; i < map_n_rows; i++)
                for (int j = 0; j < map_n_cols; j++)
                    if (map[i, j] == "S")
                    {
                        y_agent_position = i;
                        x_agent_position = j;
                        break;
                    }

            tile_name = map[y_agent_position, x_agent_position];
            agent_hp = 100;
            done = false;

            get_observation();

            return observation;
        }

        public void Render() // TODO try changing into new window
        {
            // Clears Console
            Console.Clear();

            // Renders game
            for (int i = 1; i < map_n_rows-1; i++)
            {
                for (int j = 1; j < map_n_cols-1; j++)
                {
                    if (i == y_agent_position && j == x_agent_position) // Agent
                        Console.BackgroundColor = ConsoleColor.Blue;

                    Console.Write(map[i, j] + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.WriteLine();
            }
        }

        public int Random_Action()
        {
            // Returns random action
            return random.Next(0, 4);
        }


        // ******** Private methods below ******** 


        private void get_observation()
        {
            // Updates agent position as if array was 1dimensional
            observation = y_agent_position * map_n_cols + x_agent_position;
        }

        private void tile_name_updates()
        {
            // Updates reward and agent_hp based on agent position
            agent_hp += hp_penalties[tile_name];
            reward = rewards[tile_name];

            // Removes reward from the map
            if (tile_name == "+")
                map[y_agent_position, x_agent_position] = " ";

            // Condition for "~"(slipery) tile
            if (tile_name == "~")
            {
                // Probability of slipping to another tile
                double probability = 0.33;
                if (random.NextDouble() < probability)
                    Step(Random_Action());
            }
        }

        private void get_done()
        {
            // Updates true if game has ended
            if (tile_name == "E" || agent_hp <= 0)
                done = true;
            else
                done = false;
        }
    }
}
