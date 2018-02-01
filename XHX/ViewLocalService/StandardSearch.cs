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

namespace XHX.ViewLocalService
{
    public partial class StandardSearch : BaseForm
    {
        localhost.Service webService = new localhost.Service();
        //XtraGridDataHandler<StandardSearchDto> dataHandler = null;
        public StandardSearch()
        {
            InitializeComponent();
            BindComBox.BindProject(cboProject);
            CommonHandler.SetRowNumberIndicator(grvStandardSearch);
        }
        public List<TwoLevelColumnInfo> SearchHead(string projectCode, string shopCode)
        {
            DataSet ds = webService.SearchHead_InspectionStandard(projectCode);
            List<TwoLevelColumnInfo> columnInfoList = new List<TwoLevelColumnInfo>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TwoLevelColumnInfo info = new TwoLevelColumnInfo();
                    info.Column1 = Convert.ToString(ds.Tables[0].Rows[i]["Column1"]);
                    info.Column2 = Convert.ToString(ds.Tables[0].Rows[i]["Column2"]);
                    info.Caption1 = Convert.ToString(ds.Tables[0].Rows[i]["Caption1"]);
                    info.Caption2 = Convert.ToString(ds.Tables[0].Rows[i]["Caption2"]);
                    info.Order = Convert.ToInt32(ds.Tables[0].Rows[i]["Order"]);
                    columnInfoList.Add(info);
                }
            }
            return columnInfoList;
        }
        public List<ShopInstandardBodyDto> SearchBodyData(string projectCode, string shopCode)
        {
            DataSet ds = webService.SearchBodyData_InspectionStandard(projectCode, shopCode);
            List<ShopInstandardBodyDto> shopScoreBodyList = new List<ShopInstandardBodyDto>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopInstandardBodyDto info = new ShopInstandardBodyDto();
                    info.Column1 = Convert.ToString(ds.Tables[0].Rows[i]["Column1"]);
                    info.Column2 = Convert.ToString(ds.Tables[0].Rows[i]["Column2"]);

                    info.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    if (ds.Tables[0].Rows[i]["Value"] != DBNull.Value)
                    {
                        info.Value = Convert.ToString(ds.Tables[0].Rows[i]["Value"]);
                    }

                    shopScoreBodyList.Add(info);
                }
            }
            return shopScoreBodyList;
        }
        public List<ShopInstandardInfo> SearchLeft(string projectCode, string shopCode)
        {
            DataSet ds = webService.SearchLeft_InspectionStandard(projectCode,shopCode);
            List<ShopInstandardInfo> shopScoreLeftList = new List<ShopInstandardInfo>();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopInstandardInfo info = new ShopInstandardInfo();
                    info.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    info.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    shopScoreLeftList.Add(info);
                }
            }
            return shopScoreLeftList;
        }
        private void SearchRateAll()
        {
            //grcStandardSearch.DataSource = null;
            //List<StandardSearchDto> fileList = new List<StandardSearchDto>();
            //DataSet ds = webService.SearchCheckOptionByProjectCodeAndShopCode(CommonHandler.GetComboBoxSelectedValue(cboProject).ToString(),tbnShop.Text.Trim());
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //    {
            //        StandardSearchDto file = new StandardSearchDto();
            //        file.ProjectName = Convert.ToString(ds.Tables[0].Rows[i]["ProjectName"]);
            //        file.SubjectCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectCode"]);
            //        file.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
            //        file.SeqNO = Convert.ToInt32(ds.Tables[0].Rows[i]["SeqNO"]);
            //        file.InspectionStandardName = Convert.ToString(ds.Tables[0].Rows[i]["InspectionStandardName"]);
            //        file.CheckOptionName = Convert.ToString(ds.Tables[0].Rows[i]["CheckOptionName"]);
            //        fileList.Add(file);
            //    }
            //    grcStandardSearch.DataSource = fileList;
            //}
            grcStandardSearch.DataSource = null;
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProject).ToString();
            string shopCode = tbnShop.Text;
            List<TwoLevelColumnInfo> columnInfoList = SearchHead(projectCode,shopCode);
            List<ShopInstandardBodyDto> dataList = SearchBodyData(projectCode, shopCode);
            List<ShopInstandardInfo> leftList = SearchLeft(projectCode,shopCode);

            DynamicColumnDataSet<TwoLevelColumnInfo, ShopInstandardBodyDto, ShopInstandardInfo> list = new DynamicColumnDataSet<TwoLevelColumnInfo, ShopInstandardBodyDto, ShopInstandardInfo>(columnInfoList, dataList, leftList);
            if (list != null && list.ColumnInfoList != null)
            {
                list.DtoList = DynamicColumnUtil.CombineDynamicColumnDto<TwoLevelColumnInfo, ShopInstandardBodyDto, ShopInstandardInfo>(list.ColumnInfoList, list.DataList, list.DtoList);
            }

            //BindGrid

            CommonHandler.BuildDynamicColumn<TwoLevelColumnInfo, ShopInstandardInfo>(grvStandardSearch, list.ColumnInfoList, list.DtoList);


            grvStandardSearch.LeftCoord = 0;
        }

        public override void SearchButtonClick()
        {
            if (CommonHandler.GetComboBoxSelectedValue(cboProject) == null)
            {
                CommonHandler.ShowMessage(MessageType.Information, "请选择项目");
                return;
            }
            SearchRateAll();
        }
        public override void InitButtonClick()
        {
            BindComBox.BindProject(cboProject);
            //dataHandler = new XtraGridDataHandler<StandardSearchDto>(grvStandardSearch);
            CommonHandler.SetRowNumberIndicator(grvStandardSearch);
        }
        public override List<ButtonType> CreateButton()
        {
            List<ButtonType> list = new List<ButtonType>();
            list.Add(ButtonType.InitButton);
            list.Add(ButtonType.SearchButton);
            return list;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (grcStandardSearch.DataSource != null)
                CommonHandler.ExcelExportByExporter(grvStandardSearch);
        }
        private void tbnShop_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Shop_Popup shop = new Shop_Popup("", "", false);
            shop.ShowDialog();
            if (shop != null)
            {
                string shopName = string.Empty;
                string shopCode = string.Empty;
                ShopDto shopDto = shop.Shopdto;
                txtShop.Text = shopDto.ShopName;
                tbnShop.Text = shopDto.ShopCode;
            }
        }
    }
}
