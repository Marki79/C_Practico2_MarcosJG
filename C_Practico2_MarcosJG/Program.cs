using System;

namespace CasoPractico2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Inicializamos el proveedor de SQLite (me daba problemas al ejecutar la aplicación sin esto)
                SQLitePCL.Batteries.Init();

                //Crear la base de datos
                ConexionBD.CrearBaseDatos();

                // Inicializamos el juego
                Juego juego = new Juego();
                juego.Iniciar();
            }

            catch (Exception ex)
            {
                // Mostramos el error si no se puede iniciar la aplicación.
                Console.WriteLine("No se pudo iniciar la aplicación.");
                Console.WriteLine(ex.Message);
            }
        }
    }
}