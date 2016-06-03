using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRUD.Utility
{
    public static class PagerHelper
    {
        #region 数字分页类
		//一共多少条数据 每页几条 一共查到了多少页 用户要查询的页码
        public static string strPage(int intCounts, int intPageSizes, int intPageCounts, int intThisPages, string strUrl)
        {
            int intCount = Convert.ToInt32(intCounts); //总记录数
            int intPageCount = Convert.ToInt32(intPageCounts); //总共页数
            int intPageSize = Convert.ToInt32(intPageSizes); //每页显示
            int intPage = 7;  //数字显示
            int intThisPage = Convert.ToInt32(intThisPages); //当前页数
            int intBeginPage = 0; //开始页数
            int intCrossPage = 0; //变换页数
            int intEndPage = 0; //结束页数
            string strPage = null; //返回值

            intCrossPage = intPage / 2;
                strPage = "共 <font color=\"#FF0000\">" + intCount.ToString() + "</font> 条记录 第 <font color=\"#FF0000\">" + intThisPage.ToString() + "/" + intPageCount.ToString() + "</font> 页 每页 <font color=\"#FF0000\">" + intPageSize.ToString() + "</font> 条 &nbsp;&nbsp;&nbsp;&nbsp;";
                if (intThisPage > 1)
                {
                    strPage = strPage + "<a href=\"" + strUrl + "1\">首页</a> ";
                    strPage = strPage + "<a href=\"" + strUrl + Convert.ToString(intThisPage - 1) + "\">上一页</a> ";
                }
                if (intPageCount > intPage)
                {
                    if (intThisPage > intPageCount - intCrossPage)
                    {
                        intBeginPage = intPageCount - intPage + 1;
                        intEndPage = intPageCount;
                    }
                else
                {
                    if (intThisPage <= intPage - intCrossPage)
                    {
                        intBeginPage = 1;
                        intEndPage = intPage;
                    }
                    else
                    {
                        intBeginPage = intThisPage - intCrossPage;
                        intEndPage = intThisPage + intCrossPage;
                    }
                }
            }
            else
            {
                intBeginPage = 1;
                intEndPage = intPageCount;
            }
            if (intCount > 0)
            {

                for (int i = intBeginPage; i <= intEndPage; i++)
                {
                    if (i == intThisPage)
                    {
                        strPage = strPage + " <font color=\"#FF0000\">" + i.ToString() + "</font> ";
                    }
                    else
                    {
                        strPage = strPage + " <a href=\"" + strUrl + i.ToString() + "\" title=\"第" + i.ToString() + "页\">" + i.ToString() + "</a> ";
                    }
                }
            }
            if (intThisPage < intPageCount)
            {
                strPage = strPage + "<a href=\"" + strUrl + Convert.ToString(intThisPage + 1) + "\">下一页</a> ";
                strPage = strPage + "<a href=\"" + strUrl + intPageCount.ToString() + "\">尾页</a> ";
            }
            return strPage;
        }
        #endregion
    }
}
