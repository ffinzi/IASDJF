using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IASD_JardimFinotti
{
    public partial class FormDataSet : Form
    {
        public FormDataSet()
        {
            InitializeComponent();
        }

        private void FormDataSet_Load(object sender, EventArgs e)
        {
            this.tB_TESTETableAdapter.Fill(this.iASDJFDataSet.TB_TESTE);
            this.tBTESTEBindingSource.DataSource = this.iASDJFDataSet.TB_TESTE;
            dataGridView1.DataSource = this.tBTESTEBindingSource;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.tBTESTEBindingSource.EndEdit();
                this.tB_TESTETableAdapter.Update(this.iASDJFDataSet.TB_TESTE);
                this.iASDJFDataSet.AcceptChanges();
                MessageBox.Show("Dados registrados com sucesso !");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Update failed");
            }


        }
    }
}
