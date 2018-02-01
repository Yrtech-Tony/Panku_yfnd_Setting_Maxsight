namespace XHX.View
{
    partial class RecheckErrorPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grcLink = new DevExpress.XtraGrid.GridControl();
            this.grvLink = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gcRecheckErrorCode = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gcRecheckErrorName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAreaCode = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.chkUseChk = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.grcLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvLink)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAreaCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseChk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grcLink
            // 
            this.grcLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grcLink.Location = new System.Drawing.Point(0, 42);
            this.grcLink.MainView = this.grvLink;
            this.grcLink.Name = "grcLink";
            this.grcLink.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboAreaCode,
            this.chkUseChk});
            this.grcLink.Size = new System.Drawing.Size(592, 480);
            this.grcLink.TabIndex = 21;
            this.grcLink.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvLink});
            // 
            // grvLink
            // 
            this.grvLink.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gcRecheckErrorCode,
            this.gcRecheckErrorName});
            this.grvLink.GridControl = this.grcLink;
            this.grvLink.Name = "grvLink";
            this.grvLink.OptionsView.ShowGroupPanel = false;
            // 
            // gcRecheckErrorCode
            // 
            this.gcRecheckErrorCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gcRecheckErrorCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcRecheckErrorCode.Caption = "错误代码";
            this.gcRecheckErrorCode.FieldName = "ErrorTypeCode";
            this.gcRecheckErrorCode.Name = "gcRecheckErrorCode";
            this.gcRecheckErrorCode.OptionsColumn.AllowEdit = false;
            this.gcRecheckErrorCode.Visible = true;
            this.gcRecheckErrorCode.VisibleIndex = 0;
            // 
            // gcRecheckErrorName
            // 
            this.gcRecheckErrorName.AppearanceHeader.Options.UseTextOptions = true;
            this.gcRecheckErrorName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcRecheckErrorName.Caption = "错误名称";
            this.gcRecheckErrorName.FieldName = "ErrorTypeName";
            this.gcRecheckErrorName.Name = "gcRecheckErrorName";
            this.gcRecheckErrorName.OptionsColumn.AllowEdit = false;
            this.gcRecheckErrorName.OptionsColumn.AllowSize = false;
            this.gcRecheckErrorName.OptionsColumn.ReadOnly = true;
            this.gcRecheckErrorName.Visible = true;
            this.gcRecheckErrorName.VisibleIndex = 1;
            // 
            // cboAreaCode
            // 
            this.cboAreaCode.AutoHeight = false;
            this.cboAreaCode.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAreaCode.Name = "cboAreaCode";
            // 
            // chkUseChk
            // 
            this.chkUseChk.AutoHeight = false;
            this.chkUseChk.Name = "chkUseChk";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnConfirm.Location = new System.Drawing.Point(478, 14);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(91, 23);
            this.btnConfirm.TabIndex = 9;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnConfirm);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelControl1.Size = new System.Drawing.Size(592, 42);
            this.panelControl1.TabIndex = 19;
            // 
            // RecheckErrorPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 522);
            this.Controls.Add(this.grcLink);
            this.Controls.Add(this.panelControl1);
            this.Name = "RecheckErrorPopup";
            this.Text = "RecheckErrorPopup";
            ((System.ComponentModel.ISupportInitialize)(this.grcLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvLink)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAreaCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkUseChk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl grcLink;
        private DevExpress.XtraGrid.Views.Grid.GridView grvLink;
        private DevExpress.XtraGrid.Columns.GridColumn gcRecheckErrorName;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboAreaCode;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkUseChk;
        private DevExpress.XtraEditors.SimpleButton btnConfirm;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gcRecheckErrorCode;
    }
}