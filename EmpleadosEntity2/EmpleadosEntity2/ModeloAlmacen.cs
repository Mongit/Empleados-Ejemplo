using Empleados;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EmpleadosEntity2
{
    class ModeloAlmacen : INotifyPropertyChanged
    {
        private AlmacenWindow vista;
        private BackgroundWorker worker;
        private empleadosEntities1 empleadosEntitiesRef;
        private Opciones opcion;
        private enum Opciones
        {
            consultar,
            nuevo,
            editar,
            eliminar
        };

        public static ModeloAlmacen modeloAlmacenRef;
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
                if(id != value)
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
                if(nombre != value)
                {
                    nombre = value;
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
                if(color != value)
                {
                    color = value;
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
                if(proveedor != value)
                {
                    proveedor = value;
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
                    if (value != null)
                    {
                        Nombre = value.nombre;
                        Color = value.color;
                        Cantidad = value.cantidad;
                        Proveedor = value.proveedor;
                    }
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
                if(productos != value)
                {
                    productos = value;
                    OnPropertyChanged("Productos");
                }
            }
        }

        #endregion

        public ModeloAlmacen()
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
                    Nombre = Color = Proveedor = "";
                    Cantidad = "";
                    ProductoSeleccionado = null;
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
                    Productos = (from p in empleadosEntitiesRef.almacens select p).ToList();
                    break;
                case Opciones.nuevo:
                    empleadosEntitiesRef.almacens.Add(new Empleados.almacen()
                    {
                        nombre = Nombre,
                        color = Color,
                        cantidad = Cantidad,
                        proveedor = Proveedor
                    });
                    empleadosEntitiesRef.SaveChanges();
                    break;
                case Opciones.editar:
                    almacen producto = (from p in empleadosEntitiesRef.almacens where p.id == ProductoSeleccionado.id select p).FirstOrDefault();
                    producto.nombre = Nombre;
                    producto.color = Color;
                    producto.cantidad = Cantidad;
                    producto.proveedor = Proveedor;
                    empleadosEntitiesRef.SaveChanges();
                    break;
                case Opciones.eliminar:
                    almacen prod = (from p in empleadosEntitiesRef.almacens where p.id == ProductoSeleccionado.id select p).FirstOrDefault();
                    empleadosEntitiesRef.almacens.Remove(prod);
                    empleadosEntitiesRef.SaveChanges();
                    break;
                default:
                    break;
            }
        }

        public void Consultar(object obj)
        {
            if (vista == null)
                vista = obj as AlmacenWindow;
            opcion = Opciones.consultar;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Nuevo(object obj)
        {
            if (vista == null)
                vista = obj as AlmacenWindow;
            opcion = Opciones.nuevo;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Editar(object obj)
        {
            if (vista == null)
                vista = obj as AlmacenWindow;
            opcion = Opciones.editar;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void Eliminar(object obj)
        {
            if (vista == null)
                vista = obj as AlmacenWindow;
            opcion = Opciones.eliminar;
            if (!worker.IsBusy)
                worker.RunWorkerAsync();
        }

        public void OnPropertyChanged(string Property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Property));
        }
    }
}
