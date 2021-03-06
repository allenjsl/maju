﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Enow.MAJU.BLL;
using Enow.MAJU.Model;
using Enow.MAJU.Utility;

namespace Enow.MAJU.Web.Manage.Service
{
    public partial class InterView : System.Web.UI.Page
    {
        /// <summary>
        /// 每页条数
        /// </summary>
        protected int PageSize = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["PageSize"]), RecordCount = 0, PageIndex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region 处理ajax请求
            if (Utils.GetQueryStringValue("dotype") == "delete")
            {
                DelInterView();
            }
            #endregion
            if (!IsPostBack)
            {
                ManageUserAuth.ManageLoginCheck();
               
            }
            InitList();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void InitList()
        {
            PageIndex = UtilsCommons.GetPadingIndex();
            var list = BProductInterview.GetViewList(ref RecordCount, PageSize, PageIndex, GetSearchModel());
            if (RecordCount > 0)
            {
                rptList.DataSource = list;
                rptList.DataBind();
                ExportPageInfo1.LinkType = 3;
                ExportPageInfo1.intPageSize = PageSize;
                ExportPageInfo1.intRecordCount = RecordCount;
                ExportPageInfo1.CurrencyPage = PageIndex;
                ExportPageInfo1.PageLinkURL = Request.ServerVariables["SCRIPT_NAME"] + "?";
                ExportPageInfo1.UrlParams = Request.QueryString;
            }
            else
            {
                phNoData.Visible = true;
            }
        }

        /// <summary>
        /// 构造查询实体
        /// </summary>
        /// <returns></returns>
        private ProductInterViewSearch GetSearchModel()
        {
            ProductInterViewSearch SearchModel = new ProductInterViewSearch();
            string KeyWords = Utils.GetQueryStringValue("KeyWords");
            DateTime? OrderTimeStart = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("startDate"));
            DateTime? OrderEndTime = Utils.GetDateTimeNullable(Utils.GetQueryStringValue("endDate"));

            if (!string.IsNullOrWhiteSpace(KeyWords))
            {
                SearchModel.KeyWords = KeyWords;
                txtKeyWords.Text = KeyWords;
            }
            if (OrderTimeStart.HasValue)
            {
                SearchModel.OrderTimeStart = OrderTimeStart;
                txtIBeginDate.Text = Convert.ToDateTime(OrderTimeStart.ToString()).ToString("yyyy-MM-dd");
               
            }
            if (OrderEndTime.HasValue)
            {
                SearchModel.OrderTimeEnd = OrderTimeStart;
                txtIEndDate.Text = Convert.ToDateTime(OrderEndTime.ToString()).ToString("yyyy-MM-dd");
            }
            return SearchModel;
        }
        #region 删除面签信息
        private void DelInterView()
        {
            string Id = Utils.GetQueryStringValue("Id");
            if (string.IsNullOrEmpty(Id))
            {
                Utils.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败：未选择任何要删除的信息！"));
            }
            int bllRetCode = 0;
            string[] Idlist = Id.Split(',');
            for (int i = 0; i < Idlist.Length; i++)
            {
                bllRetCode = BProductInterview.Delete(Idlist[i]) == true ? 1 : 99;
            }
            if (bllRetCode == 1)
            {
                Utils.RCWE(UtilsCommons.AjaxReturnJson("1", "操作成功!"));
            }
            else
            {
                Utils.RCWE(UtilsCommons.AjaxReturnJson("0", "操作失败：未选择任何要删除的信息！"));
            }
        }
        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string KeyWords = Utils.GetFormValue(txtKeyWords.UniqueID);
            string startDate = Utils.GetFormValue(txtIBeginDate.UniqueID);
            string endDate = Utils.GetFormValue(txtIEndDate.UniqueID);
            string uri = UtilsCommons.GetMenuUri(Request.ServerVariables["SCRIPT_NAME"]);
            Response.Redirect(uri + "&KeyWords=" + KeyWords + "&StartDate=" + startDate + "&EndDate=" + endDate, true);
     
        }
    }
}