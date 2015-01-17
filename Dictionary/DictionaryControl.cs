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
 * function: 通过管理存放控件类型的字典
 * history:  created by 金洋 
 *           2011-4-11 整理
 * **********************************************
 */


using System.Collections.Generic;
using Nature.Data;
using Nature.MetaData.Enum;
using Nature.UI.WebControl.BaseControl.List;
using Nature.UI.WebControl.BaseControl.TextBox;

namespace Nature.UI.WebControl.BaseControl.Dictionary
{
    /// <summary>
    /// 通过管理存放控件类型的字典
    /// </summary>
    public class DictionaryControl
    {
        /// <summary>
        /// 表单用的控件类型
        /// </summary>
        private static Dictionary<ControlType, Func<System.Web.UI.Control>> _dicFormControls;

        /// <summary>
        /// 查询用的控件类型
        /// </summary>
        public static Dictionary<ControlType, Func<System.Web.UI.Control>> DicFindControls;

        /// <summary>
        /// 加载 生成表单控件的“实例方法”
        /// </summary>
        /// <returns></returns>
        public static Dictionary<ControlType, Func<System.Web.UI.Control>> GetDicFormControls()
        {
            return _dicFormControls ?? (_dicFormControls = LoadFormControl());
        }

        /// <summary>
        /// 加载 生成查询控件的“实例方法”
        /// </summary>
        /// <returns></returns>
        public static Dictionary<ControlType, Func<System.Web.UI.Control>> GetDicFindControls()
        {
            return DicFindControls ?? (DicFindControls = LoadFindControl());
        }

        #region 加载表单用的子控件的类型
        /// <summary>
        ///  加载表单用的子控件的类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<ControlType, Func<System.Web.UI.Control>> LoadFormControl()
        {
            var dicTemp = new Dictionary<ControlType, Func<System.Web.UI.Control>>
                              {
                                  {ControlType.SingleTextBox, () => new MyTextBox()},
                                  {ControlType.MultipleTextBox, () => new MyTextBox()},
                                  {ControlType.PasswordTextBox, () => new MyTextBox()},
                                  {ControlType.DateTimeTextBox, () => new MyTextBoxDateTime()},
                                  {ControlType.DropDownList, () => new MyDropDownList()},
                                  {ControlType.UpdateFile, () => new MyTextUpLoad()},
                                  {ControlType.SelectRecords, () => new MyTextChoose()},
                                  {ControlType.UniteList, () => new MyUniteList()},
                                  {ControlType.CheckBoxList, () => new MyCheckBoxList()},
                                  {ControlType.CheckBox, () => new MyCheckBox()},
                                  {ControlType.RadioButtonList, () => new MyRadioButtonList()},
                                  //{ControlType.FckEditor, () => new MyFCKeditor()} ,
                                  {ControlType.ListBox,()=> new MyListBox() } 
                              };

            //dic_temp.Add(20502, delegate() { return new MyListChoice(); });           //下拉列表框形式的选择

            return dicTemp;
             
        }
        #endregion
         

        #region 加载查询用的子控件的类型
        /// <summary>
        ///  加载查询用的子控件的类型
        /// </summary>
        /// <returns></returns>
        public static Dictionary<ControlType, Func<System.Web.UI.Control>> LoadFindControl()
        {
            var dicTemp = new Dictionary<ControlType, Func<System.Web.UI.Control>>
                              {
                                  {ControlType.SingleTextBox, () => new MyTextBoxTwo()},      
                                  {ControlType.MultipleTextBox, () => new MyTextBoxTwo()},      
                                  {ControlType.PasswordTextBox, () => new MyTextBoxTwo()},      
                                  {ControlType.DateTimeTextBox, () => new MyTextBoxTwo()},      
                                  {ControlType.DropDownList, () => new MyDropDownList()},    
                                  {ControlType.UniteList, () => new MyUniteList()},       
                                  {ControlType.CheckBoxList, () => new MyCheckBoxList()},    
                                  {ControlType.CheckBox , () => new MyCheckBox()},        
                                  {ControlType.RadioButtonList, () => new MyCheckBoxList()} ,
                                  {ControlType.ListBox,()=> new MyListBox() }
                              };

            return dicTemp;

        }
        #endregion

        


    }
}
