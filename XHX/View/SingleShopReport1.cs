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
    public partial class SingleShopReport1 : BaseForm
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
        public SingleShopReport1()
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
            DataSet ds = service.SearchShopByProjectCode(projectCode);
            List<ShopDto> shopDtoList = new List<ShopDto>();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopDto shopDto = new ShopDto();
                    shopDto.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    shopDto.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    shopDtoList.Add(shopDto);
                }
            }
            grcShop.DataSource = shopDtoList;
            return shopDtoList;
        }

        private ShopReportDto GetShopReportDto(string projectCode, string shopCode)
        {
            ShopReportDto shopReportDto = new ShopReportDto();
            if (checkBox1.Checked)
            {
                DataSet[] dataSetList = service.GetShopReportDtoDMS(projectCode, shopCode, "A");
                List<ShopInfoDto> shopInfoDtoList = new List<ShopInfoDto>();
                List<AnswerInfoDto> answerInfoDtoList = new List<AnswerInfoDto>();
                List<PerTypeFailCountDto> perTypeFailCountDtoList = new List<PerTypeFailCountDto>();
                List<SubjectsScoreADto> subjectsScoreDtoList = new List<SubjectsScoreADto>();
                shopReportDto.ShopInfoDtoList = shopInfoDtoList;
                shopReportDto.AnswerInfoDtoList = answerInfoDtoList;
                shopReportDto.PerTypeFailCountDtoList = perTypeFailCountDtoList;
                shopReportDto.SubjectsScoreADtoList = subjectsScoreDtoList;

                #region 经销商信息
                DataSet ds = dataSetList[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ShopInfoDto shopinfo = new ShopInfoDto();
                        shopinfo.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                        shopinfo.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                        shopinfo.AreaName = Convert.ToString(ds.Tables[0].Rows[i]["AreaName"]);
                        shopinfo.StartDate = Convert.ToString(ds.Tables[0].Rows[i]["StartDate"]);
                        shopinfo.sellStartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InvoiceStartDate"]);
                        shopinfo.sellEndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["InvoiceEndDate"]);
                        shopInfoDtoList.Add(shopinfo);
                    }
                }
                #endregion
                #region Answer 信息
                ds = dataSetList[1];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AnswerInfoDto answerInfo = new AnswerInfoDto();
                        answerInfo.InvoiceCount = Convert.ToInt32(ds.Tables[0].Rows[i]["SumCount"]);//样本配额
                        answerInfo.FailInvoicCount = Convert.ToInt32(ds.Tables[0].Rows[i]["FailCount"]);//合格数量
                        answerInfo.LocalCount = Convert.ToString(ds.Tables[0].Rows[i]["LocalCount"]);//样本差额
                        if (Convert.ToInt32(ds.Tables[0].Rows[i]["SumCount"]) == 0)
                        {
                            answerInfo.FailInvoicePercent = 0;
                        }
                        else
                        {
                            answerInfo.FailInvoicePercent = Convert.ToDecimal(ds.Tables[0].Rows[i]["FailCount"]) /
                                                 Convert.ToDecimal(ds.Tables[0].Rows[i]["SumCount"]);
                        }
                        answerInfoDtoList.Add(answerInfo);
                    }
                }
                #endregion
                #region Fail 信息
                ds = dataSetList[2];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PerTypeFailCountDto failInfo = new PerTypeFailCountDto();
                        failInfo.A1Count = Convert.ToInt32(ds.Tables[0].Rows[i]["A1Count"]);
                        failInfo.A2Count = Convert.ToInt32(ds.Tables[0].Rows[i]["A2Count"]);
                        failInfo.A3Count = Convert.ToInt32(ds.Tables[0].Rows[i]["A3Count"]);
                        failInfo.A4Count = Convert.ToInt32(ds.Tables[0].Rows[i]["A4Count"]);
                        failInfo.A5Count = Convert.ToInt32(ds.Tables[0].Rows[i]["A5Count"]);
                        perTypeFailCountDtoList.Add(failInfo);
                    }
                }
                #endregion
                #region Detail
                ds = dataSetList[3];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SubjectsScoreADto subject = new SubjectsScoreADto();
                        subject.SubjectCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectCode"]);
                        subject.SpCode = Convert.ToString(ds.Tables[0].Rows[i]["SpCode"]);
                        subject.A1 = Convert.ToString(ds.Tables[0].Rows[i]["A1"]);
                        subject.A2 = Convert.ToString(ds.Tables[0].Rows[i]["A2"]);
                        subject.A3 = Convert.ToString(ds.Tables[0].Rows[i]["A3"]);
                        subject.A4 = Convert.ToString(ds.Tables[0].Rows[i]["A4"]);
                        subject.A5 = Convert.ToString(ds.Tables[0].Rows[i]["A5"]);
                        subject.SellCustomerName = Convert.ToString(ds.Tables[0].Rows[i]["SellCustomerName"]);
                        subject.SellVINCode = Convert.ToString(ds.Tables[0].Rows[i]["SellVINCode"]);
                        subject.SellInvoiceDate = Convert.ToString(ds.Tables[0].Rows[i]["SellInvoiceDate"]);
                        subject.SellInvoiceDMSDate = Convert.ToString(ds.Tables[0].Rows[i]["SellInvoiceDMSDate"]);
                        subject.Remark = Convert.ToString(ds.Tables[0].Rows[i]["Remark"]);
                        subject.LoseDesc = Convert.ToString(ds.Tables[0].Rows[i]["LossDesc"]);
                        subject.Score = Convert.ToString(ds.Tables[0].Rows[i]["Score"]);
                        subjectsScoreDtoList.Add(subject);
                    }
                }
                #endregion
            }
            else
            {
                DataSet[] dataSetList = service.GetShopReportDtoDMS(projectCode, shopCode, "B");
                List<ShopInfoDto> shopInfoDtoList = new List<ShopInfoDto>();
                List<AnswerInfoDto> answerInfoDtoList = new List<AnswerInfoDto>();
                List<PerTypeFailCountDto> perTypeFailCountDtoList = new List<PerTypeFailCountDto>();
                List<SubjectsScoreBDto> subjectsScoreDtoList = new List<SubjectsScoreBDto>();
                shopReportDto.ShopInfoDtoList = shopInfoDtoList;
                shopReportDto.AnswerInfoDtoList = answerInfoDtoList;
                shopReportDto.PerTypeFailCountDtoList = perTypeFailCountDtoList;
                shopReportDto.SubjectsScoreBDtoList = subjectsScoreDtoList;

                #region 经销商信息
                DataSet ds = dataSetList[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ShopInfoDto shopinfo = new ShopInfoDto();
                        shopinfo.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                        shopinfo.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                        shopinfo.AreaName = Convert.ToString(ds.Tables[0].Rows[i]["AreaName"]);
                        shopinfo.StartDate = Convert.ToString(ds.Tables[0].Rows[i]["StartDate"]);
                        shopinfo.AfterStartDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["AfterInvoiceStartDate"]);
                        shopinfo.AfterEndDate = Convert.ToDateTime(ds.Tables[0].Rows[i]["AfterInvoiceEndDate"]);
                        shopInfoDtoList.Add(shopinfo);
                    }
                }
                #endregion
                #region Answer 信息
                ds = dataSetList[1];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        AnswerInfoDto answerInfo = new AnswerInfoDto();
                        answerInfo.InvoiceCount = Convert.ToInt32(ds.Tables[0].Rows[i]["SumCount"]);
                        answerInfo.FailInvoicCount = Convert.ToInt32(ds.Tables[0].Rows[i]["FailCount"]);
                        answerInfo.LocalCount = Convert.ToString(ds.Tables[0].Rows[i]["LocalCount"]);
                        if (Convert.ToInt32(ds.Tables[0].Rows[i]["SumCount"]) == 0)
                        {
                            answerInfo.FailInvoicePercent = 0;
                        }
                        else
                        {
                            answerInfo.FailInvoicePercent = Convert.ToDecimal(ds.Tables[0].Rows[i]["FailCount"]) /
                                                 Convert.ToDecimal(ds.Tables[0].Rows[i]["SumCount"]);
                        }
                        answerInfoDtoList.Add(answerInfo);
                    }
                }
                #endregion
                #region Fail 信息
                ds = dataSetList[2];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        PerTypeFailCountDto failInfo = new PerTypeFailCountDto();
                        failInfo.B1Count = Convert.ToInt32(ds.Tables[0].Rows[i]["B1Count"]);
                        failInfo.B2Count = Convert.ToInt32(ds.Tables[0].Rows[i]["B2Count"]);
                        failInfo.B3Count = Convert.ToInt32(ds.Tables[0].Rows[i]["B3Count"]);
                        perTypeFailCountDtoList.Add(failInfo);
                    }
                }
                #endregion
                #region Detail
                ds = dataSetList[3];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        SubjectsScoreBDto subject = new SubjectsScoreBDto();
                        subject.SubjectCode = Convert.ToString(ds.Tables[0].Rows[i]["SubjectCode"]);
                        subject.SpCode = Convert.ToString(ds.Tables[0].Rows[i]["SpCode"]);
                        subject.B1 = Convert.ToString(ds.Tables[0].Rows[i]["B1"]);
                        subject.B2 = Convert.ToString(ds.Tables[0].Rows[i]["B2"]);
                        subject.B3 = Convert.ToString(ds.Tables[0].Rows[i]["B3"]);
                        subject.Remark = Convert.ToString(ds.Tables[0].Rows[i]["Remark"]);
                        subject.LoseDesc = Convert.ToString(ds.Tables[0].Rows[i]["LossDesc"]);
                        subject.AfterInvoiceDate = Convert.ToString(ds.Tables[0].Rows[i]["AfterInvoiceDate"]);
                        subject.AfterInvoiceDMSDate = Convert.ToString(ds.Tables[0].Rows[i]["AfterInvoiceDMSDate"]);
                        subject.AfterInvoiceMony = Convert.ToString(ds.Tables[0].Rows[i]["AfterInvoiceMony"]);
                        subject.AfterDMSMony = Convert.ToString(ds.Tables[0].Rows[i]["AfterDMSMony"]);
                        subject.Score = Convert.ToString(ds.Tables[0].Rows[i]["Score"]);
                        subjectsScoreDtoList.Add(subject);
                    }
                }
                #endregion
            }
            return shopReportDto;
        }

        private void WriteDataToExcel(ShopReportDto shopReportDto)
        {
            //  Workbook workbook = msExcelUtil.OpenExcelByMSExcel(AppDomain.CurrentDomain.BaseDirectory + @"\Resources\Template\SingleShopReportTemplate_20130812.xlsx");
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            if (checkBox1.Checked)
            {
                #region 审计概述
                {
                    Worksheet worksheet_FengMian = workbook.Worksheets["审计概述"] as Worksheet;

                    msExcelUtil.SetCellValue(worksheet_FengMian, "B4", shopReportDto.ShopInfoDtoList[0].ShopCode);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "E4", shopReportDto.ShopInfoDtoList[0].ShopName);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "B5", shopReportDto.ShopInfoDtoList[0].AreaName);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "E5", shopReportDto.ShopInfoDtoList[0].StartDate);

                    msExcelUtil.SetCellValue(worksheet_FengMian, "B11", shopReportDto.ShopInfoDtoList[0].sellStartDate.ToShortDateString().Replace("-", "/") + "-" + shopReportDto.ShopInfoDtoList[0].sellEndDate.ToShortDateString().Replace("-", "/"));
                    msExcelUtil.SetCellValue(worksheet_FengMian, "C11", shopReportDto.AnswerInfoDtoList[0].InvoiceCount);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "D11", shopReportDto.AnswerInfoDtoList[0].FailInvoicCount);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "E11", shopReportDto.AnswerInfoDtoList[0].FailInvoicePercent);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "F11", shopReportDto.AnswerInfoDtoList[0].LocalCount);


                    msExcelUtil.SetCellValue(worksheet_FengMian, "G11", shopReportDto.PerTypeFailCountDtoList[0].A1Count);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "H11", shopReportDto.PerTypeFailCountDtoList[0].A2Count);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "I11", shopReportDto.PerTypeFailCountDtoList[0].A3Count);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "J11", shopReportDto.PerTypeFailCountDtoList[0].A4Count);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "K11", shopReportDto.PerTypeFailCountDtoList[0].A5Count);
                }
                #endregion

                #region 审计详情
                // 审计详情-销售
                List<SubjectsScoreADto> subjectsScoreDtoAListDetail = shopReportDto.SubjectsScoreADtoList;
                Worksheet worksheet_ShopScoreADetail = workbook.Worksheets["审计详情"] as Worksheet;
                int count = 180 - subjectsScoreDtoAListDetail.Count;
                for (int i = 0; i < count; i++)
                {
                    subjectsScoreDtoAListDetail.Add(new SubjectsScoreADto());
                }
                {

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
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "B", rowIndex1, subjectsScoreDto.SpCode);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "C", rowIndex1, subjectsScoreDto.SellCustomerName);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "D", rowIndex1, subjectsScoreDto.SellVINCode);

                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "E", rowIndex1, subjectsScoreDto.SellInvoiceDate);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "F", rowIndex1, subjectsScoreDto.SellInvoiceDMSDate);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "G", rowIndex1, subjectsScoreDto.A2);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "H", rowIndex1, subjectsScoreDto.A3);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "I", rowIndex1, subjectsScoreDto.A1);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "J", rowIndex1, subjectsScoreDto.A4);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "K", rowIndex1, subjectsScoreDto.A5);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "L", rowIndex1, subjectsScoreDto.Score);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreADetail, "M", rowIndex1, subjectsScoreDto.Remark);
                            rowIndex1++;
                        }
                       
                    }

                    //for (int i = 5; i < 150; i++)
                    //{
                    //    if (string.IsNullOrEmpty(msExcelUtil.GetCellValue(worksheet_ShopScoreADetail, "A", i).ToString()))
                    //    {
                    //        msExcelUtil.DeleteRow(worksheet_ShopScoreADetail, i);
                    //    }
                    //}
                }
                #endregion
                #region 失分照片
                {
                    Worksheet worksheet_ShopScoreDetail2 = workbook.Worksheets["照片"] as Worksheet;
                    List<SubjectsScoreADto> subjectsScoreDtoAList = shopReportDto.SubjectsScoreADtoList;
                    int rowIndex = 4;
                    foreach (SubjectsScoreADto subjectsScoreDto in subjectsScoreDtoAList)
                    {
                        if (!(subjectsScoreDto.A1 == "Y") || !(subjectsScoreDto.A2 == "Y")
                            || !(subjectsScoreDto.A3 == "Y")
                            || !(subjectsScoreDto.A4 == "Y")
                            || !(subjectsScoreDto.A5 == "Y"))
                        {
                            msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "A", rowIndex, subjectsScoreDto.SubjectCode);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "B", rowIndex, subjectsScoreDto.SpCode);
                            msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "C", rowIndex, subjectsScoreDto.LoseDesc);
                            List<string> picNameArray =
                                service.SearchPicNameList(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString(), shopReportDto.ShopInfoDtoList[0].ShopName, subjectsScoreDto.SubjectCode).ToList<string>();
                            int picIndex = 0;
                            foreach (string picName in picNameArray)
                            {
                                if (picIndex != 0 && picIndex % 2 == 0)
                                {
                                    msExcelUtil.AddRow(worksheet_ShopScoreDetail2, ++rowIndex);
                                    msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "X", rowIndex, subjectsScoreDto.SubjectCode);
                                }
                                if (string.IsNullOrEmpty(picName)) continue;
                                byte[] bytes = service.SearchAnswerDtl2Pic(picName.Replace(".jpg", ""), CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString() + shopReportDto.ShopInfoDtoList[0].ShopName, subjectsScoreDto.SubjectCode, "", "");
                                if (bytes == null || bytes.Length == 0) continue;
                                Image.FromStream(new MemoryStream(bytes)).Save(Path.Combine(Path.GetTempPath(), picName + ".jpg"));
                                int colIndex = 4 + picIndex % 2;

                                msExcelUtil.InsertPicture(worksheet_ShopScoreDetail2, worksheet_ShopScoreDetail2.Cells[rowIndex, colIndex] as Microsoft.Office.Interop.Excel.Range, Path.Combine(Path.GetTempPath(), picName + ".jpg"), rowIndex);
                                picIndex++;
                            }

                            rowIndex++;
                        }
                    }
                    int rowIndex1 = 5;
                    for (int i = 5; i < 180; i++)
                    {
                        if (string.IsNullOrEmpty(msExcelUtil.GetCellValue(worksheet_ShopScoreDetail2, "A", i).ToString())
                            && string.IsNullOrEmpty(msExcelUtil.GetCellValue(worksheet_ShopScoreDetail2, "X", i).ToString()))
                        {
                            rowIndex1 = i;
                            break;
                        }
                    }
                    for (int i = 0; i < 180; i++)
                    {
                        msExcelUtil.DeleteRow(worksheet_ShopScoreDetail2, rowIndex1);
                    }

                }
                workbook.Close(true, Path.Combine(tbnFilePath.Text, shopReportDto.ShopInfoDtoList[0].AreaName + "_" + shopReportDto.ShopInfoDtoList[0].ShopCode + "_" + shopReportDto.ShopInfoDtoList[0].ShopName + "_销售" + ".xlsx"), Type.Missing);
                #endregion
            }
            else
            {
                #region 审计概述
                {
                    Worksheet worksheet_FengMian = workbook.Worksheets["审计概述"] as Worksheet;

                    msExcelUtil.SetCellValue(worksheet_FengMian, "B4", shopReportDto.ShopInfoDtoList[0].ShopCode);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "E4", shopReportDto.ShopInfoDtoList[0].ShopName);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "B5", shopReportDto.ShopInfoDtoList[0].AreaName);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "E5", shopReportDto.ShopInfoDtoList[0].StartDate);

                    msExcelUtil.SetCellValue(worksheet_FengMian, "B11", shopReportDto.ShopInfoDtoList[0].AfterStartDate.ToShortDateString().Replace("-", "/") + "-" + shopReportDto.ShopInfoDtoList[0].AfterEndDate.ToShortDateString().Replace("-", "/"));
                    msExcelUtil.SetCellValue(worksheet_FengMian, "C11", shopReportDto.AnswerInfoDtoList[0].InvoiceCount);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "D11", shopReportDto.AnswerInfoDtoList[0].FailInvoicCount);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "E11", shopReportDto.AnswerInfoDtoList[0].FailInvoicePercent);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "F11", shopReportDto.AnswerInfoDtoList[0].LocalCount);


                    msExcelUtil.SetCellValue(worksheet_FengMian, "G11", shopReportDto.PerTypeFailCountDtoList[0].B1Count);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "H11", shopReportDto.PerTypeFailCountDtoList[0].B2Count);
                    msExcelUtil.SetCellValue(worksheet_FengMian, "I11", shopReportDto.PerTypeFailCountDtoList[0].B3Count);
                }
                #endregion

                #region 审计详情

                List<SubjectsScoreBDto> subjectsScoreDtoBListDetail = shopReportDto.SubjectsScoreBDtoList;
                Worksheet worksheet_ShopScoreBDetail = workbook.Worksheets["审计详情"] as Worksheet;
                int count = 250 - subjectsScoreDtoBListDetail.Count;
                for (int i = 0; i < count; i++)
                {
                    subjectsScoreDtoBListDetail.Add(new SubjectsScoreBDto());
                }
                {

                    int rowIndex1 = 4;
                    foreach (SubjectsScoreBDto subjectsScoreDto in subjectsScoreDtoBListDetail)
                    {
                        if (string.IsNullOrEmpty(subjectsScoreDto.SubjectCode) || subjectsScoreDto.Remark.Contains("规避检查"))
                        {
                            msExcelUtil.DeleteRow(worksheet_ShopScoreBDetail, rowIndex1);
                            continue;
                        }
                        else
                        {
                            try
                            {
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "A", rowIndex1, subjectsScoreDto.SubjectCode);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "B", rowIndex1, subjectsScoreDto.SpCode);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "C", rowIndex1, subjectsScoreDto.AfterInvoiceDate);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "D", rowIndex1, subjectsScoreDto.AfterInvoiceDMSDate);
                                if (subjectsScoreDto.AfterInvoiceMony == "0" || subjectsScoreDto.AfterInvoiceMony == "0.00")
                                {
                                    msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "E", rowIndex1, "-");
                                }
                                else
                                {
                                    msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "E", rowIndex1, subjectsScoreDto.AfterInvoiceMony);
                                }

                                if (subjectsScoreDto.AfterDMSMony == "0" || subjectsScoreDto.AfterDMSMony == "0.00")
                                { msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "F", rowIndex1, "-"); }
                                else
                                {
                                    msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "F", rowIndex1, subjectsScoreDto.AfterDMSMony);
                                }
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "G", rowIndex1, subjectsScoreDto.B1);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "H", rowIndex1, subjectsScoreDto.B2);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "I", rowIndex1, subjectsScoreDto.B3);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "J", rowIndex1, subjectsScoreDto.Score);
                                string remark = subjectsScoreDto.Remark.Split('_')[1];
                                msExcelUtil.SetCellValue(worksheet_ShopScoreBDetail, "K", rowIndex1, remark);
                                rowIndex1++;
                            }
                            catch (Exception ex)
                            {
                                string aa = "";
                            }

                        }
                        
                    }
                    //for (int i = 5; i < 250; i++)
                    //{
                    //    if (string.IsNullOrEmpty(msExcelUtil.GetCellValue(worksheet_ShopScoreBDetail, "A", i).ToString()))
                    //    {
                    //        msExcelUtil.DeleteRow(worksheet_ShopScoreBDetail, i);
                    //    }
                    //}
                }
                #endregion
                #region 失分照片
                {
                    Worksheet worksheet_ShopScoreDetail2 = workbook.Worksheets["照片"] as Worksheet;
                    List<SubjectsScoreBDto> subjectsScoreDtoBList = shopReportDto.SubjectsScoreBDtoList;
                    int rowIndex = 4;
                    foreach (SubjectsScoreBDto subjectsScoreDto in subjectsScoreDtoBList)
                    {
                        try
                        {
                            if (((!(subjectsScoreDto.B1 == "Y") || !(subjectsScoreDto.B2 == "Y") || !(subjectsScoreDto.B3 == "Y"))
                                && !subjectsScoreDto.Remark.Contains("规避检查"))
                                )
                            {
                                msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "A", rowIndex, subjectsScoreDto.SubjectCode);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "B", rowIndex, subjectsScoreDto.SpCode);
                                msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "C", rowIndex, subjectsScoreDto.LoseDesc);
                                List<string> picNameArray = service.SearchPicNameList(CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString(), shopReportDto.ShopInfoDtoList[0].ShopName, subjectsScoreDto.SubjectCode).ToList<string>();
                                int picIndex = 0;
                                foreach (string picName in picNameArray)
                                {
                                    if (picIndex != 0 && picIndex % 2 == 0)
                                    {
                                        msExcelUtil.AddRow(worksheet_ShopScoreDetail2, ++rowIndex);
                                        msExcelUtil.SetCellValue(worksheet_ShopScoreDetail2, "X", rowIndex, subjectsScoreDto.SubjectCode);
                                    }
                                    if (string.IsNullOrEmpty(picName)) continue;
                                    byte[] bytes = service.SearchAnswerDtl2Pic(picName.Replace(".jpg", ""), CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString() + shopReportDto.ShopInfoDtoList[0].ShopName, subjectsScoreDto.SubjectCode, "", "");
                                    if (bytes == null || bytes.Length == 0) continue;
                                    Image.FromStream(new MemoryStream(bytes)).Save(Path.Combine(Path.GetTempPath(), picName + ".jpg"));
                                    int colIndex = 4 + picIndex % 2;

                                    msExcelUtil.InsertPicture(worksheet_ShopScoreDetail2, worksheet_ShopScoreDetail2.Cells[rowIndex, colIndex] as Microsoft.Office.Interop.Excel.Range, Path.Combine(Path.GetTempPath(), picName + ".jpg"), rowIndex);
                                    picIndex++;
                                }

                                rowIndex++;
                            }
                        }
                        catch (Exception ex)
                        {
                            string aa = "";
                        }

                    }
                    int rowIndex1 = 5;
                    for (int i = 5; i < 250; i++)
                    {
                        if (string.IsNullOrEmpty(msExcelUtil.GetCellValue(worksheet_ShopScoreDetail2, "A", i).ToString())
                            && string.IsNullOrEmpty(msExcelUtil.GetCellValue(worksheet_ShopScoreDetail2, "X", i).ToString()))
                        {
                            rowIndex1 = i;
                            break;
                        }
                    }
                    for (int i = 0; i < 250; i++)
                    {
                        msExcelUtil.DeleteRow(worksheet_ShopScoreDetail2, rowIndex1);
                    }
                }
                workbook.Close(true, Path.Combine(tbnFilePath.Text, shopReportDto.ShopInfoDtoList[0].AreaName + "_" + shopReportDto.ShopInfoDtoList[0].ShopCode + "_" + shopReportDto.ShopInfoDtoList[0].ShopName + "_客户服务" + ".xlsx"), Type.Missing);
                #endregion
            }
            //workbook.Save(Path.Combine(tbnFilePath.Text,shopReportDto.ProjectCode+"_"+shopReportDto.ShopName+".xls"));

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
                CommonHandler.ShowMessage(MessageType.Information, "请选择报告生成路径");
                return;
            }
            GenerateReport();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
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
                btnModule.Text = fbd.SelectedPath;
            }
        }
    }
}
