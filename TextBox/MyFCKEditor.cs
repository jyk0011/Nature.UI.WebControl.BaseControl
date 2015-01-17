/**
 * 自然框架之基础控件
 * http://www.natureFW.com/
 *
 * @author
 * 金洋（金色海洋jyk）
 * 
 * @copyright
 * Copyright (C) 2005-2011 金洋.
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
 * function: 继承FCKeditor
 * history:  created by 金洋  2010-05-22
 *           2011-4-11 整理
 * **********************************************
 */


using System.ComponentModel;
using System.Web.UI;
using FredCK.FCKeditorV2;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.TextBox
{
    /// <summary>
    /// FCKeditor
    /// </summary>
    [DefaultProperty("Value")]
    [ValidationProperty("Value")]
    [ToolboxData("<Nature:MyFCKeditor runat=server></Nature:MyFCKeditor>")]
    [Designer("FredCK.FCKeditorV2.FCKeditorDesigner")]
    [ParseChildren(false)]
    public class MyFCKeditor : FCKeditor, IControlHelp
    {
        #region 实现接口
        #region ControlValue
        /// <summary>
        /// 实现接口，设置Text属性，返回Text.Trim().Replace("'", "")
        /// </summary>
        public string ControlValue
        {
            get { return Value; }
            set { Value = value; }
        }
        #endregion

        #region GetControlValue
        /// <summary>
        /// 返回FreeTextBox的内容
        /// </summary>
        /// <param name="kind">取值的方式</param>
        /// <returns></returns>
        public string GetControlValue(string kind)
        {
            return Value;
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
            Value = value;
        }
        #endregion

        #region ControlKind
        /// <summary>
        /// 实现接口
        /// </summary>
        public string ControlKind
        {
            get { return "214"; }
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
                    //reader = false;
                    break;

                case "2":   //只读
                    //ReadOnly = true;
                    break;

                case "3":   //不可用
                    //ReadOnly = true;
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
            //ImageGalleryPath = "/aspnet_client/FreeTextBox/";

            // = "zh-CN";
            // = "";

            var info = (BaseTextBoxExtend)((FormColumnMeta)formColumnMeta).ControlExtend;

            #region 设置文本框的TextMode、Columns、MaxLength

            if (info.ModWidth > 0)
                //设置宽度
                
                Width = 700;// info.ModWidth;

            //if (info.Rows > 0)
            //    //设置高度 
            Height = 400;// info.Rows;

            #endregion

        }
        #endregion

    }
}
