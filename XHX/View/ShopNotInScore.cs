using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.DTO;
using XHX.Common;

namespace XHX.View
{
    public partial class ShopNotInScore : Form
    {
        localhost.Service webService = new localhost.Service();
        XtraGridDataHandler<ShopNotScoreDto> dataHandler = null;
        public string _projectCode = string.Empty;
        //GridCheckMarksSelection selection;
        //internal GridCheckMarksSelection Selection
        //{
        //    get
        //    {
        //        return selection;
        //    }
        //}
        public ShopNotInScore()
        {
            InitializeComponent();
        }
        public ShopNotInScore(string projectCode)
        {
            InitializeComponent();
            _projectCode = projectCode;
            InitData();
        }
        public void InitData()
        {
            dataHandler = new XtraGridDataHandler<ShopNotScoreDto>(gridView1);
            CommonHandler.SetRowNumberIndicator(gridView1);
            grcShop.DataSource = new List<ShopNotScoreDto>();
            //selection = new GridCheckMarksSelection(gridView1);
            //selection.CheckMarkColumn.VisibleIndex = 0;
            BindComBox.BindProject(cboProjects);
            CommonHandler.SetComboBoxSelectedValue(cboProjects, _projectCode);
        }
        private void Search()
        {
            DataSet ds = webService.SearchShopNotScore(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString());
            List<ShopNotScoreDto> list = new List<ShopNotScoreDto>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopNotScoreDto project = new ShopNotScoreDto();
                    project.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    project.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    project.NotScoreChk = Convert.ToBoolean(ds.Tables[0].Rows[i]["NotScoreChk"]);
                    list.Add(project);
                }
                grcShop.DataSource = list;

            }
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                foreach (ShopNotScoreDto s in grcShop.DataSource as List<ShopNotScoreDto>)
                {
                    webService.InsertShopNotScore(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString(), s.ShopCode, s.NotScoreChk);
                }
                Search();
            }
            
        }
    }
}
