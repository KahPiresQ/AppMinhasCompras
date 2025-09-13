using System.Globalization;
using AppMinhasCompras.Helpers;

namespace AppMinhasCompras
{
    public partial class App : Application
    {
        static SQLiteDatabaseHelper _db;
        public static SQLiteDatabaseHelper Db
        {  get {
                if (_db == null)
                {
                    string path = Path.Combine(
                        Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        "banco_sqlite_compras.d3"
                        );

                    _db = new SQLiteDatabaseHelper(path);
                }

                return _db; 
            
            } 
        }
        public App()
        {
            InitializeComponent();
            //inclusão de Current seguindo aula 6

            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            // MainPage = new AppShell();
            //vincular a tela principal a tela de navegação desejado. no caso Listaproduto

            MainPage = new NavigationPage(new Views.ListaProduto());

        }
    }
}
