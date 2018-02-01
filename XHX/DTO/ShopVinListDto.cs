using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XHX.DTO
{
    public class ShopVinListDto
    {
        public string ProjectCode { get; set; }
        public string VinCode { get; set; }
        public string ShopCode { get; set; }
        public string AreaCode { get; set; }
        public string Vincode8{ get; set; }
        public string ModelName { get; set; }
        public string SubModelName { get; set; }
        public string StockAge { get; set; }
        public char StatusType { get; set; }
        public string SaleFlag { get; set; }
        public string GradeNameCn { get; set; }
        public string ExteriorColor { get; set; }
        public string InteriorColor { get; set; }
        public string PhotoName { get; set; }
        public string Remark { get; set; }
        public string AddChk { get; set; }
        public string InUserId { get; set; }
        public string InDateTime { get; set; }
    }
}
