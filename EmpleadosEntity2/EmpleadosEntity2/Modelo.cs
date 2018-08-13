using Empleados;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpleadosEntity2
{
    class Modelo : INotifyPropertyChanged
    {
        public static Modelo modeloRef;
        public event PropertyChangedEventHandler PropertyChanged;
        private MainWindow vista;
        private BackgroundWorker worker;
        private empleadosEntities1 empleadosEntitiesRef;
        private Opciones opcion;
        private enum Opciones
        {
            consultar,
            nuevo,
            editar,
            eliminar
        }

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
                    if (value != null)
                    {
                        Nombre = value.nombre;
                        Domicilio = value.domicilio;
                        Telefono = value.telefono;
                    }
                    OnPropertyChanged("EmpleadoSeleccionado");
                }
            }
        }

        #endregion

        public Modelo()
        {
            worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            switch (opcion)
            {
                case Opciones.consultar:
                    break;
                case Opciones.nuevo:
                case Opciones.editar:
                case Opciones.eliminar:
                    Nombre = Domicilio = Telefono = "";
                    EmpleadoSeleccionado = null;
                    opcion = Opciones.consultar;
                    if (!worker.IsBusy)
                        worker.RunWorkerAsync();
                    break;
                default:
                    break;
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            empleadosEntitiesRef = new empleadosEntities1();
            switch (opcion)
            {
                case Opciones.consultar:
                    Empleados = (from emp in empleadosEntitiesRef.empleados where emp.idempleados > 0 select emp).ToList();
                    break;
                case Opciones.nuevo:
                    empleadosEntitiesRef.empleados.Add(new Empleados.empleado()
                    {
                        nombre = Nombre,
                        domicilio = Domicilio,
                        telefono = Telefono
                    });
                    empleadosEntitiesRef.SaveChanges();
                    break;
                case Opciones.editar:
                    empleado empleado = (from emp in empleadosEntitiesRef.empleados where emp.idempleados == EmpleadoSeleccionado.idempleados select emp).FirstOrDefault();
                    empleado.nombre = Nombre;
                    empleado.domicilio = Domicilio;
                    empleado.telefono = Telefono;
                    empleadosEntitiesRef.SaveChanges();
                    break;
                case Opciones.eliminar:
                    empleado empl = (from emp in empleadosEntitiesRef.empleados where emp.idempleados == EmpleadoSeleccionado.idempleados select emp).FirstOrDefault();
                    empleadosEntitiesRef.empleados.Remove(empl);
                    empleadosEntitiesRef.SaveChanges();
                    break;
                default:
                    break;
            }
        }

        public void Consultar(object obj)
        {
            if (vista == null)
                vista = obj as MainWindow;
            opcion = Opciones.consultar;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Nuevo(object obj)
        {
            if (vista == null)
                vista = obj as MainWindow;
            opcion = Opciones.nuevo;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Editar(object obj)
        {
            if (vista == null)
                vista = obj as MainWindow;
            opcion = Opciones.editar;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Eliminar(object obj)
        {
            if (vista == null)
                vista = obj as MainWindow;
            opcion = Opciones.eliminar;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        protected void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }

        public void OpenWindow()
        {
            AlmacenWindow open = new AlmacenWindow();
            open.ShowDialog();
        }
    }
}
