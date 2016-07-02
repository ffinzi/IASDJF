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
            SqlConnection conn = Connection.obterConexao();

            // a conexão foi efetuada com sucesso?
            if (conn == null)
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
            SqlConnection conn = Connection.obterConexao();
            DataGridViewRow linha;
            DataGridViewCell celula1,celula2;
            linha = dataGridView1.Rows[0]; 
            celula1 = linha.Cells[0];
            celula2 = linha.Cells[1];

            MessageBox.Show("valor 1" + celula1.Value);
            MessageBox.Show("valor 2" + celula2.Value);


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

  

    }
}
