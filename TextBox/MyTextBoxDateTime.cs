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
 * function: 继承BaseTextBox，和My97DatePicker结合，实现日期、时间的选择
 * history:  created by 金洋 2009-7-15 10:35:45
 *           2011-4-11 整理
 * **********************************************
 */

using System;
using System.ComponentModel;
using System.Web.UI;
using Nature.Common;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.TextBox
{
    /// <summary>
    /// 继承BaseTextBox，和My97DatePicker结合，实现日期、时间的选择
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<Nature:MyTextBoxDateTime runat=server></Nature:MyTextBoxDateTime>")]
    public  class MyTextBoxDateTime : BaseTextBox
    {
        /// <summary>
        /// 实现接口，返回控件类别
        /// </summary>
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("204")]
        public override string ControlKind
        {
            get
            {
                return "204";
            }
        }

        /// <summary>
        /// 控件值
        /// </summary>
        public override string ControlValue
        {
            get
            {
                return base.ControlValue;
            }
            set
            {
                if (Functions.IsDateTime(value))
                    Text = Convert.ToDateTime(value).ToString("yyyy-MM-dd");
            }
        }
        /// <summary>
        /// 显示选择日期的My97DatePicker
        /// </summary>
        /// <param name="formColumnMeta">配置信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <param name="isForm">True：表单控件；False：查询控件</param>
        public override void ShowMe(IColumn formColumnMeta, IDal dal, bool isForm)
        {
            base.ShowMe(formColumnMeta, dal, isForm);

            var info = (TextBoxTimeExtend)((FormColumnMeta)formColumnMeta).ControlExtend;

            #region 表单形式

            const string my97 = "WdatePicker({0})";
            Attributes.Add(info.EventName, string.Format(my97,info.Parameter));
            
            //判断选择哪种选取日期的方式
            //switch (info.DateKind)
            //{
            //    case "1":   //日期
            //        Attributes.Add("onClick", "WdatePicker()"); break;
            //    case "2":   //日期和时间（小时）
            //        Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH'})"); break;
            //    case "3":   //日期和时间（小时、分钟）
            //        Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm'})"); break;
            //    case "4":   //日期和时间（小时、分钟、秒）
            //        Attributes.Add("onClick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"); break;

            //    default:   //自定义
            //        Attributes.Add("onClick", info.DateKind);
            //        break;

            //}
            #endregion

        }

    }
}
