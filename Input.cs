using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;
using System.Windows.Forms;

namespace SnakeGame
{
    class Input
    {
        private static Hashtable keyTable = new Hashtable(); //this class is used to optimize the keys

        public static bool KeyPress(Keys key)
        {
            if (keyTable[key] == null)
            {
                // if the hashtable is empy we return false.
                return false;
            }
            // if the hashtable is empy we return true.
            return (bool)keyTable[key];
        }

        public static void changeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
