using System;
using System.Threading.Tasks;
using AppMinhasCompras.Models;

namespace AppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
	public NovoProduto()
	{
		InitializeComponent();
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Produto p = new Produto
			{
				Descricao = txt_descricao.Text,
				Quantidade = Convert.ToDouble(txt_quantidade.Text),
				Preco = Convert.ToDouble(txt_preco.Text),

                // NOVO: categoria, pegar picker
                Categoria = pk_categoria.SelectedItem?.ToString() ?? "Sem categoria"

            };

			await App.Db.Insert(p);
			await DisplayAlert("Sucesso!", "Registro Inserido", "OK");
			await Navigation.PopAsync();

		}catch(Exception ex)


		{
			await DisplayAlert("Ops", ex.Message, "Ok");

		}

    }
}