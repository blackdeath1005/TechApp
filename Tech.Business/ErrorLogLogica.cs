using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.DataAccess;

namespace Tech.Business
{
    public class ErrorLogLogica
    {
        static string directoryPath = @"D:\Hitrax";
        static string destinationPath = @"C:\Users\LUIS\Desktop\UPC\CICLO XII\Proyecto Informatico 3\Proyecto Tech\ProyectoTech\Tech.Interfaces\HitraxLogs";

        DBUNLIMITEDEntities errorLogDA = new DBUNLIMITEDEntities();

        public FileSystemWatcher StartWatcher()
        {
            FileSystemWatcher watcher = null;

            try
            {
                watcher = new FileSystemWatcher
                {
                    Filter = "*.htx",
                    Path = directoryPath,
                    EnableRaisingEvents = true,
                    IncludeSubdirectories = true
                };

                watcher.Created += new FileSystemEventHandler(OnCreated);

                return watcher;
            }
            catch (Exception e)
            {
                return watcher;
            }
        }
        public Boolean StopWatcher(FileSystemWatcher watcher)
        {
            try
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
                watcher = null;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string rutaOrigen = e.FullPath;
            string nombreFile = e.Name;

            Boolean isReady = false;
            while (!isReady) {
                isReady = IsFileReady(rutaOrigen);
            }
            
            string nombreDestino = nombreFile;
            string rutaDestino = System.IO.Path.Combine(destinationPath, nombreDestino);

            if (!System.IO.Directory.Exists(destinationPath))
            {
                System.IO.Directory.CreateDirectory(destinationPath);
            }
            System.IO.File.Copy(rutaOrigen, rutaDestino, true);

        }

        public static Boolean IsFileReady(String sFilename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public FileInfo[] FilesOnLogsFolder()
        {
            DirectoryInfo d = new DirectoryInfo(destinationPath);//Assuming Test is your Folder
            FileInfo[] files = d.GetFiles("*.*"); //Getting Text files

            return files;
        }

        public List<String> cargarArchivo(string path)
        {
            List<String> lineas = new List<String>();
            try
            {

                string line;
                // Read the file and display it line by line.
                System.IO.StreamReader file = new System.IO.StreamReader(path);
                while ((line = file.ReadLine()) != null)
                {
                    lineas.Add(line);
                }

                file.Close();

                return lineas;
            }
            catch
            {
                return lineas;
            }
            
        }
        public void borrarArchivo(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public List<LOG_ERROR> errorLogListarInvertido()
        {
            List<LOG_ERROR> errorLogs = errorLogDA.LOG_ERROR.OrderByDescending(x => x.Fe_LogError).ToList();

            return errorLogs;
        }
        public LOG_ERROR ObtenerErrorLog(int codigo)
        {
            List<LOG_ERROR> errorLogs = errorLogDA.LOG_ERROR.ToList();

            LOG_ERROR errorLogObtenida = errorLogs.Single(delegate(LOG_ERROR errorLog)
            {
                if (errorLog.Co_LogError == codigo)
                    return true;
                else
                    return false;
            });
            return errorLogObtenida;
        }
        public List<LOG_ERROR> ObtenerErrorLogNombre(string nombre)
        {
            List<LOG_ERROR> errorLogs = errorLogDA.LOG_ERROR.ToList();

            List<LOG_ERROR> errorLogsObtenida = new List<LOG_ERROR>();

            IEnumerable<LOG_ERROR> errorLogsFiltrado = from p in errorLogs select p;
            errorLogsFiltrado = errorLogsFiltrado.Where(x => x.Tx_Archivo.Equals(nombre));

            foreach (LOG_ERROR errorLog in errorLogsFiltrado)
            {
                errorLogsObtenida.Add(errorLog);
            }

            //solicitudAtencionesObtenidas = solicitudAtencionesObtenidas.OrderByDescending(x => x.Co_Solicitud).ToList();

            return errorLogsObtenida;
        }
        public Boolean AgregarErrorLog(LOG_ERROR errorLogNuevo)
        {
            try
            {
                errorLogDA.LOG_ERROR.Add(errorLogNuevo);
                errorLogDA.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }


}
