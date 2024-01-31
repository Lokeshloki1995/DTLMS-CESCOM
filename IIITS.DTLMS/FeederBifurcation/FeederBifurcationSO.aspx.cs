﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI.WebControls;
using System.Data;
using IIITS.DTLMS.BL;
using System.Collections;
using System.Configuration;

namespace IIITS.DTLMS.FeederBifurcation
{
    public partial class FeederBifurcationSO : System.Web.UI.Page
    {
        string strFormCode = "FeederBifurcationSO";
        clsSession objsession;
        /// <summary>
        /// This method used for page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objsession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {
                    LoadOfficeCodes(sender, e);
                    CheckAccessRights("3", "1");

                    if (Request.QueryString["FBS_ID"] != null && Request.QueryString["FBS_ID"].ToString() != "")
                    {
                        string strFbsId = Genaral.UrlDecrypt(Request.QueryString["FBS_ID"].ToString());
                        LoadFbcnDetails(strFbsId);
                    }
                }

            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        /// <summary>
        /// This method used to bind the office codes to drop down 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadOfficeCodes(object sender, EventArgs e)
        {
            try
            {
                string officeCode = objsession.OfficeCode;
                string Qry = "SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE";
                Genaral.Load_Combo(Qry, "--Select--", cmbCircle);

                if (officeCode.Length >= 1)
                {
                    cmbCircle.Items.FindByValue(officeCode.Substring(0, 1)).Selected = true;
                    cmbCircle.Enabled = false;
                    string Qry1 = "SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION WHERE DIV_CICLE_CODE='"
                        + cmbCircle.SelectedValue + "'";
                    Genaral.Load_Combo(Qry1, "--Select--", cmbDivision);
                    if (officeCode.Length >= 2)
                    {
                        cmbDivision.Items.FindByValue(officeCode.Substring(0, 2)).Selected = true;
                        cmbDivision.Enabled = false;
                        Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST " +
                            "WHERE SD_DIV_CODE='" + cmbDivision.SelectedValue + "'", "--Select--", cmbSubDivision);
                        if (officeCode.Length >= 3)
                        {
                            cmbSubDivision.Items.FindByValue(officeCode.Substring(0, 3)).Selected = true;
                            cmbSubdivision_SelectedIndexChanged(sender, e);
                        }
                        cmbSubDivision.Enabled = false;
                    }
                }
            }

            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used to fetch the feeder bifurcation details 
        /// </summary>
        /// <param name="strFbsId"></param>
        private void LoadFbcnDetails(string strFbsId)
        {
            try
            {
                clsDtcMaster obj = new clsDtcMaster();
                obj.sOfficeCode = objsession.OfficeCode;
                obj.lDtcId = strFbsId;
                hdfId.Value = strFbsId;
                DataTable dt = obj.GetFeederBfcnRecordsID(strFbsId);
                string newFeederCode = string.Empty;
                string oldDTCIds = string.Empty;
                string status = string.Empty;

                if (dt.Rows.Count > 0)
                {
                    obj.sFeederCode = dt.Rows[0]["FBDS_NEW_FEEDER_CODE"].ToString();
                    oldDTCIds = dt.Rows[0]["FBDS_OLD_DTC_ID"].ToString();
                    status = dt.Rows[0]["FBS_STATUS"].ToString();
                }
                dt = obj.GetDTCDetailsUsingIdFeederCodeApproval(obj, oldDTCIds);

                if (dt.Rows.Count > 0)
                {
                    gridview2.DataSource = dt;
                    gridview2.DataBind();
                }
                DisableOldFeederCodeCheckBoxApproval();

                if (status == "PENDING" && objsession.RoleId == "1")
                {
                    btnApprove.Visible = true;
                }
                LoadDropDownDetails(obj);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used to bind the feeder and station values to dropdown 
        /// </summary>
        /// <param name="obj"></param>
        public void LoadDropDownDetails(clsDtcMaster obj)
        {
            try
            {
                // for new feeder load only that are bifurcated 
                string strQry = "SELECT FD_FEEDER_CODE , FD_FEEDER_CODE || '-' || FD_FEEDER_NAME ";
                strQry += " FROM TBLFEEDERMAST ,  TBLFEEDEROFFCODE WHERE  FD_FEEDER_ID = FDO_FEEDER_ID ";
                Genaral.Load_Combo("SELECT FD_FEEDER_CODE , FD_FEEDER_CODE || '-' || FD_FEEDER_NAME FROM " +
                   " TBLFEEDERMAST ,  TBLFEEDEROFFCODE WHERE FD_FEEDER_ID = FDO_FEEDER_ID ", "--Select--", cmbFeeder);
                Genaral.Load_Combo(strQry, "--Select--", cmbNewFeeder);
                strQry = "  SELECT ST_ID, ST_STATION_CODE || '-' || ST_NAME  StationName FROM " +
                    "TBLSTATION ORDER BY ST_STATION_CODE";
                Genaral.Load_Combo(strQry, "--Select--", cmbStation);
                DataTable dt = obj.GetFdrBfcnDetails(obj);
                if (dt.Rows.Count > 0)
                {
                    string sOldFeederCode = string.Empty;
                    string sNewFeederCode = string.Empty;
                    string sOfficeCode = string.Empty;
                    sOldFeederCode = dt.Rows[0]["FBS_OLD_FEEDER_CODE"].ToString();
                    sNewFeederCode = dt.Rows[0]["FBS_NEW_FEEDER_CODE"].ToString();
                    sOfficeCode = dt.Rows[0]["FDO_OFFICE_CODE"].ToString();
                    cmbFeeder.SelectedValue = sOldFeederCode;
                    cmbNewFeeder.SelectedValue = sNewFeederCode;
                    string stationID = obj.GetStationCode(sNewFeederCode);
                    cmbStation.SelectedValue = stationID;
                    cmbCircle.SelectedValue = sOfficeCode.Substring(0, 1);
                    cmbCircle_SelectedIndexChanged(null, null);
                    cmbDivision.SelectedValue = sOfficeCode.Substring(0, 2);
                    cmbdivision_SelectedIndexChanged(null, null);
                    cmbSubDivision.SelectedValue = sOfficeCode;
                    cmbNewFeeder.Enabled = false;
                    cmbFeeder.Enabled = false;
                    cmbStation.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        #region Access Rights
        /// <summary>
        /// This method used to checked user access for form
        /// </summary>
        /// <param name="sAccessType"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool CheckAccessRights(string sAccessType, string flag)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "FeederBifurcationSO";
                objApproval.sRoleId = objsession.RoleId;
                objApproval.sAccessType = "2" + "," + sAccessType;
                bool bResult = objApproval.CheckAccessRights(objApproval);
                if (flag == "2")
                {
                    //&& objSession.UserId != "39"
                    if (UserValid() == false)
                    {
                        if (bResult == true)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                            bResult = false;
                        }
                    }

                }
                else if (flag == "1")
                {
                    if (UserValid() == false)
                    {
                        if (bResult == false)
                        {
                            Response.Redirect("~/UserRestrict.aspx", false);
                        }
                    }
                }

                return bResult;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;

            }
        }
        /// <summary>
        /// This method used to check valid user
        /// </summary>
        /// <returns></returns>
        public bool UserValid()
        {
            bool res = true;
            try
            {
                string Userid = Convert.ToString(ConfigurationManager.AppSettings["SELECTEDUSER"]);
                string[] sUserid = Userid.Split(',');
                for (int i = 0; i < sUserid.Length; i++)
                {
                    if (objsession.UserId != sUserid[i])
                    {
                        res = false;
                    }
                    else
                    {
                        res = true;
                        return res;
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return false;
            }
        }

        #endregion
        /// <summary>
        /// This method used to load feeder details
        /// </summary>
        private void LoadFeederDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                clsFeederView objFeeder = new clsFeederView();
                dt = objFeeder.LoadFeederMastDet("");
                gridview2.DataSource = dt;
                gridview2.DataBind();
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// This method will call when select circle in drop down for get the division values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCircle.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT DIV_CODE,DIV_NAME FROM TBLDIVISION " +
                        " WHERE DIV_CICLE_CODE='" + cmbCircle.SelectedValue
                        + "'", "--Select--", cmbDivision);
                }
                else
                {
                    cmbDivision.Items.Clear();
                    cmbSubDivision.Items.Clear();
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used to get the subdivision details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT SD_SUBDIV_CODE,SD_SUBDIV_NAME from TBLSUBDIVMAST WHERE " +
                       " SD_DIV_CODE='" + cmbDivision.SelectedValue + "'", "--Select--", cmbSubDivision);
                }
                else
                {
                    cmbFeeder.Items.Clear();
                    cmbSubDivision.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This method used to get the feeder values based on subdivision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSubdivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSubDivision.SelectedIndex > 0)
                {
                    Genaral.Load_Combo("SELECT FD_FEEDER_CODE , FD_FEEDER_CODE || '-' || FD_FEEDER_NAME " +
                        " FROM TBLFEEDERMAST ,  TBLFEEDEROFFCODE WHERE FD_FEEDER_ID = FDO_FEEDER_ID and " +
                        " FDO_OFFICE_CODE LIKE '" + cmbSubDivision.SelectedValue + "%'", "--Select--", cmbFeeder);
                }
                else
                {
                    cmbFeeder.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        protected void gridview2_RowDataBound(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// This method used to load the dtc details based on selected feeder 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFeeder.SelectedIndex > 0)
                {
                    LoadDTCDetails(cmbFeeder.SelectedValue);
                }
                if (gridview2.Rows.Count == 0)
                {
                    // load the station code once he selects the old feeder code 
                    string strQry = " SELECT ST_ID ,ST_STATION_CODE || '-' || ST_NAME StationName ";
                    strQry += "  FROM TBLSTATION  ORDER BY ST_STATION_CODE";
                    Genaral.Load_Combo(strQry, "--Select--", cmbStation);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// This used to get the values to new feeder based on selected feeder code and station 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbStation.SelectedIndex > 0)
                {
                    // for new feeder load only that are bifurcated 
                    string strQry = "SELECT FD_FEEDER_CODE , FD_FEEDER_CODE || '-' || FD_FEEDER_NAME  FROM ";
                    strQry += " TBLFEEDERMAST WHERE  FD_FEEDER_CODE not in ('" + cmbFeeder.SelectedValue + "' ) ";
                    strQry += " AND FD_ST_ID IN (" + cmbStation.SelectedValue + ") order by FD_FEEDER_CODE ";
                    Genaral.Load_Combo(strQry, "--Select--", cmbNewFeeder);
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// If select the new feeder this method used to check the feeder status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbNewFeeder_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridview2.Rows.Count > 0)
                {
                    gridview2.DataSource = new DataTable();
                    gridview2.DataBind();
                }

                //check if the feeder code is pending in any other aet forms .
                clsDtcMaster obj = new clsDtcMaster();
                string status = obj.CheckFeederCodeStatus(cmbNewFeeder.SelectedValue);
                if (status != "")
                {
                    cmbNewFeeder.SelectedIndex = 0;
                    ShowMsgBox(status);
                }
                else
                {
                    if (grdOldDTC.Rows.Count > 0)
                    {
                        btnUpdate.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// Load dtc details
        /// </summary>
        /// <param name="p"></param>
        private void LoadDTCDetails(string p)
        {
            try
            {
                clsDtcMaster obj = new clsDtcMaster();
                obj.sFeederCode = p;
                DataTable dt = obj.GetDTCDetailsUsingFeederCodeSectionOfficer(obj);

                if (dt.Rows.Count > 0)
                {

                    grdOldDTC.DataSource = dt;
                    grdOldDTC.DataBind();

                    foreach (GridViewRow row in grdOldDTC.Rows)
                    {
                        Label lblstatus = (Label)row.FindControl("lblSTATUS");
                        Label lblTCCode = (Label)row.FindControl("lblTCCode");

                        if(lblTCCode.Text == "0")
                        {
                            ((CheckBox)row.FindControl("cbSelect")).Enabled = false;
                            ((CheckBox)row.FindControl("cbSelect")).ToolTip = "Can't Bifurcate, DTC not mapped with DTr";
                        }
                        if (lblstatus.Text == "FAILURE")
                        {
                            ((CheckBox)row.FindControl("cbSelect")).Enabled = false;
                            ((CheckBox)row.FindControl("cbSelect")).ToolTip = "Can't Bifurcate, DTC Declared Failure/Enhancement";
                        }
                    }
                    grdOldDTC.Visible = true;
                    ViewState["olddtclist"] = dt;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// WorkFlowObjects
        /// </summary>
        /// <returns></returns>
        public string WorkFlowObjects()
        {
            try
            {
                string sClientIP = string.Empty;

                string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ip))
                {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    sClientIP = ipRange[0];
                }
                else
                {
                    sClientIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                return sClientIP;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
                return null;
            }
        }
        /// <summary>
        /// This method will be called for grid functionalities
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdDtc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string strDtcId, feederCode, strSerialNo, strDTCCodes = string.Empty;
                GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
                if (e.CommandName == "AutoGenerate")
                {

                    strDtcId = ((Label)row.FindControl("lblDTCID")).Text;
                    feederCode = ((Label)row.FindControl("lblNewFeederCode")).Text;
                    strSerialNo = string.Empty;
                    string tempMaxDtc = string.Empty;
                    if (ViewState["AutoGenDTCCodes"] != null)
                    {
                        strDTCCodes = ViewState["AutoGenDTCCodes"].ToString();
                        clsDtcMaster objDTC = new clsDtcMaster();
                        objDTC.sFeederCode = feederCode;
                        strSerialNo = GetMaxPlusOneDTcCode(strDTCCodes);
                        strDTCCodes = strDTCCodes + "," + strSerialNo;
                        ViewState["AutoGenDTCCodes"] = strDTCCodes;
                    }
                    else
                    {
                        strSerialNo = AutoGenerateDTCCode(feederCode);
                        if (strSerialNo.Contains("Invalid"))
                        {
                            ShowMsgBox(strSerialNo);
                            return;
                        }

                        ViewState["AutoGenDTCCodes"] = strSerialNo;
                    }

                    // populate the values and disable it.
                    {
                        ((TextBox)row.FindControl("lblDTCseraial")).Text = strSerialNo.Substring(4, 2);
                        ((TextBox)row.FindControl("lblDTCseraial")).Enabled = false;
                        ((CheckBox)row.FindControl("cbSelect")).Checked = true;
                        ((CheckBox)row.FindControl("cbSelect")).Enabled = false;
                    }

                }
                if (e.CommandName == "search")
                {

                    feederCode = ((Label)row.FindControl("lblNewFeederCode")).Text;
                    strSerialNo = ((TextBox)row.FindControl("lblDTCseraial")).Text;
                    strDtcId = ((Label)row.FindControl("lblDTCID")).Text;

                    if (ViewState["AutoGenDTCCodes"] != null)
                    {
                        strDTCCodes = ViewState["AutoGenDTCCodes"].ToString();
                    }

                    if (strSerialNo != "")
                    {
                        strDTCCodes = strDTCCodes.Replace(feederCode + strSerialNo, "");
                    }

                    strDTCCodes = strDTCCodes.Replace(",,", ",");
                    if (strDTCCodes.Length == 1)
                    {
                        strDTCCodes = null;
                    }
                    strDTCCodes = strDTCCodes.Trim(',');
                    ViewState["AutoGenDTCCodes"] = strDTCCodes;


                    StringBuilder sb = new StringBuilder();
                    if (ViewState["CheckedDTCCodes"] != null)
                    {
                        sb = (StringBuilder)ViewState["CheckedDTCCodes"];
                    }

                    foreach (GridViewRow grd in grdOldDTC.Rows)
                    {
                        CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                        if (strDtcId == ((Label)grd.FindControl("lblDTCID")).Text)
                        {
                            chk.Checked = false;
                        }

                        strDtcId = ((Label)row.FindControl("lblDTCID")).Text;
                        if (chk != null && chk.Checked)
                        {
                            sb.Append(grdOldDTC.DataKeys[grd.RowIndex].Value.ToString());
                            sb.Append(",");

                        }
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }


                    string[] arr = sb.ToString().Split(',');
                    sb.Clear();
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (!(arr[i] == strDtcId))
                        {
                            sb.Append(arr[i]);
                            sb.Append(',');
                        }
                    }
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }

                    clsDtcMaster objDTC = new clsDtcMaster();
                    objDTC.sFeederCode = cmbNewFeeder.SelectedValue;

                    DataTable dt = objDTC.GetDTCDetailsUsingIdFeederCode(objDTC, Convert.ToString(sb));
                    if (dt.Rows.Count > 0)
                    {
                        gridview2.DataSource = dt;
                        gridview2.DataBind();
                        btnbifurcate.Visible = true;
                        ViewState["AutoGenDTCCodes"] = null;
                    }
                    else
                    {
                        gridview2.DataSource = dt;
                        gridview2.DataBind();
                        ViewState["CheckedDTCCodes"] = null;
                        btnbifurcate.Visible = false;
                        ViewState["AutoGenDTCCodes"] = null;
                    }

                    DisableOldFeederCodeCheckBox();
                }

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        /// <summary>
        /// This method used to get the max plus one dtc code
        /// </summary>
        /// <param name="strDTCCodes"></param>
        /// <returns></returns>
        public string GetMaxPlusOneDTcCode(string strDTCCodes)
        {
            try
            {
                // sort and get the last dtc code .
                List<string> TagIds = strDTCCodes.Split(',').ToList<string>();
                TagIds.Sort();
                string sLastDtcCode = TagIds[TagIds.Count - 1];


                string final = string.Empty;
                char last, lastupdated;
                char first, firstupdated;

                {

                    string s = sLastDtcCode.Substring(4);

                    byte[] asciiBytes = Encoding.ASCII.GetBytes(s);
                    if (asciiBytes[0] >= 48 && asciiBytes[0] <= 57)
                    {
                        int temp = Convert.ToInt32(s);
                        temp = temp + 1;


                        if (temp.ToString().Length == 1)
                        {
                            string strTemp = "0" + Convert.ToString(temp);
                            final = sLastDtcCode.Substring(0, 4) + strTemp;
                            return final;
                        }

                        if (temp > 99)
                        {
                            final = "AA";
                        }
                        else
                        {
                            final = Convert.ToString(temp);
                        }

                    }
                    else
                    {

                        s.ToUpper();
                        char[] arr = s.ToCharArray();
                        first = firstupdated = arr[0];
                        last = lastupdated = arr[1];

                        lastupdated++;
                        if (lastupdated.Equals('['))
                        {
                            lastupdated = 'A';
                            firstupdated++;
                            if (firstupdated.Equals('['))
                            {
                                firstupdated = 'A';
                            }
                        }
                        final = Convert.ToString(firstupdated) + Convert.ToString(lastupdated);
                    }
                }
                final = sLastDtcCode.Substring(0, 4) + final;
                return final;

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }
        /// <summary>
        /// This method used to auto generate the dtc code
        /// </summary>
        /// <param name="feederCode"></param>
        /// <returns></returns>
        public string AutoGenerateDTCCode(string feederCode)
        {
            try
            {
                clsDTCCommision obj = new clsDTCCommision();
                obj.sFeedercode = feederCode;
                return obj.AutoGenerateDTCCode(obj);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
                return "";
            }
        }

        /// <summary>
        /// which calls after I click on DTC's To Be  Bifurcated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnUpdate_click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                String newFeederCode = string.Empty;
                StringBuilder sbfeederCodes = new StringBuilder();
                if (cmbNewFeeder.SelectedIndex == 0)
                {
                    ShowMsgBox("Please Select New Feeder Code");
                    return;
                }
                else
                {
                    newFeederCode = cmbNewFeeder.SelectedValue;
                }
                foreach (GridViewRow grd in grdOldDTC.Rows)
                {
                    CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                    if (chk != null && chk.Checked)
                    {
                        sb.Append(grdOldDTC.DataKeys[grd.RowIndex].Value.ToString());
                        sb.Append(",");

                    }
                }
                if (sb.Length == 0)
                {
                    ShowMsgBox("Please Select atleast one DTC Code");
                    return;
                }
                sb.Remove(sb.Length - 1, 1);
                clsDtcMaster objDTC = new clsDtcMaster();
                objDTC.sFeederCode = newFeederCode;

                /// Selected DTC's will be added to the Second GRID
                DataTable dt = objDTC.GetDTCDetailsUsingIdFeederCode(objDTC, Convert.ToString(sb));
                if (dt.Rows.Count > 0)
                {
                    gridview2.DataSource = dt;
                    gridview2.DataBind();
                    ViewState["NewDTCCodes"] = dt;
                }
                ViewState["AutoGenDTCCodes"] = null;
                DisableOldFeederCodeCheckBox();

                btnbifurcate.Visible = true;
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        /// <summary>
        /// This function used to disable old feeder code check box
        /// </summary>
        public void DisableOldFeederCodeCheckBox()
        {
            try
            {
                foreach (GridViewRow grd in gridview2.Rows)
                {
                    Label lblOldFeed = (Label)(grd.FindControl("lblOldFeederCode"));
                    Label lblNEw = (Label)(grd.FindControl("lblNewFeederCode"));


                    if (lblOldFeed.Text == lblNEw.Text)
                    {
                        ImageButton img = (ImageButton)grd.FindControl("imgBtnEdit");
                        img.Enabled = false;
                        ImageButton imgAuto = (ImageButton)grd.FindControl("imgBtnAuto");
                        imgAuto.Enabled = false;
                    }
                    TextBox txtSerail = (TextBox)grd.FindControl("lblDTCseraial");
                    txtSerail.Enabled = false;
                    CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                    chk.Enabled = false;

                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// this function used to disable the old feeder code check box
        /// </summary>
        public void DisableOldFeederCodeCheckBoxApproval()
        {
            try
            {
                foreach (GridViewRow grd in gridview2.Rows)
                {
                    Label lblOldFeed = (Label)(grd.FindControl("lblOldFeederCode"));
                    Label lblNEw = (Label)(grd.FindControl("lblNewFeederCode"));


                    if (lblOldFeed.Text == lblNEw.Text)
                    {
                        CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                        chk.Enabled = false;

                    }
                    else
                    {
                        CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                        chk.Enabled = false;
                        chk.Checked = true;
                    }
                    ImageButton img = (ImageButton)grd.FindControl("imgBtnEdit");
                    img.Enabled = false;
                    ImageButton imgAuto = (ImageButton)grd.FindControl("imgBtnAuto");
                    imgAuto.Enabled = false;
                    TextBox txtSerail = (TextBox)grd.FindControl("lblDTCseraial");
                    txtSerail.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        /// <summary>
        /// it will bifurcate the DTC's
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnbifurcate_click(object sender, EventArgs e)
        {
            try
            {

                ArrayList al = new ArrayList();
    
                StringBuilder oldDTCID = new StringBuilder();
                StringBuilder newDTCCodes = new StringBuilder();
                StringBuilder failedDTCCodes;
                HashSet<string> hsselectedFeederCode = new HashSet<string>();
                string final;

                if (!(CheckAccessRights("2", "1")))
                {
                    ShowMsgBox(" You are not authorized");
                    Response.Redirect("~/UserRestrict.aspx", false);
                    return;
                }

                ///from second grid add the values to final variable 
                foreach (GridViewRow grd in gridview2.Rows)
                {
                    CheckBox chk = (CheckBox)grd.FindControl("cbSelect");
                    if (chk != null && chk.Checked)
                    {
                        Label lblDTCID = (Label)(grd.FindControl("lblDTCID"));
                        Label lblDTCCode = (Label)(grd.FindControl("lblDTCCode"));
                        Label lblTCCode = (Label)(grd.FindControl("lblTCCode"));
                        Label lblOldFeederCode = (Label)(grd.FindControl("lblOldFeederCode"));
                        Label lblNewFeederCode = (Label)(grd.FindControl("lblNewFeederCode"));
                        TextBox lblDTCseraial = (TextBox)(grd.FindControl("lblDTCseraial"));

                        if (lblDTCseraial.Text.Length == 0 || lblDTCseraial.Text.Length == 1)
                        {
                            ShowMsgBox(lblDTCCode.Text + " DTC Code Serial Number is Empty");
                            return;
                        }
                        if (lblTCCode.Text.Length > 6)
                        {
                            ShowMsgBox("Cant save details, DTC Code "+ lblDTCCode.Text + " Mapped with invalid DTr " + lblTCCode.Text + ".");
                            return;
                        }
                        final = lblDTCID.Text + "~" + lblDTCCode.Text + "~" + lblTCCode.Text + "~"
                            + lblOldFeederCode.Text + "~" + lblNewFeederCode.Text + "~"
                            + lblNewFeederCode.Text + lblDTCseraial.Text;
                        al.Add(final);
                        hsselectedFeederCode.Add(lblOldFeederCode.Text);

                        oldDTCID.Append(lblDTCID.Text);
                        oldDTCID.Append(",");
                        newDTCCodes.Append("'" + lblNewFeederCode.Text + lblDTCseraial.Text + "'");
                        newDTCCodes.Append(",");
                    }
                }
                int ar = al.Count;

              
                if (ar == 0)
                {
                    ShowMsgBox("No DTC Code Selected");
                    return;
                }

                if (oldDTCID.Length != 0)
                {
                    oldDTCID.Remove(oldDTCID.Length - 1, 1);
                }
                if (newDTCCodes.Length != 0)
                {
                    newDTCCodes.Remove(newDTCCodes.Length - 1, 1);
                }
                string clientIP = WorkFlowObjects();
                clsDtcMaster objDTC = new clsDtcMaster();
                objDTC.sCrBy = objsession.UserId;
                objDTC.sOfficeCode = objsession.OfficeCode;

                var status = objDTC.UpdateFeederBifurcation(al, oldDTCID, newDTCCodes,
                    hsselectedFeederCode, objDTC, clientIP, "SECTION OFFICER");

                string[] arrStatus = status.Item1;
                List<string> failedDTC = status.Item2;

                if (failedDTC.Count > 0)
                {
                    string[] arrfailedDTC = failedDTC.ToArray();
                    failedDTCCodes = new StringBuilder();
                    for (int i = 0; i < arrfailedDTC.Length; i++)
                    {
                        failedDTCCodes.Append(failedDTCCodes[i]);
                        if (i != arrfailedDTC.Length - 1)
                        {
                            failedDTCCodes.Append(",");
                        }
                    }
                    ShowMsgBox(failedDTC.ToArray() + "DTC has been Failed or DTC has been duplicated");

                }

                ShowMsgBox(arrStatus[0]);

                btnReset_click(sender, e);

            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        /// <summary>
        /// Used to show the pop up messages
        /// </summary>
        /// <param name="sMsg"></param>
        private void ShowMsgBox(string sMsg)
        {
            try
            {
                string sShowMsg = string.Empty;
                sShowMsg = "<script language=javascript> alert ('" + sMsg + "')</script>";
                this.Page.RegisterStartupScript("Msg", sShowMsg);
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                     System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                     System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }
        public void btnClose_click(object sender, EventArgs e)
        {

            Response.Redirect("FeederBifurcationView.aspx");
        }


        /// <summary>
        /// It will calls when i click on reset button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnReset_click(object sender, EventArgs e)
        {
            try
            {
                if (grdOldDTC.Rows.Count > 0)
                {
                    grdOldDTC.DataSource = new DataTable();
                    grdOldDTC.DataBind();
                }
                if (gridview2.Rows.Count > 0)
                {
                    gridview2.DataSource = new DataTable();
                    gridview2.DataBind();
                }
                ViewState["CheckedDTCCodes"] = null;
                ViewState["SelectedFeederCodes"] = null;

                LoadOfficeCodes(sender, e);
                cmbFeeder.SelectedIndex = 0;
                cmbNewFeeder.Items.Clear();
                cmbStation.Items.Clear();

                btnbifurcate.Visible = false;
                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace, ex.Message,
                   System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                   System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }
        /// <summary>
        /// function after aet clicks on approve button .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnApprove_click(object sender, EventArgs e)
        {
            try
            {
                // code to approve the records .

                string strID = hdfId.Value;
                clsDtcMaster obj = new clsDtcMaster();
                obj.sCrBy = objsession.UserId;
                obj.lDtcId = strID;
                string[] resultArr = new string[3];
                resultArr = obj.ApproveFbcnRecords(obj);

                if (resultArr[1] == "1")
                {
                    ShowMsgBox("Approved Successfully");
                    btnApprove.Visible = false;
                }
                else if (resultArr[1] == "-1")
                {
                    // not approved exception occured  .
                    ShowMsgBox("OOPS something went wrong, Please contact support team");
                }
                else
                {
                    ShowMsgBox(resultArr[0]);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

        }


    }
}