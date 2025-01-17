﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Globalization;

namespace IIITS.DTLMS.DTCFailure
{
    public partial class EnhancementView : System.Web.UI.Page
    {
        string strFormCode = "EnhancementView";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }

                objSession = (clsSession)Session["clsSession"];
                lblMessage.Text = string.Empty;

                if (!IsPostBack)
                {                   
                    LoadAllDTCEnhancements();
                    CheckAccessRights("4");
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
        }

        protected void cmdNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Check AccessRights
                bool bAccResult = CheckAccessRights("2");
                if (bAccResult == false)
                {
                    return;
                }
                string sStatasFalg = HttpUtility.UrlEncode(Genaral.UrlEncrypt("2"));
                Response.Redirect("Enhancement.aspx?StatusFalg="+ sStatasFalg + "", false);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdNew_Click");
            }
        }




        public void LoadAllDTCEnhancements()
        {
            try
            {
                clsEnhancement objEnhance = new clsEnhancement();

                objEnhance.sOfficeCode = objSession.OfficeCode;

                DataTable dtDetails = objEnhance.LoadAllDTCEnhancement(objEnhance);

                grdEnhancement.DataSource = dtDetails;
                grdEnhancement.DataBind();
                ViewState["Enhancement"] = dtDetails;

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAllDTCEnhancements");
            }

        }



        public void LoadAlreadyEnhanced()
        {
            try
            {
                clsEnhancement objEnhance = new clsEnhancement();
                objEnhance.sOfficeCode = objSession.OfficeCode;
                DataTable dtDetails = objEnhance.LoadAlreadyEnhanced(objEnhance);
                grdEnhancement.DataSource = dtDetails;
                grdEnhancement.DataBind();
                ViewState["Enhancement"] = dtDetails;

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadAlreadyFailure");
            }

        }


        protected void rdbViewAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAllDTCEnhancements();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "rdbViewAll_CheckedChanged");
            }
        }

        protected void rdbAlready_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadAlreadyEnhanced();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "rdbAlready_CheckedChanged");
            }
        }

  
        protected void grdEnhancement_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdEnhancement.PageIndex = e.NewPageIndex;
                DataTable dt = (DataTable)ViewState["Enhancement"];
                grdEnhancement.DataSource = SortDataTable(dt as DataTable, true);
                grdEnhancement.DataBind();
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdEnhancement_PageIndexChanging");
            }
        }

        protected void grdEnhancement_Sorting(object sender, GridViewSortEventArgs e)
        {
            GridViewSortExpression = e.SortExpression;
            int pageIndex = grdEnhancement.PageIndex;
            DataTable dt = (DataTable)ViewState["Enhancement"];
            string sortingDirection = string.Empty;
            if (dt.Rows.Count > 0)
            {
                grdEnhancement.DataSource = SortDataTable(dt as DataTable, false);
            }
            else
            {
                grdEnhancement.DataSource = dt;
            }
            grdEnhancement.DataBind();
            grdEnhancement.PageIndex = pageIndex;
        }

        protected DataView SortDataTable(DataTable dataTable, bool isPageIndexChanging)
        {
            if (dataTable != null)
            {
                DataView dataView = new DataView(dataTable);
                if (GridViewSortExpression != string.Empty)
                {
                    if (isPageIndexChanging)
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection);
                        ViewState["Enhancement"] = dataView.ToTable();

                    }
                    else
                    {
                        dataView.Sort = string.Format("{0} {1}", GridViewSortExpression, GetSortDirection());
                        ViewState["Enhancement"] = dataView.ToTable();

                    }


                }
                return dataView;
            }
            else
            {
                return new DataView();
            }

        }
        private string GridViewSortDirection
        {
            get { return ViewState["SortDirection"] as string ?? "ASC"; }
            set { ViewState["SortDirection"] = value; }


        }
        private string GridViewSortExpression
        {
            get { return ViewState["SortExpression"] as string ?? string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        private string GetSortDirection()
        {
            switch (GridViewSortDirection)
            {
                case "ASC":
                    GridViewSortDirection = "DESC";

                    break;
                case "DESC":
                    GridViewSortDirection = "ASC";

                    break;
            }


            return GridViewSortDirection;
        }


        protected void grdEnhancement_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Create" || e.CommandName == "UpdateEnhance")
                {
                    if (e.CommandName == "Create")
                    {
                        //Check AccessRights
                       bool bAccResult = CheckAccessRights("2");
                        if (bAccResult == false)
                        {
                            return;
                        }
                    }

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDTCId = (Label)row.FindControl("lblDTCId");
                    Label lblEnhanceId = (Label)row.FindControl("lblEnhanceId");
                    Label lblDtcCode = (Label)row.FindControl("lblDtcCode");
                    Label lblProjectType = (Label)row.FindControl("lblProjectType");

                    string[] Arr = new string[2];
                    Arr = CheckAlreadyFailureEnry(lblDtcCode.Text);

                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }


                  
                    //Check Self Execution DTC to Declare Failure
                    Arr = CheckSelfExecutionDTC(lblDtcCode.Text, lblProjectType.Text);
                    if (Arr[1].ToString() == "2")
                    {
                        ShowMsgBox(Arr[0].ToString());
                        return;
                    }
                    string status = GetFeederBifurcationStatus(lblDTCId.Text);

                    if (status != "BIFURCATED" && status != "")
                    {
                        ShowMsgBox("DTC Code has been partially bifurcated,Please contact support team");
                    }
                    else
                    {
                        string sDTCId = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblDTCId.Text));
                        string sEnhanceID = HttpUtility.UrlEncode(Genaral.UrlEncrypt(lblEnhanceId.Text));
                        string sStatasFalg = HttpUtility.UrlEncode(Genaral.UrlEncrypt("2"));
                        Response.Redirect("Enhancement.aspx?DTCId=" + sDTCId + "&EnhanceId=" + sEnhanceID + "&StatusFalg=" + sStatasFalg, false);
                    }
                }


                if (e.CommandName == "Preview")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblDTCCode = (Label)row.FindControl("lblDtcCode");
                    Label lblTCcode = (Label)row.FindControl("lblTcCode");
                    string sDTCcode = lblDTCCode.Text;
                    string sTCcode = lblTCcode.Text;
                    string sOfcCode = objSession.OfficeCode;
                    string sWoID = getWoID(sDTCcode, sOfcCode);
                    string strParam = string.Empty;
                    strParam = "id=EnhanceEstimationSO&TCcode=" + sTCcode + "&WOId=" + sWoID;
                    RegisterStartupScript("Print", "<script>window.open('/Reports/ReportView.aspx?" + strParam + "','Print','addressbar=no, scrollbars =yes, resizable=yes')</script>");
                }

                if (e.CommandName == "search")
                {
                    string sFilter = string.Empty;
                    DataView dv = new DataView();

                    GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                    TextBox txtDtCode = (TextBox)row.FindControl("txtDtCode");
                    TextBox txtDtName = (TextBox)row.FindControl("txtDtName");
                    TextBox txtDtrCode = (TextBox)row.FindControl("txtDtrCode");

                    DataTable dt = (DataTable)ViewState["Enhancement"];
                    dv = dt.DefaultView;

                    if (txtDtCode.Text != "")
                    {
                        sFilter = "DT_CODE Like '%" + txtDtCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtName.Text != "")
                    {
                        sFilter += " DT_NAME Like '%" + txtDtName.Text.Replace("'", "`") + "%' AND";
                    }
                    if (txtDtrCode.Text != "")
                    {
                        sFilter += " TC_CODE Like '%" + txtDtrCode.Text.Replace("'", "`") + "%' AND";
                    }
                    if (sFilter.Length > 0)
                    {
                        sFilter = sFilter.Remove(sFilter.Length - 3);
                        grdEnhancement.PageIndex = 0;
                        dv.RowFilter = sFilter;
                        if (dv.Count > 0)
                        {
                            grdEnhancement.DataSource = dv;
                            ViewState["Enhancement"] = dv.ToTable();
                            grdEnhancement.DataBind();

                        }
                        else
                        {
                            ViewState["Enhancement"] = dv.ToTable();
                            ShowEmptyGrid();
                        }
                    }
                    else
                    {
                        if (rdbAlready.Checked == true)
                        {
                            LoadAlreadyEnhanced();
                        }
                        else if (rdbViewAll.Checked == true)
                        {
                            LoadAllDTCEnhancements();
                        }

                    }


                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdEnhancement_RowCommand");
            }
        }

        private string GetFeederBifurcationStatus(string lblDTCid)
        {
            try
            {
                clsDtcMaster obj = new clsDtcMaster();
                obj.lDtcId = lblDTCid;
                return obj.GetFeederBifurcationStatus(obj);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.ToString(), ex.Message, strFormCode, "Page_Load");
                return null;
            }
        }

        protected void grdEnhancement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    LinkButton lnkUpdate = (LinkButton)e.Row.FindControl("lnkUpdate");
                    LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                    Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                    Label lblDtcCode = (Label)e.Row.FindControl("lblDtcCode");
                    LinkButton lnkWaiting = (LinkButton)e.Row.FindControl("lnkWaiting");
                    LinkButton lnkPrev = (LinkButton)e.Row.FindControl("lnkEstPrev");

                    if (lblStatus.Text == "YES")
                    {
                        lnkUpdate.Visible = true;
                        lnkEdit.Visible = false;
                    }
                    else
                    {
                        lnkUpdate.Visible = false;
                        lnkEdit.Visible = true;

                        // Check DTC in waiting for approval

                        string[] Arr = new string[2];
                        Arr = CheckAlreadyFailureEnry(lblDtcCode.Text);

                        if (Arr[1].ToString() == "2")
                        {
                            lnkWaiting.Visible = true;
                            lnkPrev.Visible = true;
                            lnkEdit.Visible = false;
                        }
                        else
                        {
                            lnkWaiting.Visible = false;
                            lnkPrev.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "grdEnhancement_RowDataBound");
            }
        }
        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "Enhancement";
                objApproval.sRoleId = objSession.RoleId;
                objApproval.sAccessType = "1" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (bResult == false)
                {
                    if (sAccessType == "4")
                    {
                        Response.Redirect("~/UserRestrict.aspx", false);
                    }
                    else
                    {
                        ShowMsgBox("Sorry , You are not authorized to Access");
                    }
                }
                return bResult;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion


        private void ShowMsgBox(string sMsg)
        {
            try
            {
                String sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowMsgBox");
            }
        }


        public void ShowEmptyGrid()
        {
            try
            {
                DataTable dt = new DataTable();
                DataRow newRow = dt.NewRow();
                dt.Rows.Add(newRow);
                dt.Columns.Add("DF_ID");
                dt.Columns.Add("DT_ID");
                dt.Columns.Add("DT_CODE");
                dt.Columns.Add("DT_NAME");
                dt.Columns.Add("TC_CODE");
                dt.Columns.Add("TC_SLNO");
                dt.Columns.Add("TM_NAME");
                dt.Columns.Add("TC_CAPACITY");
                dt.Columns.Add("STATUS");
                dt.Columns.Add("DT_PROJECTTYPE");

                grdEnhancement.DataSource = dt;
                grdEnhancement.DataBind();

                int iColCount = grdEnhancement.Rows[0].Cells.Count;
                grdEnhancement.Rows[0].Cells.Clear();
                grdEnhancement.Rows[0].Cells.Add(new TableCell());
                grdEnhancement.Rows[0].Cells[0].ColumnSpan = iColCount;
                grdEnhancement.Rows[0].Cells[0].Text = "No Records Found";

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "ShowEmptyGrid");

            }
        }

        public string[] CheckAlreadyFailureEnry(string sDTCCode)
        {
            string[] Arr = new string[2];
            try
            {
                clsApproval objApproval = new clsApproval();
                bool bResult = objApproval.CheckAlreadyExistEntry(sDTCCode, "9");
                if (bResult == true)
                {
                    Arr[0] = "Failure Declare Already done for DTC Code " + sDTCCode + ", Waiting for Approval";
                    Arr[1] = "2";
                    return Arr;

                }

                bResult = objApproval.CheckAlreadyExistEntry(sDTCCode, "10");
                if (bResult == true)
                {
                    Arr[0] = "Capacity Enhancement Already done for DTC Code " + sDTCCode + ", Waiting for Approval";
                    Arr[1] = "2";
                    return Arr;

                }
                Arr[0] = "Success";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAlreadyFailureEnry");
                return Arr;

            }
        }

        public string[] CheckSelfExecutionDTC(string sDTCCode, string sProjectType)
        {
            string[] Arr = new string[2];
            try
            {
                clsDTCCommision objDTCComm = new clsDTCCommision();
                objDTCComm.sDtcCode = sDTCCode;
                objDTCComm.sProjecttype = sProjectType;

                bool bResult = objDTCComm.CheckSelfExecutionSchemeType(objDTCComm);
                if (bResult == true)
                {
                    Arr[0] = "Project/Scheme Type of  DTC Code " + sDTCCode + " is Self Execution so can't declare DTC Enhancement untill one Year";
                    Arr[1] = "2";
                    return Arr;
                }

                Arr[0] = "Success";
                Arr[1] = "0";
                return Arr;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckSelfExecutionDTC");
                return Arr;
            }
        }

        public string getWoID(string sDtcCode, string sOfcCode)
        {
            string sWOId = string.Empty;
            clsFailureEntry objFailure = new clsFailureEntry();

            try
            {
                sWOId = objFailure.getWoIDforEstimation(sOfcCode, sDtcCode);
                return sWOId;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "getWoID");
                return sWOId;
            }
        }


        protected void Export_ClickEnhancement(object sender, EventArgs e)
        {
            //DataTable dtDetails = new DataTable();

            //if (rdbViewAll.Checked)
            //{
            //    clsEnhancement objEnhance = new clsEnhancement();

            //    objEnhance.sOfficeCode = objSession.OfficeCode;

            //     dtDetails = objEnhance.LoadAllDTCEnhancement(objEnhance);
            //}
            //else
            //{

            //    clsEnhancement objEnhance = new clsEnhancement();
            //    objEnhance.sOfficeCode = objSession.OfficeCode;
            //     dtDetails = objEnhance.LoadAlreadyEnhanced(objEnhance);
            // }
            DataTable dtDetails = (DataTable)ViewState["Enhancement"];

            if (dtDetails.Rows.Count > 0)
            {

                dtDetails.Columns["DT_CODE"].ColumnName = "DTC CODE";
                dtDetails.Columns["DT_NAME"].ColumnName = "DTC NAME";
                dtDetails.Columns["TC_CODE"].ColumnName = "DTR CODE";
                dtDetails.Columns["TM_NAME"].ColumnName = "MAKE NAME";
                dtDetails.Columns["TC_CAPACITY"].ColumnName = "Capacity(in KVA)";


                //dtLoadDetails.Columns["FEEDER NAME"].SetOrdinal(0);

                List<string> listtoRemove = new List<string> { "DF_ID", "DT_ID", "TC_SLNO", "STATUS", "DT_PROJECTTYPE" };
                string filename = "AllEnhancementDetails" + DateTime.Now + ".xls";
                string pagetitle = " Capacity Enhancement View";

                Genaral.getexcel(dtDetails, listtoRemove, filename, pagetitle);
            }
            else
            {
                ShowMsgBox("No record found");
                ShowEmptyGrid();
            }



        }
    }
}