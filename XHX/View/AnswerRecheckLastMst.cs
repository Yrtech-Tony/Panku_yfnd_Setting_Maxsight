using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.Common;
using XHX.DTO;
using DevExpress.XtraEditors.Repository;
//using XHX.WebService;

namespace XHX.View
{
    public partial class AnswerRecheckLastMst : BaseForm
    {
        XtraGridDataHandler<ShopDto> dataHandler = null;
        localhost.Service webService = new localhost.Service();
        GridCheckMarksSelection selection;
        internal GridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }
        public AnswerRecheckLastMst()
        {
            InitializeComponent();
            OnLoadView();
        }

        public void OnLoadView()
        {
            XHX.Common.BindComBox.BindProject(cboProjects);
            
            dataHandler = new XtraGridDataHandler<ShopDto>(grvShop);
        }
        public void InitializeView()
        {
            txtShopName.Text = "";
            grcShop.DataSource = null;
        }

        public override List<ButtonType> CreateButton()
        {
            List<ButtonType> list = new List<ButtonType>();
            list.Add(ButtonType.InitButton);
            list.Add(ButtonType.SearchButton);
           
            list.Add(ButtonType.SaveButton);
            
            return list;
        }
        public override void InitButtonClick()
        {
            base.InitButtonClick();
            InitializeView();
        }
        public override void SearchButtonClick()
        {
            SearchShop();
            selection = new GridCheckMarksSelection(grvShop);
            selection.CheckMarkColumn.VisibleIndex = 0;
        }
        public override void SaveButtonClick()
        {
            grvShop.CloseEditor();
            grvShop.UpdateCurrentRow();
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<ShopDto> shopList = dataHandler.DataList;
                foreach (ShopDto shop in shopList)
                {
                    if (shop.StatusType == 'I' || shop.StatusType == 'U')
                    {
                        webService.RechekComplete(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString(), shop.ShopCode, "S3", this.UserInfoDto.UserID);
                    }
      
                }
            }
            SearchShop();
            CommonHandler.ShowMessage(MessageType.Information, "保存完毕");
        }

        private void SearchShop()
        {
            List<ShopDto> shopList = new List<ShopDto>();
            DataSet ds = webService.AnswerRecheckLastMst_R(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString(),btnShopCode.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopDto shop = new ShopDto();
                    shop.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    shop.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    shopList.Add(shop);
                }
            }

            grcShop.DataSource = shopList;
            
        }
        private void btnShopCode_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            Shop_Popup pop = new Shop_Popup("", "", false);
            pop.ShowDialog();
            ShopDto dto = pop.Shopdto;
            if (dto != null)
            {
                btnShopCode.Text = dto.ShopCode;
                txtShopName.Text = dto.ShopName;
            }
            else
            {
                btnShopCode.Text = "";
                txtShopName.Text = "";
            }
        }
      
    }


}
