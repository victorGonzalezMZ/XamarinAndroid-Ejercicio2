using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ejercicio2
{
    public class ClaseSQLite
    {
        string basededatos = "Informacion.db3";
        public int folio, edad;
        public string nombre, domicilio, correo;
        public double saldo;

        public void ConexionBase()
        {
            var RutaBase = Path.Combine(
                System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal), basededatos);

            bool existencia = File.Exists(RutaBase);
            if (!existencia)
            {
                SqliteConnection.CreateFile(RutaBase);
                var conexion = new SqliteConnection("Data Source=" + RutaBase);
                var sql = "CREATE TABLE Datos (Folio INTEGER PRIMARY KEY AUTOINCREMENT, " +
                    "Nombre VARCHAR(50), Domicilio VARCHAR(50), Correo VARCHAR(50), " +
                    "Edad INTEGER, Saldo NUMERIC(18,2));";
                conexion.Open();

                using (var query = conexion.CreateCommand())
                {
                    query.CommandText = sql;
                    query.ExecuteNonQuery();
                }
                conexion.Close();
            }
        }

        public bool IngresarDatos(string Nombre, string Domicilio, string Correo, int Edad, double Saldo)
        {
            var RutaBase = Path.Combine(System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal), basededatos);
            var conexion = new SqliteConnection("Data Source=" + RutaBase);

            try
            {

                var sql = "INSERT INTO Datos (Nombre, Domicilio, Correo, Edad, Saldo)" +
                    "VALUES (@Nombre, @Domicilio, @Correo, @Edad, @Saldo);";

                conexion.Open();

                using (var query = conexion.CreateCommand())
                {
                    query.CommandText = sql;
                    query.Parameters.AddWithValue("@Nombre", Nombre);
                    query.Parameters.AddWithValue("@Domicilio", Domicilio);
                    query.Parameters.AddWithValue("@Correo", Correo);
                    query.Parameters.AddWithValue("@Edad", Edad);
                    query.Parameters.AddWithValue("@Saldo", Saldo);
                    query.ExecuteNonQuery();
                }
                conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                conexion.Close();
                return false;
            }
        }

        public void Buscar(int folio)
        {
            var RutaBase = Path.Combine(System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal), basededatos);
            var conexion = new SqliteConnection("Data Source=" + RutaBase);

            try
            {
                conexion.Open();

                using (var contenido = conexion.CreateCommand())
                {
                    contenido.CommandText = "SELECT * FROM Datos WHERE Folio='" + folio + "';";
                    var lectura = contenido.ExecuteReader();
                    while (lectura.Read())
                    {
                        folio = int.Parse(lectura[0].ToString());
                        nombre = lectura[1].ToString();
                        domicilio = lectura[2].ToString();
                        correo = lectura[3].ToString();
                        edad = int.Parse(lectura[4].ToString());
                        saldo = double.Parse(lectura[5].ToString());
                    }
                }
                conexion.Close();
            }
            catch (Exception ex)
            {
                conexion.Close();
            }
        }
    }
}