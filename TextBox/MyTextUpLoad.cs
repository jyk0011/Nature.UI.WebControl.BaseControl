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
 * function: 继承MyTextBox，上传文件、图片的文本框
 * history:  created by 金洋 2010-05-18
 *           2011-4-11 整理
 * **********************************************
 */


using System.ComponentModel;
using System.Web.UI;
using Nature.Data;
using Nature.MetaData.Entity;
using Nature.MetaData.Entity.MetaControl;

namespace Nature.UI.WebControl.BaseControl.TextBox
{
    /// <summary>
    /// 继承MyTextBox，上传文件、图片的文本框
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<Nature:MyTextUpFile runat=server></Nature:MyTextUpFile>")]
    public class MyTextUpLoad : BaseTextBox
    {
        /// <summary>
        /// 描述上传文件、图片的文本框
        /// </summary>
        /// <param name="formColumnMeta">配置信息</param>
        /// <param name="dal">数据访问函数库的实例</param>
        /// <param name="isForm">True：表单控件；False：查询控件</param>
        public override void ShowMe(IColumn formColumnMeta, IDal dal, bool isForm)
        {
            base.ShowMe(formColumnMeta, dal, isForm);

            //TextUploadInfo uploadInfo = (TextUploadInfo)FormColumnMeta.ControlInfo;
            //string par = string.Format("{0}~{1}~{2}", uploadInfo.UploadKind , uploadInfo.FileNameKind ,uploadInfo.FilePath );

            //添加单击事件
            if (Enabled)// == true
            {
                Attributes.Add("onclick", "openChoose(this,'/_CommonPage/upload.aspx?u=" + ((FormColumnMeta)formColumnMeta).ColumnID + "',0,800,400)");
            }

        }
    }
}
