namespace XHX.View
{
    partial class StandardSearch
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.grdShop = new DevExpress.XtraEditors.PanelControl();
            this.txtShop = new DevExpress.XtraEditors.TextEdit();
            this.tbnShop = new DevExpress.XtraEditors.ButtonEdit();
            this.lblShop = new DevExpress.XtraEditors.LabelControl();
            this.cboProject = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.grcStandardSearch = new DevExpress.XtraGrid.GridControl();
            this.grvStandardSearch = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gcProject = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gcShop = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gcShopCode = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.comFileType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).BeginInit();
            this.grdShop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtShop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbnShop.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProject.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcStandardSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvStandardSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comFileType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelControl1.Size = new System.Drawing.Size(998, 42);
            this.panelControl1.TabIndex = 29;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.simpleButton1.Location = new System.Drawing.Point(895, 9);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(91, 23);
            this.simpleButton1.TabIndex = 9;
            this.simpleButton1.Text = "下载";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl2.Location = new System.Drawing.Point(0, 42);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(998, 5);
            this.labelControl2.TabIndex = 33;
            // 
            // labelControl1
            // 
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(0, 89);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(998, 5);
            this.labelControl1.TabIndex = 35;
            // 
            // grdShop
            // 
            this.grdShop.Controls.Add(this.txtShop);
            this.grdShop.Controls.Add(this.tbnShop);
            this.grdShop.Controls.Add(this.lblShop);
            this.grdShop.Controls.Add(this.cboProject);
            this.grdShop.Controls.Add(this.labelControl3);
            this.grdShop.Dock = System.Windows.Forms.DockStyle.Top;
            this.grdShop.Location = new System.Drawing.Point(0, 47);
            this.grdShop.Margin = new System.Windows.Forms.Padding(0);
            this.grdShop.Name = "grdShop";
            this.grdShop.Size = new System.Drawing.Size(998, 42);
            this.grdShop.TabIndex = 34;
            // 
            // txtShop
            // 
            this.txtShop.Enabled = false;
            this.txtShop.Location = new System.Drawing.Point(360, 13);
            this.txtShop.Name = "txtShop";
            this.txtShop.Size = new System.Drawing.Size(238, 21);
            this.txtShop.TabIndex = 15;
            // 
            // tbnShop
            // 
            this.tbnShop.EditValue = "";
            this.tbnShop.Location = new System.Drawing.Point(254, 13);
            this.tbnShop.Name = "tbnShop";
            this.tbnShop.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.tbnShop.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.tbnShop.Size = new System.Drawing.Size(100, 21);
            this.tbnShop.TabIndex = 14;
            this.tbnShop.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.tbnShop_ButtonClick);
            // 
            // lblShop
            // 
            this.lblShop.Location = new System.Drawing.Point(212, 17);
            this.lblShop.Name = "lblShop";
            this.lblShop.Size = new System.Drawing.Size(36, 14);
            this.lblShop.TabIndex = 13;
            this.lblShop.Text = "经销商";
            // 
            // cboProject
            // 
            this.cboProject.Location = new System.Drawing.Point(72, 13);
            this.cboProject.Name = "cboProject";
            this.cboProject.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboProject.Size = new System.Drawing.Size(100, 21);
            this.cboProject.TabIndex = 1;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(30, 17);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 0;
            this.labelControl3.Text = "项目";
            // 
            // grcStandardSearch
            // 
            this.grcStandardSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grcStandardSearch.Location = new System.Drawing.Point(0, 94);
            this.grcStandardSearch.MainView = this.grvStandardSearch;
            this.grcStandardSearch.Name = "grcStandardSearch";
            this.grcStandardSearch.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.comFileType,
            this.repositoryItemTextEdit1});
            this.grcStandardSearch.Size = new System.Drawing.Size(998, 519);
            this.grcStandardSearch.TabIndex = 39;
            this.grcStandardSearch.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.grvStandardSearch});
            // 
            // grvStandardSearch
            // 
            this.grvStandardSearch.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand2});
            this.grvStandardSearch.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.gcProject,
            this.gcShopCode,
            this.gcShop});
            this.grvStandardSearch.GridControl = this.grcStandardSearch;
            this.grvStandardSearch.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
            this.grvStandardSearch.Name = "grvStandardSearch";
            this.grvStandardSearch.OptionsView.ColumnAutoWidth = false;
            this.grvStandardSearch.OptionsView.ShowGroupPanel = false;
            // 
            // gcProject
            // 
            this.gcProject.AppearanceHeader.Options.UseTextOptions = true;
            this.gcProject.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcProject.Caption = "项目";
            this.gcProject.FieldName = "ProjectName";
            this.gcProject.Name = "gcProject";
            this.gcProject.OptionsColumn.AllowEdit = false;
            this.gcProject.Visible = true;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "经销商";
            this.gridBand2.Columns.Add(this.gcShop);
            this.gridBand2.Columns.Add(this.gcShopCode);
            this.gridBand2.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.Width = 150;
            // 
            // gcShop
            // 
            this.gcShop.AppearanceHeader.Options.UseTextOptions = true;
            this.gcShop.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShop.Caption = "经销商";
            this.gcShop.FieldName = "ShopName";
            this.gcShop.Name = "gcShop";
            this.gcShop.Visible = true;
            // 
            // gcShopCode
            // 
            this.gcShopCode.AppearanceHeader.Options.UseTextOptions = true;
            this.gcShopCode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gcShopCode.Caption = "经销商代码";
            this.gcShopCode.FieldName = "ShopCode";
            this.gcShopCode.Name = "gcShopCode";
            this.gcShopCode.Visible = true;
            // 
            // comFileType
            // 
            this.comFileType.AutoHeight = false;
            this.comFileType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comFileType.Name = "comFileType";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Mask.EditMask = "##0.00%";
            this.repositoryItemTextEdit1.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.repositoryItemTextEdit1.Mask.UseMaskAsDisplayFormat = true;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // StandardSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grcStandardSearch);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.grdShop);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "StandardSearch";
            this.Size = new System.Drawing.Size(998, 613);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdShop)).EndInit();
            this.grdShop.ResumeLayout(false);
            this.grdShop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtShop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbnShop.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboProject.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grcStandardSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvStandardSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comFileType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl grdShop;
        private DevExpress.XtraEditors.ComboBoxEdit cboProject;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit txtShop;
        private DevExpress.XtraEditors.ButtonEdit tbnShop;
        private DevExpress.XtraEditors.LabelControl lblShop;
        private DevExpress.XtraGrid.GridControl grcStandardSearch;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox comFileType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView grvStandardSearch;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcProject;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcShop;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gcShopCode;

    }
}