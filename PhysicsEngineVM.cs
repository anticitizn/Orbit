using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Orbit
{
    public class PhysicsEngineVM : ViewModelBase<List<EntityVM>>
    {
        // Principal issue with the integrator - at higher timestep values, zooming in shows wild inaccuracies
        // floating-point precision errors are also likely; switch to double?

        // 1 distance unit = 1 km
        // 1 weight unit = 1 000 000 000 000 kg
        // 1 speed unit = 1 km/s
        // 1 time unit = 1 s

        // G = 6.67408e-11 m^3 kg^-1 sec^-2
        private float simspeed = 5.0f;
        public float SIMSPEED
        {
            get { return simspeed; }
            set { simspeed = value; GRAV = 6.67430 * Math.Pow(10, -8 + SIMSPEED); }
        }

        private EntityVM selectedEntity;
        public EntityVM SelectedEntity 
        { 
            get
            {
                return selectedEntity;
            }
            set
            {
                selectedEntity = value;
                if (value == null)
                {
                    OffsetCamera.X = 0;
                    OffsetCamera.Y = 0;
                }
                else
                {
                    OffsetCamera.X = 1000;
                    OffsetCamera.Y = 500;
                }
            }
        }

        // gravity constant
        public double GRAV;

        public CameraVM UserCamera { get; set; }
        private Camera OffsetCamera { get; set; } = new Camera();
        private readonly DispatcherTimer TickTimer = new DispatcherTimer();

        public int Scale { get; set; } = 5000000;

        public List<EntityVM> Entities
        {
            get { return Model; }
            set { Model = value; }
        }

        public void CalculateVelocities()
        {
            foreach (EntityVM e1 in Entities)
            {
                foreach (EntityVM e2 in Entities)
                {
                    if (e2 != e1)
                    {
                        Vector4 direction = Vector4.Normalize(e2.Pos - e1.Pos);
                        float distance = (float)Math.Abs( Math.Sqrt( Math.Pow(e1.Pos.X - e2.Pos.X, 2) + Math.Pow(e1.Pos.Y - e2.Pos.Y, 2) + Math.Pow(e1.Pos.Z - e2.Pos.Z, 2) ) );
                        float fGrav = (float)((GRAV * e2.Mass) / Math.Pow(distance, 2));

                        Vector4 acceleration = new Vector4(direction.X, direction.Y, direction.Z, 1.0f) * fGrav;

                        //Console.WriteLine(e2.Name + " accelerating " + e1.Name + " to " + acceleration.X + ' ' + acceleration.Y + " from distance: " + distance);
                        //Console.WriteLine("Direction: " + direction);

                        e1.Velocity += acceleration;
                    }
                }
            }
        }

        public void CalculatePositions()
        {
            foreach (EntityVM e in Entities)
            {
                e.Pos += new Vector4(e.Velocity.X, e.Velocity.Y, e.Velocity.Z, 1.0f) * (float)Math.Pow(10, SIMSPEED);
            }
        }

        public void Tick(object sender, EventArgs e)
        {
            CalculateVelocities();
            CalculatePositions();
            CalculateCamera();
            AddOrbitalPaths();
            
            //Console.WriteLine("Camera X/Y: " + UserCamera.X + ' ' + UserCamera.Y);
        }

        private void AddOrbitalPaths()
        {
            foreach(EntityVM entity in Entities)
            {
                entity.OrbitalPath.Add(new Point(entity.Pos.X, entity.Pos.Y));
                if (entity.OrbitalPath.Count > 1000)
                {
                    entity.OrbitalPath.RemoveAt(0);
                }
            }
        }

        private void CalculateCamera()
        {
            if (SelectedEntity is null)
            {
                UserCamera.X = OffsetCamera.X;
                UserCamera.Y = OffsetCamera.Y;
            }
            else
            {
                UserCamera.X = -(SelectedEntity.Pos.X / UserCamera.Scale) + OffsetCamera.X;
                UserCamera.Y = -(SelectedEntity.Pos.Y / UserCamera.Scale) + OffsetCamera.Y;
            }
            
        }

        public Command PressZoomIn { get; private set; }
        public Command PressZoomOut { get; private set; }
        public Command PressMoveUp { get; private set; }
        public Command PressMoveRight { get; private set; }
        public Command PressMoveDown { get; private set; }
        public Command PressMoveLeft { get; private set; }
        public Command PressSlowTime { get; private set; }
        public Command PressFastTime { get; private set; }

        private void ZoomIn(object o)
        {
            UserCamera.Scale = (int)(UserCamera.Scale * 0.9);
        }

        private void ZoomOut(object o)
        {
            UserCamera.Scale = (int)(UserCamera.Scale * 1.1);
        }

        private void MoveUp(object o)
        {
            OffsetCamera.Y += 100;
        }

        private void MoveRight(object o)
        {
            OffsetCamera.X -= 100;
        }

        private void MoveDown(object o)
        {
            OffsetCamera.Y -= 100;
        }

        private void MoveLeft(object o)
        {
            OffsetCamera.X += 100;
        }

        private void SlowTime(object o)
        {
            SIMSPEED *= 0.9f;
        }

        private void FastTime(object o)
        {
            SIMSPEED *= 1.1f;
        }

        public PhysicsEngineVM(List<EntityVM> entityList) : base(entityList) { }

        public PhysicsEngineVM() : this(new List<EntityVM>())
        {
            GRAV = 6.67430 * Math.Pow(10, -8 + SIMSPEED);
            PressZoomIn = new Command(ZoomIn);
            PressZoomOut = new Command(ZoomOut);
            PressMoveUp = new Command(MoveUp);
            PressMoveRight = new Command(MoveRight);
            PressMoveDown = new Command(MoveDown);
            PressMoveLeft = new Command(MoveLeft);
            PressSlowTime = new Command(SlowTime);
            PressFastTime = new Command(FastTime);

            Entity Sun = new Entity("Sun", new Vector4(4500e+6f, 2500e+6f, 0, 1), new Vector4(0, 0, 0, 0), (ulong)1.989e+18);
            Entities.Add(new EntityVM(Sun));

            Entity Mercury = new Entity("Mercury", Sun.Pos + new Vector4(46e+6f, 0, 0, 0), new Vector4(0, 58.98f, 0, 0), (ulong)0.330e+12);
            Entities.Add(new EntityVM(Mercury));

            Entity Venus = new Entity("Venus", Sun.Pos + new Vector4(108.94e+6f, 0, 0, 0), new Vector4(0, 34.79f, 0, 0), (ulong)4.868e+12);
            Entities.Add(new EntityVM(Venus));

            Entity Earth = new Entity("Earth", Sun.Pos + new Vector4(147.10e+6f, 0, 0, 0), new Vector4(0, 30.29f, 0, 0), (ulong)5.972e+12);
            Entities.Add(new EntityVM(Earth));

            Entity Moon = new Entity("Moon", Earth.Pos + new Vector4(0, 405500, 0, 1), Earth.Velocity, (ulong)7.35e+9);
            Moon.Velocity += new Vector4(0.970f, 0, 0, 0);
            Entities.Add(new EntityVM(Moon));

            Entity Mars = new Entity("Mars", Sun.Pos + new Vector4(249.10e+6f, 0, 0, 0), new Vector4(0, 21.97f, 0, 0), (ulong)0.641e+12);
            Entities.Add(new EntityVM(Mars));

            Entity Jupiter = new Entity("Jupiter", Sun.Pos + new Vector4(816e+6f, 0, 0, 0), new Vector4(0, 12.44f, 0, 0), (ulong)1898e+12);
            Entities.Add(new EntityVM(Jupiter));

            Entity Saturn = new Entity("Saturn", Sun.Pos + new Vector4(1514e+6f, 0, 0, 0), new Vector4(0, 9.09f, 0, 0), (ulong)568e+12);
            Entities.Add(new EntityVM(Saturn));

            Entity Uranus = new Entity("Uranus", Sun.Pos + new Vector4(3003e+6f, 0, 0, 0), new Vector4(0, 6.49f, 0, 0), (ulong)86e+12);
            Entities.Add(new EntityVM(Uranus));

            Entity Neptune = new Entity("Neptune", Sun.Pos + new Vector4(4545e+6f, 0, 0, 0), new Vector4(0, 5.37f, 0, 0), (ulong)102e+12);
            Entities.Add(new EntityVM(Neptune));

            UserCamera = new CameraVM(0, 0, 5000000);
            SelectedEntity = Entities.FirstOrDefault();

            //Entity Star1 = new Entity("Star1", new Vector4(3000e+6f, 2000e+6f, 0, 1), new Vector4(0, 0, 0, 0), (ulong)6e+18);
            //Entities.Add(new EntityVM(Star1));
            //Entity Star2 = new Entity("Star2", new Vector4(4000e+6f, 3000e+6f, 0, 1), new Vector4(0, 0, 0, 0), (ulong)6e+18);
            //Entities.Add(new EntityVM(Star2));
            //Entity Star3 = new Entity("Star3", new Vector4(5000e+6f, 2500e+6f, 0, 1), new Vector4(0, 0, 0, 0), (ulong)6e+18);
            //Entities.Add(new EntityVM(Star3));

            TickTimer.Interval = TimeSpan.FromSeconds(0.01);
            TickTimer.Tick += Tick;
            TickTimer.Start();
        }
    }
}
