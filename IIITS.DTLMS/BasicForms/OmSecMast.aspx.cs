﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IIITS.DTLMS.BL;
using System.Data;
using System.Text.RegularExpressions;


namespace IIITS.DTLMS.BasicForms
{
    public partial class OmSecMast : System.Web.UI.Page
    {

        string tempDepName = string.Empty;
        string strUserLogged = string.Empty;
        string strFormCode = "OmSecMast.aspx";
        clsSession objSession;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                Form.DefaultButton = cmdSave.UniqueID;
                objSession = (clsSession)Session["clsSession"];
                if (!IsPostBack)
                {


                    Genaral.Load_Combo("select CM_CIRCLE_CODE,CM_CIRCLE_NAME from TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                    if (Request.QueryString["OmSecId"] != null && Request.QueryString["OmSecId"].ToString() != "")
                    {
                        CheckAccessRights("4");
                        txtOMId.Text = Genaral.UrlDecrypt(HttpUtility.UrlDecode(Request.QueryString["OmSecId"]));
                        LoadOmSecDetails(txtOMId.Text);
                    }


                }
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Page_Load");
            }
          
            
        }

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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "ShowMsgBox");
            }
        }
        protected void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {

                //Check AccessRights
                bool bAccResult;
                if (cmdSave.Text == "Update")
                {
                    bAccResult = CheckAccessRights("3");
                }
                else
                {
                    bAccResult = CheckAccessRights("2");
                }

                if (bAccResult == false)
                {
                    return;
                }

                string[] Arrmsg = new string[2];
                strUserLogged = objSession.UserId;
                if (txtOmSecCode.Text.Trim().Length == 0 || txtOmSecName.Text.Trim().Length == 0 || cmbCircle.SelectedIndex <= 0 || cmbDivision.SelectedIndex <= 0 || cmbSubDiv.SelectedIndex <= 0 ||  txtPhoneNo.Text.Trim().Length == 0 || txtSecHead.Text.Trim().Length == 0)
                {
                     ShowMsgBox("Enter All Mandatory Fields");
                    return;
                }
                if (txtOmSecCode.Text.Trim().Length != 4)
                {
                     ShowMsgBox("O & M Code must be of Length 4");
                     txtOmSecCode.Focus();
                    return;
                }
                if (cmbCircle.SelectedValue.ToString() != txtOmSecCode.Text.Substring(0, 1))
                {
                    ShowMsgBox("Circle Code & OM Section Code Code Does not Match");
                    txtOmSecCode.Focus();
                    return;
                }

                if (cmbDivision.SelectedValue.ToString() != txtOmSecCode.Text.Substring(0, 2))
                {
                    ShowMsgBox("Division Code and OM Section Code Code Does not Match");
                    txtOmSecCode.Focus();
                    return;
                }

                if (cmbSubDiv.SelectedValue.ToString() != txtOmSecCode.Text.Substring(0, 3))
                {
                     ShowMsgBox("SubDivision-Code and OM Section Code Code Does not Match");
                     txtOmSecCode.Focus();
                    return;
                }
               

                if (txtPhoneNo.Text != "")
                {
                    if (txtPhoneNo.Text.Length < 10)
                    {
                        txtPhoneNo.Focus();
                        ShowMsgBox("Please enter Valid Mobile No");
                        return ;
                    }
                    //if (txtPhone.Text.indexOf("-") > 1)
                    //{
                    //}
                    if ((txtPhoneNo.Text.Length - txtPhoneNo.Text.Replace("-", "").Length) >= 2)
                    {
                        txtPhoneNo.Focus();
                        ShowMsgBox("You cannot use more than one hyphen (-)");
                        return ;
                    }
                    if (txtPhoneNo.Text.Contains(".") == true)
                    {
                        txtPhoneNo.Focus();
                        ShowMsgBox("You cannot enter (.) in Mobile Number");
                        return ;
                    }

                }
            
                if (cmdSave.Text.Equals("Save"))
                {
                    clsOmSecMast ObjOmSecMast = new clsOmSecMast();
                    Arrmsg = ObjOmSecMast.SaveOmSecMastDetails(txtOmSecCode.Text.Trim().ToUpper(), txtOmSecName.Text.Trim().ToUpper(),
                        cmbSubDiv.SelectedValue.ToString(), txtSecHead.Text.Trim().ToUpper(), txtPhoneNo.Text.Trim().ToUpper(), strUserLogged);
                   if (Arrmsg[1].ToString() == "0")
                   {
                       ShowMsgBox(Arrmsg[0]);
                      
                       Reset();
                   }
                   if (Arrmsg[1].ToString() == "4")
                   {
                        ShowMsgBox(Arrmsg[0]);
                   }
                }
                if (cmdSave.Text.Equals("Update"))
                {
                    clsOmSecMast ObjOmSecMast = new clsOmSecMast();
                    Arrmsg = ObjOmSecMast.UpdateOmSecMastDetails(txtOMId.Text, txtOmSecCode.Text.Trim().ToUpper(), txtOmSecName.Text.Trim().ToUpper(), cmbSubDiv.SelectedValue.ToString(), txtSecHead.Text.Trim().ToUpper(), txtPhoneNo.Text.Trim().ToUpper(), strUserLogged);
                    if (Arrmsg[1].ToString() == "0")
                    {
                        ShowMsgBox(Arrmsg[0]);
                        
                        Reset();
                    }
                    if (Arrmsg[1].ToString() == "4")
                    {
                        ShowMsgBox(Arrmsg[0]);
                    }
                }

               
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmdSave_Click");
                return;
            }
        }


        protected void cmdReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

   

        /// <summary>
        /// protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void imbBtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton imgDel = (ImageButton)sender;
            GridViewRow rw = (GridViewRow)imgDel.NamingContainer;
        }

     

        /// <summary>
        /// Reset Fn
        /// </summary>
        private void Reset()
        {
            try
            {
                txtOMId.Text = string.Empty;
                txtOmSecName.Text = string.Empty;
                txtOmSecCode.Text = string.Empty;
                txtPhoneNo.Text = string.Empty;
                txtPhoneNo.Text = string.Empty;
                txtSecHead.Text = string.Empty;

                cmbCircle.SelectedIndex = 0;
                if (cmbDivision.SelectedIndex > 0)
                {
                    cmbDivision.SelectedIndex = 0;
                }
                if (cmbSubDiv.SelectedIndex > 0)
                {
                    cmbSubDiv.SelectedIndex = 0;
                }

                cmbCircle.Enabled = true;
                cmbDivision.Enabled = true;
                cmbSubDiv.Enabled = true;
                txtOmSecCode.Enabled = true;
                cmdSave.Text = "Save";
                txtOmSecCode.ReadOnly = false;
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "Reset");
            }

        }

        public void LoadOmSecDetails(string strOmSec)
        {
            try
            {
                clsOmSecMast ObjOmSec = new clsOmSecMast();

                DataTable DtCircleOffDet = ObjOmSec.LoadOmSecvOffDet(strOmSec.ToString());

                txtOmSecCode.Text = Convert.ToString(DtCircleOffDet.Rows[0]["OM_CODE"]);

                cmbCircle.Enabled = false;
                cmbDivision.Enabled = false;
                txtOmSecCode.Enabled = false;
                cmbSubDiv.Enabled = false;

                txtOmSecName.Text = Convert.ToString(DtCircleOffDet.Rows[0]["OM_NAME"]);
                txtPhoneNo.Text = Convert.ToString(DtCircleOffDet.Rows[0]["OM_MOBILE_NO"]);

                txtSecHead.Text = Convert.ToString(DtCircleOffDet.Rows[0]["OM_HEAD_EMP"]);
               
                Genaral.Load_Combo("select CM_CIRCLE_CODE,CM_CIRCLE_NAME from TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
                cmbCircle.SelectedValue = Convert.ToString(DtCircleOffDet.Rows[0]["OM_CODE"]).Substring(0, 1);
                Genaral.Load_Combo("select DIV_CODE,DIV_NAME,DIV_CICLE_CODE FROM TBLDIVISION where DIV_CICLE_CODE='" + cmbCircle.SelectedValue + "' ORDER BY DIV_CODE", "--Select--", cmbDivision);
                cmbDivision.SelectedValue = Convert.ToString(DtCircleOffDet.Rows[0]["OM_CODE"]).Substring(0, 2);
                Genaral.Load_Combo("select SD_SUBDIV_CODE,SD_SUBDIV_NAME,SD_DIV_CODE FROM TBLSUBDIVMAST where SD_DIV_CODE='" + cmbDivision.SelectedValue + "' ORDER BY SD_SUBDIV_CODE", "--Select--", cmbSubDiv);
                cmbSubDiv.SelectedValue = Convert.ToString(DtCircleOffDet.Rows[0]["OM_CODE"]).Substring(0, 3);

                cmdSave.Text = "Update";
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "LoadOmSecDetails");
            }




        }

        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Genaral.Load_Combo("select DIV_CODE,DIV_NAME,DIV_CICLE_CODE FROM TBLDIVISION  WHERE DIV_CICLE_CODE LIKE  '" + cmbCircle.SelectedValue + "%' ORDER BY DIV_CODE ", "--Select--", cmbDivision);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbCircle_SelectedIndexChanged");
            }
        }

        protected void cmbDivision_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Genaral.Load_Combo("select SD_SUBDIV_CODE,SD_SUBDIV_CODE || '-' || SD_SUBDIV_NAME from tblsubdivmast WHERE SD_DIV_CODE like '" + cmbDivision.SelectedValue + "%'  order by SD_SUBDIV_CODE", "--Select--", cmbSubDiv);
            }
            catch (Exception ex)
            {
                clsException.LogError(ex.StackTrace,ex.Message, strFormCode, "cmbDivision_SelectedIndexChanged");
            }
        }

        #region Access Rights
        public bool CheckAccessRights(string sAccessType)
        {
            try
            {
                // 1---> ALL ; 2---> CREATE ;  3---> MODIFY/DELETE ; 4 ----> READ ONLY

                clsApproval objApproval = new clsApproval();

                objApproval.sFormName = "OmSecMast";
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
                lblErrormsg.Text = clsException.ErrorMsg();
                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "CheckAccessRights");
                return false;

            }
        }

        #endregion

        protected void cmbSubDiv_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               clsOmSecMast objOmSec = new clsOmSecMast();
               objOmSec.sSubDivCode = cmbSubDiv.SelectedValue;
               txtOmSecCode.Text= objOmSec.GenerateOmSecCode(objOmSec);
               txtOmSecCode.ReadOnly = true;
            }
            catch (Exception ex)
            {

                clsException.LogError(ex.StackTrace, ex.Message, strFormCode, "cmbSubDiv_SelectedIndexChanged");
            }
        }


    

    }
}