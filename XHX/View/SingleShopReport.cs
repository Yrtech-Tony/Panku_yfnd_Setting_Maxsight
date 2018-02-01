using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.DTO.SingleShopReport;
using XHX.DTO;
using XHX.Common;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;

namespace XHX.View
{
    public partial class SingleShopReport : BaseForm
    {
        public static localhost.Service service = new localhost.Service();
        LocalService localService = new LocalService();
        MSExcelUtil msExcelUtil = new MSExcelUtil();
        List<ShopDto> shopList = new List<ShopDto>();
        List<ShopDto> shopLeft = new List<ShopDto>();
        public List<ShopDto> ShopList
        {
            get { return shopList; }
            set { shopList = value; }
        }
        GridCheckMarksSelection selection;
        internal GridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }
        public SingleShopReport()
        {
            InitializeComponent();
            XHX.Common.BindComBox.BindProject(cboProjects);
            tbnFilePath.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            btnModule.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
           // SearchAllShopByProjectCode(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString());
            //selection = new GridCheckMarksSelection(gridView1);
            //selection.CheckMarkColumn.VisibleIndex = 0;
        }

        public override List<BaseForm.ButtonType> CreateButton()
        {
            List<XHX.BaseForm.ButtonType> list = new List<XHX.BaseForm.ButtonType>();
            return list;
        }

        private List<ShopDto> SearchAllShopByProjectCode(string projectCode)
        {

            List<ShopDto> shopInfoDtoList = new List<ShopDto>();
            List<string> str_shopInfoList = localService.SearchShopByProjectCode(projectCode);
            foreach (string shopInfo in str_shopInfoList)
            {
                ShopDto shop = new ShopDto();
                string[] str = shopInfo.Split('$');
                shop.ShopCode = str[0];
                shop.ShopName = str[1];
                shopInfoDtoList.Add(shop);
            }
            grcShop.DataSource = shopInfoDtoList;
            
            return shopInfoDtoList;
        }

        private ShopReportDto GetShopReportDto(string projectCode, string shopCode)
        {
            // 经销商的基本信息
            List<ShopInfoDto> shopInfoDtoList = new List<ShopInfoDto>();
            List<String> str_shopInfoList = localService.Report_SearchShopInfoByShopCode(projectCode, shopCode);
            foreach (string shopInfo in str_shopInfoList)
            {
                ShopInfoDto shop = new ShopInfoDto();
                string[] str = shopInfo.Split('$');
                shop.ShopCode = str[0];
                shop.ShopName = str[1];
                //shop.AreaName = str[2];
                //shop.Invoiceregion = str[3].Substring(0, 10) + "至" + str[4].Substring(0, 10);
                //shop.AfterInvoiceregion = str[5].Substring(0, 10) + "至" + str[6].Substring(0, 10);
                shop.StartDate = Convert.ToDateTime(str[2]).ToShortDateString();
                shopInfoDtoList.Add(shop);
            }

            // 经销商发票情况(销售)
            List<AnswerInfoDto> answerDtoInfoAList = new List<AnswerInfoDto>();
            List<String> str_answerInfoAList = localService.Report_SearchShopInvoice(projectCode, shopCode, "A");
            foreach (string answerInfo in str_answerInfoAList)
            {
                AnswerInfoDto shop = new AnswerInfoDto();
                string[] str = answerInfo.Split('$');
                shop.InvoiceCount = Convert.ToInt32(str[0]);
                shop.FailInvoicCount = Convert.ToInt32(str[1]);
                if (Convert.ToInt32(str[0]) == 0)
                {
                    shop.FailInvoicePercent = 0;
                }
                else
                {
                    shop.FailInvoicePercent = Convert.ToDecimal(str[1]) / Convert.ToDecimal(str[0]);
                }
                answerDtoInfoAList.Add(shop);
            }

            // 经销商发票情况(售后)
            List<AnswerInfoDto> answerDtoInfoBList = new List<AnswerInfoDto>();
            List<String> str_answerInfoBList = localService.Report_SearchShopInvoice(projectCode, shopCode, "B");
            foreach (string answerInfo in str_answerInfoBList)
            {
                AnswerInfoDto shop = new AnswerInfoDto();
                string[] str = answerInfo.Split('$');
                shop.InvoiceCount = Convert.ToInt32(str[0]);
                shop.FailInvoicCount = Convert.ToInt32(str[1]);
                if (Convert.ToInt32(str[0]) == 0)
                {
                    shop.FailInvoicePercent = 0;
                }
                else
                {
                    shop.FailInvoicePercent = Convert.ToInt32(str[1]) / Convert.ToInt32(str[0]);
                }
                answerDtoInfoBList.Add(shop);
            }

            //每个失分说明发票数量
            List<PerTypeFailCountDto> perTypeFailCountDtoList = new List<PerTypeFailCountDto>();
            List<String> str_perTypeFailCountDto = localService.Report_SearchPerTypeFailCount(projectCode, shopCode);
            foreach (string perTypeFail in str_perTypeFailCountDto)
            {
                PerTypeFailCountDto shop = new PerTypeFailCountDto();
                string[] str = perTypeFail.Split('$');
                shop.A1Count = Convert.ToInt32(str[0]);
                shop.A2Count = Convert.ToInt32(str[1]);
                shop.A3Count = Convert.ToInt32(str[2]);
                shop.A4Count = Convert.ToInt32(str[3]);
                shop.B1Count = Convert.ToInt32(str[4]);
                shop.B2Count = Convert.ToInt32(str[5]);
                shop.B3Count = Convert.ToInt32(str[6]);
                perTypeFailCountDtoList.Add(shop);
            }

            //审计详细 销售
            List<SubjectsScoreADto> subjectsScoreDtoAList = new List<SubjectsScoreADto>();
            List<String> str_subjectsScoreADto = localService.Report_PerInvoiceInfo(projectCode, shopCode, "A");
            foreach (string subjectScore in str_subjectsScoreADto)
            {
                SubjectsScoreADto shop = new SubjectsScoreADto();
                string[] str = subjectScore.Split('$');
                shop.SubjectCode = str[0];
                shop.SellInvoiceCode = str[1];
                shop.A1 = str[2];
                shop.A2 = str[3];
                shop.A3 = str[4];
                shop.A4 = str[5];
                shop.Remark = str[6];
                shop.SpCode = str[7];
                shop.LoseDesc = str[8];
                shop.PicName = str[9];
                subjectsScoreDtoAList.Add(shop);
            }
            //审计详细 售后
            List<SubjectsScoreBDto> subjectsScoreDtoBList = new List<SubjectsScoreBDto>();
            List<String> str_subjectsScoreBDto = localService.Report_PerInvoiceInfo(projectCode, shopCode, "B");
            foreach (string subjectScore in str_subjectsScoreBDto)
            {
                SubjectsScoreBDto shop = new SubjectsScoreBDto();
                string[] str = subjectScore.Split('$');
                shop.SubjectCode = str[0];
                shop.AfterInvoiceCode = str[1];
                shop.B1 = str[2];
                shop.B2 = str[3];
                shop.B3 = str[4];
                shop.Remark = str[5];
                shop.SpCode = str[6];
                shop.LoseDesc = str[7];
                subjectsScoreDtoBList.Add(shop);
            }
            ShopReportDto shopReportDto = new ShopReportDto();
            shopReportDto.ShopInfoDtoList = shopInfoDtoList;
            //shopReportDto.AnswerInfoADtoList = answerDtoInfoAList;
            //shopReportDto.AnswerInfoBDtoList = answerDtoInfoBList;
            shopReportDto.PerTypeFailCountDtoList = perTypeFailCountDtoList;
            shopReportDto.SubjectsScoreADtoList = subjectsScoreDtoAList;
            shopReportDto.SubjectsScoreBDtoList = subjectsScoreDtoBList;
            return shopReportDto;
        }

        private void WriteDataToExcel(ShopReportDto shopReportDto)
        {
            //  Workbook workbook = msExcelUtil.OpenExcelByMSExcel(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\Template\SingleShopReportTemplate_20130812.xlsx");
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);

            #region 审计概述
            //{
            //    Worksheet worksheet_FengMian = workbook.Worksheets["审计概述"] as Worksheet;


            //    msExcelUtil.SetCellValue(worksheet_FengMian, "B4", shopReportDto.ShopInfoDtoList[0].ShopCode);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "D4", shopReportDto.ShopInfoDtoList[0].ShopName);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "B5", shopReportDto.ShopInfoDtoList[0].AreaName);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "D5", shopReportDto.ShopInfoDtoList[0].StartDate);

            //    msExcelUtil.SetCellValue(worksheet_FengMian, "B11", shopReportDto.ShopInfoDtoList[0].Invoiceregion);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "B12", shopReportDto.ShopInfoDtoList[0].AfterInvoiceregion);

            //    msExcelUtil.SetCellValue(worksheet_FengMian, "C11", shopReportDto.AnswerInfoADtoList[0].InvoiceCount);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "D11", shopReportDto.AnswerInfoADtoList[0].FailInvoicCount);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "E11", shopReportDto.AnswerInfoADtoList[0].FailInvoicePercent);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "C12", shopReportDto.AnswerInfoBDtoList[0].InvoiceCount);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "D12", shopReportDto.AnswerInfoBDtoList[0].FailInvoicCount);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "E12", shopReportDto.AnswerInfoBDtoList[0].FailInvoicePercent);

            //    msExcelUtil.SetCellValue(worksheet_FengMian, "F11", shopReportDto.PerTypeFailCountDtoList[0].A1Count);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "G11", shopReportDto.PerTypeFailCountDtoList[0].A2Count);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "H11", shopReportDto.PerTypeFailCountDtoList[0].A3Count);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "I11", shopReportDto.PerTypeFailCountDtoList[0].A4Count);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "J12", shopReportDto.PerTypeFailCountDtoList[0].B1Count);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "K12", shopReportDto.PerTypeFailCountDtoList[0].B2Count);
            //    msExcelUtil.SetCellValue(worksheet_FengMian, "L11", shopReportDto.PerTypeFailCountDtoList[0].B3Count);

            //}
            #endregion

            #region 审计详情
            // 审计详情-销售
            List<SubjectsScoreADto> subjectsScoreDtoAListDetail = shopReportDto.SubjectsScoreADtoList;
            int count =150- subjectsScoreDtoAListDetail.Count;
            for (int i = 0; i < count; i++)
            {
                subjectsScoreDtoAListDetail.Add(new SubjectsScoreADto());
            }
            {
                Worksheet worksheet_ShopScoreADetail = workbook.Worksheets["审计详情-销售"] as Worksheet;
                msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "B", 2, shopReportDto.ShopInfoDtoList[0].ShopCode);
                msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "D", 2, shopReportDto.ShopInfoDtoList[0].ShopName);
                msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "G", 2, shopReportDto.ShopInfoDtoList[0].StartDate);
                int rowIndex1 = 4;
                foreach (SubjectsScoreADto subjectsScoreDto in subjectsScoreDtoAListDetail)
                {
                    if (string.IsNullOrEmpty(subjectsScoreDto.SubjectCode))
                    {
                        msExcelUtil.DeleteRow(worksheet_ShopScoreADetail, rowIndex1);
                        continue;
                    }
                    else
                    {
                        msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "A", rowIndex1, subjectsScoreDto.SubjectCode);
                        //msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "B", rowIndex1, subjectsScoreDto.SellInvoiceCode);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "B", rowIndex1, subjectsScoreDto.SpCode);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "C", rowIndex1, subjectsScoreDto.A1);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "D", rowIndex1, subjectsScoreDto.A2);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "E", rowIndex1, subjectsScoreDto.A3);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "F", rowIndex1, subjectsScoreDto.A4);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "G", rowIndex1, subjectsScoreDto.Remark);
                        
                    }
                    rowIndex1++;
                }
            }
            // 审计详情-售后
            List<SubjectsScoreBDto> subjectsScoreDtoBListDetail = shopReportDto.SubjectsScoreBDtoList;
            int count1 = 300 - subjectsScoreDtoBListDetail.Count;
            for (int i = 0; i < count1; i++)
            {
                subjectsScoreDtoBListDetail.Add(new SubjectsScoreBDto());
            }
            {
                Worksheet worksheet_ShopScoreBDetail = workbook.Worksheets["审计详情-售后"] as Worksheet;
                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "B", 2, shopReportDto.ShopInfoDtoList[0].ShopCode);
                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "D", 2, shopReportDto.ShopInfoDtoList[0].ShopName);
                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "F", 2, shopReportDto.ShopInfoDtoList[0].StartDate);
                int rowIndex1 = 4;
                foreach (SubjectsScoreBDto subjectsScoreDto in subjectsScoreDtoBListDetail)
                {
                    if (string.IsNullOrEmpty(subjectsScoreDto.SubjectCode))
                    {
                        msExcelUtil.DeleteRow(worksheet_ShopScoreBDetail, rowIndex1);
                        continue;
                    }
                    else
                    {
                        msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "A", rowIndex1, subjectsScoreDto.SubjectCode);
                        //msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "B", rowIndex1, subjectsScoreDto.AfterInvoiceCode);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "B", rowIndex1, subjectsScoreDto.SpCode);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "C", rowIndex1, subjectsScoreDto.B1);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "D", rowIndex1, subjectsScoreDto.B2);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "E", rowIndex1, subjectsScoreDto.B3);
                        msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "F", rowIndex1, subjectsScoreDto.Remark);
                    }
                    rowIndex1++;
                }
            }
            #endregion
            #region 失分照片
            {
                //Worksheet worksheet_ShopScoreDetail2 = workbook.Worksheets["照片"] as Worksheet;
                //List<SubjectsScoreADto> subjectsScoreDtoAList = shopReportDto.SubjectsScoreADtoList;
                //List<SubjectsScoreBDto> subjectsScoreDtoBList = shopReportDto.SubjectsScoreBDtoList;
                //int rowIndex = 4;
                //foreach (SubjectsScoreADto subjectsScoreDto in subjectsScoreDtoAList)
                //{
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "A", rowIndex, subjectsScoreDto.SubjectCode);
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "B", rowIndex, subjectsScoreDto.SpCode);
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "C", rowIndex, subjectsScoreDto.LoseDesc);
                //    string[] picNameArray = subjectsScoreDto.PicName.Split(';');
                //    int picIndex = 0;
                //    foreach (string picName in picNameArray)
                //    {
                //        if (picIndex != 0 && picIndex % 2 == 0)
                //        {
                //            msExcelUtil.AddRow(worksheet_ShopScoreDetail2, ++rowIndex);
                //        }
                //        if (string.IsNullOrEmpty(picName)) continue;
                //        byte[] bytes = localService.SearchAnswerDtl2Pic(picName.Replace(".jpg", ""), shopReportDto.ShopInfoDtoList[0].ProjectCode + shopReportDto.ShopInfoDtoList[0].ShopName, subjectsScoreDto.SubjectCode, buttonEdit1.Text);
                //        if (bytes == null || bytes.Length == 0) continue;
                //        Image.FromStream(new MemoryStream(bytes)).Save(Path.Combine(Path.GetTempPath(), picName + ".jpg"));
                //        int colIndex = 2 + picIndex % 2;

                //        msExcelUtil.InsertPicture(worksheet_ShopScoreDetail2, worksheet_ShopScoreDetail2.Cells[rowIndex, colIndex] as Microsoft.Office.Interop.Excel.Range, Path.Combine(Path.GetTempPath(), picName + ".jpg"), rowIndex);
                //        picIndex++;
                //    }

                //    rowIndex++;
                //}
                //foreach (SubjectsScoreBDto subjectsScoreDto in subjectsScoreDtoBList)
                //{
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "A", rowIndex, subjectsScoreDto.SubjectCode);
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "B", rowIndex, subjectsScoreDto.SpCode);
                //    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "C", rowIndex, subjectsScoreDto.LoseDesc);
                //    string[] picNameArray = subjectsScoreDto.PicName.Split(';');
                //    int picIndex = 0;
                //    foreach (string picName in picNameArray)
                //    {
                //        if (picIndex != 0 && picIndex % 2 == 0)
                //        {
                //            msExcelUtil.AddRow(worksheet_ShopScoreDetail2, ++rowIndex);
                //        }
                //        if (string.IsNullOrEmpty(picName)) continue;
                //        byte[] bytes = localService.SearchAnswerDtl2Pic(picName.Replace(".jpg", ""), shopReportDto.ShopInfoDtoList[0].ProjectCode + shopReportDto.ShopInfoDtoList[0].ShopName, subjectsScoreDto.SubjectCode, buttonEdit1.Text);
                //        if (bytes == null || bytes.Length == 0) continue;
                //        Image.FromStream(new MemoryStream(bytes)).Save(Path.Combine(Path.GetTempPath(), picName + ".jpg"));
                //        int colIndex = 2 + picIndex % 2;

                //        msExcelUtil.InsertPicture(worksheet_ShopScoreDetail2, worksheet_ShopScoreDetail2.Cells[rowIndex, colIndex] as Microsoft.Office.Interop.Excel.Range, Path.Combine(Path.GetTempPath(), picName + ".jpg"), rowIndex);
                //        picIndex++;
                //    }

                //    rowIndex++;
                //}
            }
            #endregion
            //workbook.Save(Path.Combine(tbnFilePath.Text,shopReportDto.ProjectCode+"_"+shopReportDto.ShopName+".xls"));
            workbook.Close(true, Path.Combine(tbnFilePath.Text, shopReportDto.ShopInfoDtoList[0].AreaName + "_" + shopReportDto.ShopInfoDtoList[0].ShopCode + "_" + shopReportDto.ShopInfoDtoList[0].ShopName + ".xlsx"), Type.Missing);
        }

        private void GenerateReport()
        {
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            _shopDtoList = new List<ShopDto>();
            //_shopDtoList = SearchAllShopByProjectCode(projectCode);
            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (gridView1.GetRowCellValue(i, "CheckMarkSelection") != null && gridView1.GetRowCellValue(i, "CheckMarkSelection").ToString() == "True")
                {
                    _shopDtoList.Add(gridView1.GetRow(i) as ShopDto);
                }
            }
            _shopDtoListCount = _shopDtoList.Count;
            this.Enabled = false;
            _bw = new BackgroundWorker();
            _bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            _bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            _bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            _bw.WorkerReportsProgress = true;
            _bw.RunWorkerAsync(new object[] { projectCode });
        }

        BackgroundWorker _bw;
        List<ShopDto> _shopDtoList;
        int _shopDtoListCount = 0;
        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbrProgress.Value = (e.ProgressPercentage) * 100 / _shopDtoListCount;
            System.Windows.Forms.Application.DoEvents();
        }
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            string[] shopNames;
            int currentShopDtoIndex = 0;
            foreach (ShopDto shopDto in _shopDtoList)
            {
                try
                {
                    object[] arguments = e.Argument as object[];
                    ShopReportDto shopReportDto = GetShopReportDto(arguments[0] as string, shopDto.ShopCode);
                    WriteDataToExcel(shopReportDto);
                    _bw.ReportProgress(currentShopDtoIndex++);
                }
                catch (Exception ex)
                {
                    shopLeft.Add(shopDto);
                    // MessageBox.Show(shopDto.ShopCode);
                    WriteErrorLog(shopDto.ShopCode + shopDto.ShopName + ex.Message.ToString());
                    continue;
                }

            }
        }
        void WriteErrorLog(string errMessage)
        {
            string path = tbnFilePath.Text + "\\" + "Error.txt";

            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (FileStream fs = File.Create(path))
            {
                AddText(fs, errMessage + "\r\n");
            }

        }
        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            this.Enabled = true;
            List<ShopDto> gridSource = grcShop.DataSource as List<ShopDto>;

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                gridView1.SetRowCellValue(i, "CheckMarkSelection", false);
                foreach (ShopDto shop in shopLeft)
                {
                    if (shop.ShopCode == gridSource[i].ShopCode)
                    {
                        gridView1.SetRowCellValue(i, "CheckMarkSelection", true);
                    }
                    //else
                    //{
                    //    gridView1.SetRowCellValue(i, "CheckMarkSelection", false);
                    //}
                }
            }
            //if (shopLeft.Count > 0)
            //{
            //    string str = string.Empty;
            //    foreach (ShopDto shop in shopLeft)
            //    {
            //        str += shop.ShopCode + ":" + shop.ShopName + ";";
            //    }
            //    CommonHandler.ShowMessage(MessageType.Information, "报告生成完毕未生成报告经销商如下:" + str);
            //}
            //else
            //{
            CommonHandler.ShowMessage(MessageType.Information, "报告生成完毕");
            //}

        }

        private void tbnFilePath_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                tbnFilePath.Text = fbd.SelectedPath;
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbnFilePath.Text))
            {
                CommonHandler.ShowMessage(MessageType.Information, "请选择输出报告路径");
                return;
            }
            if (string.IsNullOrEmpty(btnModule.Text))
            {
                CommonHandler.ShowMessage(MessageType.Information, "请选择模板路径");
                return;
            }
            if (string.IsNullOrEmpty(buttonEdit1.Text))
            {
                CommonHandler.ShowMessage(MessageType.Information, "请选择数据源路径");
                return;
            }

            DirectoryInfo dataDir = new DirectoryInfo(buttonEdit1.Text);
            FileInfo[] filesInfo = dataDir.GetFiles();

            bool isExistDBFile = false;
            foreach (FileInfo fileInfo in filesInfo)
            {
                if (fileInfo.Name == "writeable.db")
                {
                    isExistDBFile = true;
                    SqliteHelper.SetConnectionString("Data Source=" + fileInfo.FullName, "");
                }
            }
            if (!isExistDBFile)
            {
                CommonHandler.ShowMessage(MessageType.Information, "数据库不存在");
                return;
            }
            GenerateReport();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DirectoryInfo dataDir = new DirectoryInfo(buttonEdit1.Text);
            FileInfo[] filesInfo = dataDir.GetFiles();
            bool isExistDBFile = false;
            foreach (FileInfo fileInfo in filesInfo)
            {
                if (fileInfo.Name == "writeable.db")
                {
                    isExistDBFile = true;
                    SqliteHelper.SetConnectionString("Data Source=" + fileInfo.FullName, "");
                }
            }
            if (!isExistDBFile)
            {
                CommonHandler.ShowMessage(MessageType.Information, "数据库不存在");
                return;
            }
            SearchAllShopByProjectCode(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString());
            selection = new GridCheckMarksSelection(gridView1);
            selection.CheckMarkColumn.VisibleIndex = 0;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            ShopNotInScore shop = new ShopNotInScore(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString());
            shop.ShowDialog();

        }

        private void btnModule_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.Filter = "Excel(*.xlsx)|";
            ofp.FilterIndex = 2;
            if (ofp.ShowDialog() == DialogResult.OK)
            {
                btnModule.Text = ofp.FileName;
            }
        }

        private void buttonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                buttonEdit1.Text = fbd.SelectedPath;
            }
        }
    }
}
