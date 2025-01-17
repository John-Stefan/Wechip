﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeChip.App.Models;
using WeChip.App.Repository;

namespace WeChip.App.Forms
{
    public partial class FormProduto : Form
    {
        IProdutoTipoRepository _repositorioProdutoTipo = new ProdutoTipoRepository();
        IProdutoRepository _repositorio = new ProdutoRepository();
        Produto produto = new Produto();

        public FormProduto()
        {
            InitializeComponent();

            InicializeComboBox();
            HabilitaCampos(false);
        }

        private async void InicializeComboBox()
        {
            var listaProdutoTipos = await _repositorioProdutoTipo.GetProdutoTiposAsync();

            foreach (var produtoTipo in listaProdutoTipos)
                comboBoxProduto.Items.Add(produtoTipo.Descricao);
        }

        private void HabilitaCampos(bool active)
        {
            textBoxCodigo.Enabled = active;
            textBoxDescricao.Enabled = active;
            textBoxPreco.Enabled = active;
            comboBoxProduto.Enabled = active;

            buttonEditar.Enabled = active;
        }

        private void LimpaCampos()
        {
            textBoxCodigo.Text = "";
            textBoxDescricao.Text = "";
            textBoxPreco.Text = "";
            comboBoxProduto.Text = "";
        }

        private void ValidarCampos(string campo, string campoName)
        {
            if (string.IsNullOrEmpty(campo))
                throw new Exception($"O campo {campoName} é obrigatorio");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            produto = null;
            HabilitaCampos(true);
            buttonEditar.Enabled = false;
            LimpaCampos();
        }

        private async void buttonSalvar_Click(object sender, EventArgs e)
        {
            var notSelection = -1;

            try
            {
                ValidarCampos(textBoxCodigo.Text, "Codigo");
                ValidarCampos(textBoxDescricao.Text, "Descrição");
                ValidarCampos(textBoxPreco.Text, "Preço");

                if (comboBoxProduto.SelectedIndex == notSelection)                
                    throw new Exception("Selecione o tipo do produto");                

                var response = await _repositorio.CadastroProdutoAsync(new Produto
                {
                    Codigo = textBoxCodigo.Text,
                    Descricao = textBoxDescricao.Text,
                    Preco = Convert.ToDecimal(textBoxPreco.Text.Replace("R$ ", "")),
                    Tipo = comboBoxProduto.SelectedIndex + 1
                });

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Cadastro do produto {textBoxDescricao.Text} realizado com sucesso", "Produto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpaCampos();
                    textBoxCodigo.Enabled = false;
                    textBoxDescricao.Enabled = false;
                    textBoxPreco.Enabled = false;
                    comboBoxProduto.Enabled = false;
                    buttonSalvar.Enabled = false;
                }
                else                
                    throw new Exception("Erro ao tentar realizar requisição ao servidor");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonPesquisar_Click(object sender, EventArgs e)
        {
            var produtoResult = await _repositorio.PesquisarProdutoAsync(textBoxPesquisa.Text);

            if (produtoResult != null)
            {
                textBoxCodigo.Text = produtoResult.Codigo;
                textBoxDescricao.Text = produtoResult.Descricao;
                textBoxPreco.Text = produtoResult.Preco.ToString();
                comboBoxProduto.SelectedIndex = 1;
                comboBoxProduto.SelectedIndex = produtoResult.TipoId - 1;
                produto = produtoResult;

                HabilitaCampos(true);
                textBoxCodigo.Enabled = false;
                buttonSalvar.Enabled = false;
            }
        }

        private async void buttonEditar_Click(object sender, EventArgs e)
        {
            var notSelection = -1;

            try
            {
                ValidarCampos(textBoxCodigo.Text, "Codigo");
                ValidarCampos(textBoxDescricao.Text, "Descrição");
                ValidarCampos(textBoxPreco.Text, "Preço");

                if (comboBoxProduto.SelectedIndex == notSelection)                
                    throw new Exception("Selecione o tipo do produto");

                produto.Codigo = textBoxCodigo.Text;
                produto.Descricao = textBoxDescricao.Text;
                produto.Preco = Convert.ToDecimal(textBoxPreco.Text.Replace("R$ ", ""));

                var response = await _repositorio.AlterarProdutoAsync(produto);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Produto {textBoxDescricao.Text} foi atualizado com sucesso", "Produto", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HabilitaCampos(false);
                    LimpaCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpaCampos();
            }
        }
    }
}
