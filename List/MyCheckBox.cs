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
 * function: 继承 CheckBox
 * history:  created by 金洋 
 *           2011-4-11 整理
 * **********************************************
 */

using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nature.Data;
using Nature.MetaData.Entity;

namespace Nature.UI.WebControl.BaseControl.List
{
    /// <summary>
    /// CheckBox
    /// </summary>
    [ToolboxData("<Nature:MyCheckBox runat=server></Nature:MyCheckBox>")]
    public class MyCheckBox : CheckBox, IControlHelp
    {
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
            get { return Checked.ToString(); }
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
            return Checked.ToString();
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
        /// 设置控件的状态，复选框2、3都表示不可用
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
            
        }

        #endregion

    }
}
