using System;
using System.Threading;

namespace Project
{
    class Program
    {
        static void Main(string[] args)
        {
            //RandomGame(level: 0, num_episodes: 10);

            // *** Diffrent maps using Q-Learning *** 
            //FrozenLake(30);
            //ActiveVolcano(30);
            //DeadlyCorridor(30);
            //DeadlyMaze(30);
            //BurningSands(30);
        }

        static void BurningSands(int num_episodes = 5)
        {
            // Creating environment
            Environment env = new Environment(4);

            // Creating model
            Q_Learning model = new Q_Learning(
                env,
                Max_Steps_Per_Episode: 100,
                Learning_Rate: 0.05,
                Discount_Rate: 0.99,
                Exploration_Rate: 1,
                Max_Exploration_Rate: 1,
                Min_Exploration_Rate: 0.1,
                Exploration_Decay_Rate: 0.000001
            );

            // Training model
            model.Learn(
                num_episodes: 3_000_000,
                num_console_outputs: 100
                );

            // Saving model
            model.Save("Burning_Sands_Model");

            // Loading model
            //model.Load("Burning_Sands_Model");

            // Testing model
            GameUsingModel(env, model, num_episodes);

            // Closing environment
            env = null;
        }

        static void DeadlyMaze(int num_episodes = 5)
        {
            // Creating environment
            Environment env = new Environment(3);

            // Creating model
            Q_Learning model = new Q_Learning(
                env,
                Max_Steps_Per_Episode: 100,
                Learning_Rate: 0.1,
                Discount_Rate: 0.99,
                Exploration_Rate: 1,
                Max_Exploration_Rate: 1,
                Min_Exploration_Rate: 0.05,
                Exploration_Decay_Rate: 0.000002
            );

            // Training model
            model.Learn(
                num_episodes: 2_000_000,
                num_console_outputs: 100
                );

            // Saving model
            model.Save("Deadly_Maze_Model");

            // Loading model
            //model.Load("Deadly_Maze_Model");

            // Testing model
            GameUsingModel(env, model, num_episodes);

            // Closing environment
            env = null;
        }

        static void DeadlyCorridor(int num_episodes = 5)
        {
            // Creating environment
            Environment env = new Environment(2);

            // Creating model
            Q_Learning model = new Q_Learning(
                env,
                Max_Steps_Per_Episode: 100,
                Learning_Rate: 0.7,
                Discount_Rate: 0.99,
                Exploration_Rate: 1,
                Max_Exploration_Rate: 1,
                Min_Exploration_Rate: 0.05,
                Exploration_Decay_Rate: 0.000026
            );

            // Loading model
            //model.Load("Deadly_Corridor_Model");

            // Training model
            model.Learn(
                num_episodes: 100_000,
                num_console_outputs: 100
                );

            // Saving model
            model.Save("Deadly_Corridor_Model");

            // Testing model
            GameUsingModel(env, model, num_episodes);

            // Closing environment
            env = null;
        }

        static void ActiveVolcano(int num_episodes = 5)
        {
            // Creating environment
            Environment env = new Environment(1);

            // Creating model
            Q_Learning model = new Q_Learning(
                env,
                Max_Steps_Per_Episode: 100,
                Learning_Rate: 0.05,
                Discount_Rate: 0.95,
                Exploration_Rate: 1,
                Max_Exploration_Rate: 1,
                Min_Exploration_Rate: 0.05,
                Exploration_Decay_Rate: 0.0003
            );

            // Training model
            model.Learn(10_000);

            // Saving model
            //model.Save("Activate_Volcano_Model");

            // Loading model
            //model.Load("Activate_Volcano_Model");

            // Testing model
            GameUsingModel(env, model, num_episodes);

            // Closing environment
            env = null;
        }

        static void FrozenLake(int num_episodes = 5)
        {
            // Creating environment
            Environment env = new Environment(0);

            // Creating model
            Q_Learning model = new Q_Learning(
                env,
                Max_Steps_Per_Episode: 100,
                Learning_Rate: 0.05,
                Discount_Rate: 0.95,
                Exploration_Rate: 1,
                Max_Exploration_Rate: 1,
                Min_Exploration_Rate: 0.1,
                Exploration_Decay_Rate: 0.0004
            );

            // Training model
            model.Learn(10_000);

            // Saving model
            //model.Save("Frozen_Lake_Model");

            // Loading model
            //model.Load("Frozen_Lake_Model");

            // Testing model
            GameUsingModel(env, model, num_episodes);

            // Closing environment
            env = null;
        }

        static void GameUsingModel(Environment env, Q_Learning model, int num_episodes)
        {
            bool done;
            double sum_of_rewards, reward, agent_hp;
            int state, action;

            // Lowering console size
            Console.SetWindowSize(28, 17);

            for (int episode = 0; episode < num_episodes; episode++)
            {
                sum_of_rewards = 0;
                agent_hp = 100;
                done = false;

                state = env.Reset();
                
                env.Render();
                Console.WriteLine("HP = " + agent_hp);
                Console.WriteLine("Rewards = " + sum_of_rewards);
                Thread.Sleep(600);

                while (!done)
                {
                    action = model.Predict(state);
                    (state, reward, done, agent_hp) = env.Step(action);
                    sum_of_rewards += reward;
                    env.Render();
                    Console.WriteLine("HP = " + agent_hp);
                    Console.WriteLine("Rewards = " + sum_of_rewards);
                    Thread.Sleep(400);
                }
            }
        }

        static void RandomGame(int level = 0, int num_episodes = 4)
        {
            bool done;
            double sum_of_rewards, reward, agent_hp;
            int state, action;

            // Creating environment
            Environment env = new Environment(level);

            // Lowering console size
            Console.SetWindowSize(20, 10);

            // N random games
            for (int episode=0; episode<num_episodes; episode++)
            {
                sum_of_rewards = 0;
                agent_hp = 100;
                done = false;

                state = env.Reset();

                env.Render();
                Console.WriteLine("HP = " + agent_hp);
                Console.WriteLine("Rewards = " + sum_of_rewards);
                Thread.Sleep(600);

                while (!done)
                {
                    action = env.Random_Action();
                    (state, reward, done, agent_hp) = env.Step(action);
                    sum_of_rewards += reward;
                    env.Render();
                    Console.WriteLine("HP = " + agent_hp);
                    Console.WriteLine("Rewards = " + sum_of_rewards);
                    Thread.Sleep(400);
                }
            }
            // Closing environment
            env = null;
        }
    }
}
