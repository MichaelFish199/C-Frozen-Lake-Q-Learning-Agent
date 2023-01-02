using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Project
{
    class Q_Learning
    {
        // Parameters for Q_Learning algorithm
        public int Max_Steps_Per_Episode { get; set; }
        public double Learning_Rate { get; set; }
        public double Discount_Rate { get; set; }
        public double Exploration_Rate { get; set; }
        public double Max_Exploration_Rate { get; set; }
        public double Min_Exploration_Rate { get; set; }
        public double Exploration_Decay_Rate { get; set; }

        public static Environment env;

        private double[,] q_Table;
        private double[] state_rewards;

        public Q_Learning(
            Environment environment,
            int Max_Steps_Per_Episode = 100,
            double Learning_Rate = 0.1,
            double Discount_Rate = 0.99,
            double Exploration_Rate = 1,
            double Max_Exploration_Rate = 1,
            double Min_Exploration_Rate = 0.01,
            double Exploration_Decay_Rate = 0.001
            )
        {
            env = environment;
            this.Max_Steps_Per_Episode = Max_Steps_Per_Episode;
            this.Learning_Rate = Learning_Rate;
            this.Discount_Rate = Discount_Rate;
            this.Exploration_Rate = Exploration_Rate;
            this.Max_Exploration_Rate = Max_Exploration_Rate;
            this.Min_Exploration_Rate = Min_Exploration_Rate;
            this.Exploration_Decay_Rate = Exploration_Decay_Rate;
        }

        public void Learn(int num_episodes = 10_000, int num_console_outputs = 10)
        {
            // Creating Q-Table
            q_Table = new double[env.Observation_Space_N, env.Action_Space_N];
            Array.Clear(q_Table, 0, q_Table.Length);
            state_rewards = new double[q_Table.GetLength(1)];

            // Declaring other variables
            Random rand = new Random();
            double reward, agent_hp, exploration_rate_threshold;
            bool done;
            int action, state, new_state;
            float outputs = num_episodes / num_console_outputs;

            // History of rewards and exploration rate
            Console.WriteLine("Mean of rewards and exploration rate per last " + outputs + " episodes: \n");
            List<double> rewards_all_episodes = new List<double>();
            List<double> exploration_rate_all_episodes = new List<double>();

            // Q_Learning algorithm
            for (int episode = 0; episode < num_episodes; episode++)
            {
                state = env.Reset();
                done = false;
                double rewards_current_episode = 0;

                for (int step = 0; step < Max_Steps_Per_Episode; step++)
                {
                    // Exploration-exploitation trade-off
                    exploration_rate_threshold = rand.NextDouble();
                    if (exploration_rate_threshold > Exploration_Rate)
                    {
                        action = Predict(state);
                    }
                    else
                        action = env.Random_Action();

                    // Taking action in environment
                    (new_state, reward, done, agent_hp) = env.Step(action);

                    // Update Q-table for Q(state, action)
                    q_Table[state, action] = q_Table[state, action] * (1 - Learning_Rate) +
                        Learning_Rate * (reward + Discount_Rate * rewards(new_state).Max());

                    state = new_state;
                    rewards_current_episode += reward;

                    if (done == true)
                        break;
                }

                // Exploration rate decay
                Exploration_Rate = Min_Exploration_Rate +
                (Max_Exploration_Rate - Min_Exploration_Rate) *
                Math.Exp(-Exploration_Decay_Rate * episode);

                // Appending exploration rate and rewards
                rewards_all_episodes.Add(rewards_current_episode);
                exploration_rate_all_episodes.Add(Exploration_Rate);

                // Displaying average of exploration rate and reward during training
                if (episode % outputs == 0)
                {
                    double avg_exploration_rate = Math.Round(exploration_rate_all_episodes.Average(), 2);
                    double avg_reward = Math.Round(rewards_all_episodes.Average(), 2);
                    Console.WriteLine(episode + " episodes | exploration rate = " + avg_exploration_rate + " | average reward = " + avg_reward);
                    rewards_all_episodes.Clear();
                    exploration_rate_all_episodes.Clear();
                }
            }
        }

        public int Predict(int state)
        {
            // Returns action that gives the highest reward
            state_rewards = rewards(state);

            return Array.IndexOf(state_rewards, state_rewards.Max());
        }

        public void DisplayQTable()
        {
            for (int i = 0; i < env.Observation_Space_N; i++)
            {
                for (int j = 0; j < env.Action_Space_N; j++)
                {
                    Console.Write(q_Table[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        public void Save(string filename)
        {
            // Convert q_Table to a string representation
            string qTableString = ConvertToString(q_Table);

            // Write the string to a file
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), filename), qTableString);
        }

        public void Load(string filePath)
        {
            // Read the contents of the file as a string
            string qTableString = File.ReadAllText(filePath);

            // Convert the string to a q_Table
            q_Table = ConvertFromString(qTableString);
        }


        // ******** Private methods below ******** 


        private double[] rewards(int state)
        {
            // Returns rewards at given state
            for (int i = 0; i < state_rewards.Length; i++)
                state_rewards[i] = q_Table[state, i];

            return state_rewards;
        }

        private string ConvertToString(double[,] q_Table)
        {
            // Create a string builder to store the string representation of the q_Table
            StringBuilder sb = new StringBuilder();

            // Create a new instance of NumberFormatInfo
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            // Iterate through the rows and columns of the q_Table
            for (int row = 0; row < q_Table.GetLength(0); row++)
            {
                for (int col = 0; col < q_Table.GetLength(1); col++)
                {
                    // Append the value at the current cell to the string builder using the modified NumberFormatInfo object
                    sb.Append(q_Table[row, col].ToString("F", nfi));

                    // Add a comma after each value, except for the last value in the row
                    if (col < q_Table.GetLength(1) - 1)
                        sb.Append(',');
                }
                // Add a newline after each row, except for the last row
                if (row < q_Table.GetLength(0) - 1)
                    sb.AppendLine();
            }

            // Return the string representation of the q_Table
            return sb.ToString();
        }

        private double[,] ConvertFromString(string qTableString)
        {
            // Create a new instance of NumberFormatInfo
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";

            // Split the string into rows
            string[] rows = qTableString.Split('\n');

            // Create a 2D array to store the values of the q_Table
            double[,] qTable = new double[rows.Length, rows[0].Split(',').Length];

            // Iterate through the rows and columns of the q_Table
            for (int row = 0; row < qTable.GetLength(0); row++)
            {
                // Split the current row into columns
                string[] cols = rows[row].Split(',');

                for (int col = 0; col < qTable.GetLength(1); col++)
                    // Convert the string value at the current cell to a double and store it in the q_Table
                    qTable[row, col] = double.Parse(cols[col], NumberStyles.Float, nfi);
            }

            // Return the q_Table
            return qTable;
        }

    }
}
