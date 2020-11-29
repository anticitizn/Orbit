using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace Orbit
{
    public class Entity
    {
        public string Name { get; set; }
        public Vector4 Pos { get; set; }

        public Vector4 Velocity { get; set; }

        public ulong Mass { get; set; }

        public Entity(string name, Vector4 pos, Vector4 velocity, ulong mass)
        {
            Name = name;
            Mass = mass;
            Pos = pos;
            Velocity = velocity;
        }

        public Entity()
        {

        }
    }
}
