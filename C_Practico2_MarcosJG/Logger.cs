using System;
using System.IO;

namespace CasoPractico2
{
    public class Logger
    {
        // Registra una operación en el archivo de log del día.
        public void Registrar(string mensaje)
        {
            try
            {
                // Nombre de la carpeta donde se guardan los logs.
                string carpeta = "logs";

                //Creamos la carpeta si todavia no existe
                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                // Generamos el nombre del archivo con la fecha actual.
                string archivo = DateTime.Now.ToString("yyyy-MM-dd") + ".log";

                // Combinamos la carpeta y el nombre del archivo.
                string ruta = Path.Combine(carpeta, archivo);

                // Añadimos la hora y el mensaje de la operación.
                string texto = DateTime.Now.ToString("HH:mm:ss")
                               + " - " + mensaje;

                // Guardamos el texto sin borrar las líneas anteriores.
                File.AppendAllText(ruta, texto + Environment.NewLine);
            }
            catch (IOException)
            {
                // Un fallo en el log no debe cerrar la aplicación.
            }
            catch (UnauthorizedAccessException)
            {
                // Puede no haber permisos para escribir.
            }
        }
    }
}