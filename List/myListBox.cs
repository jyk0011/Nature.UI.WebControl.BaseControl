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
 * function: 列表框自我描绘的一个帮助，可以设置Item和宽度
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
    /// ListBox
    /// </summary>
    [ToolboxData("<Nature:MyListBox runat=server></Nature:MyListBox>")]
    public class MyListBox : ListBox, IControlHelp, IFillItemHelp
    {
        /// <summary>
        /// 
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
            get { return SelectedValue; }
            set { Text = value; }
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
            return SelectedValue;
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
            //Text = value;
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口
        /// </summary>
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("201")]
        public string ControlKind
        {
            get { return "205"; }
        }
        #endregion

        #region SetControlKind
        /// <summary>
        /// 设置控件的状态，下拉框2、3都表示不可用
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
        public virtual void ShowMe(IColumn formColumnMeta,IDal  dal, bool isForm)
        {
            var info = (BaseListExpand)((FormColumnMeta)formColumnMeta).ControlExtend;
            ListHelp.SetList(this, this, info, dal);

        }

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

        #endregion
    }
}
