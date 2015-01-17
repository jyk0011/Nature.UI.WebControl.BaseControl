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
 * function: 多级联动下拉列表框
 * history:  created by 金洋 2009-7-22 10:04:55 
 *           2011-4-11 整理
 * **********************************************
 */


using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Nature.Common;
using Nature.UI.WebControl.BaseControl.TextBox;

namespace Nature.UI.WebControl.BaseControl.List
{
    /// <summary>
    /// 多级联动下拉列表框
    /// </summary>
    [ToolboxData("<Nature:MyUniteList runat=server></Nature:MyUniteList>")]
    public partial class MyUniteList : PlaceHolder, INamingContainer, IControlHelp
    {
        private DataSet _dsList ;               //存放绑定下拉列表框用的数据集

        private readonly MyTextBox _txtItemValue = new MyTextBox();    //获取下拉列表的ID值，用“,”号分隔
        private readonly MyTextBox _txtItemText = new MyTextBox();     //获取下拉列表的text值，用“,”号分隔

        //定义js脚本的数组名
        string _jsArray = "arr_"; //ClientID + "arr";

        ///// <summary>
        ///// 
        ///// </summary>
        //protected override void CreateChildControls()
        //{
        //    base.CreateChildControls();

        //}

        /// <summary>
        /// 绑定控件
        /// </summary>
        public override void DataBind()
        {
            //添加文本框
            SetTextBox();

            //base.DataBind();
            if (_dsList == null)
            {
                //没有获取到记录集
                HttpContext.Current.Response.Write("没有记录集");
                return;
            }

            _jsArray = ClientID + "arr_";

            if (!Page.IsPostBack)
            {
                //设置列表框 
                BindList();

                //设置文本框里面保存的
                SetTextSelectedValue();

            }
            else
            {
                //设置列表框 
                BindListPostBack();

            }

            //设置下拉列表框选项变化触发的js函数
            SetListJsFunction();

            //输出js数组，用js数组保存下拉列表框需要的选项
            WriteJsArray();
        }

        #region  第一次访问时绑定下拉列表框
        /// <summary>
        /// 绑定下拉列表框
        /// </summary>
        /// <returns></returns>
        protected bool BindList()
        {
            //设置下拉列表框的默认选项用
            string[] strHtml = ListHtml;

            //获取第一个下拉列表框的记录集
            if (_dsList.Tables.Count < 2)
            {
                //没有获取到记录集
                HttpContext.Current.Response.Write("没有数据");
                return false;
            }
            if (_dsList.Tables[0].Rows.Count == 0)
            {
                //没有获取到记录集
                HttpContext.Current.Response.Write("第一个表没有记录！");
                return false;
            }

            DataTable dt = _dsList.Tables[0];

            #region 绑定第一个列表框

            var lst = new MyDropDownList {ID = "lst0", DataTextField = "txt", DataValueField = "id", DataSource = dt};

            lst.DataBind();

            //添加分割标志
            Controls.Add(new LiteralControl(strHtml[0]));
            //添加第一个下拉列表框
            Controls.Add(lst);
            Controls.Add(new LiteralControl(strHtml[1]));

            #endregion

            Int32 i;

            //父节点的ID。第一次访问，取第一个下拉列表框的第一个选项的值
            string parentID = _dsList.Tables[0].Rows[0][0].ToString();

            for (i = 1; i < _dsList.Tables.Count; i++)
            {
                DataView dv = _dsList.Tables[i].DefaultView; //准备绑定非第一个下拉列表框的记录集

                #region 绑定其他的列表框

                //获取过滤条件
                dv.RowFilter = "ParentID=" + parentID;

                //定义新的下拉列表框
                lst = new MyDropDownList
                          {EnableViewState = true, ID = "lst" + i, DataTextField = "txt", DataValueField = "id"};

                if (strHtml.Length > 2*i)
                    Controls.Add(new LiteralControl(strHtml[2*i]));

                //添加下拉列表框
                Controls.Add(lst);
                if (strHtml.Length > 2*i + 1)
                    Controls.Add(new LiteralControl(strHtml[2*i + 1]));

                lst.DataSource = dv;
                lst.DataBind();

                //设置父ID。第一次访问，取下拉列表框的第一个选项的值
                parentID = dv.Count > 0 ? dv[0][1].ToString() : "-9999";

                #endregion
            }

            return true;
        }

        #endregion

        #region  在回发的时候，绑定下拉列表框
        /// <summary>
        /// 在回发的时候，绑定下拉列表框
        /// </summary>
        /// <returns></returns>
        protected bool BindListPostBack()
        {
            //设置下拉列表框的默认选项用 
            string[] strValue = _txtItemValue.TextTrimNone.Split(',');

            //设置下拉列表框的默认选项用
            string[] strHtml = ListHtml;

            //获取第一个下拉列表框的记录集
            DataTable dt = _dsList.Tables[0];

            #region 绑定第一个列表框
            var lst = new MyDropDownList {ID = "lst0", DataTextField = "txt", DataValueField = "id", DataSource = dt};

            lst.DataBind();

            //添加分割标志
            Controls.Add(new LiteralControl(strHtml[0]));
            //添加第一个下拉列表框
            Controls.Add(lst);
            Controls.Add(new LiteralControl(strHtml[1]));

            #endregion

            Int32 i;

            //父节点的ID
            string parentID = "";
            if (strValue.Length > 1)
                parentID = strValue[0];

            for (i = 1; i < _dsList.Tables.Count; i++)
            {
                DataView dv = _dsList.Tables[i].DefaultView;     //准备绑定非第一个下拉列表框的记录集

                #region 绑定其他的列表框
                //获取过滤条件
                dv.RowFilter = "ParentID=" + parentID;

                //定义新的下拉列表框
                lst = new MyDropDownList
                          {EnableViewState = false, ID = "lst" + i, DataTextField = "txt", DataValueField = "id"};

                if (strHtml.Length > 2 * i)
                    Controls.Add(new LiteralControl(strHtml[2 * i]));
                Controls.Add(lst);
                if (strHtml.Length > 2 * i+1)
                    Controls.Add(new LiteralControl(strHtml[2 * i + 1]));

                lst.DataSource = dv;
                lst.DataBind();


                //设置下拉列表框的默认选项
                if ((strValue.Length > 1))
                {
                    //有选项
                    lst.SetSelectedByValue(strValue[i]);
                    parentID = lst.SelectedValue;
                }
                else
                {
                    //没有设置选项
                    parentID = dv.Count > 0 ? dv[0][1].ToString() : "-9999";

                }

                #endregion
            }

            return true;

        }
        #endregion


        #region 设置文本框里面保存的选项值，第一次访问的时候把下拉列表框的第一个选项放到文本框里面。
        private void SetTextSelectedValue()
        {
            string tmpValue = "";
            string tmpText = "";

            for (int i = 0; i < _dsList.Tables.Count; i++)
            {
                var lst = (MyDropDownList)FindControl("lst" + i);
                tmpValue += lst.SelectedValue + ",";
                tmpText += lst.SelectedItem.Text + ",";
            }

            _txtItemValue.Text = tmpValue.TrimEnd(',');
            _txtItemText.Text = tmpText.TrimEnd(',');
        }
        #endregion

      
        #region 设置默认选项，取值和赋值的时候使用
        /// <summary>
        /// 设置默认选项
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetSelectedValue(string value)
        {
            string[] strValue = value.Split(',');

            if (_dsList == null)
                return false;

            if (_dsList.Tables.Count < 2)
                return false;

            //设置文本框里的item的ID值
            _txtItemValue.Text = value;

            Int32 i ;

            //设置第一个下拉列表框

            var lst = (MyDropDownList)FindControl("lst0");
            lst.SetSelectedByValue(strValue[0]);

            for (i = 1; i < strValue.Length; i++)
            {
                #region 绑定其他的下拉列表框
                lst = (MyDropDownList)FindControl("lst" + i);
                DataView dv = _dsList.Tables[i].DefaultView;      //准备绑定非第一个下拉列表框的记录集
                dv.RowFilter = "ParentID=" + strValue[i - 1];

                lst.DataSource = dv;
                lst.DataBind();

                //设置下拉列表框的默认选项
                lst.SetSelectedByValue(strValue[i]);
                #endregion
            }

            return true;
        }
        #endregion

        #region 给下拉列表框添加客户端的事件，js函数。
        private void SetListJsFunction()
        {
            MyDropDownList lst;

            #region 设置下拉列表框的js函数，在前台通过js脚本来实现联动的情况
            Int32 i  ;

            for (i = 0; i < _dsList.Tables.Count - 1; i++)
            {
                //查找下拉列表框，不包括最后一个
                lst = (MyDropDownList)FindControl("lst" + i);
                var lst2 = (MyDropDownList)FindControl("lst" + (i + 1));

                //设置下拉列表框的js函数
                lst.Attributes.Add("onchange", "lst_change('" + lst.ClientID + "','" + lst2.ClientID + "','" + _jsArray + "','" + (i + 1) + "' )");
            }

            //最后一个列表框
            lst = (MyDropDownList)FindControl("lst" + i);
            lst.Attributes.Add("onchange", "lstSelected('" + ClientID + "')");

            #endregion
        }
        #endregion

        #region 输出js数组
        /// <summary>
        /// 输出js数组
        /// </summary>
        /// <returns></returns>
        private bool WriteJsArray()
        {
            var str = new StringBuilder(2000);
            str.Append("<script language=\"javascript\">");
            //循环填写数组
            Int32 i ;       //a[0] = new Array('1','aa')
            Int32 j ;

            //输出下拉列表框的数量 vbCrLf 
            str.Append("\n var LstCount = " + ListCount + ";");
            str.Append("\n var ListClientID = \"" + ClientID + "\";");
            str.Append("\n var IsPostBack = \"" + Page.IsPostBack + "\";");
            str.Append("\n var a ;");

            str.Append("\n var " + _jsArray + "0 = new Array();");
            str.Append("\n a = " + _jsArray + "0;");

            DataTable dt = _dsList.Tables[0];

            for (j = 0; j < dt.Rows.Count; j++)
            {
                #region 第一个下拉列表框的选项变成js数组的形式
                str.Append("\na[");
                str.Append(j);
                str.Append("] = new Array('");
                str.Append(dt.Rows[j][0]);
                str.Append("','");
                str.Append(dt.Rows[j][1]);
                str.Append("');");
                #endregion
            }

            #region 其他的下拉列表框
            for (i = 1; i < _dsList.Tables.Count; i++)
            {
                dt = _dsList.Tables[i];

                str.Append("\n var " + _jsArray + (i) + " = new Array();a = " + _jsArray + i + ";");

                for (j = 0; j < dt.Rows.Count; j++)
                {
                    #region 下拉列表框的选项变成js数组的形式
                    str.Append("\na[");
                    str.Append(j);
                    str.Append("] = new Array('");
                    str.Append(dt.Rows[j][0]);
                    str.Append("','");
                    str.Append(dt.Rows[j][1]);
                    str.Append("','");
                    str.Append(dt.Rows[j][2]);
                    str.Append("');");
                    #endregion
                }
            }
            #endregion

            str.Append("\n</script>\n");
            Functions.PageRegisterString(base.Page, str.ToString());

            return true;
        }

        #endregion

        #region  设置记录其他列表框的信息的文本框
        /// <summary>
        /// 添加子控件——文本框，利用文本框保存选择的下拉列表框的选项
        /// </summary>
        private void SetTextBox()
        {
            _txtItemValue.ID = "txtValue";
            _txtItemText.ID = "txtText";
            _txtItemValue.Attributes.Add("style", "display:none");
            _txtItemText.Attributes.Add("style", "display:none");

            Controls.Add(_txtItemValue);
            Controls.Add(_txtItemText);

            //用表单的形式获取文本框的值，不知道为什么txt_ItemValue.Text 是上一次的值，不是最新的。
            string tmp =  Page.Request.Form[ClientID + "$" + _txtItemValue.ID];

            if (tmp != null)
            {
                if (tmp != _txtItemValue.TextTrimNone )
                    _txtItemValue.Text = tmp;
            }

        }
        #endregion

        #region SetControlKind
        /// <summary>
        /// 设置控件的状态，复选组2、3都表示不可用
        /// </summary>
        /// <param name="kind">1：正常；2：不可用；3：不可用</param>
        public virtual void SetControlKind(string kind)
        {
            switch (kind)
            {
                case "1":   //正常
                    break;

                case "2":   //只读
                case "3":   //不可用
                    break;
            }
        }
        #endregion

    }
}
