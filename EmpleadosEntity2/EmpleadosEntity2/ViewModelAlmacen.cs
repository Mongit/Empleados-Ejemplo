using Empleados;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EmpleadosEntity2
{
    class ViewModelAlmacen : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        #region Propiedades

        private int id;

        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

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
                    ModeloAlmacen.modeloAlmacenRef.Nombre = value;
                    OnPropertyChanged("Nombre");
                }
            }
        }

        private string color;

        public string Color
        {
            get
            {
                return color;
            }
            set
            {
                if (color != value)
                {
                    color = value;
                    ModeloAlmacen.modeloAlmacenRef.Color = value;
                    OnPropertyChanged("Color");
                }
            }
        }

        private string cantidad;

        public string Cantidad
        {
            get
            {
                return cantidad;
            }
            set
            {
                if (cantidad != value)
                {
                    cantidad = value;
                    ModeloAlmacen.modeloAlmacenRef.Cantidad = value;
                    OnPropertyChanged("Cantidad");
                }
            }
        }

        private string proveedor;

        public string Proveedor
        {
            get
            {
                return proveedor;
            }
            set
            {
                if (proveedor != value)
                {
                    proveedor = value;
                    ModeloAlmacen.modeloAlmacenRef.Proveedor = value;
                    OnPropertyChanged("Proveedor");
                }
            }
        }

        //almacen represents the EF object
        private almacen productoSeleccionado;

        public almacen ProductoSeleccionado
        {
            get
            {
                return productoSeleccionado;
            }
            set
            {
                if (productoSeleccionado != value)
                {
                    productoSeleccionado = value;
                    ModeloAlmacen.modeloAlmacenRef.ProductoSeleccionado = value;
                    ((GeneralCommand)editar).IsEnabled = (value != null) ? true : false;
                    ((GeneralCommand)eliminar).IsEnabled = (value != null) ? true : false;
                    OnPropertyChanged("ProductoSeleccionado");
                }
            }
        }

        private List<almacen> productos;

        public List<almacen> Productos
        {
            get
            {
                return productos;
            }
            set
            {
                if (productos != value)
                {
                    productos = value;
                    OnPropertyChanged("Productos");
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

        #endregion

        public ViewModelAlmacen()
        {
            ModeloAlmacen.modeloAlmacenRef = new ModeloAlmacen();
            ModeloAlmacen.modeloAlmacenRef.PropertyChanged += ModeloAlmacenRef_PropertyChanged;
            consultar = new GeneralCommand((obj) => { ModeloAlmacen.modeloAlmacenRef.Consultar(obj); }) { IsEnabled = true };
            nuevo = new GeneralCommand((obj) => { ModeloAlmacen.modeloAlmacenRef.Nuevo(obj); }) { IsEnabled = true };
            editar = new GeneralCommand((obj) => { ModeloAlmacen.modeloAlmacenRef.Editar(obj); }) { IsEnabled = false };
            eliminar = new GeneralCommand((obj) => { ModeloAlmacen.modeloAlmacenRef.Eliminar(obj); }) { IsEnabled = false };
        }

        private void ModeloAlmacenRef_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Nombre = ModeloAlmacen.modeloAlmacenRef.Nombre;
            Color = ModeloAlmacen.modeloAlmacenRef.Color;
            Cantidad = ModeloAlmacen.modeloAlmacenRef.Cantidad;
            Proveedor = ModeloAlmacen.modeloAlmacenRef.Proveedor;
            ProductoSeleccionado = ModeloAlmacen.modeloAlmacenRef.ProductoSeleccionado;
            Productos = ModeloAlmacen.modeloAlmacenRef.Productos;
        }

        protected void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
