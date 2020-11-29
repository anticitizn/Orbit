using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbit
{
    public class Camera
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Scale { get; set; }

        public Camera()
        {
            X = 0;
            Y = 0;
            Scale = 1;
        }

        public Camera(float x, float y, float scale)
        {
            X = x;
            Y = y;
            Scale = scale;
        }
    }
}
