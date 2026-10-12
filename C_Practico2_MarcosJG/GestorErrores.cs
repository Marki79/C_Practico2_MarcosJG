using Microsoft.Data.Sqlite;
using System;

namespace CasoPractico2
{
    public class GestorErrores
    {
        // Guarda un mensaje de error junto con la fecha y la hora.
        public void RegistrarError(string mensaje)
        {
            try
            {
                //Abrimos la conexión con la base de datos
                using (SqliteConnection conexion = ConexionBD.ObtenerConexion())
                {
                    conexion.Open();

                    // Insertamos el mensaje y la fecha del error.
                    string sql = @"INSERT INTO Errores
                        (Mensaje, Fecha)
                        VALUES (@mensaje, @fecha)";

                    using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                    {
                        // Añadimos el mensaje recibido.
                        comando.Parameters.AddWithValue(
                            "@mensaje", mensaje);

                        // Guardamos la fecha y la hora actuales.
                        comando.Parameters.AddWithValue(
                            "@fecha",
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        //Ejecutamos la inserción
                        comando.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception)
            {
                // No dejamos que falle el registro del error.
            }
        }
    }
}