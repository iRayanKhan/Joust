using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Joust
{
    public class Enemy
    {
        public int x;
        public int y;
        public bool visible;
        public bool destroyed = false;
        public int spawnPoint = 0;
        public bool flip = false;

        public void setInfo(int ax, int ay, bool av, int aSpawn)
        {
            x = ax;
            y = ay;
            visible = av;
            spawnPoint = aSpawn;
        }

        public bool Intersects(Rectangle r)
        {
            return new Rectangle(x, y, 50, 50).Intersects(r);
        }

        public bool Intersects(Enemy e)
        {
            return new Rectangle(x, y, 50, 50).Intersects(new Rectangle(e.x, e.y, 50, 50));
        }
    }
}
