using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Orbit
{
    /// <summary>
    /// A class that's just a container for Vector4's values. Because Vector4 is a struct, it uses fields which cannot be bound to in XAML.
    /// A Vector4 is used to do the math in the backend and this class is used to expose its x, y, z fields as properties to the View
    /// </summary>
    public class PropVector4
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public PropVector4() { }

        public PropVector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public PropVector4(Vector4 vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = vec.Z;
            W = vec.W;
        }
    }
}
