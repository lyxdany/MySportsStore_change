using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using System.Web.Mvc;


namespace MySportsStore.Common
{

    public class BaseExcelResult : NPOIBase
    {

        string[] __headers = null;

        DataTable __mytable;
        int __postfix ;
        string __filename;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaseMaterialsList">类型为Datatable</param>
        /// <param name="headers">表头</param>
        /// <param name="filename"></param>
        /// <param name="postfix"></param>
        public BaseExcelResult(DataTable BaseMaterialsList, List<string> headers,string filename, int postfix)
        {
            __mytable = BaseMaterialsList;
            __headers = headers.ToArray();
            __filename = filename;
            __postfix = postfix;
        }
        
        public override void ExecuteResult(ControllerContext context)
        {
            if (__mytable == null || __mytable.Rows.Count == 0) return;
            IniNPOI(__postfix);

            for (int rowIndex = 0; rowIndex < __mytable.Rows.Count; rowIndex++)
            {
                int colIndex = 0;
                if (rowIndex == 65535 && __postfix == 0 || rowIndex == 0 || rowIndex == 1000000 && __postfix == 1)
                {
                    if (rowIndex != 0)
                        _sheet = _workbook.CreateSheet();
                    

                    //表头列为空表示不需要表头
                    if (__headers != null)
                    {
                        IRow headerRow = _sheet.CreateRow(rowIndex);
                        foreach (var head in __headers)
                            FillHeadCell(headerRow, colIndex++, head);
                        rowIndex = 1;
                    }
                }


                //导入剩下的行
                IRow dataRow = _sheet.CreateRow(rowIndex);
                colIndex = 0;
                for (int i = 0; i < __mytable.Columns.Count;i++ )
                {
                    FillCell(dataRow, colIndex, __mytable.Rows[rowIndex][colIndex++].ToString());
                }

            }

            _sheet.CreateFreezePane(1, 1, 1, 1);
            ResponseOutPutExcelStream(__filename, __postfix);
        }

    }


}
