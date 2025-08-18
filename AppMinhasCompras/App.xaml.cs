namespace AppMinhasCompras
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // MainPage = new AppShell();
            //vincular a tela principal a tela de navegação desejado. no caso Listaproduto

            MainPage = new NavigationPage(new Views.ListaProduto());

        }
    }
}
