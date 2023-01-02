﻿using System.Collections.Generic;


namespace Project
{
    partial class Environment
    {
        private static readonly Dictionary<int, int[]> action_space = new Dictionary<int, int[]>()
        {
            {0, new int[2]{-1,0}}, // left
            {1, new int[2]{+1,0}}, // right
            {2, new int[2]{0,-1}}, // up
            {3, new int[2]{0,+1}}  // down
        };

        private static readonly Dictionary<string, double> rewards = new Dictionary<string, double>()
        {
            {"S", -0.01}, // start tile
            {" ", 0}, // path tile
            {"#", 0}, // wall tile
            {"0", 0}, // hole tile
            {"~", 0}, // frozen ice tile
            {"!", 0}, // burning tile
            {"+", +10}, // reward tile
            {"E", +100} // end tile
        };

        private static readonly Dictionary<string, double> hp_penalties = new Dictionary<string, double>()
        {
            {"S", 0}, // start tile
            {" ", 0}, // path tile
            {"#", 0}, // wall tile
            {"0", -100}, // hole tile
            {"~", 0}, // frozen ice tile
            {"!", -15}, // burning tile
            {"+", 0}, // reward tile
            {"E", 0} // end tile
        };

        public static readonly Dictionary<int, string[,]> maps = new Dictionary<int, string[,]>()
        {
            {0, new string[6, 6] // Frozen Lake
            {
                {"#","#","#","#","#","#"},
                {"#","S","~","~","~","#"},
                {"#","~","0","~","0","#"},
                {"#","~","~","~","0","#"},
                {"#","0","~","~","E","#"},
                {"#","#","#","#","#","#"}
            }},

            {1, new string[7, 7] // Active Volcano
            {
                {"#","#","#","#","#","#","#"},
                {"#","S","!","~","!","+","#"},
                {"#","!","~","~","~","!","#"},
                {"#","!","~","0","~","!","#"},
                {"#","!","~","!","~","!","#"},
                {"#","+","!","~","!","E","#"},
                {"#","#","#","#","#","#","#"}
            }},

            {2, new string[5, 8] // Deadly Corridor
            {
                {"#","#","#","#","#","#","#","#"},
                {"#"," ","!","~","~"," ","+","#"},
                {"#","S","0","!","0"," ","E","#"},
                {"#"," ","~","~","+"," ","0","#"},
                {"#","#","#","#","#","#","#","#"}
            }},

            {3, new string[9, 9] // Deadly Maze
            {
                {"#","#","#","#","#","0","#","#","#"},
                {"#","E","~"," ","~","~"," ","E","#"},
                {"#","+","0","#","~","0","#","+","#"},
                {"0","~","#","#"," ","#","#","+","#"},
                {"0","~"," "," ","S","~","~","~","0"},
                {"#","+","#","#","~","0","#"," ","#"},
                {"#"," ","#","0","~","0","#"," ","#"},
                {"#","E","+","~","+","+","+","E","#"},
                {"#","#","#","#","#","#","#","#","#"}
            }},

            {4, new string[13,13] // Burning Sands
            {
                {"#","#","#","#","#","#","#","#","#","#","#","#","#"},
                {"#","E","!","!","!","!","!"," ","!"," ","!","E","#"},
                {"#"," "," ","!","#","!","!","#","#","!","!","!","#"},
                {"#","!","!","!","!","!","!","!","!"," ","!"," ","#"},
                {"#"," ","#","!","0","#","!","!","!","0","!","!","#"},
                {"#","!","!","!","!","!","!","!","!","!","#","!","#"},
                {"#"," ","#","#","#","!","S"," ","!"," ","!"," ","#"},
                {"#"," ","!","!"," "," ","!"," ","!","!","#","!","#"},
                {"#"," "," "," ","#","!"," "," ","!"," ","!"," ","#"},
                {"#","0","!"," ","!","!","!","!","!","!","!","!","#"},
                {"#","!","#","#","!","#"," ","#","#","#","#","!","#"},
                {"#","E","!","!","!","!","!","!"," ","!","+","E","#"},
                {"#","#","#","#","#","#","#","#","#","#","#","#","#"}
            }}
        };
    }
}
