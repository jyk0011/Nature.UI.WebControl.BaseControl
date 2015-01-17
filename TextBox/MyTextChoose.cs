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
 * function: 文本框形式，单击文本框，打开新窗口，选择记录，保存记录ID，同时显示记录的文本
 * history:  created by 金洋 2009-9-16 16:23:22
 *           2011-4-11 整理
 * **********************************************
 */

using System.ComponentModel;
using System.Web.UI;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.TextBox
{
    /// <summary>
    /// 文本框形式，单击文本框，打开新窗口，选择记录，保存记录ID，同时显示记录的文本
    /// </summary>
    [ToolboxData("<Nature:MyTextChoose runat=server></Nature:MyTextChoose>")]
    public class MyTextChoose : System.Web.UI.WebControls.WebControl, IControlHelp, INamingContainer
    {
        #region 内部成员
        /// <summary>
        /// 记录文本内容
        /// </summary>
        private readonly MyTextBox _txtText = new MyTextBox();
        /// <summary>
        /// 记录ID
        /// </summary>
        private readonly MyTextBox _txtID = new MyTextBox();
        #endregion

        /// <summary>
        /// 创建控件
        /// </summary>
        protected override void CreateChildControls()
        {
            _txtText.ID = "txt1";
            _txtID.ID = "txt2";
            _txtText.ReadOnly = true;

            Controls.Add(_txtText);

            var lc1 = new LiteralControl();
            var lc2 = new LiteralControl();
            lc1.Text = "<span style=\"display:none1\">";
            lc2.Text = "</span>";

            Controls.Add(lc1);
            Controls.Add(_txtID);
            Controls.Add(lc2);

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
                return _txtID.Text;
            }
            set
            {
                string[] tmp = value.Split('~');

                if (tmp.Length == 1)
                {
                    _txtID.Text = tmp[0];
                }
                else if (tmp.Length == 2)
                {
                    _txtText.Text = tmp[1];
                    _txtID.Text = tmp[0];
                }
            }
        }
        #endregion

        #region GetControlValue
        /// <summary>
        /// 根据kind返回对应值
        /// </summary>
        /// <param name="kind">取值的方式
        /// 1：返回ID
        /// 2：返回Text
        /// 3：返回ID ~ Text
        /// </param>
        /// <returns></returns>
        public string GetControlValue(string kind)
        {
            switch (kind)
            {
                case "1":
                    return _txtID.Text.Replace("'","");
                case "2":
                    return _txtID.Text;

                case "11":
                    return _txtText.Text.Replace("'", "");
                case "12":
                    return _txtText.Text;

                default:
                    return _txtID.Text + "~" + _txtText.Text;
                    

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
            string[] tmp = value.Split('~');

            if (tmp.Length == 2)
            {
                _txtText.Text = tmp[1];
                _txtID.Text = tmp[0];
            }
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口
        /// </summary>
        [Category("默认值"), Bindable(true), Description("获取控件类别"), DefaultValue("201")]
        public string ControlKind
        {
            get { return "20502"; }
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
        /// 显示两个文本框，单击第一个文本框可以打开一个窗口选择记录，然后把选择的ID记录到第二个文本框。
        /// 第一个文本框显示文字形式的内容。
        /// </summary>
        /// <param name="formColumnMeta">配置信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <param name="isForm">True：表单控件；False：查询控件</param>
        public virtual void ShowMe(IColumn formColumnMeta, IDal  dal, bool isForm)
        {
            _txtText.ShowMe(formColumnMeta, dal, isForm);

            var info = (TextChooseExpand)((FormColumnMeta)formColumnMeta).ControlExtend;

            string url = info.URL;   // tmpChoose[0];
            int fid = info.PageViewID;
            int width = info.Width;
            int height = info.Height;

            //添加单击事件
            if (_txtText.Enabled)
            {
                _txtText.Attributes.Add("onclick", "openChoose(this,'" + url + "'," + fid + "," + width + "," + height + ")");
            }
        }
        #endregion

    }
}
