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
 * function: 集成.net 里的DropDownList，加几个属性和接口
 * history:  created by 金洋 
 *           2011-4-11 整理
 * **********************************************
 */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

//namespace System.Runtime.CompilerServices
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    [AttributeUsage(AttributeTargets.Method
//    | AttributeTargets.Class
//    | AttributeTargets.Assembly)]
//    public sealed class ExtensionAttribute : Attribute
//    {

//    }

//}

namespace Nature.UI.WebControl.BaseControl.List
{

    //static class CommonExtendedMethods
    //{
    //    internal static void Twist(this Control ctrl)
    //    {
    //        Console.Write(ctrl.ID + " is twisting.");
    //    }
    //}

    //static class CommonExtendedMethods2
    //{
    //    internal static void Twist(this WebControl ctrl)
    //    {
    //        Console.Write(ctrl.ID + " is twisting.");
    //    }
    //}

    /// <summary>
    /// 集成.net 里的DropDownList，加几个属性和接口
    /// </summary>
    [ToolboxData("<Nature:MyDropDownList runat=server></Nature:MyDropDownList>")]
    public class MyDropDownList : DropDownList, IControlHelp, IFillItemHelp
    {
        /// <summary>
        /// 
        /// </summary>
        private string _dataType;       //验证类型

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(CssClass))
                CssClass = "lst select01";

            if (string.IsNullOrEmpty(DataValueField))
                DataValueField = "ID";

            if (string.IsNullOrEmpty(DataTextField))
                DataTextField = "txt";

            Font.Size = FontUnit.Point(12);

        }

        #region 扩充方法

        #region 通过Value设置选项
        /// <summary>
        /// 通过Value设置选项
        /// </summary>
        /// <param name="itemValue"></param>
        public void SetSelectedByValue(string itemValue)
        {
            if (itemValue == "True")
                itemValue = "1";
            if (itemValue == "False")
                itemValue = "0";
        
            SelectedIndex = -1;
            foreach (ListItem item in Items)
            {
                if (item.Value.Equals(itemValue))
                {
                    item.Selected = true;
                    return;
                }
            }
        }
        #endregion

        #region 通过Text设置选项
        /// <summary>
        /// 通过Text设置选项
        /// </summary>
        /// <param name="itemText">Text</param>
        public void SetSelectedByText(string itemText)
        {
            SelectedIndex = -1;
            foreach (ListItem item in Items)
            {
                if (item.Text.Equals(itemText))
                {
                    item.Selected = true;
                    return;
                }
            }
        }
        #endregion

        #region 前台验证
        /// <summary>
        /// 验证用的正则表达式
        /// </summary>
        [Category("数据验证"), Description("验证输入的内容，传入正则表达式。"), DefaultValue("0")]
        public string CheckDataReg
        {
            get
            {
                if (Attributes["check"] == null)
                    return "";
                return Attributes["check"];
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
                    return "";
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
        public string CheckDataType
        {
            get
            {
                if (_dataType == null)
                    return "";
                return _dataType;
            }
            set
            {
                _dataType = value;
                #region 设置正则表达式
                switch (_dataType)
                {
                    case "106": //必选，且不能选第一个选项
                        Attributes.Add("check", "[1-9]{1}[0-9]*");
                        break;
                }
                #endregion
            }
        }

        #endregion


        #endregion

        #region 实现接口
        #region ControlValue
        /// <summary>
        /// 实现接口，SelectedValue
        /// </summary>
        [Bindable(true)]
        [Category("文本值")]
        [Description("通过SelectedValue设置下拉列表框的选项；返回 SelectedValue 属性 ")]
        public virtual string ControlValue
        {
            get { return SelectedValue ; }
            set { SetSelectedByValue(value); }
        }
        #endregion

        #region GetControlValue
        /// <summary>
        /// kind："1"：返回SelectedValue；"11"：返回SelectedItem.Text 
        /// </summary>
        /// <param name="kind">取值的方式。"1"：返回SelectedValue；"11"：返回SelectedItem.Text</param>
        /// <returns></returns>
        public string GetControlValue(string kind)
        {
            switch (kind)
            {
                default :       //返回SelectedValue
                    return SelectedValue;

                case "11":       //返回SelectedItem.Text
                    return SelectedItem.Text;
            }
        }
        #endregion

        #region SetControlValue
        /// <summary>
        /// 设置选项
        /// </summary>
        /// <param name="value"></param>
        /// <param name="kind">1：通过value设置选项；2：通过Text设置选项</param>
        public void SetControlValue(string value, string kind)
        {
            switch (kind)
            {
                default:       //设置SelectedValue
                    SetSelectedByValue(value);
                    break;

                case "2":       //设置SelectedItem.Text
                    SetSelectedByText(value);
                    break;
            }
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口
        /// </summary>
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("205")]
        public string ControlKind
        {
            get { return "205"; }
        }
        #endregion

        #region SetControlKind
        /// <summary>
        /// 设置控件的状态，下拉列表框2、3都表示不可用
        /// </summary>
        /// <param name="kind">1：正常；2：不可用；3：不可用</param>
        public virtual void SetControlState(string kind)
        {
            switch (kind)
            {
                case "1":   //正常
                    Enabled = true;
                    break;

                case "2":   //只读
                case "3":   //不可用
                    Enabled = false;
                    break;
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
        public virtual void ShowMe(IColumn formColumnMeta, IDal  dal, bool isForm)
        {
            var info = (DropDownListExpand)((FormColumnMeta)formColumnMeta).ControlExtend;
            ListHelp.SetList(this, this, info, dal);

            if (isForm)
            {
                //表单，判断是否添加“请选择”
                switch (info.IsShowChoose)
                {
                    case -1:    //不加请选择
                        break;
                    case -2:
                        Items.Insert(0, new ListItem("请选择", "-2"));
                        break;
                    default:
                        Items.Insert(0,new ListItem("默认",info.IsShowChoose.ToString(CultureInfo.InvariantCulture)));
                        break;
                }
            }
            else
            {
                Items.Insert(0,new ListItem("全部", "-99999"));
            }

            #region 设置只读、是否可用
            switch (((FormColumnMeta)formColumnMeta).ControlState)
            {
                case "2":    //只读
                case "3":   //不可用
                    Enabled = false;
                    break;

            }
            #endregion

        }
        #endregion
        
        #endregion

        #region 填充选项

        #region 从数据库绑定控件
        /// <summary>
        /// 从数据库绑定控件
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <returns></returns>
        public string BindListBySql(string sql, IDal dal)
        {
            DataSource = dal.ExecuteFillDataTable(sql);
            DataBind();
           
            return dal.ErrorMessage;
        }
        #endregion

        #region 字符串填充
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAndTexts"></param>
        public void ItemAddByString(string valuesAndTexts)
        {
            string[] strArray = valuesAndTexts.Split(new[] { '~' });
            int num2 = strArray.Length / 2;
            int num3 = num2 - 1;
            for (int i = 0; i <= num3; i++)
            {
                Items.Add(new ListItem(strArray[num2 + i], strArray[i]));
            }
        }
        #endregion

        #region 两个字符串填充
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="texts"></param>
        public void ItemAddByString(string values, string texts)
        {
            string[] strArray = texts.Split(new[] { '~' });
            string[] strArray2 = values.Split(new[] { '~' });
            int length = strArray.Length;
            for (int i = 1; i <= length; i++)
            {
                Items.Add(new ListItem(strArray[i], strArray2[i]));
            }
        }
        #endregion

        #region 两个数组填充
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="texts"></param>
        public void ItemAddByArray(string[] values, string[] texts)
        {
            int length = texts.Length;
            for (int i = 0; i <= length; i++)
            {
                Items.Add(new ListItem(texts[i], values[i]));
            }
        }
        #endregion

        #region 一个数组填充
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valuesAndTexts"></param>
        public void ItemAddByArray(string[] valuesAndTexts)
        {
            int num2 = (int)Math.Round(valuesAndTexts.Length / 2.0);
            int num3 = num2;
            for (int i = 0; i <= num3; i++)
            {
                Items.Add(new ListItem(valuesAndTexts[num2 + i], valuesAndTexts[i]));
            }
        }
        #endregion

        #region 填充月份
        /// <summary>
        /// 
        /// </summary>
        public void ItemAddMonth()
        {
            int num = 1;
            do
            {
                Items.Add(new ListItem(num.ToString(CultureInfo.InvariantCulture), num.ToString(CultureInfo.InvariantCulture)));
                num++;
            }
            while (num <= 12);
        }
        #endregion

        #region 填充日期
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lastDay"></param>
        public void ItemAddDate(int lastDay)
        {
            int num2 = lastDay;
            for (int i = 1; i <= num2; i++)
            {
                Items.Add(new ListItem(i.ToString(CultureInfo.InvariantCulture), i.ToString(CultureInfo.InvariantCulture)));
            }
        }
        #endregion

        #endregion
    }
}
