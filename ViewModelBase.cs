using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbit
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ViewModelBase<T> : ViewModelBase
    {
        private T model;

        public T Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged(); }
        }

        public ViewModelBase(T model)
        {
            Model = model;
        }
    }
}
