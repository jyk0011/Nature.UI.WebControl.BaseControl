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
 * function: 多级联动下拉列表框
 * history:  created by 金洋  2009-7-22 10:08:02 
 *           2011-4-11 整理
 * **********************************************
 */


using System;
using System.ComponentModel;
using System.Web.UI;
using System.Data;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.List
{
    /// <summary>
    /// 多级联动下拉列表框
    /// </summary>
    public partial class MyUniteList 
    {
        ///// 第一个下拉列表框的查询语句的说明
        ///// 格式：select col1  , col2 from table
        /////       
        ///// 说明：col1 填充Value， Col2填充 Text 。
        ///// 
        ///// 其他下拉列表框的查询语句的说明。
        ///// 有多少写多少，不用分割（加空格就行了:)）。
        ///// 格式：select pid,id,txt from table
        /////       pid 和前一个列表框相关联的字段，类型不限。
        /////       id  对应列表框的 value
        /////       txt 对应列表框的 text
        /////       多个查询语句用空格分开 ，写在后面就行了。格式是一样的。
        /////       
        ///// 要求：字段的顺序要和示例里的一致，否则会出错。
       
        #region 属性

        #region 联动的级数 —— ListCount
        /// <summary>
        /// 几级联动。最小是2；最大——理论上是不限的。
        /// </summary>
        [Bindable(true)]
        [Category("list")]
        [Localizable(true)]
        [Description("联动的级数，最小是2")]
        public Int32 ListCount
        {
            set
            {
                ViewState["ListCount"] = value;
            }
            get
            {
                if (ViewState["ListCount"] == null)
                    return 2;
                return (Int32)ViewState["ListCount"];
            }
        }
        #endregion

        #region 分隔列表框的用的html代码 —— ListHTML
        /// <summary>
        /// 分隔列表框的用的html代码 —— ListHTML
        /// </summary>
        [Bindable(true)]
        [Category("list")]
        [Localizable(true)]
        [Description("分隔代码。字符串数组，数量要和级数一致，等于级数减一")]
        public string[] ListHtml
        {
            set
            {
                ViewState["LevelHTML"] = value;
            }
            get
            {
                if (ViewState["LevelHTML"] == null)
                    return new[] { "", "", "", "" };
                return (String[])ViewState["LevelHTML"];
            }
        }
        #endregion

        #region 获取、设置下拉列表的ID值 —— ListSelectedValue
        /// <summary>
        /// 获取、设置下拉列表的ID值。
        /// </summary>
        [Bindable(true)]
        [Category("list")]
        [Localizable(true)]
        [Description("获取、设置下拉列表的ID值。")]
        public string SelectedValue
        {
            set
            {
                _txtItemValue.TextTrimNone = value;
                SetSelectedValue(value);
            }
            get
            {
                return _txtItemValue.TextTrimNone;  
            }
        }
        #endregion

        #region 获取下拉列表的Text值 —— ListSelectedText
        /// <summary>
        /// 获取下拉列表的Text值
        /// </summary>
        [Category("list")]
        [Description("获取下拉列表的Text值。")]
        public string SelectedText
        {
            get { return _txtItemText.TextTrimNone; }
        }
        #endregion

        #region 获取、设置下拉列表的记录集。
        /// <summary>
        /// 获取、设置下拉列表的记录集。
        /// </summary>
        [Category("list")]
        [Description("获取、设置下拉列表的记录集。只能是DataSet类型")]
        public DataSet DataSource
        {
            get
            {
                return _dsList;
            }
            set
            {
                _dsList = value;
                //根据DataSet里的DataTable的数量设置下拉列表框的个数
                ListCount = value.Tables.Count;
            }

        }
        #endregion

        #endregion

        #region 实现接口
        #region ControlValue
        /// <summary>
        /// 实现接口，获取、设置下拉列表框的value
        /// </summary>
        [Category("list")]
        [Description("获取、设置下拉列表框的value")]
        public string ControlValue
        {
            get { return SelectedValue; }
            set { SelectedValue = value; }
        }
        #endregion

        #region GetControlValue
        /// <summary>
        /// 获取下拉列表框的value
        /// </summary>
        /// <param name="kind">取值的方式</param>
        /// <returns></returns>
        public string GetControlValue(string kind)
        {
            return _txtItemValue.TextTrimNone;  
        }
        #endregion

        #region SetControlValue
        /// <summary>
        /// 设置下拉列表框的选项
        /// </summary>
        /// <param name="value"></param>
        /// <param name="kind"></param>
        public void SetControlValue(string value, string kind)
        {
            SelectedValue = value;
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口
        /// </summary>
        [Category("list"),Description("获取控件类别"), DefaultValue("206")]
        public string ControlKind
        {
            get { return "206"; }
        }
        #endregion

        #region SetControlKind
        /// <summary>
        /// 设置控件的状态，复选组2、3都表示不可用
        /// </summary>
        /// <param name="kind">1：正常；2：不可用；3：不可用</param>
        public virtual void SetControlState(string kind)
        {
            switch (kind)
            {
                case "1":   //正常
                    //Enabled = true;
                    break;

                case "2":   //只读
                case "3":   //不可用
                    //Enabled = false;
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 自我描述
        /// </summary>
        /// <param name="formColumnMeta">配置信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <param name="isForm">True：表单控件；False：查询控件</param>
        public virtual void ShowMe(IColumn formColumnMeta, IDal dal, bool isForm)
        {
            var info = (UniteListExtend)((FormColumnMeta)formColumnMeta).ControlExtend;
            ListCount = info.ListCount;
            ListHtml = info.ListHtml;

            DataSource = dal.ExecuteFillDataSet(info.Sql);
            DataBind();

            //base.Page.Response.Write(info + "<BR>");
            //HttpContext.Current.Response.Write(info.ControlInfo  + "<BR>");
            

            //#region 设置前台的验证

            ////设置验证类型
            //CheckDataType = ColInfo.ControlCheckKind;

            ////设置未通过验证的提示信息
            //if (infos.Length > 0)
            //{
            //    CheckErrorMessage = infos[0];
            //}

            ////直接设置正则表达式
            //if (infos.Length >= 6)
            //{
            //    CheckDataReg = infos[5];
            //}
            //#endregion

            //#region 设置只读、是否可用
            //switch (ColInfo.ControlState)
            //{
            //    case "2":    //只读
            //        ReadOnly = true;
            //        break;

            //    case "3":   //不可用
            //        Enabled = true;
            //        break;

            //}
            //#endregion
        }

        #endregion

        #region 设计时支持
        /// <summary>
        /// 设计时支持
        /// </summary>
        /// <param name="output"></param>
        protected override void Render(HtmlTextWriter output)
        {
            if ((Site != null) && Site.DesignMode)
            {
                output.Write(" <select ><option value='1'>联动下拉列表框</option></select><select ><option value='1'>子列表框</option></select> ");
            }
            else
            {
                base.Render(output);
            }
        }
        #endregion

    }
}
