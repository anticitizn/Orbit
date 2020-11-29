using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Orbit
{
    /// <summary>
    /// Interaction logic for OrbitDrawField.xaml
    /// </summary>
    public partial class OrbitDrawField : UserControl
    {
        private readonly PhysicsEngineVM dataContext;
        public OrbitDrawField()
        {
            InitializeComponent();
             dataContext = (PhysicsEngineVM)this.DataContext;
        }

        private void canvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            IInputElement clickedElement = Mouse.DirectlyOver;
            if (clickedElement is Path path)
            {
                if (path.DataContext is EntityVM entity)
                {
                    dataContext.SelectedEntity = entity;
                    SelectedLabel.Content = entity.Name;
                }
            }
            else
            {
                dataContext.SelectedEntity = null;
                SelectedLabel.Content = null;
            }
        }
    }
}