using System;
using System.Collections.Generic;
using System.Globalization;

namespace CasoPractico2
{
    public class Juego
    {

        // Objetos que permiten acceder a la base de datos y a los logs
        private HabitoDAO habitoDAO = new HabitoDAO();
        private GestorErrores gestorErrores = new GestorErrores();
        private Logger logger = new Logger();


        //Muestra el menú hasta que el usuario decida salir
        public void Iniciar()
        {
            bool salir = false;

            //Registramos el inicio de la aplicación
            logger.Registrar("Aplicación iniciada");

            while (!salir)
            {
                Console.WriteLine("\n===== REGISTRO DE HÁBITOS =====");
                Console.WriteLine("1. Registrar hábito");
                Console.WriteLine("2. Ver hábitos");
                Console.WriteLine("3. Actualizar hábito");
                Console.WriteLine("4. Eliminar hábito");
                Console.WriteLine("5. Salir");
                Console.Write("Elige una opción: ");

                string opcion = Console.ReadLine();

                try
                {
                    switch (opcion)
                    {
                        case "1":
                            Insertar();
                            break;
                        case "2":
                            Mostrar();
                            break;
                        case "3":
                            Actualizar();
                            break;
                        case "4":
                            Eliminar();
                            break;
                        case "5":
                            salir = true;
                            logger.Registrar("Aplicación cerrada");
                            break;
                        default:
                            Console.WriteLine("Opción no válida.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Mostramos un mensaje sin cerrar el menu
                    Console.WriteLine("Se ha producido un error.");

                    // Guardamos los detalles del error
                    gestorErrores.RegistrarError(ex.ToString());

                    //Registramos que ocurrió un error en el log diario
                    logger.Registrar("Error durante una operación");
                }
            }
        }

        // Solicita los datos e inserta un hábito nuevo.
        private void Insertar()
        {
            string nombre = PedirNombre();
            int cantidad = PedirEntero("Cantidad: ", 0);
            string fecha = PedirFecha();

            Habito habito = new Habito(nombre, cantidad, fecha);
            habitoDAO.Insertar(habito);

            Console.WriteLine("Hábito registrado correctamente.");
            logger.Registrar("Hábito insertado");
        }

        // Consulta y muestra todos los hábitos registrados.
        private void Mostrar()
        {
            List<Habito> lista = habitoDAO.ObtenerTodos();

            // Comprobamos que no hay registros
            if (lista.Count == 0)
            {
                Console.WriteLine("No hay hábitos registrados.");
                return;
            }

            //Recorremos la lista y mostramos cada hábito
            for (int i = 0; i < lista.Count; i++)
            {
                Console.WriteLine(
                    "ID: " + lista[i].Id +
                    " | Nombre: " + lista[i].Nombre +
                    " | Cantidad: " + lista[i].Cantidad +
                    " | Fecha: " + lista[i].Fecha);
            }

            logger.Registrar("Hábitos consultados");
        }

        // Solicita el ID y los nuevos datos de un hábito.
        private void Actualizar()
        {
            Mostrar();
            int id = PedirEntero("ID que quieres actualizar: ", 1);

            if (!habitoDAO.Existe(id))
            {
                Console.WriteLine("No existe un hábito con ese ID.");
                return;
            }

            string nombre = PedirNombre();
            int cantidad = PedirEntero("Nueva cantidad: ", 0);
            string fecha = PedirFecha();

            Habito habito = new Habito(nombre, cantidad, fecha);
            habito.Id = id;

            habitoDAO.Actualizar(habito);
            Console.WriteLine("Hábito actualizado correctamente.");
            logger.Registrar("Hábito actualizado");
        }

        // Solicita el ID de un hábito y lo elimina.
        private void Eliminar()
        {
            Mostrar();
            int id = PedirEntero("ID que quieres eliminar: ", 1);

            if (habitoDAO.Eliminar(id))
            {
                Console.WriteLine("Hábito eliminado correctamente.");
                logger.Registrar("Hábito eliminado");
            }
            else
            {
                Console.WriteLine("No existe un hábito con ese ID.");
            }
        }

        // Solicita un nombre que no esté vacío.
        private string PedirNombre()
        {
            while (true)
            {
                Console.Write("Nombre del hábito: ");
                string nombre = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(nombre))
                {
                    return nombre.Trim();
                }

                Console.WriteLine("El nombre no puede estar vacío.");
            }
        }
        // Solicita un número entero que cumpla el mínimo indicado.
        private int PedirEntero(string mensaje, int minimo)
        {
            int numero;

            while (true)
            {
                Console.Write(mensaje);

                if (int.TryParse(Console.ReadLine(), out numero) && numero >= minimo)
                {
                    return numero;
                }

                Console.WriteLine("Introduce un entero válido (mínimo " + minimo + ").");
            }
        }

        // Solicita una fecha válida o permite utilizar la fecha de hoy.
        private string PedirFecha()
        {
            while (true)
            {
                Console.Write("Fecha (dd/MM/yyyy) o HOY: ");
                string texto = Console.ReadLine();

                // Si se escribe HOY, utilizamos la fecha actual.
                if (texto != null && texto.Trim().Equals( "HOY", StringComparison.OrdinalIgnoreCase))
                {
                    return DateTime.Today.ToString("dd/MM/yyyy");
                }

                DateTime fecha;

                // Exigimos el formato día/mes/año.
                bool valida = DateTime.TryParseExact(
                    texto,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fecha);

                if (valida)
                {
                    return fecha.ToString("dd/MM/yyyy");
                }

                Console.WriteLine("Fecha incorrecta. Ejemplo válido: 12/10/2026.");
            }
        }
    }
}