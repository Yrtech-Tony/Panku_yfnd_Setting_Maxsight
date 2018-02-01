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
    public partial class RecheckErrorPopup : Form
    {
        localhost.Service service = new localhost.Service();
        public List<RecheckErrorDto> linkList = new List<RecheckErrorDto>();
        public string _chapterCode = "";
        public List<RecheckErrorDto> LinkList
        {
            get { return linkList; }
            set { linkList = value; }
        }
        XtraGridDataHandler<RecheckErrorDto> dataHandler = null;
        GridCheckMarksSelection selection;
        internal GridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }
        public RecheckErrorPopup(string recheckErrorList)
        {
            InitializeComponent();
            service.Url = "http://192.168.13.240/XHX.BMWServer/service.asmx";
            OnLoadView(recheckErrorList);
        }

        private void OnLoadView(string recheckErrorList)
        {
            dataHandler = new XtraGridDataHandler<RecheckErrorDto>(grvLink);
            CommonHandler.SetRowNumberIndicator(grvLink);
            grcLink.DataSource = new List<RecheckErrorDto>();
            selection = new GridCheckMarksSelection(grvLink);
            selection.CheckMarkColumn.VisibleIndex = 0;
            SearchLink(recheckErrorList);
        }

        private void SearchLink(string recheckErrorList)
        {
            List<RecheckErrorDto> sourcelinkList = new List<RecheckErrorDto>();
            DataSet ds = service.SearchRecheckError("", "");
            if (ds.Tables.Count > 0)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    RecheckErrorDto link = new RecheckErrorDto();
                    link.ErrorTypeCode = Convert.ToString(ds.Tables[0].Rows[j]["RecheckErrorCode"]);
                    link.ErrorTypeName = Convert.ToString(ds.Tables[0].Rows[j]["RecheckErrorName"]);
                    sourcelinkList.Add(link);
                }
            }
            grcLink.DataSource = sourcelinkList;

            for (int i = 0; i < grvLink.RowCount; i++)
            {
                if (recheckErrorList.Split('$').Contains(grvLink.GetRowCellValue(i, gcRecheckErrorCode)))
                {
                    grvLink.SetRowCellValue(i, "CheckMarkSelection", "True");
                }
            }
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grvLink.RowCount; i++)
            {
                if (grvLink.GetRowCellValue(i, "CheckMarkSelection").ToString() == "True")
                {
                    linkList.Add(grvLink.GetRow(i) as RecheckErrorDto);
                }
            }
            this.Close();
        }

    }
}
