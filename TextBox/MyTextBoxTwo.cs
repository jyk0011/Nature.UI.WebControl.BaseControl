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
 * function: 继承WebControl，显示一个或者两个文本框，查询的时候使用，比如：从____ 到 _____
 * history:  created by 金洋 2009-7-15 15:32:23 
 *           2011-4-11 整理
 * **********************************************
 */


using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Nature.Data;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;
using Nature.MetaData.Enum;
using Nature.UI.WebControl.BaseControl.Dictionary;

namespace Nature.UI.WebControl.BaseControl.TextBox
{
    /// <summary>
    /// 继承WebControl，显示一个或者两个文本框，查询的时候使用，比如：从____ 到 _____
    /// </summary>
    [ToolboxData("<Nature:MyTextBoxTwo runat=server></Nature:MyTextBoxTwo>")]
    public class MyTextBoxTwo : System.Web.UI.WebControls.WebControl, IControlHelp,INamingContainer
    {
        private IControlHelp  _txt1Help;
        private IControlHelp  _txt2Help;

        private Control _txt1 = new MyTextBox () ;
        private Control _txt2 = new MyTextBox();
        private readonly Label _lbl = new Label();

        /// <summary>
        /// 创建两个文本框，为时间段这类的查询作准备
        /// </summary>
        protected override void CreateChildControls()
        {
            _lbl.ID = "Lbl_Title";
            _lbl.Text = " 到 ";

            Controls.Add(_txt1);
            Controls.Add(_lbl);
            Controls.Add(_txt2);
           
        }

        #region 实现接口
        #region ControlValue
        /// <summary>
        /// 实现接口，设置Text属性，返回Text.Trim().Replace("'", "")
        /// </summary>
        [Bindable(true)]
        [Category("文本值")]
        [Description("设置文本框的 Text 属性；返回 Text.Trim().Replace(\"'\", \"\")。")]
        public string ControlValue
        {
            get
            {
                string tmp = "";

                if (_txt2.Visible == false)
                {
                    //提取一个文本框
                    _txt1Help = (IControlHelp)_txt1;
                    tmp = _txt1Help.ControlValue;
                }
                else
                {
                    //提取两个文本框
                    _txt1Help = (IControlHelp)_txt1;
                    _txt2Help = (IControlHelp)_txt2;

                    if (_txt1Help != null)
                        tmp = _txt1Help.ControlValue;

                    if (_txt2Help != null)
                        tmp += "`" + _txt2Help.ControlValue;
                }
                return tmp;

            }
            set { }
             
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
            string tmp = "";
            if (_txt2.Visible == false)
            {
                //提取一个文本框
                _txt1Help = (IControlHelp)_txt1;
                tmp = _txt1Help.GetControlValue(kind);
            }
            else
            {
                //提取两个文本框
                _txt1Help = (IControlHelp)_txt1;
                _txt2Help = (IControlHelp)_txt2;

                if (_txt1Help != null)
                    tmp = _txt1Help.GetControlValue(kind);

                if (_txt2Help != null)
                    tmp += "`" + _txt2Help.GetControlValue(kind);
            }

            return tmp;
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
             
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口
        /// </summary>
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("201")]
        public string ControlKind
        {
            get { return "201"; }
        }
        #endregion

        #region SetControlKind
        /// <summary>
        /// 设置控件的状态，下拉列表框框2、3都表示不可用
        /// </summary>
        /// <param name="kind">1：正常；2：不可用；3：不可用</param>
        public virtual void SetControlState(string kind)
        {
            switch (kind)
            {
                case "1":   //正常
                    ((BaseTextBox)_txt1).Enabled = true;
                    ((BaseTextBox)_txt2).Enabled = true;
                    ((BaseTextBox)_txt1).ReadOnly = false;
                    ((BaseTextBox)_txt2).ReadOnly = false;
                    break;

                case "2":   //只读
                    ((BaseTextBox)_txt1).ReadOnly = true;
                    ((BaseTextBox)_txt2).ReadOnly = true;
                    break;

                case "3":   //不可用
                    ((BaseTextBox)_txt1).Enabled = false;
                    ((BaseTextBox)_txt2).Enabled = false;
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 显示一个或者两个文本框，查询的时候使用
        /// </summary>
        /// <param name="formColumnMeta">配置信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <param name="isForm">True：表单控件；False：查询控件</param>
        public virtual void ShowMe(IColumn formColumnMeta, IDal  dal, bool isForm)
        {
            Dictionary<ControlType, Func<Control>> dicFormControls = DictionaryControl.GetDicFormControls();
 
            //依据查询条件，加载控件。
            int kind = ((FindColumnMeta)formColumnMeta).FindKind;
            if (kind >= 101 && kind <= 199)
            {
                //输出两个文本框
                _txt1 = dicFormControls[((FormColumnMeta)formColumnMeta).ControlKind]();
                _txt2 = dicFormControls[((FormColumnMeta)formColumnMeta).ControlKind]();
                _txt1Help = (IControlHelp)_txt1;
                _txt2Help = (IControlHelp)_txt2;

                _txt1Help.ShowMe(formColumnMeta, dal, isForm);
                _txt2Help.ShowMe(formColumnMeta, dal, isForm);

            }
            else
            {
                //输出一个文本框
                _txt1 = dicFormControls[((FormColumnMeta)formColumnMeta).ControlKind]();
                _txt1Help = (IControlHelp)_txt1;
                _txt1Help.ShowMe(formColumnMeta, dal, isForm);

                _txt2.Visible = false;
                _lbl.Visible = false;
            }

           
        }

        #endregion
    }
}
