using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace Ejercicio2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        string Usuario, Password, Ruta;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            SupportActionBar.Hide();
            var btnIgresa = FindViewById<Button>(Resource.Id.btnLogin);
            var txtUser = FindViewById<EditText>(Resource.Id.txtUsername);
            var txtPassword = FindViewById<EditText>(Resource.Id.editTextPassword);
            var ImagenLogin = FindViewById<ImageView>(Resource.Id.ivLogin);
            ImagenLogin.SetImageResource(Resource.Drawable.logo);

            btnIgresa.Click += delegate
            {
                Usuario = txtUser.Text;
                Password = txtPassword.Text;
                var WS = new ServicioWeb.ServicioWeb();
                int resultado = WS.validaUsuario(Usuario, Password);
                if (resultado == 1)
                    Cargar();
                else
                    Toast.MakeText(this, "Favor de revisar el usuario y password", ToastLength.Long).Show();
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public void Cargar()
        {
            Intent objIntent = new Intent(this, typeof(ActividadPrincipal));
            objIntent.PutExtra("Usuario", Usuario);
            StartActivity(objIntent);
        }
    }
}