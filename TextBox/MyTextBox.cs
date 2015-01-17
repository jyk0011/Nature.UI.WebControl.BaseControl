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
 * history:  created by 金洋 2009-7-15 11:06:38 
 *           2011-4-11 整理
 * **********************************************
 */

/* ***********************************************
* author :  jyk
* email  :  jyk0011@live.cn 
* function: 单行文本框、多行文本框、密码框
* history:  created by Administrator 2009-7-15 11:06:38 
* ***********************************************/


using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nature.Data;
using Nature.MetaData.ControlExtend;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;
using Nature.MetaData.Enum;

namespace Nature.UI.WebControl.BaseControl.TextBox
{
    /// <summary>
    /// 继承MyTextBox，单行文本框、多行文本框、密码框
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<Nature:MyTextBox runat=server></Nature:MyTextBox>")]
    public class MyTextBox : BaseTextBox
    {
        /// <summary>
        /// 描述单行文本框、多行文本框和密码框
        /// </summary>
        /// <param name="formColumnMeta">配置信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <param name="isForm">True：表单控件；False：查询控件</param>
        public override void ShowMe(IColumn formColumnMeta, IDal  dal, bool isForm)
        {
            base.ShowMe(formColumnMeta, dal, isForm);
           
            var textExt = ((FormColumnMeta)formColumnMeta).ControlExtend ;

            #region 设置文本框的TextMode、Columns、MaxLength
            switch (((FormColumnMeta)formColumnMeta).ControlKind)
            {
                case ControlType.SingleTextBox  : //单行文本框
                    TextMode = TextBoxMode.SingleLine;
                    //设置文本框的MaxLength，文本框的最大输入字符数
                    MaxLength = ((BaseTextBoxExtend)textExt).ModMaxLength;
                    break;
                
                case ControlType.DateTimeTextBox: //日期
                    TextMode = TextBoxMode.SingleLine;
                    //设置文本框的MaxLength，文本框的最大输入字符数
                    MaxLength = ((BaseTextBoxExtend)textExt).ModMaxLength;
                    break;

                case ControlType.MultipleTextBox : //多行文本框
                    TextMode = TextBoxMode.MultiLine;
                    Rows = ((TextBoxMulExtend)textExt).Rows;
                    break;

                case ControlType.PasswordTextBox : //密码文本框
                    TextMode = TextBoxMode.Password;
                    
                    break;
            }
            #endregion

            #region 判断是不是查询
            if (isForm)
            {
                //添加、修改
                //设置文本框的Columns，文本框的宽度
                Columns = ((BaseTextBoxExtend)textExt).ModWidth;
            }
            else
            {
                //查询控件用
                TextMode = TextBoxMode.SingleLine;

                if (((FormColumnMeta)formColumnMeta).ControlKind != ControlType.PasswordTextBox )
                {
                    //密码框不能作为查询条件
                    Columns = ((BaseTextBoxExtend)textExt).FindWidth;
                    MaxLength = ((BaseTextBoxExtend)textExt).FindMaxLength;
                }
            }
            #endregion

        }
    }
}
