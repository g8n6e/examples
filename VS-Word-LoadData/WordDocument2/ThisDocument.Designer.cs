﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#pragma warning disable 414
namespace WordDocument2 {
    
    
    /// 
    [Microsoft.VisualStudio.Tools.Applications.Runtime.StartupObjectAttribute(0)]
    [global::System.Security.Permissions.PermissionSetAttribute(global::System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
    public sealed partial class ThisDocument : Microsoft.Office.Tools.Word.DocumentBase {
        
        internal Microsoft.Office.Tools.ActionsPane ActionsPane;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl plainTextContentControl3;
        
        internal Microsoft.Office.Tools.Word.PlainTextContentControl plainTextContentControl4;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        private global::System.Object missing = global::System.Type.Missing;
        
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        internal Microsoft.Office.Interop.Word.Application ThisApplication;
        
        internal Microsoft.Office.Tools.Word.Controls.Button button1;
        
        internal Microsoft.Office.Tools.Word.Controls.TextBox accountNumberTextBox;
        
        internal Microsoft.Office.Tools.Word.Controls.Label label1;
        
        internal Microsoft.Office.Tools.Word.Controls.TextBox bankIDTextBox;
        
        internal Microsoft.Office.Tools.Word.Controls.Label label2;
        
        internal WordDocument2.ERDS2DataSet erds2DataSet;
        
        internal WordDocument2.ERDS2DataSetTableAdapters.TableAdapterManager tableAdapterManager;
        
        internal WordDocument2.ERDS2DataSetTableAdapters.AccountsTableAdapter accountsTableAdapter;
        
        internal System.Windows.Forms.BindingSource accountsBindingSource;
        
        private System.ComponentModel.IContainer components;
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        public ThisDocument(global::Microsoft.Office.Tools.Word.Factory factory, global::System.IServiceProvider serviceProvider) : 
                base(factory, serviceProvider, "ThisDocument", "ThisDocument") {
            Globals.Factory = factory;
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void Initialize() {
            base.Initialize();
            this.ThisApplication = this.GetHostItem<Microsoft.Office.Interop.Word.Application>(typeof(Microsoft.Office.Interop.Word.Application), "Application");
            Globals.ThisDocument = this;
            global::System.Windows.Forms.Application.EnableVisualStyles();
            this.InitializeCachedData();
            this.InitializeControls();
            this.InitializeComponents();
            this.InitializeData();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void FinishInitialization() {
            this.InternalStartup();
            this.OnStartup();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void InitializeDataBindings() {
            this.BeginInitialization();
            this.BindToData();
            this.EndInitialization();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeCachedData() {
            if ((this.DataHost == null)) {
                return;
            }
            if (this.DataHost.IsCacheInitialized) {
                this.DataHost.FillCachedData(this);
            }
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeData() {
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void BindToData() {
            this.plainTextContentControl3.DataBindings.Add("Text", this.accountsBindingSource, "AccountName", true, this.plainTextContentControl3.DataBindings.DefaultDataSourceUpdateMode);
            this.plainTextContentControl4.DataBindings.Add("Text", this.accountsBindingSource, "AccountID", true, this.plainTextContentControl4.DataBindings.DefaultDataSourceUpdateMode);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private void StartCaching(string MemberName) {
            this.DataHost.StartCaching(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private void StopCaching(string MemberName) {
            this.DataHost.StopCaching(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private bool IsCached(string MemberName) {
            return this.DataHost.IsCached(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void BeginInitialization() {
            this.BeginInit();
            this.ActionsPane.BeginInit();
            this.plainTextContentControl3.BeginInit();
            this.plainTextContentControl4.BeginInit();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void EndInitialization() {
            this.plainTextContentControl4.EndInit();
            this.plainTextContentControl3.EndInit();
            this.ActionsPane.EndInit();
            this.EndInit();
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeControls() {
            this.ActionsPane = Globals.Factory.CreateActionsPane(null, null, "ActionsPane", "ActionsPane", this);
            this.plainTextContentControl3 = Globals.Factory.CreatePlainTextContentControl(null, null, "1373271101", "plainTextContentControl3", this);
            this.plainTextContentControl4 = Globals.Factory.CreatePlainTextContentControl(null, null, "3814888078", "plainTextContentControl4", this);
            this.button1 = new Microsoft.Office.Tools.Word.Controls.Button(Globals.Factory, this.ItemProvider, this.HostContext, "07A460F5F0347B04210097B9046FF1DF87F0C0", "07A460F5F0347B04210097B9046FF1DF87F0C0", this, "button1");
            this.accountNumberTextBox = new Microsoft.Office.Tools.Word.Controls.TextBox(Globals.Factory, this.ItemProvider, this.HostContext, "1C7CF36051330C14B8718DEC14BB0E60A73191", "1C7CF36051330C14B8718DEC14BB0E60A73191", this, "accountNumberTextBox");
            this.label1 = new Microsoft.Office.Tools.Word.Controls.Label(Globals.Factory, this.ItemProvider, this.HostContext, "2ED4516E726B6324A76288A02469334961DF32", "2ED4516E726B6324A76288A02469334961DF32", this, "label1");
            this.bankIDTextBox = new Microsoft.Office.Tools.Word.Controls.TextBox(Globals.Factory, this.ItemProvider, this.HostContext, "3F2E1114F3A84534C343A4C932A6FA30EC96F3", "3F2E1114F3A84534C343A4C932A6FA30EC96F3", this, "bankIDTextBox");
            this.label2 = new Microsoft.Office.Tools.Word.Controls.Label(Globals.Factory, this.ItemProvider, this.HostContext, "4E125455E4957F44CC14A7A3414CF36FB57BF4", "4E125455E4957F44CC14A7A3414CF36FB57BF4", this, "label2");
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        private void InitializeComponents() {
            this.components = new System.ComponentModel.Container();
            if ((this.erds2DataSet == null)) {
                // Instantiate the object if not yet loaded from the data cache.
                this.erds2DataSet = new WordDocument2.ERDS2DataSet();
            }
            this.tableAdapterManager = new WordDocument2.ERDS2DataSetTableAdapters.TableAdapterManager();
            this.accountsTableAdapter = new WordDocument2.ERDS2DataSetTableAdapters.AccountsTableAdapter();
            this.accountsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.erds2DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsBindingSource)).BeginInit();
            // 
            // ActionsPane
            // 
            this.ActionsPane.AutoSize = false;
            this.ActionsPane.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            // 
            // erds2DataSet
            // 
            this.erds2DataSet.DataSetName = "ERDS2DataSet";
            this.erds2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.AccountsTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.UpdateOrder = WordDocument2.ERDS2DataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // accountsTableAdapter
            // 
            this.accountsTableAdapter.ClearBeforeFill = true;
            // 
            // accountsBindingSource
            // 
            this.accountsBindingSource.DataMember = "Accounts";
            this.accountsBindingSource.DataSource = this.erds2DataSet;
            // 
            // plainTextContentControl3
            // 
            this.plainTextContentControl3.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // plainTextContentControl4
            // 
            this.plainTextContentControl4.DefaultDataSourceUpdateMode = System.Windows.Forms.DataSourceUpdateMode.Never;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Name = "button1";
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // accountNumberTextBox
            // 
            this.accountNumberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.accountsBindingSource, "AccountNumber", true));
            this.accountNumberTextBox.Name = "accountNumberTextBox";
            // 
            // label1
            // 
            this.label1.Name = "label1";
            this.label1.Text = "AccountNumber: ";
            // 
            // bankIDTextBox
            // 
            this.bankIDTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.accountsBindingSource, "BankID", true));
            this.bankIDTextBox.Name = "bankIDTextBox";
            // 
            // label2
            // 
            this.label2.Name = "label2";
            this.label2.Text = "BankID: ";
            // 
            // ThisDocument
            // 
            ((System.ComponentModel.ISupportInitialize)(this.erds2DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.accountsBindingSource)).EndInit();
            this.button1.BindingContext = this.BindingContext;
            this.accountNumberTextBox.BindingContext = this.BindingContext;
            this.label1.BindingContext = this.BindingContext;
            this.bankIDTextBox.BindingContext = this.BindingContext;
            this.label2.BindingContext = this.BindingContext;
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        private bool NeedsFill(string MemberName) {
            return this.DataHost.NeedsFill(this, MemberName);
        }
        
        /// 
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Never)]
        protected override void OnShutdown() {
            this.plainTextContentControl4.Dispose();
            this.plainTextContentControl3.Dispose();
            this.ActionsPane.Dispose();
            base.OnShutdown();
        }
    }
    
    /// 
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
    internal sealed partial class Globals {
        
        /// 
        private Globals() {
        }
        
        private static ThisDocument _ThisDocument;
        
        private static global::Microsoft.Office.Tools.Word.Factory _factory;
        
        private static ThisRibbonCollection _ThisRibbonCollection;
        
        internal static ThisDocument ThisDocument {
            get {
                return _ThisDocument;
            }
            set {
                if ((_ThisDocument == null)) {
                    _ThisDocument = value;
                }
                else {
                    throw new System.NotSupportedException();
                }
            }
        }
        
        internal static global::Microsoft.Office.Tools.Word.Factory Factory {
            get {
                return _factory;
            }
            set {
                if ((_factory == null)) {
                    _factory = value;
                }
                else {
                    throw new System.NotSupportedException();
                }
            }
        }
        
        internal static ThisRibbonCollection Ribbons {
            get {
                if ((_ThisRibbonCollection == null)) {
                    _ThisRibbonCollection = new ThisRibbonCollection(_factory.GetRibbonFactory());
                }
                return _ThisRibbonCollection;
            }
        }
    }
    
    /// 
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Tools.Office.ProgrammingModel.dll", "16.0.0.0")]
    internal sealed partial class ThisRibbonCollection : Microsoft.Office.Tools.Ribbon.RibbonCollectionBase {
        
        /// 
        internal ThisRibbonCollection(global::Microsoft.Office.Tools.Ribbon.RibbonFactory factory) : 
                base(factory) {
        }
    }
}
