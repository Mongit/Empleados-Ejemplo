using Empleados;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EmpleadosEntity2
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Propiedades

        private string nombre;

        public string Nombre
        {
            get
            {
                return nombre;
            }
            set
            {
                if (nombre != value)
                {
                    nombre = value;
                    Modelo.modeloRef.Nombre = value;
                    OnPropertyChanged("Nombre");
                }
            }
        }

        private string domicilio;

        public string Domicilio
        {
            get
            {
                return domicilio;
            }
            set
            {
                if (domicilio != value)
                {
                    domicilio = value;
                    Modelo.modeloRef.Domicilio = value;
                    OnPropertyChanged("Domicilio");
                }
            }
        }

        private string telefono;

        public string Telefono
        {
            get
            {
                return telefono;
            }
            set
            {
                if (telefono != value)
                {
                    telefono = value;
                    Modelo.modeloRef.Telefono = value;
                    OnPropertyChanged("Telefono");
                }
            }
        }

        private List<empleado> empleados;

        public List<empleado> Empleados
        {
            get
            {
                return empleados;
            }
            set
            {
                if (empleados != value)
                {
                    empleados = value;
                    OnPropertyChanged("Empleados");
                }
            }
        }

        private empleado empleadoSeleccionado;

        public empleado EmpleadoSeleccionado
        {
            get
            {
                return empleadoSeleccionado;
            }
            set
            {
                if (empleadoSeleccionado != value)
                {
                    empleadoSeleccionado = value;
                    Modelo.modeloRef.EmpleadoSeleccionado = value;
                    ((GeneralCommand)editar).IsEnabled = (value != null) ? true : false;
                    ((GeneralCommand)eliminar).IsEnabled = (value != null) ? true : false;
                    OnPropertyChanged("EmpleadoSeleccionado");
                }
            }
        }

        #endregion

        #region Comandos

        private ICommand consultar;

        public ICommand Consultar => consultar;

        private ICommand nuevo;

        public ICommand Nuevo => nuevo;

        private ICommand editar;

        public ICommand Editar => editar;

        private ICommand eliminar;

        public ICommand Eliminar => eliminar;

        private ICommand openWindow;

        public ICommand OpenWindow => openWindow;
        
        #endregion

        public ViewModel()
        {
            Modelo.modeloRef = new Modelo();
            Modelo.modeloRef.PropertyChanged += ModeloRef_PropertyChanged;
            consultar = new GeneralCommand((obj) => { Modelo.modeloRef.Consultar(obj); }) { IsEnabled = true };
            nuevo = new GeneralCommand((obj) => { Modelo.modeloRef.Nuevo(obj); }) { IsEnabled = true };
            editar = new GeneralCommand((obj) => { Modelo.modeloRef.Editar(obj); }) { IsEnabled = false };
            eliminar = new GeneralCommand((obj) => { Modelo.modeloRef.Eliminar(obj); }) { IsEnabled = false };
            openWindow = new GeneralCommand((obj) => { Modelo.modeloRef.OpenWindow(); }) { IsEnabled = true };
        }
        
        private void ModeloRef_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Nombre = Modelo.modeloRef.Nombre;
            Domicilio = Modelo.modeloRef.Domicilio;
            Telefono = Modelo.modeloRef.Telefono;
            Empleados = Modelo.modeloRef.Empleados;
            EmpleadoSeleccionado = Modelo.modeloRef.EmpleadoSeleccionado;
        }

        protected void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }

    public class GeneralCommand : ICommand
    {
        private Action<object> _handler;
        private bool _IsEnabled;

        public GeneralCommand(Action<object> handler) => _handler = handler;

        public bool IsEnabled
        {
            get { return _IsEnabled; }
            set
            {
                if (value != _IsEnabled)
                {
                    _IsEnabled = value;
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) => _handler(parameter);
    }
}
