using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Word;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace WordDocument2
{
    public partial class ThisDocument
    {
        string accountNumber = "";
        string bankID = "";
        private void ThisDocument_Startup(object sender, System.EventArgs e)
        {
            // TODO: Delete this line of code to remove the default AutoFill for 'erds2DataSet.Accounts'.
            //if (this.NeedsFill("erds2DataSet"))
            //{
            //    this.accountsTableAdapter.Fill(this.erds2DataSet.Accounts);
            //}
        }

        private void ThisDocument_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            this.accountNumberTextBox.TextChanged += new System.EventHandler(this.AccountNumberTextBox_TextChanged);
            this.bankIDTextBox.TextChanged += new System.EventHandler(this.BankIDTextBox_TextChanged);
            this.Startup += new System.EventHandler(this.ThisDocument_Startup);
            this.Shutdown += new System.EventHandler(this.ThisDocument_Shutdown);

        }

        #endregion

        private void Button1_Click(object sender, EventArgs e)
        {
            this.accountsBindingSource.Filter = "AccountNumber = '" + accountNumber + "' and BankID = '" + bankID + "'";
            this.accountsTableAdapter.Fill(this.erds2DataSet.Accounts);
        }

        private void BankIDTextBox_TextChanged(object sender, EventArgs e)
        {
            bankID = bankIDTextBox.Text.ToString();
        }

        private void AccountNumberTextBox_TextChanged(object sender, EventArgs e)
        {
            accountNumber = accountNumberTextBox.Text.ToString();
        }
    }
}
