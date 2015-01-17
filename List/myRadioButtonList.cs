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
 * function: 继承RadioButtonList
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
    /// 继承RadioButtonList
    /// </summary>
    [ToolboxData("<Nature:MyRadioButtonList runat=server></Nature:MyRadioButtonList>")]
    public class MyRadioButtonList : RadioButtonList, IControlHelp, IFillItemHelp
    {
        private int _dataType;       //验证类型

        /// <summary>
        /// 设置css名称和DataValueField、DataTextField
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

        #region 扩充属性
        #endregion

        #region 扩充方法

        #region 通过Value设置选项
        /// <summary>
        /// 通过Value设置选项
        /// </summary>
        /// <param name="itemValue"></param>
        public void SetSelectedByValue(string itemValue)
        {
            SelectedIndex = -1;
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
        public int CheckDataType
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
                    case 106: //必选
                        //Attributes.Add("check", "[0-9]{1}[0-9]*");
                        foreach (ListItem item in Items)
                        {
                            item.Attributes.Add("check", "[0-9]{1}[0-9]*");
                            item.Attributes.Add("warning", CheckErrorMessage);
                        }
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
            get { return SelectedValue ; }
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
                    return SelectedValue;

                case "11":       //返回SelectedItem.Text
                    return SelectedItem.Text;
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
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("209")]
        public string ControlKind
        {
            get { return "209"; }
        }
        #endregion

        #region SetControlKind
        /// <summary>
        /// 设置控件的状态，单选组2、3都表示不可用
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
            //System.Web.HttpContext.Current.Response.Write(ColInfo.ControlInfo + "<BR>");

            var info = (GroupListExpand)((FormColumnMeta)formColumnMeta).ControlExtend;
            ListHelp.SetList(this, this, info, dal);

            if (info.RepeatColumns != 0)
                RepeatColumns = info.RepeatColumns;
            else
                RepeatColumns = 4;
           
            RepeatDirection = RepeatDirection.Horizontal;

            #region 设置前台的验证

            var modColumn = (ModColumnMeta) formColumnMeta;
            //设置验证类型
            CheckDataType = modColumn.ControlCheckKind;

            //设置未通过验证的提示信息
            CheckErrorMessage = modColumn.CheckTip;

            if (modColumn.CustomerCheckKind.Length > 0)
            {
                //设置了自定义的验证方式
                CheckDataReg = modColumn.CustomerCheckKind;
            }
            else
            {
                //使用提供的验证方式
                CheckDataType = (int)modColumn.ControlKind;
            }
            #endregion

        }

        #endregion

        #region 填充选项

        #region 从数据库绑定控件
        /// <summary>
        /// 通过SQL语句绑定控件
        /// </summary>
        /// <param name="sql">SQL 语句</param>
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
        /// 通过字符串，填充Item
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


        #endregion
    

    }
}
