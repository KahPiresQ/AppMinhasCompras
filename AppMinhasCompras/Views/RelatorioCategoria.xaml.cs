using AppMinhasCompras.Models;

namespace AppMinhasCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    public RelatorioCategoria()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var dados = await App.Db.GetTotaisPorCategoria();
        lstRelatorio.ItemsSource = dados;
    }
}