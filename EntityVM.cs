using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Orbit
{
    public class EntityVM : ViewModelBase<Entity>
    {
        public string Name
        {
            get { return Model.Name; }
            set { Model.Name = value; OnPropertyChanged(); }
        }

        private PropVector4 vPos = new PropVector4();

        /// <summary>
        /// This is a hack used to change the scale of the real coordinates.
        /// TO-DO: Replace with some kind of Binding Converter
        /// </summary>
        public PropVector4 VPos
        {
            get { return vPos; }
            set { vPos = value; OnPropertyChanged(); }
        }

        public Vector4 Pos
        {
            get 
            {
                return Model.Pos;
            }
            set 
            {
                VPos.X = Pos.X;
                VPos.Y = Pos.Y;
                VPos.Z = Pos.Z;
                VPos.W = Pos.W;
                Model.Pos = value; 
                OnPropertyChanged(); 
                OnPropertyChanged("VPos"); 
            }
        }

        public Vector4 Velocity
        {
            get { return Model.Velocity; }
            set { Model.Velocity = value; OnPropertyChanged(); }
        }
        public ulong Mass
        {
            get { return Model.Mass;}
            set { Model.Mass = value; OnPropertyChanged(); }
        }

        private int diameter;
        public int Diameter
        {
            get { return diameter; }
            set { diameter = value; OnPropertyChanged(); }
        }

        private PointCollection orbitalPath;
        public PointCollection OrbitalPath 
        {
            get { return orbitalPath; } 
            set { orbitalPath = value; OnPropertyChanged(); OnPropertyChanged("OrbitalPath"); }
        }

        public EntityVM() : this(new Entity()) { }

        public EntityVM(Entity entity) : base(entity)
        {
            OrbitalPath = new PointCollection();
            Diameter = (int)Math.Pow(Model.Mass, 0.06) / 2;
            //Diameter = (int)((Mass * 2e-3) / (ulong)Scale);
        }
    }
}
