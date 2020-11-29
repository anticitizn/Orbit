using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbit
{
    public class CameraVM : ViewModelBase<Camera>
    {
        public float X
        {
            get { return Model.X; }
            set { Model.X = value; OnPropertyChanged(); }
        }
        public float Y
        {
            get { return Model.Y; }
            set { Model.Y = value; OnPropertyChanged(); }
        }
        public float Scale
        {
            get { return Model.Scale; }
            set { Model.Scale = value; OnPropertyChanged(); }
        }

        public CameraVM() : this(new Camera()) { }

        public CameraVM(Camera camera) : base(camera) { }

        public CameraVM(float x, float y, float scale) :this(new Camera(x, y, scale)) { }
    }
}
