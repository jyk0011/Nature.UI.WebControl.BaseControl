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
 * function: 继承CheckBoxList
 * history:  created by 金洋 
 *           2011-4-11 整理
 * **********************************************
 */

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.List
{
    /// <summary>
    /// CheckBoxList
    /// </summary>
    [ToolboxData("<Nature:MyCheckBoxList runat=server></Nature:MyCheckBoxList>")]
    public class MyCheckBoxList : CheckBoxList, IControlHelp, IFillItemHelp
    {
        /// <summary>
        /// 
        /// </summary>
        private string _dataType;       //验证类型

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (string.IsNullOrEmpty(CssClass))
                CssClass = "lst";

            if (string.IsNullOrEmpty(DataValueField))
                DataValueField = "ID";

            if (string.IsNullOrEmpty(DataTextField))
                DataTextField = "txt";

            Font.Size = FontUnit.Point(12);
        }

        #region 扩充方法

        #region 获取选中的选项的Value值。多选，1,2,3的形式
        /// <summary>
        /// 获取选中的选项的Value值。多选，1,2,3的形式
        /// </summary>
        /// <returns></returns>
        public string GetSelectedItemValue()
        {
            string tmp = "";
            foreach (ListItem item in Items)
            { 
                if (item.Selected)
                    tmp += item.Value + ",";
            }

            tmp = tmp.TrimEnd(',');
            return tmp;
        }
        #endregion

        #region 获取选中的选项的Text值。多选，a,b,c的形式
        /// <summary>
        /// 获取选中的选项的Value值。多选，1,2,3的形式
        /// </summary>
        /// <returns></returns>
        public string GetSelectedItemText()
        {
            string tmp = "";
            foreach (ListItem item in Items)
            {
                if (item.Selected)
                    tmp += item.Text + ",";
            }

            tmp = tmp.TrimEnd(',');
            return tmp;
        }
        #endregion

        #region 通过Value设置选项
        /// <summary>
        /// 通过Value设置选项
        /// </summary>
        /// <param name="itemValue"></param>
        public void SetSelectedByValue(string itemValue)
        {
            SelectedIndex = -1;
            string[] arrValue = itemValue.Split(',');
            foreach (ListItem item in Items)
            {
                foreach (string value in arrValue)
                    if (item.Value.Equals(value))
                        item.Selected = true;
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
            string[] arrValue = itemText.Split(',');
            foreach (ListItem item in Items)
            {
                foreach (string text in arrValue)
                    if (item.Text.Equals(text))
                        item.Selected = true;

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
        public string ControlValue
        {
            get { return GetSelectedItemValue(); }
            set { SetSelectedByValue(value); }
        }
        #endregion

        #region GetControlValue
        /// <summary>
        /// 返回 
        /// </summary>
        /// <param name="kind">取值的方式</param>
        /// <returns></returns>
        public string GetControlValue(string kind)
        {
            switch (kind)
            {
                default:       //返回SelectedValue
                    return GetSelectedItemValue();

                case "11":       //返回SelectedItemText
                    return GetSelectedItemText();
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
            switch (kind)
            {
                default:       //返回SelectedValue
                    SetSelectedByValue(value);
                    break;

                case "2":       //返回SelectedItemText
                    SetSelectedByText(value);
                    break;
            }
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口
        /// </summary>
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("210")]
        public string ControlKind
        {
            get { return "210"; }
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
                    Enabled = true;
                    break;

                case "2":   //只读
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
        public virtual void ShowMe(IColumn formColumnMeta, IDal dal, bool isForm)
        {
            //base.Page.Response.Write(info + "<BR>");
            //System.Web.HttpContext.Current.Response.Write(info.ControlInfo  + "<BR>");

            var info = (GroupListExpand)((FormColumnMeta)formColumnMeta).ControlExtend;
            ListHelp.SetList(this, this, info, dal);

            RepeatColumns = info.RepeatColumns != 0 ? info.RepeatColumns : 4;
            
            RepeatDirection = RepeatDirection.Horizontal;

        }

        #endregion

        #region 填充选项

        #region 从数据库绑定控件
        /// <summary>
        /// 从数据库绑定控件
        /// </summary>
        /// <param name="sql">提取数据的SQL语句</param>
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
        /// 字符串填充
        /// </summary>
        /// <param name="valuesAndTexts"></param>
        public void ItemAddByString(string valuesAndTexts)
        {
            string[] strArray = valuesAndTexts.Split(new[] { '~' });
            int num2 = strArray.Length / 2;
            int num3 = num2 - 1;

            Items.Clear();
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
            string[] strArray = texts.Split('~');
            string[] strArray2 = values.Split('~');
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
            var num2 = (int)Math.Round(valuesAndTexts.Length / 2.0);
            int num3 = num2;
            for (int i = 0; i <= num3; i++)
            {
                Items.Add(new ListItem(valuesAndTexts[num2 + i], valuesAndTexts[i]));
            }
        }
        #endregion

        #endregion
    
    }
}
