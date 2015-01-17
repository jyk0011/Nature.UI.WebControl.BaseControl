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
 * function: 继承.net 里的TextBox，加几个属性和接口
 * history:  created by 金洋 
 *           2011-4-11 整理
 * **********************************************
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using Nature.Data;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.TextBox
{
    #region 枚举，定义常用的验证方式
    /// <summary>
    /// 几个常用的验证方式
    /// </summary>
    public enum CheckType
    {
        /// <summary>
        /// 不验证
        /// </summary>
        NotCheck = 101,

        /// <summary>
        /// 自然数
        /// </summary>
        NaturalNumber = 102,

        /// <summary>
        /// 整数
        /// </summary>
        Integer = 103,

        /// <summary>
        /// 小数
        /// </summary>
        Decimal = 104,

        /// <summary>
        /// 日期
        /// </summary>
        DateTime = 105,

        /// <summary>
        /// 必填
        /// </summary>
        Required = 106
    }
    #endregion

    /// <summary>
    /// 继承.net 里的TextBox，加几个属性和接口
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<Nature:BaseTextBox runat=server></Nature:BaseTextBox>")]
    public class BaseTextBox : System.Web.UI.WebControls.TextBox, IControlHelp
    {
        private CheckType _dataType = CheckType.NotCheck ;       //验证类型

        #region 扩充属性
        #region TextTrim
        /// <summary>
        /// 获取Text.Trim()
        /// </summary>
        [Bindable(true)]
        [Category("文本值")]
        [DefaultValue("")]
        [Description("设置文本框的 Text 属性；返回 Text.Trim()。")]
        [Localizable(true)]
        public string TextTrim
        {
            get { return Text.Trim(); }
            set { Text = value; }
        }
        #endregion

        #region TextTrimNone
        /// <summary>
        /// 获取Text.Trim().Replace("'", "''")，避免注入攻击
        /// </summary>
        [Bindable(true)]
        [Category("文本值")]
        [Description("设置文本框的 Text 属性；返回 Text.Trim().Replace(\"'\", \"''\")。")]
        public string TextTrimNone
        {
            get { return Text.Trim().Replace("'", "''"); }
            set { Text = value; }
        }
        #endregion

        #region 前台验证
        /// <summary>
        /// 验证用的正则表达式
        /// </summary>
        [Category("数据验证"), Description("验证输入的内容，传入正则表达式。"), DefaultValue("")]
        public string CheckDataReg
        {
            get
            {
                if (Attributes["check"] == null)
                {
                    return "";
                }
                return Attributes["check"].ToString(CultureInfo.InvariantCulture);
            }
            set
            {
                Attributes.Add("check", value);
            }
        }

        /// <summary>
        /// 未通过验证的时候的提示信息
        /// </summary>
        [Category("数据验证"), Description("未通过验证的时候的提示信息")]
        public string CheckErrorMessage
        {
            get
            {
                if (ViewState["msg"] == null)
                {
                    return "";
                }
                return ViewState["msg"].ToString();
            }
            set
            {
                ViewState["msg"] = value;
                Attributes.Add("warning", value);
            }
        }
 

        /// <summary>
        /// 验证类型，通过设置验证类型，转换成正则表达式
        /// </summary>
        [Category("数据验证"), DefaultValue("101"), Description("验证输入的内容，需要js脚本配合。")]
        public CheckType CheckDataType
        {
            get
            {
                return _dataType;
            }
            set
            {
                _dataType = value;
                #region 设置正则表达式
                switch (_dataType)
                {
                    case CheckType.NotCheck :
                        Attributes.Remove("check");
                        break;

                    case CheckType.NaturalNumber :      // 102: //自然数
                        Attributes.Add("check", "^[0-9]+$");
                        break;

                    case CheckType.Integer :            // 103: //整数
                        Attributes.Add("check", @"^\S?([0-9]+)$");
                        break;

                    case CheckType.Decimal :            // 104: //小数
                        Attributes.Add("check", @"^\S(\-?[0-9]*(\.[0-9]*)?)$");
                        break;

                    case CheckType.DateTime :           // 105: //日期
                        Attributes.Add("check", "^d{4}-d{1,2}-d{1,2}");
                        break;

                    case CheckType.Required :           // 106: //必填项
                        Attributes.Add("check", ".+");
                        break;
                }
                #endregion
            }
        }

        #endregion
        
        #endregion

        /// <summary>
        /// 设置文本框的css属性
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CssClass = "cssTxt inputT01";

        }

        #region 实现接口
        #region ControlValue
        /// <summary>
        /// 实现接口，设置Text属性，返回Text.Trim()
        /// </summary>
        [Bindable(true)]
        [Category("文本值")]
        [Description("设置文本框的 Text 属性；返回 Text.Trim()。")]
        public virtual string ControlValue
        {
            get { return TextTrimNone; }
            set { Text = value; }
        }
        #endregion

        #region GetControlValue
        /// <summary>
        /// 返回Text.Trim().Replace("'", "")
        /// </summary>
        /// <param name="kind">取值的方式</param>
        /// <returns></returns>
        public string GetControlValue(string kind)
        {
            switch (kind)
            {
                case "1":   //SQL语句
                    return TextTrimNone;
                case "2":   //参数
                    return TextTrim;
                default :   //查询
                    return TextTrim.Replace("'","");
            
            }
        }
        #endregion

        #region SetControlValue
        /// <summary>
        /// 给Text赋值
        /// </summary>
        /// <param name="value"></param>
        /// <param name="kind"></param>
        public void SetControlValue(string value, string kind)
        {
            Text = value;
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口，返回控件类别
        /// </summary>
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("201")]
        public virtual string ControlKind
        {
            get { return "201"; }
        }
        #endregion

        #region SetControlKind
        /// <summary>
        /// 设置控件的状态
        /// 1：正常
        /// 2：只读
        /// 3：不可用
        /// </summary>
        /// <param name="kind">1：正常；2：只读；3：不可用</param>
        public virtual void SetControlState(string kind)
        {
            switch (kind)
            {
                case "1":   //正常
                    ReadOnly = false ;
                    Enabled = true;
                    break;
                case "2":   //只读
                    ReadOnly = true;
                    break;
                case "3":   //不可用
                    Enabled = false;
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
        public virtual void ShowMe(IColumn formColumnMeta, IDal  dal, bool isForm)
        {
            //base.Page.Response.Write(info + "<BR>");
            //System.Web.HttpContext.Current.Response.Write(info.ControlInfo  + "<BR>");

            if (formColumnMeta is ModColumnMeta)
            {
                #region 设置前台的验证

                //设置验证类型
                CheckDataType = (CheckType)(((ModColumnMeta)formColumnMeta).ControlCheckKind);

                //设置未通过验证的提示信息
                CheckErrorMessage = ((ModColumnMeta)formColumnMeta).CheckTip;

                if (((ModColumnMeta)formColumnMeta).CustomerCheckKind.Length > 0)
                {
                    //设置了自定义的验证方式
                    CheckDataReg = ((ModColumnMeta)formColumnMeta).CustomerCheckKind;
                }
                else
                {
                    //使用提供的验证方式
                    CheckDataType = (CheckType)((FormColumnMeta)formColumnMeta).ControlKind;
                }
                #endregion
            }

            #region 设置只读、是否可用
            switch (((FormColumnMeta)formColumnMeta).ControlState)
            {
                case "2":    //只读
                    ReadOnly = true;
                    break;

                case "3":   //不可用
                    Enabled = false;
                    break;

            }
            #endregion
        }
        
        #endregion


    }
}
