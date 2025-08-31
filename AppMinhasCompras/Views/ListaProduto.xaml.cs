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
	{
		List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => lista.Add(i));	
	}

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());
		} catch (Exception ex)
		{
			DisplayAlert("ops", ex.Message, "OK");
		}
    }

    private async void txt_search_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue;

        lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));

    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);
		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");


    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
		// parte a ser criada com ajuda da IA, sem alterar ou descaracterizar o que foi feito até aqui
		{
			if (sender is MenuItem mi && mi.CommandParameter is Produto p)
			{
				bool ok = await DisplayAlert("Remover", $"Excluir' {p.Descricao}'?", "Sim", "Não");
				if (ok) return;

				try
				{

                    // se seu repositório tiver Delete(Produto p):
                    await App.Db.Delete(p.Id);

                    // (alternativa) se for Delete(int id), use:
                    // await App.Db.Delete(p.Id);

                    lista.Remove(p); // atualiza a UI
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Ops", ex.Message, "OK");
                }
            }
        }

    }
}
