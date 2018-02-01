using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.IO;
using System.Data.SqlClient;

namespace XHX.Common
{
    public class LocalService
    {
        public LocalService()
        {

            CommonHandler.DBConnect();
            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        #region mou.junsheng

        #region DSAT 1.0
        #region 查询所有的Project

        public DataSet SearchProjectNameAndCode()
        {
            string sql = string.Format("SELECT ProjectCode,ProjectName FROM Projects");
            DataSet ds = CommonHandler.query(sql);
            return ds;

        }
        #endregion

        #region 保存问卷的信息

        public void SaveAnswer(string projectCode, string subjectCode, string shopCode,
                            decimal? score, string remark, string imageName, string userid,
                            char checkType, string passReCheck, string date)
        {
            string sql = "";
            if (score == null)
            {
                sql = string.Format(@"EXEC up_DSAT_Answer_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@Score = null
                                                 ,@Remark = '{4}'
                                                 ,@ImageName = '{5}'
                                                 ,@UserID = '{6}'
                                                 ,@CheckType = '{7}'
                                                 ,@PassCheck = '{8}'
                                                 ,@AssessmentDate = '{9}'",
                                                        projectCode, subjectCode, shopCode, score, remark, imageName, userid,
                                                        checkType, passReCheck, date);
            }
            else
            {
                sql = string.Format(@"EXEC up_DSAT_Answer_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@Score = '{3}'
                                                 ,@Remark = '{4}'
                                                 ,@ImageName = '{5}'
                                                 ,@UserID = '{6}'
                                                 ,@CheckType = '{7}'
                                                 ,@PassCheck = '{8}'
                                                 ,@AssessmentDate = '{9}'",
                                                        projectCode, subjectCode, shopCode, score, remark, imageName, userid,
                                                        checkType, passReCheck, date);
            }
            CommonHandler.query(sql);
        }
        #endregion

        #region 保存问卷的信息AnswerDtl

        public void SaveAnswerDtl(string projectCode, string subjectCode, string shopCode,
                                int SeqNO, string userid, string checkOptionCode)
        {
            string sql = string.Format(@"EXEC up_DSAT_AnswerDtl_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@SeqNO = {3}
                                                 ,@UserID = '{4}'
                                                 ,@CheckOptionCode = '{5}'",
                                                      projectCode, subjectCode, shopCode,
                                                      SeqNO, userid, checkOptionCode);
            CommonHandler.query(sql);
        }
        #endregion

        #region 保存问卷的信息AnswerDtl2

        public void SaveAnswerDtl2(string projectCode, string subjectCode, string shopCode, int seqNo, string userID, string checkOption, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            int streamLength = (int)fs.Length;
            byte[] image = new byte[streamLength];
            fs.Read(image, 0, streamLength);
            fs.Close();

            string cString = "Data Source =.;initial Catalog = DSAT_DEV;User ID = DSAT;Password = DSAT;";
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = cString;
            //cnn.Open();
            SqlCommand command = new SqlCommand("EXEC up_DSAT_AnswerDtl2_S @ProjectCode,@SubjectCode,@ShopCode,@SeqNO,@UserID,@CheckOptions,@Image", cnn);

            SqlParameter parProjectCode = new SqlParameter("@ProjectCode", SqlDbType.VarChar, 6);
            parProjectCode.Value = projectCode;
            command.Parameters.Add(parProjectCode);

            SqlParameter parSubjectCode = new SqlParameter("@SubjectCode", SqlDbType.VarChar, 20);
            parSubjectCode.Value = subjectCode;
            command.Parameters.Add(parSubjectCode);

            SqlParameter parShopCode = new SqlParameter("@ShopCode", SqlDbType.VarChar, 20);
            parShopCode.Value = shopCode;
            command.Parameters.Add(parShopCode);

            SqlParameter parSeqNO = new SqlParameter("@SeqNO", SqlDbType.Int);
            parSeqNO.Value = seqNo;
            command.Parameters.Add(parSeqNO);

            SqlParameter parUserID = new SqlParameter("@UserID", SqlDbType.VarChar, 50);
            parUserID.Value = userID;
            command.Parameters.Add(parUserID);

            SqlParameter parCheckOptionCode = new SqlParameter("@CheckOptions", SqlDbType.VarChar, 2);
            parCheckOptionCode.Value = checkOption;
            command.Parameters.Add(parCheckOptionCode);

            SqlParameter parImage = new SqlParameter("@Image", SqlDbType.Image);
            parImage.Value = image;
            command.Parameters.Add(parImage);

            cnn.Open();
            int num = command.ExecuteNonQuery();
            cnn.Close();
        }


        public void SaveAnswerDtl2Streampic(string userID, byte[] image, string shopName, string fileName)
        {

            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadImagePath = appDomainPath + @"UploadImage\" + shopName + @"\";

            if (!Directory.Exists(appDomainPath + @"UploadImage\" + shopName))
            {
                Directory.CreateDirectory(uploadImagePath);
            }
            if (image != null)
            {
                MemoryStream buf = new MemoryStream(image);

                Image picimage = Image.FromStream(buf, true);
                picimage.Save(uploadImagePath + fileName + ".jpg");
            }

        }

        public void SaveAnswerDtl2Stream(string projectCode, string subjectCode, string shopCode, int seqNo, string userID, string checkOption, byte[] image, string shopName, string fileName)
        {

            string sql = string.Format(@"EXEC up_DSAT_AnswerDtl2_S 
                                                @ProjectCode = '{0}'
                                                 ,@SubjectCode = '{1}'
                                                 ,@ShopCode = '{2}'
                                                 ,@SeqNO = {3}
                                                 ,@UserID = '{4}'
                                                 ,@CheckOptionCode = '{5}'
                                                 , @FileExtend = '{6}'",
                                                     projectCode, subjectCode, shopCode,
                                                     seqNo, userID, checkOption, fileName);
            CommonHandler.query(sql);
        }
        #endregion

        #region 保存复核信息

        public void SaveRecheck(string projectCode, string subjectCode, string shopCode,
                                string userid, string recheckContent, string recheckTypeCode, string passReCheck, decimal? score)
        {
            string sql = "";
            if (score == null)
            {
                sql = string.Format(@"[up_DSAT_ReCheck_S]      
	                                        @ProjectCode = '{0}',
	                                        @SubjectCode = '{1}',
	                                        @ShopCode='{2}',
	                                        @ReCheckUser= '{3}',
	                                        @ReCheckContent	= '{4}',
	                                        @ReCheckTypeCode = '{5}',
	                                        @PassReCheck = {6}, 
                                            @Score = null",
                                                 projectCode, subjectCode, shopCode, userid,
                                                 recheckContent, recheckTypeCode, passReCheck);
            }
            else
            {
                sql = string.Format(@"[up_DSAT_ReCheck_S]      
	                                        @ProjectCode = '{0}',
	                                        @SubjectCode = '{1}',
	                                        @ShopCode='{2}',
	                                        @ReCheckUser= '{3}',
	                                        @ReCheckContent	= '{4}',
	                                        @ReCheckTypeCode = '{5}',
	                                        @PassReCheck = {6}, 
                                            @Score = {7}",
                                                   projectCode, subjectCode, shopCode, userid,
                                                   recheckContent, recheckTypeCode, passReCheck, score);
            }
            CommonHandler.query(sql);
        }
        #endregion

        #region 查询下一个问卷信息并显示

        public DataSet SearchNextSubject(string projectCode, string shopCode,
                                        int orderNo, char checkType, string examType)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectNext_R] @ProjectCode= '{0}',@ShopCode = '{1}',@OrderNO = {2},@Type='N',@CheckType= '{3}',@SubjectTypeCodeExam = '{4}' ",
                                        projectCode, shopCode,
                                        orderNo, checkType, examType);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 查询下一个问卷的检查标准信息

        public DataSet SearchNextSubjectInsectionStardard(string projectCode, string subjectCode, string shopCode)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl_R] @ProjectCode= '{0}',@SubjectCode = '{1}',@ShopCode = '{2}' ",
                                   projectCode, subjectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 查询上一个问卷信息并显示Type = p

        public DataSet SearchPreSubject(string projectCode, string shopCode, int orderNO, char checkType)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectNext_R] @ProjectCode= '{0}',@ShopCode = '{1}',@OrderNO = {2},@Type='P',@CheckType= '{3}' ",
                                projectCode, shopCode,
                                orderNO, checkType);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 查询上一个问卷信息检查标准并显示

        public DataSet SearchPreSubjectInsecptionStardard(string projectCode, string subjectCode, string shopCode)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl_R] @ProjectCode= '{0}',@SubjectCode = '{1}',@ShopCode = '{2}' ",
                                      projectCode, subjectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 查询上一个问卷信息并显示 type = o

        public DataSet SearchPreSubjectTypeISO(string projectCode, string shopCode, int orderNO, char checkType, string examType)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectNext_R] @ProjectCode= '{0}',@ShopCode = '{1}',@OrderNO = {2},@Type='O',@CheckType = '{3}',@SubjectTypeCodeExam = '{4}' ",
                   projectCode, shopCode, orderNO, checkType, examType);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 通过shopCode查询shop

        public DataSet SearchShopByShopCode(string shopCode)
        {
            string sql = string.Format("SELECT ShopCode,ShopName FROM Shop WHERE ShopCode = '{0}' AND UseChk = 1 ",
                                            shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 问卷开始的时候查询上次没答完的那个问卷开始回答，如果没答过问卷就查询第一个

        public DataSet SearchStartSubject(string projectCode, string shopCode, char checkType, string examCode)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectStart_R] @ProjectCode= '{0}',@ShopCode = '{1}',@CheckType = '{2}',@SubjectTypeCodeExam='{3}' ",
                                                projectCode,
                                                shopCode, checkType, examCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 查询PassReCheckBySubjectCode

        public DataSet SearchPassReCheckBySubjectCode(string projectCode, string subjectCode, string shopCode)
        {
            string sqlCheckType = string.Format("EXEC DSAT_SearchPassReCheckBySubjectCodeAndShopCode_R '{0}','{1}','{2}'"
                        , projectCode, subjectCode, shopCode);
            DataSet ds = CommonHandler.query(sqlCheckType);
            return ds;
        }
        #endregion

        #region 查询PassReCheckBySubjectCode

        public DataSet SearchAnswerDtl2(string projectCode, string subjectCode, string shopCode)
        {

            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl2_R] @ProjectCode= '{0}',@SubjectCode = '{1}',@ShopCode = '{2}' ",
                                projectCode, subjectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }

        public byte[] SearchAnswerDtl2Pic(string projectCode, string userID, string picName, string shopName, string type, string code)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = "";
            if (type == "SpecialCase")
            {
                filePath = appDomainPath + @"UploadImage\" + @"SpecialCasePictures\" + code + @"\" + picName;
            }
            else if (type == "Notice")
            {
                filePath = appDomainPath + @"UploadImage\" + @"NoticeAttachment\" + code + @"\" + picName;
            }
            else
            {
                if (!string.IsNullOrEmpty(getImagePath(projectCode, userID)))
                {
                    appDomainPath = getImagePath(projectCode, userID);
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx";
                }
            }
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);

                byte[] b = new byte[fs.Length];
                fs.Read(b, 0, b.Length);
                fs.Close();
                return b;
            }
            else
            {
                return null;
            }

        }
        [Serializable]
        public class PictureDto
        {
            public Image Picture { get; set; }
            public string PictureName { get; set; }
        }

        public List<PictureDto> SearchAllPicture(string[] picName, string shopName)
        {
            List<PictureDto> picList = new List<PictureDto>();
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            DataSet ds = new DataSet();
            for (int i = 0; i < picName.Length; i++)
            {
                string filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName[i] + ".jpg";
                if (File.Exists(filePath))
                {
                    PictureDto pic = new PictureDto();
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        Image image = Image.FromStream(fs);
                        pic.Picture = image;
                        pic.PictureName = picName[i];

                    }
                    picList.Add(pic);
                }
            }
            return picList;

        }
        #endregion

        #region 更新复核内容

        public void UpdateRecheckContent(string projectCode, string subjectCode, string shopCode, int? score, string recheckContent, char checkType)
        {
            string sql = "";
            if (score == null)
            {
                sql = string.Format("Exec DSAT_UpdateforModify '{0}','{1}','{2}','Null','{4}','{5}'",
                                       projectCode,
                                       subjectCode, shopCode,
                                       score, recheckContent, checkType);
            }
            else
            {
                sql = string.Format("Exec DSAT_UpdateforModify '{0}','{1}','{2}','{3}','{4}','{5}'",
                                       projectCode,
                                       subjectCode, shopCode,
                                       score, recheckContent, checkType);
            }
            CommonHandler.query(sql);
        }
        #endregion


        #region 查询所有的CheckOptionType

        public DataSet SearchAllCheckOptions()
        {
            string sql = string.Format("SELECT CheckOptionCode,CheckOptionName FROM CheckOptions");//cboArea.SelectedItem
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        #endregion

        #region 查询检查标准

        public DataSet SearchInspectionStandard(string projectCode, string subjectCode)
        {
            string sql = string.Format("EXEC up_DSAT_InspectionStandard_R '{0}','{1}'", projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        #endregion

        #region 保存检查标准

        public void SaveInspectionStandard(string projectCode, string subjectCode,
                                                    int seqNO, string InspectionStandardName, string userID, char statusType)
        {
            string sql = string.Empty;
            if (statusType == 'I' || statusType == 'U')
            {
                sql = string.Format("EXEC up_DSAT_InspectionStandard_S '{0}','{1}','{2}','{3}','{4}'"
                   , projectCode, subjectCode, seqNO, InspectionStandardName, userID);
            }
            else if (statusType == 'D')
            {
                sql = string.Format("EXEC  up_DSAT_InspectionStandard_D '{0}','{1}','{2}'",
                    projectCode, subjectCode, seqNO);
            }
            CommonHandler.query(sql);
        }
        #endregion

        #region 查询复核结果

        public DataSet SearchRecheckResult(string projectCode, string areaCode, string shopCode)
        {
            string sql = string.Format("EXEC [up_DSAT_ReCheck_R] @ProjectCode = '{0}', @AreaCode = '{1}', @ShopCode = '{2}' ",
                    projectCode, areaCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion


        #region 查询复核Log

        public DataSet SearchRecheckLog(string projectCode, string shopCode, string subjectCode)
        {
            string sql = string.Format("EXEC [DSAT_ReCheckLog_R] '{0}','{1}','{2}' ", projectCode, shopCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 数据导入导出

        public DataSet ShopAndSubjectOut(string projectCode)
        {
            string sql = string.Format("Exec up_DSAT_DataTransfer_ShopAndSubject_OUT '{0}'", projectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }


        public void ShopAndSubjectIn(string doc)
        {
            string sql = string.Format("exec up_DSAT_DataTransfer_ShopAndSubject_IN '{0}'", doc);
            DataSet ds = CommonHandler.query(sql);

        }


        public DataSet AnswerOut(string projectcode, string shopCode)
        {
            string sql = string.Format("Exec up_DSAT_DataTransfer_Answer_OUT '{0}','{1}'", projectcode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;

        }


        public void AnswerIn(string doc, string projectCode, string shopcode)
        {
            string sql = string.Format("up_DSAT_DataTransfer_Answer_IN '{0}','{1}','{2}'", doc, projectCode, shopcode);
            DataSet ds = CommonHandler.query(sql);

        }

        public void DeleteData(string projectCode)
        {
            string sql = string.Format("Exec DSAT_DeleteData '{0}' ", projectCode);
            DataSet ds = CommonHandler.query(sql);
        }
        #endregion


        #endregion

        #region DSAT 2.0
        #region 失分说明(LossResultReg)

        #region 查询失分说明

        public DataSet SearchLoss(string projectCode, string subjectCode)
        {
            string sql = string.Format("EXEC [up_DSAT_LossResult_R] '{0}','{1}' ", projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 保存失分说明

        public void SaveLoss(string lossCode, string lossName, string inUserID)
        {
            string sql = string.Format("EXEC [up_DSAT_LossResult_S] '{0}','{1}','{2}' ", lossCode, lossName, inUserID);
            DataSet ds = CommonHandler.query(sql);
        }


        public void SaveLossForm(string projectCode, string subjectCode, string lossCode, string lossName, string inUserID, char statusType, string lossType)
        {
            string sql = "";
            if (statusType == 'I' || statusType == 'U')
            {
                sql = string.Format("EXEC [up_DSAT_LossResult_S] '{0}','{1}','{2}', '{3}','{4}','{5}'",
                    projectCode, subjectCode, lossCode, lossName, inUserID, lossType);
            }
            else
            {
                sql = string.Format("EXEC [up_DSAT_LossResult_D] '{0}','{1}','{2}'", projectCode, subjectCode, lossCode);
            }
            DataSet ds = CommonHandler.query(sql);
        }
        #endregion

        #endregion
        #region 章节
        #region 查询章节信息

        public DataSet SearchChapter(string projectCode, string chapterCode)
        {
            string sql = string.Format("EXEC [up_DSAT_Charter_R] '{0}','{1}' ", projectCode, chapterCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }

        #endregion

        #region 保存章节信息

        public void SaveChapter(string projectCode, string chapterCode, string chapterName, string chapterContent,
                                int orderNo, string InUserID, decimal weight)
        {
            string sql = string.Format("EXEC [up_DSAT_Charter_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ",
                                            projectCode, chapterCode, chapterName, chapterContent, orderNo, InUserID, weight);
            DataSet ds = CommonHandler.query(sql);
            //return ds;
        }



        #endregion
        #endregion

        #region 环节
        #region 查询环节信息

        public DataSet SearchLink(string projectCode, string chapterCode)
        {
            string sql = string.Format("EXEC [up_DSAT_Link_R] '{0}','{1}' ", projectCode, chapterCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }

        #endregion

        #region 保存环节信息

        public void SaveLink(string projectCode, string chapterCode, string linkCode, string linkName, string linkContent,
                               string InUserID)
        {
            string sql = string.Format("EXEC [up_DSAT_Link_S] '{0}','{1}','{2}','{3}','{4}','{5}' ",
                                            projectCode, chapterCode, linkCode, linkName, linkContent, InUserID);
            DataSet ds = CommonHandler.query(sql);
            //return ds;
        }



        #endregion
        #endregion
        #region 得分登记

        public void DeletePicture(string shopName, string fileName)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadImagePath = appDomainPath + @"UploadImage\" + shopName + @"\";
            if (File.Exists(uploadImagePath + fileName + ".jpg"))
            {
                File.Delete(uploadImagePath + fileName + ".jpg");
            }
        }

        public DataSet SearchSubjectOrder(int orderNO)
        {
            string sql = string.Format("EXEC up_DSAT_SearchSubjectOrderNO {0}", orderNO);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion
        #region 加权得分率

        public DataSet SearchHearForWeightRate(string projectCode, string shopCode)
        {
            string[] spiltShopCode = shopCode.Split(',');
            string sqlInit = string.Format("EXEC [DSAT_ChapterRatio_Head_R] '{0}','{1}' ", projectCode, spiltShopCode[0]);
            DataSet dsInit = CommonHandler.query(sqlInit);
            for (int i = 0; i < spiltShopCode.Length; i++)
            {
                string sql = string.Format("EXEC [DSAT_ChapterRatio_Head_R] '{0}','{1}' ", projectCode, spiltShopCode[i]);
                DataSet ds = CommonHandler.query(sql);
                dsInit.Merge(ds);
            }
            DataView dv = new DataView(dsInit.Tables[0]);
            DataTable dt = dv.ToTable(true, "Column1", "Caption1", "Column2", "Caption2", "Order");
            dsInit.Tables.Clear();
            dsInit.Tables.Add(dt);
            return dsInit;
        }

        public DataSet SearchLeftForWeightRate(string projectCode, string chapterCode, bool check)
        {
            string[] spiltChapterCode = chapterCode.Split(',');
            string sqlInit = string.Format("EXEC [DSAT_ChapterWeight_Left_R] '{0}','{1}' ", projectCode, spiltChapterCode[0]);
            DataSet dsInit = CommonHandler.query(sqlInit);


            for (int i = 0; i < spiltChapterCode.Length; i++)
            {
                string sql = string.Format("EXEC [DSAT_ChapterWeight_Left_R] '{0}','{1}' ", projectCode, spiltChapterCode[i]);
                DataSet ds = CommonHandler.query(sql);
                dsInit.Merge(ds);
            }
            if (check)
            {
                string sql = string.Format("EXEC [DSAT_FFVWeight_Left_R] '{0}' ", projectCode);
                DataSet ds = CommonHandler.query(sql);
                dsInit.Merge(ds);
            }
            DataView dv = new DataView(dsInit.Tables[0]);
            DataTable dt = dv.ToTable(true, "CharterCode", "CharterName", "Weight");
            dsInit.Tables.Clear();
            dsInit.Tables.Add(dt);
            return dsInit;
        }

        public DataSet SearchBodayForWeightRate(string projectCode, string chaterCode, string shopCode, bool fCheck, bool check)
        {
            string[] spiltChapterCode = chaterCode.Split(',');
            string[] spiltShopCode = shopCode.Split(',');
            string sqlInit = string.Format("EXEC [DSAT_ChapterWeight_Data_R] '{0}','{1}' ,'{2}',{3} ", projectCode, spiltChapterCode[0], spiltShopCode[0], fCheck == true ? 1 : 0);
            DataSet dsInit = CommonHandler.query(sqlInit);


            for (int i = 0; i < spiltChapterCode.Length; i++)
            {
                for (int j = 0; j < spiltShopCode.Length; j++)
                {
                    string sql = string.Format("EXEC [DSAT_ChapterWeight_Data_R] '{0}','{1}' ,'{2}',{3} ", projectCode, spiltChapterCode[i], spiltShopCode[j], fCheck == true ? 1 : 0);
                    DataSet ds = CommonHandler.query(sql);
                    dsInit.Merge(ds);
                }
            }
            if (check)
            {
                for (int j = 0; j < spiltShopCode.Length; j++)
                {
                    string sql = string.Format("EXEC [DSAT_FFVWeight_Data_R] '{0}','{1}' ", projectCode, spiltShopCode[j]);
                    DataSet ds = CommonHandler.query(sql);
                    dsInit.Merge(ds);
                }
            }
            DataView dv = new DataView(dsInit.Tables[0]);
            DataTable dt = dv.ToTable(true, "Column1", "Column2", "CharterCode", "Value");
            dsInit.Tables.Clear();
            dsInit.Tables.Add(dt);
            return dsInit;
        }

        public void ImportFFV(byte[] b, string projectCode, string inUserID)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadImagePath = appDomainPath + @"UploadImage\";

            if (!Directory.Exists(appDomainPath + @"UploadImage\"))
            {
                Directory.CreateDirectory(uploadImagePath);
            }
            string path = uploadImagePath + "ffv.xls";
            if (b != null)
            {
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    fs.Write(b, 0, b.Length);
                }
            }
            string sql = string.Format("EXEC [DSAT_ImportExcel] '{0}','{1}','{2}' ", path, projectCode, inUserID);
            DataSet ds = CommonHandler.query(sql);
        }

        public void SaveFFVRate(string projectCode, string allRate, string eastRate, string southRate,
                                string westRate, string northRate, string weight, string userID)
        {
            decimal? dallRate = null;
            decimal? deastRate = null;
            decimal? dsouthRate = null;
            decimal? dwestRate = null;
            decimal? dnorthRate = null;
            decimal? dweight = null;
            if (!string.IsNullOrEmpty(allRate) && allRate != null)
            {
                dallRate = Convert.ToDecimal(allRate);
            }
            if (!string.IsNullOrEmpty(eastRate) && eastRate != null)
            {
                deastRate = Convert.ToDecimal(eastRate);
            }
            if (!string.IsNullOrEmpty(southRate) && southRate != null)
            {
                dsouthRate = Convert.ToDecimal(southRate);
            }
            if (!string.IsNullOrEmpty(northRate) && northRate != null)
            {
                dnorthRate = Convert.ToDecimal(northRate);
            }
            if (!string.IsNullOrEmpty(westRate) && westRate != null)
            {
                dwestRate = Convert.ToDecimal(westRate);
            }

            if (!string.IsNullOrEmpty(weight) && weight != null)
            {
                dweight = Convert.ToDecimal(weight);
            }
            string sql = string.Format("exec [up_DSAT_SaveFFVRate] '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}'",
                projectCode, allRate, eastRate, southRate, westRate, northRate, weight, userID);
            CommonHandler.query(sql);
        }


        public void SaveFFVShopRate(string projectCode, string shopCode, string weight, string userID)
        {
            string sql = string.Format("exec [up_DSAT_SaveFFVShopRate] '{0}','{1}','{2}','{3}'",
                                     projectCode, shopCode, weight, userID);
            CommonHandler.query(sql);
        }
        #endregion
        #region 飞检数据导入

        public void SaveFeiJianSubject(string projectCode, string subjectCode, string checkPoint, string userId, string linkCode)
        {
            string sql = string.Format("exec [up_DSAT_SaveFeiJianSubject] '{0}','{1}','{2}','{3}','{4}'", projectCode, subjectCode, checkPoint, userId, linkCode);
            CommonHandler.query(sql);
        }


        public void SaveFeiJianScore(string projectCode, string subjectCode, string shopCode, decimal? score, string userId)
        {
            string sql = string.Format("exec [up_DSAT_SaveFeiJianScore] '{0}','{1}','{2}','{3}','{4}'", projectCode, subjectCode, shopCode, score, userId);
            CommonHandler.query(sql);
        }


        #endregion

        #region 用户查询


        public DataSet SearchUserInfo(string userID)
        {
            string sql = string.Format("Exec [up_DSAT_SearchUserInfo] '{0}'", userID);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion
        #endregion

        #region DSAT 3.0
        #region 查询分数设置

        public DataSet SearchScoreSet(string projectCode)
        {
            string sql = string.Format("EXEC [up_DSAT_ScoreSet_R] '{0}' ", projectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #region 删除分数设置

        public void DeleteScoreSet(string projectCode, int? seqNO)
        {
            string sql = string.Format("EXEC [up_DSAT_ScoreSet_D] '{0}','{1}' ", projectCode, seqNO);
            CommonHandler.query(sql);

        }
        #endregion
        #region 添加分数设置

        public void InsertScoreSet(string projectCode, int? seqNO, Decimal? score, bool? notInvolved, string inUserID, DateTime? inDateTime)
        {
            string sql = string.Format("EXEC [up_DSAT_ScoreSet_S] '{0}','{1}','{2}','{3}','{4}','{5}' ", projectCode, seqNO, score, notInvolved, inUserID, inDateTime);
            CommonHandler.query(sql);

        }
        #endregion
        #endregion
        #region 得分登记页面
        #region 查询失分说明


        public DataSet SearchLossDesc(string projectCode, string shopCode, string subjectCode)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerSubjectAnswerDtl3_R] '{0}','{1}','{2}' ", projectCode, shopCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion
        #region 保存失分说明

        public void SaveLossDesc(string projectCode, string shopCode, string subjectCode, string lossDesc, string picName, int? SeqNO, char statusType)
        {
            string sql = "";
            if (statusType != 'D')
            {
                sql = string.Format("EXEC [up_DSAT_AnswerDtl3_S] '{0}','{1}','{2}','{3}','{4}','{5}' ", projectCode, subjectCode, shopCode, SeqNO, lossDesc, picName);

            }
            else
            {
                sql = string.Format("EXEC [up_DSAT_AnswerDtl3_D] '{0}','{1}','{2}','{3}' ", projectCode, subjectCode, shopCode, SeqNO);
            }
            CommonHandler.query(sql);

        }
        #endregion
        #region 保存得分Log

        public void SaveAnswerLog(string projectCode, string shopCode, string subjectCode, string statusCode, decimal? score, string desc, string userID)
        {
            string sql = "";
            if (score == null)
            {
                sql = string.Format(@"EXEC [up_DSAT_AnswerLog_S] @ProjectCode = '{0}',@SubjectCode = '{1}',@ShopCode='{2}',@StatusCode='{3}',
                                            @Score = null,@Desc = '{5}',@UserID='{6}' ",
                                    projectCode, subjectCode, shopCode, statusCode, score, desc, userID);
            }
            else
            {
                sql = string.Format("EXEC [up_DSAT_AnswerLog_S] '{0}','{1}','{2}','{3}','{4}','{5}','{6}' ", projectCode, subjectCode, shopCode, statusCode, score, desc, userID);
            }
            CommonHandler.query(sql);

        }
        #endregion
        #region 申请复审


        public void SaveRecheckStatus(string projectCode, string shopCode, string statusCode, string userID)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerRecheckStatus_S] '{0}','{1}','{2}','{3}' ", projectCode, shopCode, statusCode, userID);
            CommonHandler.query(sql);

        }
        #endregion
        #region 查询当前复审进度


        public DataSet SearchRecheckStatus(string projectCode, string shopCode)
        {
            string sql = string.Format("EXEC [up_DSAT_AnswerRecheckStatus_R] '{0}','{1}' ", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;

        }
        #endregion
        #region 上传数据
        public void UploadData(string projectCode, string shopCode)
        {
            CreateLinkedServer();
            string cString = "Data Source =.;initial Catalog = DSAT_2.0;User ID = DSAT;Password = DSAT;";
            SqlConnection cnn = new SqlConnection();
            cnn.ConnectionString = cString;
            //cnn.Open();
            SqlCommand command = new SqlCommand("EXEC up_DSAT_SyncAnswerDataToServer @ProjectCode,@ShopCode", cnn);

            SqlParameter parProjectCode = new SqlParameter("@ProjectCode", SqlDbType.VarChar, 6);
            parProjectCode.Value = projectCode;
            command.Parameters.Add(parProjectCode);

            SqlParameter parShopCode = new SqlParameter("@ShopCode", SqlDbType.VarChar, 20);
            parShopCode.Value = shopCode;
            command.Parameters.Add(parShopCode);

            cnn.Open();
            int num = command.ExecuteNonQuery();
            cnn.Close();
            //string sql = string.Format("EXEC up_DSAT_SyncAnswerDataToServer '{0}','{1}'", projectCode, shopCode);
            //CommonHandler.query(sql);
            //string sql1 = string.Format("EXEC up_DSAT_SyncAnswerDataToServer1 '{0}','{1}'", projectCode, shopCode);
            //CommonHandler.query(sql1);
            //string sql2 = string.Format("EXEC up_DSAT_SyncAnswerDataToServer2 '{0}','{1}'", projectCode, shopCode);
            //CommonHandler.query(sql2);
            //string sql3 = string.Format("EXEC up_DSAT_SyncAnswerDataToServer3 '{0}','{1}'", projectCode, shopCode);
            //CommonHandler.query(sql3);
            //string sql4 = string.Format("EXEC up_DSAT_SyncAnswerDataToServer4 '{0}','{1}'", projectCode, shopCode);
            //CommonHandler.query(sql4);
        }
        #endregion
        #endregion
        #region 得分复审
        #region 复审完毕

        public void RechekComplete(string projectCode, string shopCode, string statusTypeCode, string userID)
        {
            string sql = string.Format(@"EXEC [up_DSAT_AnswerRecheckFinish_S] @ProjectCode = '{0}',@ShopCode='{1}'
                                        ,@StatusCode='{2}' ,@UserID='{3}'", projectCode, shopCode, statusTypeCode, userID);
            CommonHandler.query(sql);
        }

        public DataSet CheckShopAllPassRechk(string projectCode, string shopCode, string statusTypeCode)
        {
            string sql = string.Format(@"EXEC [up_DSAT_CheckShopAllPassRechk_R] @ProjectCode = '{0}',@ShopCode='{1}'
                                        ,@StatusCode='{2}'", projectCode, shopCode, statusTypeCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }

        #endregion
        #endregion
        #region 复审记录查询

        public DataSet SearchAnswerLog(string projectCode, string shopCode)
        {
            string sql = string.Format("EXEC up_DSAT_AnswerLog_R '{0}','{1}'", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion
        #region 复审进度查询

        public DataSet SearchReCheckProcess(string projectCode, string shopCode)
        {
            string sql = string.Format("EXEC up_DSAT_AnswerRecheckStatus_R '{0}','{1}'", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion
        #region 数据传输
        public void MasterDataDownLoad(string userID)
        {
            CreateLinkedServer();
            string sql = string.Format("EXEC up_DSAT_SyncMasterDataFromServer1 '{0}'", userID);
            CommonHandler.query(sql);
            string sql1 = string.Format("EXEC up_DSAT_SyncMasterDataFromServer '{0}'", userID);
            CommonHandler.query(sql1);
            string sql2 = string.Format("EXEC up_DSAT_SyncMasterDataFromServer2 '{0}'", userID);
            CommonHandler.query(sql2);
        }
        public void ScoreDataDownLoad(string projectCode, string shopCode)
        {
            CreateLinkedServer();
            string sql = string.Format("EXEC up_DSAT_SyncAnswerDataFromServer '{0}','{1}'", projectCode, shopCode);
            CommonHandler.query(sql);
        }
        /// <summary>
        /// 建立LinkedServer
        /// </summary>
        public void CreateLinkedServer()
        {
            string sql = string.Format("EXEC up_DSAT_SyncCreateLinkedServer ");
            CommonHandler.query(sql);
        }
        #endregion
        public DataSet SearchSubjectBySubjectCodeAndProjectCode(string projectCode, string subjectCode)
        {
            string sql = string.Format("EXEC up_DSAT_GetSubjectBySubjectCode_R '{0}','{1}'", projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #region 同步SP
        public void syncSP(string spName, string spContent)
        {
            string sql = string.Format("EXEC up_DSAT_SyncSP ");
            CommonHandler.query(sql);

        }
        #endregion
        #region 得到图片路径
        public string getImagePath(string projectCode, string userID)
        {
            string path = string.Empty;
            string sql = string.Format("exec up_DSAT_GetUserImageFilePath_R '{0}','{1}'", projectCode, userID);
            DataSet ds = CommonHandler.query(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                path = ds.Tables[0].Rows[0]["FilePath"].ToString();
            }
            return path;
        }
        #endregion
        #endregion
        #endregion

        #region zhang.xichun

        #region Shop/Shop_Popup


        public DataSet SearchShop(string shopCode, string shopName)
        {
            string sql = string.Format("EXEC [up_DSAT_Shop_R] @ShopCode= '{0}',@ShopName = '{1}' ", shopCode, shopName);//cboArea.SelectedItem
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        public void SaveShop(string saleSmallAreaCode, string afterSmallAreaCode, string shopCode, string shopName, bool useChk, string userName)
        {
            string sql = string.Format("EXEC up_DSAT_Shop_S '{0}','{1}','{2}','{3}','{4}','{5}'",
                                       saleSmallAreaCode, afterSmallAreaCode, shopCode, shopName, useChk, userName);
            CommonHandler.query(sql);
        }

        #endregion

        #region ShopScoreSearch


        public DataSet SearchHead(string projectCode, string shopCode)
        {
            string sql = string.Format("exec ReportScore_Head_R '{0}','{1}'", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet SearchBodyData(string projectCode, string shopCode, bool lossCheck, bool recheck)
        {
            string sql = string.Format("exec ReportScore_DATA_R '{0}','{1}',{2},{3}", projectCode, shopCode, lossCheck, recheck);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }


        public DataSet SearchLeft(string projectCode)
        {
            string sql = string.Format("exec ReportSocre_Left_R '{0}'", projectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }


        public DataSet SearchSubjectBySubjectCode(string subjectCode)
        {
            string sql = string.Format("select OrderNO from Subjects where SubjectCode = '{0}'", subjectCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }


        public DataSet SearchPassReCheckBySubjectCodeAndShopCode(string projectCode, string subjectCode, string shopCode)
        {
            string sql = string.Format("EXEC DSAT_SearchPassReCheckBySubjectCodeAndShopCode_R '{0}','{1}','{2}'"
                       , projectCode, subjectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }

        #endregion

        #region Cbo_DataSource


        public DataSet GetAllArea()
        {
            string sql = string.Format("SELECT AreaCode,AreaName FROM Area");
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet GetAllProject()
        {
            string sql = string.Format("SELECT ProjectCode,ProjectName FROM Projects ORDER BY ORDERNO desc");
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        #endregion

        #region StandardRate


        public DataSet SearchRateAllByProjectCode(string projectCode)
        {
            string sql = string.Format("EXEC up_DSAT_StandardRate_R '{0}'", projectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet SearchRateAllByArea(string projectCode)
        {
            string sql = string.Format("EXEC up_DSAT_StandardRateByArea_R '{0}'", projectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        #endregion

        #region SubjectFile


        public DataSet SearchSubjectFile(string projectCode, string subjectCode)
        {
            string sql = string.Format("EXEC up_DSAT_FileAndPicture_R '{0}','{1}'", projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public void SaveFileAndPicture(char statusType, string projectCode, string subjectCode, int seqNO,
                                       string fileName, string fileType)
        {
            string sql = string.Empty;
            if (statusType == 'I' || statusType == 'U')
            {
                sql = string.Format("EXEC up_DSAT_FileAndPicture_S '{0}','{1}','{2}','{3}','{4}','{5}'"
                   , projectCode, subjectCode, seqNO, fileName, fileType, "Sysadmin");
            }
            else if (statusType == 'D')
            {
                sql = string.Format("EXEC  up_DSAT_FileAndPicture_D '{0}','{1}','{2}'",
                    projectCode, subjectCode, seqNO);
            }
            CommonHandler.query(sql);
        }

        #endregion

        #region Subjects


        public DataSet SearchProject()
        {
            string sql = string.Format("EXEC up_DSAT_Projects_R");
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet SearchSubject(string projectCode, string chapterCode, string linkCode, string examTypeCode)
        {
            string sql = string.Format("EXEC up_DSAT_Subjects_R '{0}','{1}','{2}','{3}'", projectCode, chapterCode, linkCode, examTypeCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public void SaveProject(char statusType, string projectCode, string year, string quarter)
        {
            string sql = String.Empty;
            if (statusType == 'I')
            {
                sql = string.Format("EXEC up_DSAT_Projects_S '{0}','{1}','{2}','{3}','{4}'", year + quarter,
                    year + quarter, "sysadmin", year, quarter);
            }
            else if (statusType == 'U')
            {
                sql = string.Format("EXEC up_DSAT_Projects_S '{0}','{1}','{2}','{3}','{4}'", projectCode,
                    projectCode, "sysadmin", year, quarter);
            }
            CommonHandler.query(sql);
        }


        public void SaveSubject(char statusType, string projectCode, string subjectCode, string implementation, string checkPoint,
                                string desc, string additionalDesc, string inspectionDesc, string inspectionNeedFile,
                                string remark, int orderNO, string linkCode, decimal fullScore, bool? scoreCheck, string subjectTypeCode, string subjectTypeCodeExam)
        {
            string sql = String.Empty;
            if (statusType == 'I' || statusType == 'U')
            {
                sql = string.Format("EXEC up_DSAT_Subjects_S '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12},{13},'{14}','{15}'"
                                    , projectCode, subjectCode, implementation, checkPoint, desc, additionalDesc,
                                      inspectionDesc, inspectionNeedFile, remark, orderNO, "sysadmin", linkCode, fullScore, scoreCheck, subjectTypeCode, subjectTypeCodeExam);
            }
            else if (statusType == 'D')
            {
                sql = string.Format("EXEC up_DSAT_Subjects_D '{0}','{1}'", projectCode, subjectCode);
            }
            CommonHandler.query(sql);
        }


        public DataSet CheckSubjectExists(string projectCode, string subjectCode)
        {
            string sql = string.Format("EXEC up_DSAT_CheckSubjectExists_R '{0}','{1}'", projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet SearchInspectionStandardByProjectCodeAndSubjectCode(string projectCode, string subjectCode)
        {
            string sql = string.Format("SELECT InspectionStandardName FROM InspectionStandard WHERE ProjectCode = '{0}' AND SubjectCode = '{1}'",
                                    projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet SearchFileAndPictureByProjectCodeAndSubjectCode(string projectCode, string subjectCode)
        {
            string sql = string.Format("SELECT [FileName] FROM FileAndPicture  WHERE ProjectCode = '{0}' AND SubjectCode = '{1}'",
                                        projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        #endregion

        #region LoginForm


        public DataSet SearchUserByUserID(string userID)
        {
            string sql = string.Format("SELECT UserID,PSW,RoleType FROM dbo.UserInfo WHERE UserID = '{0}'",
                                        userID);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        #endregion

        #region RateSearch


        public List<DataSet[]> SearchScoreRate(string projectCode, string[] chapterCodes, string[] linkCode, string[] subjectCodes, string[] shopCodes, bool fCheck)
        {
            List<DataSet[]> result = new List<DataSet[]>();
            result.Add(SearchScoreRateForChapter(projectCode, chapterCodes, shopCodes, fCheck));
            result.Add(SearchScoreRateForLink(projectCode, linkCode, shopCodes, fCheck));
            result.Add(SearchScoreRateForSubject(projectCode, subjectCodes, fCheck));

            return result;
        }

        #region ChapterRatio


        public DataSet[] SearchScoreRateForChapter(string projectCode, string[] chapterCodes, string[] shopCodes, bool fCheck)
        {
            DataSet[] result = new DataSet[3];
            result[0] = SearchScoreRateForChapterHead(projectCode, shopCodes);
            result[1] = SearchScoreRateForChapterBodyData(projectCode, chapterCodes, shopCodes, fCheck);
            result[2] = SearchScoreRateForChapterLeft(projectCode, chapterCodes);

            return result;
        }


        public DataSet SearchScoreRateForChapterHead(string projectCode, string[] shopCodes)
        {
            string sql = string.Format("exec DSAT_ChapterRatio_Head_R '{0}','{1}'", projectCode, shopCodes[0]);
            DataSet ds = CommonHandler.query(sql);
            for (int i = 1; i < shopCodes.Length; i++)
            {
                sql = string.Format("exec DSAT_ChapterRatio_Head_R '{0}','{1}'", projectCode, shopCodes[i]);
                DataSet TempDS = CommonHandler.query(sql);
                ds.Merge(TempDS);
            }

            DataView myDataView = new DataView(ds.Tables[0]);
            DataTable tempDT = myDataView.ToTable(true, "Column1", "Caption1", "Column2", "Caption2", "Order");
            ds.Tables.Clear();
            ds.Tables.Add(tempDT);

            return ds;
        }


        public DataSet SearchScoreRateForChapterBodyData(string projectCode, string[] chapterCodes, string[] shopCodes, bool fCheck)
        {
            string sql = string.Format("exec DSAT_ChapterRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, chapterCodes[0], shopCodes[0], fCheck == true ? "1" : "0");
            DataSet ds = CommonHandler.query(sql);
            for (int i = 0; i < chapterCodes.Length; i++)
            {
                for (int j = 1; j < shopCodes.Length; j++)
                {
                    sql = string.Format("exec DSAT_ChapterRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, chapterCodes[i], shopCodes[j], fCheck == true ? "1" : "0");
                    DataSet TempDS = CommonHandler.query(sql);
                    ds.Merge(TempDS);
                }
            }

            DataView myDataView = new DataView(ds.Tables[0]);
            DataTable tempDT = myDataView.ToTable(true, "Column1", "Column2", "CharterCode", "Value");
            ds.Tables.Clear();
            ds.Tables.Add(tempDT);

            return ds;
        }


        public DataSet SearchScoreRateForChapterLeft(string projectCode, string[] chapterCodes)
        {
            string sql = string.Format("exec DSAT_ChapterRatio_Left_R '{0}','{1}'", projectCode, chapterCodes[0]);
            DataSet ds = CommonHandler.query(sql);
            for (int i = 1; i < chapterCodes.Length; i++)
            {
                sql = string.Format("exec DSAT_ChapterRatio_Left_R '{0}','{1}'", projectCode, chapterCodes[i]);
                DataSet TempDS = CommonHandler.query(sql);
                ds.Merge(TempDS);
            }

            DataView myDataView = new DataView(ds.Tables[0]);
            DataTable tempDT = myDataView.ToTable(true, "CharterCode", "CharterName");
            ds.Tables.Clear();
            ds.Tables.Add(tempDT);

            return ds;
        }

        #endregion

        #region LinkRatio


        public DataSet[] SearchScoreRateForLink(string projectCode, string[] linkCode, string[] shopCodes, bool fCheck)
        {
            DataSet[] result = new DataSet[3];
            result[0] = SearchScoreRateForLinkHead(projectCode, shopCodes);
            result[1] = SearchScoreRateForLinkBodyData(projectCode, linkCode, shopCodes, fCheck);
            result[2] = SearchScoreRateForLinkLeft(projectCode, linkCode);

            return result;
        }


        public DataSet SearchScoreRateForLinkHead(string projectCode, string[] shopCodes)
        {
            string sql = string.Format("exec DSAT_LinkRatio_Head_R '{0}','{1}'", projectCode, shopCodes[0]);
            DataSet ds = CommonHandler.query(sql);
            for (int i = 1; i < shopCodes.Length; i++)
            {
                sql = string.Format("exec DSAT_LinkRatio_Head_R '{0}','{1}'", projectCode, shopCodes[i]);
                DataSet TempDS = CommonHandler.query(sql);
                ds.Merge(TempDS);
            }

            DataView myDataView = new DataView(ds.Tables[0]);
            DataTable tempDT = myDataView.ToTable(true, "Column1", "Caption1", "Column2", "Caption2", "Order");
            ds.Tables.Clear();
            ds.Tables.Add(tempDT);

            return ds;
        }


        public DataSet SearchScoreRateForLinkBodyData(string projectCode, string[] linkCode, string[] shopCodes, bool fCheck)
        {
            string sql = string.Format("exec DSAT_LinkRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, linkCode[0], shopCodes[0], fCheck == true ? "1" : "0");
            DataSet ds = CommonHandler.query(sql);
            for (int i = 0; i < linkCode.Length; i++)
            {
                for (int j = 1; j < shopCodes.Length; j++)
                {
                    sql = string.Format("exec DSAT_LinkRatio_Data_R '{0}','{1}','{2}',{3}", projectCode, linkCode[i], shopCodes[j], fCheck == true ? "1" : "0");
                    DataSet TempDS = CommonHandler.query(sql);
                    ds.Merge(TempDS);
                }
            }

            DataView myDataView = new DataView(ds.Tables[0]);
            DataTable tempDT = myDataView.ToTable(true, "Column1", "Column2", "LinkCode", "Value");
            ds.Tables.Clear();
            ds.Tables.Add(tempDT);

            return ds;
        }


        public DataSet SearchScoreRateForLinkLeft(string projectCode, string[] linkCode)
        {
            string sql = string.Format("exec DSAT_LinkRatio_Left_R '{0}','{1}'", projectCode, linkCode[0]);
            DataSet ds = CommonHandler.query(sql);
            for (int i = 1; i < linkCode.Length; i++)
            {
                sql = string.Format("exec DSAT_LinkRatio_Left_R '{0}','{1}'", projectCode, linkCode[i]);
                DataSet TempDS = CommonHandler.query(sql);
                ds.Merge(TempDS);
            }

            DataView myDataView = new DataView(ds.Tables[0]);
            DataTable tempDT = myDataView.ToTable(true, "LinkCode", "LinkName");
            ds.Tables.Clear();
            ds.Tables.Add(tempDT);

            return ds;
        }

        #endregion

        #region SubjectRatio


        public DataSet[] SearchScoreRateForSubject(string projectCode, string[] subjectCode, bool fCheck)
        {
            string sql = string.Format("exec DSAT_SubjectRatio_Data_R2 '{0}','{1}',{2}", projectCode, subjectCode[0], fCheck == true ? "1" : "0");
            DataSet ds = CommonHandler.query(sql);
            for (int i = 0; i < subjectCode.Length; i++)
            {
                sql = string.Format("exec DSAT_SubjectRatio_Data_R2 '{0}','{1}',{2}", projectCode, subjectCode[i], fCheck == true ? "1" : "0");
                DataSet TempDS = CommonHandler.query(sql);
                ds.Merge(TempDS);
            }

            DataView myDataView = new DataView(ds.Tables[0]);
            DataTable tempDT = myDataView.ToTable(true, "SubjectCode", "CheckPoint", "全国", "东区", "南区", "西区", "北区");
            ds.Tables.Clear();
            ds.Tables.Add(tempDT);

            return new DataSet[] { ds };
        }

        #endregion

        #endregion

        #region FinallyScoreRateSearch


        public List<DataSet[]> SearchFinallyScoreRate(string projectCode)
        {
            List<DataSet[]> result = new List<DataSet[]>();
            result.Add(SearchFinallyScoreRateForWeight(projectCode));
            result.Add(SearchFinallyScoreRateForOrder(projectCode));

            return result;
        }


        public DataSet[] SearchFinallyScoreRateForWeight(string projectCode)
        {
            DataSet[] result = new DataSet[3];
            result[0] = SearchFinallyScoreRateForWeightHead(projectCode);
            result[1] = SearchFinallyScoreRateForWeightBodyData(projectCode);
            result[2] = SearchFinallyScoreRateForWeightLeft(projectCode);

            return result;
        }


        public DataSet SearchFinallyScoreRateForWeightHead(string projectCode)
        {
            string sql = string.Format("exec DSAT_ChapterRatio_Head_R '{0}'", projectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet SearchFinallyScoreRateForWeightBodyData(string projectCode)
        {
            string sql = string.Format("exec DSAT_AllWeight_Data_R '{0}'", projectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet SearchFinallyScoreRateForWeightLeft(string projectCode)
        {
            //string sql = string.Format("exec DSAT_AllWeight_Left_R '{0}'", projectCode);
            string sql = string.Format("SELECT 'All' as CharterCode, '所有' as CharterName");
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }


        public DataSet[] SearchFinallyScoreRateForOrder(string projectCode)
        {
            DataSet[] result = new DataSet[1];

            string sql = string.Format("exec DSAT_ShopRateRank_R '{0}'", projectCode);
            DataSet ds = CommonHandler.query(sql);

            result[0] = ds;
            return result;
        }

        #endregion
        #region RoleTypeProgram

        //查询RoleTypeProgram
        public DataSet SearchRoleTypeProgramByRoleTypeCode(string roleTypeCode)
        {
            string sql = string.Format("SELECT RoleTypeProgramID,RoleTypeCode,ProgramCode FROM dbo.RoleTypeProgram WHERE RoleTypeCode = '{0}'", roleTypeCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        //插入RoleTypeProgram
        public void InsertRoleTypeProgram(string roleTypeCode, string programCode, string inUserID, DateTime inDateTime)
        {
            string sql = string.Format("INSERT INTO dbo.RoleTypeProgram VALUES('{0}','{1}','{2}','{3}')", roleTypeCode, programCode, inUserID, inDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            CommonHandler.query(sql);
        }

        //删除RoleTypeProgram
        public void DeleteRoleTypeProgram(int roleTypeProgramID)
        {
            string sql = string.Format("DELETE dbo.RoleTypeProgram WHERE RoleTypeProgramID = {0}", roleTypeProgramID);
            CommonHandler.query(sql);
        }

        //查询全部RoleType
        public DataSet SearchAllRoleType()
        {
            string sql = string.Format("SELECT RoleTypeID,RoleTypeCode,RoleTypeName FROM dbo.RoleType");
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        //查询全部Program
        public DataSet SearchAllProgram()
        {
            string sql = string.Format("SELECT ProgramID,ProgramCode,ProgramName FROM dbo.Program");
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        //查询当前用户的菜单
        public DataSet SearchCurrentUserProgram(string roleTypeCode)
        {
            string sql = string.Format("SELECT p.ProgramCode,p.ProgramName FROM dbo.Program AS p INNER JOIN dbo.RoleTypeProgram AS r ON r.ProgramCode = p.ProgramCode WHERE r.RoleTypeCode = '{0}'", roleTypeCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        #endregion

        #region UserInfo

        //插入UserInfoDto
        public void InsertUserInfo(string userID, string psw, string roleType, string inUserID)
        {
            string sql = string.Format("INSERT INTO dbo.UserInfo VALUES('{0}','{1}','{2}','{3}',GETDATE())", userID, psw, roleType, inUserID);
            CommonHandler.query(sql);
        }

        //删除UserInfoDto
        public void DeleteUserInfoDto(string userID)
        {
            string sql = string.Format("DELETE dbo.UserInfo WHERE UserID = '{0}'", userID);
            CommonHandler.query(sql);
        }

        //修改UserInfoDto
        public void UpdateUserInfoDto(string userID, string psw, string roleType, string inUserID)
        {
            string sql = string.Format("UPDATE dbo.UserInfo SET PSW = '{1}', RoleType = '{2}',InUserID='{3}',InDateTime=GETDATE() WHERE UserID = '{0}'", userID, psw, roleType, inUserID);
            CommonHandler.query(sql);
        }

        //查询UserInfoDto
        public DataSet SearchUserInfoDto(string userID, string roleType)
        {
            if (string.IsNullOrEmpty(userID) && string.IsNullOrEmpty(roleType))
            {
                string sql = string.Format("SELECT UserID,RoleType,PSW FROM dbo.UserInfo");
                return CommonHandler.query(sql);
            }
            else if (string.IsNullOrEmpty(userID))
            {
                string sql = string.Format("SELECT UserID,RoleType,PSW FROM dbo.UserInfo WHERE RoleType = '{0}'", roleType);
                return CommonHandler.query(sql);
            }
            else if (string.IsNullOrEmpty(roleType))
            {
                string sql = string.Format("SELECT UserID,RoleType,PSW FROM dbo.UserInfo WHERE UserID LIKE '%{0}%'", userID);
                return CommonHandler.query(sql);
            }
            else
            {
                string sql = string.Format("SELECT UserID,RoleType,PSW FROM dbo.UserInfo WHERE UserID LIKE '%{0}%' AND RoleType = '{1}'", userID, roleType);
                return CommonHandler.query(sql);
            }
        }

        #endregion
        #region MainForm

        public DataSet SearchSpecialCaseByNeedVICoConfirm()
        {
            string sql = @"SELECT A.[ProjectCode]
                                 ,A.[ShopCode]
                                 ,B.[ShopName]
                                 ,A.[SubjectCode]
                             FROM [XinHuaXin].[dbo].[SpecialCase] AS A
                       INNER JOIN [XinHuaXin].[dbo].[Shop] AS B
                               ON A.ShopCode = B.ShopCode
                            WHERE A.NeedVICoConfirmChk = 1
                              AND (A.VICoAdvice is null OR A.VICoAdvice = '')";
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }

        #endregion
        #region 数据导入导出

        public void SaveUserImageFilePath(string projectCode, string userID, string folderPath)
        {
            string sql = string.Format("EXEC [up_DSAT_SaveUserImageFilePath_CU] @ProjectCode='{0}',@UserID='{1}',@FilePath='{2}'", projectCode, userID, folderPath);
            CommonHandler.query(sql);
        }
        public DataSet SearchUserImageFilePath(string projectCode, string userID)
        {
            string sql = string.Format("EXEC [up_DSAT_GetUserImageFilePath_R] @ProjectCode='{0}',@UserID='{1}'", projectCode, userID);
            return CommonHandler.query(sql);
        }
        #endregion

        #region Area
        public DataSet GetAllAreaType()
        {
            string sql = string.Format("SELECT A.Code AS AreaTypeCode,A.CNDesc AS AreaTypeName FROM HiddenCode as A WHERE A.GroupCode = 'AreaType'");//cboArea.SelectedItem
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        public DataSet SearchArea(string areaTypeCode)
        {
            string sql = string.Format("EXEC [up_DSAT_Area_R] '{0}'", areaTypeCode);//cboArea.SelectedItem
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        public void SaveArea(string areaCode, string areaName, string upperAreaCode, string areaTypeCode, string userID)//CodeList<XHX.DTO.AreaDto> areaList,string userID
        {
            string sql = string.Format("EXEC [up_DSAT_Area_S] '{0}','{1}','{2}','{3}','{4}'",
                             areaCode, areaName, upperAreaCode, areaTypeCode, userID);
            CommonHandler.query(sql);
        }
        public void DeleteArea(string areaCode)
        {
            string sql = string.Format("EXEC [up_DSAT_Area_D] '{0}'",
                             areaCode);
            CommonHandler.query(sql);
        }
        #endregion
        #region SingleShopReport
        public DataSet[] GetShopReportDto(string projectCode, string shopCode)
        {

            string sql = string.Format("exec [dbo].[up_DSAT_RPT_FengMian_R] '{0}','{1}'", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);
            DataSet[] dataSetList = new DataSet[] { ds, SearchChaptersScore(projectCode, shopCode), SearchLinkScore(projectCode, shopCode), SearchSubjectsScore(projectCode, shopCode), SearchAllScore(projectCode, shopCode) };

            return dataSetList;
        }
        private DataSet SearchAllScore(string projectCode, string shopCode)
        {
            string sql = string.Format("exec [dbo].[up_DSAT_RPT_AllScore_R] '{0}','{1}'", projectCode, shopCode);
            return CommonHandler.query(sql);
        }
        private DataSet SearchChaptersScore(string projectCode, string shopCode)
        {
            string sql = string.Format("exec [dbo].[up_DSAT_RPT_ChaptersScore_R] '{0}','{1}'", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        private DataSet SearchLinkScore(string projectCode, string shopCode)
        {
            string sql = string.Format("exec [dbo].[up_DSAT_RPT_LinkScore_R] '{0}','{1}'", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        private DataSet SearchSubjectsScore(string projectCode, string shopCode)
        {
            string sql = string.Format("exec [dbo].[up_DSAT_RPT_SubjectsScore_R] '{0}','{1}'", projectCode, shopCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

//        public DataSet SearchShopByProjectCode(string projectCode)
//        {
//            string sql = string.Format(@"SELECT ShopCode,ShopName FROM Shop where ShopCode in(
//                            select ShopCode from Answer where ProjectCode = '{0}' group by ShopCode) ", projectCode);
//            DataSet ds = CommonHandler.query(sql);

//            return ds;
//        }
        public byte[] SearchAnswerDtl2Pic(string picName, string shopName, string subjectCode, string type, string code)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = "";
            if (type == "SpecialCase")
            {
                filePath = appDomainPath + @"UploadImage\" + @"SpecialCasePictures\" + code + @"\" + picName;
            }
            else if (type == "Notice")
            {
                filePath = appDomainPath + @"UploadImage\" + @"NoticeAttachment\" + code + @"\" + picName;
            }
            else
            {
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName + ".jpg"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + subjectCode + @"\" + picName + ".jpg";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".jpg";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".doc";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".docx";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xls";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".xlsx";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".ppt";
                }
                if (File.Exists(appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx"))
                {
                    filePath = appDomainPath + @"UploadImage\" + shopName + @"\" + picName + ".pptx";
                }
            }
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    byte[] b = new byte[fs.Length];
                    fs.Read(b, 0, b.Length);
                    fs.Close();
                    return b;
                }
            }
            else
            {
                return null;
            }

        }
        #endregion
        #endregion

        #region Chai.YunChun

        #region 公告PopUp
        //按照NoticeID查询公告

        public DataSet GetNoticeByNoticeID(string noticeID)
        {
            string sql = string.Format(@"EXEC [up_DSAT_Notice_R] @NoticeID = '{0}'", noticeID);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        //按照NoticeID查询公告附件

        public DataSet GetAllNoticeAttachment(string noticeID)
        {
            string sql = string.Format(@"EXEC [up_DSAT_NoticeAttachment_R] @NoticeID = '{0}'", noticeID);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        //保存公告并查询

        public DataSet SaveNoticeAndSearch(string noticeID, string noticeTitle, string noticeContent, string userID)
        {
            string sql = string.Format(@"EXEC [up_DSAT_Notice_S] @NoticeID = '{0}'
                                        ,@NoticeTitle = '{1}'
                                        ,@NoticeContent = '{2}'
                                        ,@UserID = '{3}'", noticeID, noticeTitle, noticeContent, userID);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        //保存公告附件

        public void InsertNoticeAttachment(string noticeID, string attachName, byte[] file)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadImagePath = appDomainPath + @"UploadImage\NoticeAttachment\" + noticeID + "\\";
            if (!Directory.Exists(uploadImagePath))
            {
                Directory.CreateDirectory(uploadImagePath);
            }
            if (file != null)
            {
                MemoryStream buf = new MemoryStream(file);

                FileStream fs = new FileStream(uploadImagePath + attachName, FileMode.OpenOrCreate);
                buf.WriteTo(fs);
                buf.Close();
                fs.Close();
                buf = null;
                fs = null;
            }

            string sql = string.Format(@"EXEC [up_DSAT_NoticeAttachment_S] @NoticeID = '{0}'
                                        ,@AttachName = '{1}'", noticeID, attachName);
            CommonHandler.query(sql);
        }
        //删除公告附件

        public void DeleteNoticeAttachment(string noticeID, string seqNO, string attachName)
        {

            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadImagePath = appDomainPath + @"UploadImage\NoticeAttachment\";

            if (File.Exists(uploadImagePath + noticeID + "\\" + attachName))
            {
                try
                {
                    File.Delete(uploadImagePath + noticeID + "\\" + attachName);
                }
                catch
                {

                }
            }

            string sql = string.Format(@"EXEC [up_DSAT_NoticeAttachment_D] @NoticeID = '{0}'
                                        ,@SeqNO = '{1}'", noticeID, seqNO);
            CommonHandler.query(sql);


        }

        //下载公告的附件，用流

        public byte[] DownNoticeAttachment(string noticeID, string attachName)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = appDomainPath + @"UploadImage\NoticeAttachment\" + noticeID + "\\" + attachName;
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);

                byte[] b = new byte[fs.Length];
                fs.Read(b, 0, b.Length);
                fs.Close();
                return b;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 公告查询

        public DataSet GetAllNotice(DateTime startDate, DateTime endDate)
        {
            string sql = string.Format(@"EXEC [up_DSAT_NoticeSelectAll_R] 
                                            @StartDate = '{0}'
                                            ,@EndDate = '{1}'", startDate, endDate);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        public void DeleteNotice(string noticeID)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadImagePath = appDomainPath + @"UploadImage\NoticeAttachment\";

            if (Directory.Exists(uploadImagePath + noticeID))
            {
                try
                {
                    Directory.Delete(uploadImagePath + noticeID, true);
                }
                catch
                { }
            }

            string sql = string.Format(@"EXEC [up_DSAT_Notice_D]
                                            @NoticeID = '{0}'", noticeID);
            CommonHandler.query(sql);

        }
        #endregion

        #region 特殊案例登记
        //登记，确认特殊案例

        public DataSet InsertSpecialCase(string specialCaseCode, string projectCode, string shopCode
            , string subjectCode, string title, string applyDesc, string finalAdvice
            , string RegType, string userID, string imageName, bool needVICoConfirmChk, string vICoAdvice)
        {
            string sql = string.Format(@"EXEC [dbo].[up_DSAT_SpecialCase_CU]
		@SpecialCaseCode = '{0}',
		@ProjectCode = '{1}',
		@ShopCode = '{2}',
		@SubjectCode = '{3}',
		@Title = '{4}',
		@ApplyDesc = '{5}',
		@FinalAdvice = '{6}',
		@RegType = '{7}',
		@UserID = '{8}',
        @ImageName = '{9}',
        @NeedVICoConfirmChk = {10},
        @VICoAdvice = '{11}'", specialCaseCode, projectCode, shopCode, subjectCode, title, applyDesc, finalAdvice, RegType, userID, imageName, needVICoConfirmChk ? 1 : 0, vICoAdvice);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        //保存特殊安全的图片信息

        public void InsertSpecialCasePic(string specialCaseCode, string picName, byte[] pic)
        {
            string appDomainPath = AppDomain.CurrentDomain.BaseDirectory;
            string uploadImagePath = appDomainPath + @"UploadImage\SpecialCasePictures\" + specialCaseCode + "\\";
            if (!Directory.Exists(uploadImagePath))
            {
                Directory.CreateDirectory(uploadImagePath);
            }
            if (pic != null)
            {
                MemoryStream buf = new MemoryStream(pic);

                Image picimage = Image.FromStream(buf, true);
                picimage.Save(uploadImagePath + picName);
            }

            string sql = string.Format(@"EXEC [up_DSAT_SpecialCasePic_U] @SpecialCaseCode = '{0}'
                                        ,@ImageName = '{1}'", specialCaseCode, picName);
            CommonHandler.query(sql);
        }


        public DataSet GetSpecialCase(string specialCaseCode)
        {
            string sql = string.Format(@"EXEC [dbo].[up_DSAT_SpecialCase_R]
		@SpecialCaseCode = '{0}'", specialCaseCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        public DataSet GetSpecialCaseSubject(string projectCode, string subjectCode)
        {
            string sql = string.Format(@"EXEC [dbo].[up_DSAT_GetSpecialCaseSubject_R]
		@ProjectCode = '{0}',
		@SubjectCode = '{1}'", projectCode, subjectCode);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }

        #endregion

        #region 特殊案例查询


        public DataSet GetAllSpecialCase(string projectCode, string shopCode, string subjectCode, DateTime startDate, DateTime endDate)
        {
            string sql = string.Format(@"EXEC [up_DSAT_SpecialCaseSearchAll_R]
		@ProjectCode = '{0}',
		@ShopCode = '{1}',
		@SubjectCode = '{2}',
        @StartDate = '{3}',
        @EndDate = '{4}'", projectCode, shopCode, subjectCode, startDate, endDate);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        public void DeleteSpecialCase(string specialCaseCode)
        {

            string sql = string.Format(@"[up_DSAT_SpecialCase_D] @SpecialCaseCode = '{0}'", specialCaseCode);
            CommonHandler.query(sql);
        }
        #endregion

        #region 执行组修改
        //查询一审结果及修改前后的分数

        public DataSet GetAllExecuteTeamAlter(string projectCode, string reCheckTypeCode, string shopCode, string subjectCode, DateTime startDate, DateTime endDate)
        {
            string sql = string.Format(@"EXEC [up_DSAT_ExecuteTeamAlterSearchAll_R]
          @ProjectCode = '{0}',
                
          @ShopCode = '{1}',
          @SubjectCode = '{2}',
        @StartDate = '{3}',
        @EndDate = '{4}',
        @ReCheckTypeCode = '{5}' ", projectCode, shopCode, subjectCode, startDate, endDate, reCheckTypeCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        //保存修改后的分数

        public void SaveExecuteTeamAlter(string projectCode, string shopCode, string subjectCode, string reCheckTypeCode, bool AgreeCheck, string AgreeReason, decimal? newScore, string userID)
        {
            string sql = string.Format(@"EXEC [up_DSAT_ExecuteTeamAlterSave_CU]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@SubjectCode= '{2}'
                                    ,@ReCheckTypeCode= '{3}'
                                    ,@AgreeCheck = {4}
                                    ,@AgreeReason = '{5}'
                                    ,@NewScore = '{6}'
                                    ,@UserID = '{7}'", projectCode, shopCode, subjectCode, reCheckTypeCode, AgreeCheck ? 1 : 0, AgreeReason, newScore, userID);
            DataSet ds = CommonHandler.query(sql);

        }
        //修改状态为一审修改完毕

        public void SaveReCheckStatus(string projectCode, string shopCode, string statusCode, string userID)
        {
            string sql = string.Format(@"EXEC [up_DSAT_ReCheckStatus_S]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@StatusCode = '{2}'
                                    ,@UserID = '{3}'", projectCode, shopCode, statusCode, userID);
            DataSet ds = CommonHandler.query(sql);
        }

        public DataSet SearchExecuteTeamUnAgreeCount(string projectCode, string shopCode, string reCheckTypeCode)
        {
            string sql = string.Format(@"EXEC [up_DSAT_ExecuteTeamUnAgreeCount_R]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@ReCheckTypeCode = '{2}'", projectCode, shopCode, reCheckTypeCode);
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }
        #endregion

        #region 仲裁组修改
        //仲裁组查询

        public DataSet GetAllArbitrationTeamAlter(string projectCode, string shopCode, string subjectCode, DateTime startDate, DateTime endDate)
        {
            string sql = string.Format(@"EXEC [up_DSAT_ArbitrationTeamSearchAll_R]
		@ProjectCode = '{0}',
		@ShopCode = '{1}',
		@SubjectCode = '{2}',
        @StartDate = '{3}',
        @EndDate = '{4}'", projectCode, shopCode, subjectCode, startDate, endDate);
            DataSet ds = CommonHandler.query(sql);

            return ds;
        }
        //仲裁组修改

        public void SaveArbitrationTeamAlter(string projectCode, string shopCode, string subjectCode, string reCheckTypeCode, string lastConfirm, string confirmReason, string userID)
        {
            string sql = string.Format(@"EXEC [up_DSAT_ArbitrationTeamSave_U]
                                     @ProjectCode = '{0}'
                                    ,@ShopCode = '{1}'
                                    ,@SubjectCode= '{2}'
                                    ,@ReCheckTypeCode= '{3}'
                                    ,@LastConfirm = '{4}'
                                    ,@ConfirmReason = '{5}'
                                    ,@UserID = '{6}'", projectCode, shopCode, subjectCode, reCheckTypeCode, lastConfirm, confirmReason, userID);
            DataSet ds = CommonHandler.query(sql);
        }


        #endregion

        #endregion

        #region liu.yang

        #region Subjects
        public DataSet GetSubjectTypeForCbo()
        {
            string sql = string.Format("SELECT Code AS SubjectTypeCode,CNDesc AS SubjectTypeName FROM HiddenCode WHERE GroupCode = 'SubjectType'");
            DataSet ds = CommonHandler.query(sql);
            return ds;
        }

        #endregion


        #endregion


        #region 本地生成单店报告使用
        public byte[] SearchAnswerDtl2Pic(string picName, string shopName, string subjectCode, string filePath)
        {
            string appDomainPath = filePath;
            if (File.Exists(appDomainPath + shopName + @"\" + subjectCode + @"\" + picName + ".jpg"))
            {
                filePath = appDomainPath + shopName + @"\" + subjectCode + @"\" + picName + ".jpg";
            }
            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    byte[] b = new byte[fs.Length];
                    fs.Read(b, 0, b.Length);
                    fs.Close();
                    return b;
                }
            }
            else
            {
                return null;
            }

        }

        public List<string> SearchShopByProjectCode(string projectCode)
        {
            List<String> dataList = SqliteHelper.Search(@"SELECT ShopCode,ShopName FROM Shop");

            return dataList;
        }
        /// <summary>
        /// 查询经销商发票的数量和不合格数量
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public List<String> Report_SearchShopInvoice(string projectCode, string shopCode, string type)
        {
            List<String> dataList = new List<string>();
//            if (type == "A")
//            {
//                dataList = SqliteHelper.Search(@"SELECT IFNULL(SUM(CASE WHEN A.Score <> 9999 THEN 1 END),0) AS SumCount, IFNULL(SUM(CASE WHEN A.Score = 0 THEN 1 END),0) AS NotCount
// FROM  Answer A 
//WHERE ProjectCode = '" + projectCode + "' AND ShopCode = '" + shopCode + "'AND A.SubjectCode LIKE 'A%'");
//            }
//            else
//            {
//                dataList = SqliteHelper.Search(@"SELECT IFNULL(SUM(CASE WHEN A.Score <> 9999 THEN 1 END),0) AS SumCount, IFNULL(SUM(CASE WHEN A.Score = 0 THEN 1 END),0) AS NotCount
// FROM  Answer A 
//WHERE ProjectCode = '" + projectCode + "' AND ShopCode = '" + shopCode + "'AND A.SubjectCode LIKE 'B%'");

//            }
            return dataList;
        }
        /// <summary>
        /// 查询经销商的基本信息
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public List<String> Report_SearchShopInfoByShopCode(string projectCode, string shopCode)
        {
            List<String> dataList = SqliteHelper.Search(@"SELECT  A.ShopCode,A.ShopName,
IFNULL((SELECT StartDate FROM Answerstartinfo where projectCode = '" + projectCode + "' AND shopcode = a.shopcode),'1900-01-01 00:00:00') AS StartDate FROM Shop A  WHERE  A.ShopCode = '" + shopCode + "'");
            return dataList;
        }
        /// <summary>
        /// 每个类型错误数量
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="ShopCode"></param>
        /// <returns></returns>
        public List<string> Report_SearchPerTypeFailCount(string projectCode, string shopCode)
        {
            List<String> dataList = new List<string>();
//            List<String> dataList = SqliteHelper.Search(@"SELECT 
//IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a1' AND A.CheckOptionCode='02' THEN 1 END),0) AS A1Count,
//IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a2' AND A.CheckOptionCode='02' THEN 1 END),0) AS A2Count,
//IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a3' AND A.CheckOptionCode='02' THEN 1 END),0) AS A3Count,
//IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'a4' AND A.CheckOptionCode='02'  THEN 1 END),0) AS A4Count,
//IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'b1' AND A.CheckOptionCode='02' THEN 1 END) ,0)AS B1Count,
//IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'b2' AND A.CheckOptionCode='02' THEN 1 END),0) AS B2Count,
//IFNULL(SUM(CASE WHEN B.[InspectionStandardName] LIKE 'b3' AND A.CheckOptionCode='02' THEN 1 END),0) AS B3Count
// FROM AnswerDtl A INNER JOIN 
//InspectionStandard B ON A.ProjectCode = B.ProjectCode AND A.SubjectCode = B.SubjectCode AND A.SeqNO =B.SeqNO
//WHERE  A.ProjectCode='" + projectCode + "' AND A.ShopCode = '" + shopCode + "'");

            return dataList;
        }
        /// <summary>
        /// 每个发票的信息
        /// </summary>
        /// <param name="projectCode"></param>
        /// <param name="ShopCode"></param>
        /// <returns></returns>
        public List<string> Report_PerInvoiceInfo(string projectCode, string shopCode, string type)
        {
            List<String> dataList = new List<string>();
            if (type == "A")
            {
                dataList = SqliteHelper.Search(@"SELECT B.SubjectCode,C.SellInvoiceCode,
CASE WHEN LossDesc LIKE '%a1%' THEN 'Y' END AS A1,
CASE WHEN LossDesc LIKE '%a2%' THEN 'Y' END AS A2,
CASE WHEN LossDesc LIKE '%a3%' THEN 'Y' END AS A3,
CASE WHEN LossDesc LIKE '%a4%' THEN 'Y' END AS A4 ,
B.Remark,B.SpCode,LossDesc,PicName
FROM AnswerDtl3 A INNER JOIN Answer B ON A.ProjectCode = B.ProjectCode
AND A.ShopCode = B.ShopCode AND A.SubjectCode = B.SubjectCode
INNER JOIN AnswerStartInfo C ON B.ProjectCode = C.ProjectCode AND B.ShopCode = C.ShopCode
 WHERE A.ProjectCode='" + projectCode + "' AND A.ShopCode = '" + shopCode + "'AND B.SubjectCode LIKE 'A%'");
            }
            else
            {
                dataList = SqliteHelper.Search(@"SELECT B.SubjectCode,C.AfterInvoiceCode,
CASE WHEN LossDesc LIKE '%b1%' THEN 'Y' END AS B1,
CASE WHEN LossDesc LIKE '%b2%' THEN 'Y' END AS B2,
CASE WHEN LossDesc LIKE '%b3%' THEN 'Y' END AS B3,
B.Remark,B.SpCode,LossDesc,PicName
FROM AnswerDtl3 A INNER JOIN Answer B ON A.ProjectCode = B.ProjectCode
AND A.ShopCode = B.ShopCode AND A.SubjectCode = B.SubjectCode
INNER JOIN AnswerStartInfo C ON B.ProjectCode = C.ProjectCode AND B.ShopCode = C.ShopCode
 WHERE A.ProjectCode='" + projectCode + "' AND A.ShopCode = '" + shopCode + "'AND B.SubjectCode LIKE 'B%'");
            }

            return dataList;
        }
        #endregion

    }
}
