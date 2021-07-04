using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ejercicio2
{
    [Activity(Label = "ActividadPrincipal")]
    public class ActividadPrincipal : Activity
    {
        string Usuario;
        ImageView Imagen;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Principal);

            var lblDestino = FindViewById<TextView>(Resource.Id.txtvUsuario);
            var imagenUsuario = FindViewById<ImageView>(Resource.Id.imgPrincipal);
            var txtFolio = FindViewById<EditText>(Resource.Id.txtFolio);
            var txtNombre = FindViewById<EditText>(Resource.Id.txtName);
            var txtEdad = FindViewById<EditText>(Resource.Id.txtEdad);
            var txtDomicilio = FindViewById<EditText>(Resource.Id.txtDomicilio);
            var txtEmail = FindViewById<EditText>(Resource.Id.txtCorreo);
            var txtSaldo = FindViewById<EditText>(Resource.Id.txtSaldo);
            var btnGuardar = FindViewById<Button>(Resource.Id.btnSaveXML);
            var btnBuscar = FindViewById<Button>(Resource.Id.btnSearchXML);
            var btnBuscarSQL = FindViewById<Button>(Resource.Id.btnSearchSQL);
            var btnGuardarSQL = FindViewById<Button>(Resource.Id.btnSaveSQL);

            Usuario = Intent.GetStringExtra("Usuario");
            lblDestino.Text = Usuario;

            var WS = new ServicioWeb.ServicioWeb();
            string ruta = WS.extraerRutaImagen(Usuario);
            ArchivoImagen(ruta);

            var csql = new ClaseSQLite();
            csql.ConexionBase();

            btnGuardar.Click += delegate
            {
                var DC = new Datos();
                try
                {
                    DC.Folio = int.Parse(txtFolio.Text);
                    DC.Nombre = txtNombre.Text;
                    DC.Correo = txtEmail.Text;
                    DC.Domicilio = txtDomicilio.Text;
                    DC.Edad = int.Parse(txtEdad.Text);
                    DC.Saldo = double.Parse(txtSaldo.Text);

                    var serializador = new XmlSerializer(typeof(Datos));
                    var escritura = new StreamWriter(
                        Path.Combine(System.Environment.GetFolderPath(
                            System.Environment.SpecialFolder.Personal),
                            txtFolio.Text + ".xml"));
                    serializador.Serialize(escritura, DC);
                    escritura.Close();
                    txtFolio.Text = "";
                    txtNombre.Text = "";
                    txtDomicilio.Text = "";
                    txtEmail.Text = "";
                    txtEdad.Text = "";
                    txtSaldo.Text = "";

                    Toast.MakeText(this, "Archivo XML guardado correctamente", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            btnBuscar.Click += delegate
            {
                var DC = new Datos();
                try
                {
                    DC.Folio = int.Parse(txtFolio.Text);

                    var serializador = new XmlSerializer(typeof(Datos));
                    var Lectura = new StreamReader(
                        Path.Combine(System.Environment.GetFolderPath(
                            System.Environment.SpecialFolder.Personal),
                            txtFolio.Text + ".xml"));
                    var datos = (Datos)serializador.Deserialize(Lectura);
                    Lectura.Close();
                    txtNombre.Text = datos.Nombre;
                    txtDomicilio.Text = datos.Domicilio;
                    txtEmail.Text = datos.Correo;
                    txtEdad.Text = datos.Edad.ToString();
                    txtSaldo.Text = datos.Saldo.ToString();

                    Toast.MakeText(this, "Archivo XML cargado correctamente", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            btnGuardarSQL.Click += delegate
            {
                try
                {
                    csql.nombre = txtNombre.Text;
                    csql.domicilio = txtDomicilio.Text;
                    csql.correo = txtEmail.Text;
                    csql.edad = int.Parse(txtEdad.Text);
                    csql.saldo = double.Parse(txtSaldo.Text);

                    if ((csql.IngresarDatos(csql.nombre, csql.domicilio, csql.correo, csql.edad, csql.saldo)) == true)
                    {
                        Toast.MakeText(this, "Guardado correctamente en SQLite", ToastLength.Long).Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "No Guardado", ToastLength.Long).Show();
                    }

                    Toast.MakeText(this, "Archivo XML guardado correctamente", ToastLength.Long).Show();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };

            btnBuscarSQL.Click += delegate
            {
                try
                {
                    csql.folio = int.Parse(txtFolio.Text);
                    csql.Buscar(csql.folio);

                    txtNombre.Text = csql.nombre;
                    txtDomicilio.Text = csql.domicilio;
                    txtEmail.Text = csql.correo;
                    txtEdad.Text = csql.edad.ToString();
                    txtSaldo.Text = csql.saldo.ToString();
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            };
        }

        public async Task<string> DescargarImagen(string url)
        {
            string localpath = "";
            try
            {
                WebClient client = new WebClient();
                byte[] imageData = await client.DownloadDataTaskAsync(url);

                string documentspath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string localfilename = Usuario + ".jpg";
                localpath = Path.Combine(documentspath, localfilename);
                File.WriteAllBytes(localpath, imageData);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
            return localpath;
        }

        async void ArchivoImagen(string url)
        {
            try
            {
                var ruta = await DescargarImagen(url);
                Android.Net.Uri rutaImagen = Android.Net.Uri.Parse(ruta);
                Imagen.SetImageURI(rutaImagen);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
    }

    public class Datos
    {
        public int Folio;
        public string Nombre;
        public string Correo;
        public string Domicilio;
        public int Edad;
        public double Saldo;
    }
}