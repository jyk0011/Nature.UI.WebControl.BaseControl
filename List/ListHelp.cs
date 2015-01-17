/**
 * 自然框架之基础控件
 * http://www.natureFW.com/
 *
 * @author
 * 金洋（金色海洋jyk）
 * 
 * @copyright
 * Copyright (C) 2005-2013 金洋.
 *
 * Licensed under a GNU Lesser General Public License.
 * http://creativecommons.org/licenses/LGPL/2.1/
 *
 * 自然框架之基础控件 is free software. You are allowed to download, modify and distribute 
 * the source code in accordance with LGPL 2.1 license, however if you want to use 
 * 自然框架之基础控件 on your site or include it in your commercial software, you must  be registered.
 * http://www.natureFW.com/registered
 */

/* ***********************************************
 * author :  金洋（金色海洋jyk）
 * email  :  jyk0011@live.cn  
 * function: 设置下拉列表框的选项
 * history:  created by 金洋 
 *           2011-4-11 整理
 * **********************************************
 */

using System.Web.UI.WebControls;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Enum;

namespace Nature.UI.WebControl.BaseControl.List
{
    /// <summary>
    /// 列表框自我描绘的一个帮助
    /// 可以设置Item和宽度
    /// </summary>
    public class ListHelp
    {
        /// <summary>
        /// 设置下拉列表框的选项
        /// </summary>
        /// <param name="list">列表框控件</param>
        /// <param name="fillItem">填充Item的一个接口</param>
        /// <param name="info">列表框的特殊属性信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        public static void SetList(ListControl list,IFillItemHelp fillItem,  BaseListExpand info, IDal  dal)
        {
            switch (info.FillItemType)
            {
                case FillItemType.Customer :     //自定义的形式
                    list.Items.Clear();
                    fillItem.ItemAddByString(info.Item);
                    break;

                case FillItemType.SQL :     //sql语句，从数据库提取
                    fillItem.BindListBySql(info.Sql, dal);
                    break;

                case FillItemType.Listself :     //列表自带的
                    break;
 

            }

            if (info.Width != 0)
                list.Width = Unit.Pixel(info.Width);

        }
    }
}
