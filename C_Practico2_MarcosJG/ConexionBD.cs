using Microsoft.Data.Sqlite;

namespace CasoPractico2
{
    public class ConexionBD
    {

        // Cadena que indica el archivo donde se guarda la base de datos.
        private static string cadena = "Data Source=habitos.db";

        // Crea y devuelve una conexión con SQLite.
        public static SqliteConnection ObtenerConexion()
        {
            return new SqliteConnection(cadena);
        }

        // Método para crear la base de datos y las tablas si no existen
        public static void CrearBaseDatos()
        {   
            using (SqliteConnection conexion = ObtenerConexion())
            {
                // // Abrimos la conexión y la cerramos automáticamente al terminar.
                conexion.Open();

                // Instrucciones SQL para crear las dos tablas.
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Habitos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nombre TEXT NOT NULL,
                        Cantidad INTEGER NOT NULL CHECK(Cantidad >= 0),
                        Fecha TEXT NOT NULL
                    );

                    CREATE TABLE IF NOT EXISTS Errores (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Mensaje TEXT NOT NULL,
                        Fecha TEXT NOT NULL
                    );";

                // Preparamos y ejecutamos esas instrucciones.
                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                    comando.ExecuteNonQuery();
                }
            }
        }
    }
}