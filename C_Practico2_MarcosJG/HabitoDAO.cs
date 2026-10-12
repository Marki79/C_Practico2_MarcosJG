using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace CasoPractico2
{
    public class HabitoDAO
    {
        // Guarda un hábito nuevo en la base de datos.
        public void Insertar(Habito habito)
        {
            using (SqliteConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                //Insertamos los parámetros utilizando parámetros
                string sql = @"INSERT INTO Habitos
                    (Nombre, Cantidad, Fecha)
                    VALUES (@nombre, @cantidad, @fecha)";

                using (SqliteCommand comando = new SqliteCommand(sql, conexion))
                {
                    // Asignamos los valores a los parámetros SQL.
                    comando.Parameters.AddWithValue(
                        "@nombre", habito.Nombre);
                    comando.Parameters.AddWithValue(
                        "@cantidad", habito.Cantidad);
                    comando.Parameters.AddWithValue(
                        "@fecha", habito.Fecha);

                    //Ejecutamos las inserción
                    comando.ExecuteNonQuery();
                }
            }
        }

        // Devuelve una lista con todos los hábitos registrados.
        public List<Habito> ObtenerTodos()
        {
            List<Habito> lista = new List<Habito>();

            using (SqliteConnection conexion =
                   ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                // Consultamos todos los registros, ordenados por fecha.
                string sql = "SELECT * FROM Habitos ORDER BY Fecha DESC";

                using (SqliteCommand comando =new SqliteCommand(sql, conexion))

                using (SqliteDataReader lector = comando.ExecuteReader())
                {
                    // Leemos los resultados uno por uno.
                    while (lector.Read())
                    {
                        // Creamos un objeto con los datos obtenidos
                        Habito habito = new Habito();

                        habito.Id = Convert.ToInt32(lector["Id"]);

                        habito.Nombre = lector["Nombre"].ToString();

                        habito.Cantidad = Convert.ToInt32(lector["Cantidad"]);

                        habito.Fecha = lector["Fecha"].ToString();

                        //Añadimos el hábit a la lista
                        lista.Add(habito);
                    }
                }
            }
            //Devolvemos la lista 
            return lista;
        }

        // Comprueba si existe un hábito con el ID indicado.
        public bool Existe(int id)
        {
            using (SqliteConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                string sql = "SELECT COUNT(*) FROM Habitos WHERE Id = @id";

                using (SqliteCommand comando =
                       new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    // COUNT indica cuántos registros coinciden.
                    long resultado = (long)comando.ExecuteScalar();

                    // Devolvemos true si existe al menos uno.
                    return resultado > 0;
                }
            }
        }

        // Elimina un hábito utilizando su identificador.
        public bool Eliminar(int id)
        {
            using (SqliteConnection conexion = ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                string sql = "DELETE FROM Habitos WHERE Id = @id";

                using (SqliteCommand comando =
                       new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", id);

                    // Devuelve true si se ha eliminado algún registro.
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }

        // Modifica el nombre, la cantidad y la fecha de un hábito.
        public bool Actualizar(Habito habito)
        {
            using (SqliteConnection conexion =ConexionBD.ObtenerConexion())
            {
                conexion.Open();

                string sql = @"UPDATE Habitos
                    SET Nombre = @nombre,
                        Cantidad = @cantidad,
                        Fecha = @fecha
                    WHERE Id = @id";

                using (SqliteCommand comando =new SqliteCommand(sql, conexion))
                {
                    comando.Parameters.AddWithValue("@id", habito.Id);
                    comando.Parameters.AddWithValue(
                        "@nombre", habito.Nombre);
                    comando.Parameters.AddWithValue(
                        "@cantidad", habito.Cantidad);
                    comando.Parameters.AddWithValue(
                        "@fecha", habito.Fecha);

                    // Devuelve true si se ha actualizado algún registro.
                    return comando.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}