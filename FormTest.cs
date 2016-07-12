using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using IASD_JardimFinotti.br.com.iasdjf.dao;
using IASD_JardimFinotti.br.com.iasdjf.model;


namespace IASD_JardimFinotti
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = Resource_PT.label1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = Resource_EN.label1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // vamos obter a conexão com o banco de dados
            Connection.obterConexao();

            // a conexão foi efetuada com sucesso?
            if (Connection.connection == null)
            {
                MessageBox.Show("Não foi possível obter a conexão. Veja o log de erros.");
            }
            else
            {
                MessageBox.Show("A conexão foi obtida com sucesso.");
            }

            // não precisamos mais da conexão? vamos fechá-la
            Connection.fecharConexao();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            // vamos obter a conexão com o banco de dados
            Connection.obterConexao();
            DataGridViewRow linha;
            DataGridViewCell celula1,celula2;
            linha = dataGridView1.Rows[0]; 
            celula1 = linha.Cells[0];
            celula2 = linha.Cells[1];

            MessageBox.Show("valor 1" + celula1.Value);
            MessageBox.Show("valor 2" + celula2.Value);

            Connection.fecharConexao();
            //conn.

        }

        private void button5_Click(object sender, EventArgs e)
        {
            MailerSender.SendMail(textBox1.Text, richTextBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormDataSet formDataSet = new FormDataSet();
            formDataSet.Show();
        }

        private void OnLoad(object sender, EventArgs e)
        {
          
            List<Apontamentos> lista = new List<Apontamentos>();
            try
            {
                Connection.obterConexao();
                var biz = new br.com.iasdjf.biz.ApontamentosBIZ();
                lista = biz.GetAll();

                //agrupo por datas
                var grupoSabados = lista.GroupBy(c => c.Saturday);
                //cria uma tabela dinamica
                var Dtb_Apontamentos = new DataTable();
                
                //Acrescenta coluna ID Pessoa para posteriormente inserir no DAO Apontamentos
                Dtb_Apontamentos.Columns.Add("Id_Pessoa");
                
                var listaIdNomesDistintos = (from p in lista select p.Id_Pessoa).ToList().Distinct();
                foreach (var item in listaIdNomesDistintos)
                {
                    var row = Dtb_Apontamentos.NewRow();
                    row["Id_Pessoa"] = item;
                    Dtb_Apontamentos.Rows.Add(row);
                    Dtb_Apontamentos.AcceptChanges();
                }

                //Acrescenta coluna Nome Pessoa
                Dtb_Apontamentos.Columns.Add("Nome");
                var listaNomesDistintos = (from p in lista select p.Nome).ToList().Distinct();
                
                int rows = 0;
                foreach (DataRow row in Dtb_Apontamentos.Rows)
                {
                    row["Nome"] = listaNomesDistintos.ElementAtOrDefault(rows);            
                    Dtb_Apontamentos.AcceptChanges();
                    rows++;
                }

                int count = 0;
                //insere as n datas como colunas no datatable
                foreach (var itemColuna in grupoSabados)
                {
                    //Adiciona coluna de Sabado concatenando com o Id do Sabado
                    Dtb_Apontamentos.Columns.Add(itemColuna.Key.ToString("dd/MM") + "_" + itemColuna.ElementAtOrDefault(count).Id_Saturday.ToString());
                    foreach (DataRow row in Dtb_Apontamentos.Rows)
                    {
                        var presence = itemColuna.Where(c => c.Nome == row["Nome"].ToString()).FirstOrDefault();
                        if (presence != null)
                            row[itemColuna.Key.ToString("dd/MM") + "_" + itemColuna.ElementAtOrDefault(count).Id_Saturday.ToString()] = presence.Presence;
                        else
                            row[itemColuna.Key.ToString("dd/MM") + "_" + itemColuna.ElementAtOrDefault(count).Id_Saturday.ToString()] = "";
                    }
                    count++;

                }
                
                //Faz o bind para o datagrid
                dataGridView1.DataSource = Dtb_Apontamentos;

                //Oculta a 1ª coluna
                this.dataGridView1.Columns["Id_Pessoa"].Visible = false;
                
                //Altera o texto de exibicao das colunas retirando o codigo
                for (int i = 2; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].HeaderText = dataGridView1.Columns[i].Name.Split('_')[0];                 
                }


                //Altera cores celulas
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
     
                    //Trava a 1ª coluna para edicao
                    foreach(DataGridViewColumn column in dataGridView1.Columns)
                    { 
                        if (column.Index == 1)
                        {
                            column.ReadOnly = true;
                        }
                        
                        //Desabilita a ordenacao das colunas
                        column.SortMode = DataGridViewColumnSortMode.NotSortable;

                        DataGridViewCell cell = dataGridView1.Rows[row.Index].Cells[column.Index];
                        if (cell.Value != null)
                        {
                            if (cell.Value.Equals("P"))
                            {
                                cell.Style.BackColor = Color.Cyan;
                            }
                            else if (cell.Value.Equals("F"))
                            {
                                cell.Style.BackColor = Color.Pink;
                            }

                        }
                       

                    }

             }
                
                //Redimensiona as colunas
                dataGridView1.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar apontamentos. Detalhes:" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Connection.fecharConexao();
            }

            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            List<Apontamentos> lista = new List<Apontamentos>();
            try
            {
                Connection.obterConexao();
                var biz = new br.com.iasdjf.biz.ApontamentosBIZ();

                //deleta todos os apontamentos
                biz.DeleteAll();

                //lista = dataGridView1.DataSource;
                lista = biz.GetAll();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inserir apontamentos. Detalhes:" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Connection.fecharConexao();
            }


        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Valida formatacao P ou F
            if ((dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "p" &&
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "P") &&
                (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "f" &&
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "F"))
            {

                MessageBox.Show("Os valores permitidos para preenchimento são: P ou F ","Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.White;
            }
            else
            {
                //Valida cores
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals("P") || dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals("p"))
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Cyan;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "P";
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals("F") || dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.Equals("f"))
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.Pink;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "F";
                }

            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }



  

    }
}
