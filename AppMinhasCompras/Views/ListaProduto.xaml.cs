using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppMinhasCompras.Models;

namespace AppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;

	}
    protected async override void OnAppearing()
    { //inclusao try catch
        try 
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
           await DisplayAlert("Ops", ex.Message, "OK");

        }
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		}
        catch (Exception ex)
		{
			DisplayAlert("ops", ex.Message, "OK");
		}
    }

    private async void txt_search_TextChanged_1(object sender, TextChangedEventArgs e)
    {//inclusao try cath
        try
        {
            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
           await DisplayAlert("ops", ex.Message, "OK");

        } finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);
		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");

    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
		//Codigo refeito seguindo a video aula do professor Thiago, assim esta funcionando a navegação e os botões de ação para excluir.
		{
            try
            {
                MenuItem selecinado = sender as MenuItem;
                Produto p = selecinado.BindingContext as Produto;

                bool confirm = await DisplayAlert("Tem Certeza?", "Remover Produto?", "Sim", "Não");
                if (confirm)
                {
                    await App.Db.Delete(p.Id);
                    lista.Remove(p);
                }
            }
                catch (Exception ex)
                {
                    await DisplayAlert("Ops", ex.Message, "OK");
                }
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //criado junto com a video aula para navegar da tela produto para a tela de edição do p
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });

        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }

    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    { //metodo para recarga dinamica e atualização da tela, seguindo a aula 6, com o finally para parar a recarga
        try
        {
            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");

        } finally
        {
            lst_produtos.IsRefreshing = false;
        }

    }

    private async void AbrirRelatorio_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioCategoria());
    }

    // ação criada para uso do  filtro por categoria
    private async void pkFiltroCategoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        var cat = pkFiltroCategoria.SelectedItem?.ToString();

        lst_produtos.IsRefreshing = true;
        try
        {
            lista.Clear();
            var tmp = string.IsNullOrWhiteSpace(cat) || cat == "Todas"
                ? await App.Db.GetAll()
                : await App.Db.Search(cat); // Search já filtra por Descrição OU Categoria
            tmp.ForEach(i => lista.Add(i));
        }
        finally
        {
            lst_produtos.IsRefreshing = false;
        }
    }
}


