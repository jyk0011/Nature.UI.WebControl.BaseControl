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
 * function:  
 * history:  created by 金洋 2009-9-4 14:16:42 
 *           2011-4-11 整理
 * **********************************************
 */

using System.ComponentModel;
using System.Web.UI.WebControls;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.List
{
    /// <summary>
    /// 
    /// </summary>
    public class MyListPerson : MyDropDownList
    {
        #region ControlValue
        /// <summary>
        /// 实现接口，SelectedValue
        /// </summary>
        [Bindable(true)]
        [Category("文本值")]
        [Description("通过SelectedValue设置下拉列表框的选项；返回 SelectedValue 属性 ")]
        public override string ControlValue
        {
            get { return SelectedValue; }
            set
            {
                Items[0].Value = value;
            }
        }
        #endregion

        #region 自我描述
        /// <summary>
        /// 自我描述
        /// </summary>
        /// <param name="formColumnMeta">配置信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <param name="isForm">True：表单控件；False：查询控件</param>
        public override void ShowMe(IColumn formColumnMeta, IDal  dal, bool isForm)
        {
            var info = (BaseListExpand)((FormColumnMeta)formColumnMeta).ControlExtend;

            ItemAddByString("1~添加人");

            if (info.Width != 0)
                Width = Unit.Pixel(info.Width);


        }
        #endregion

    }
}
