﻿using IIITS.DTLMS.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IIITS.DTLMS.Reports
{
    public partial class MisGuarantyTypeChangedDetails : System.Web.UI.Page
    {
        private clsSession objSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["clsSession"] == null || Session["clsSession"].ToString() == "")
                {
                    Response.Redirect("~/Login.aspx", false);
                }
                objSession = (clsSession)Session["clsSession"];

                if (!IsPostBack)
                {
                    txtToDate.Attributes.Add("readonly", "readonly");
                    txtFromDate.Attributes.Add("readonly", "readonly");

                    Genaral.Load_Combo("SELECT CM_CIRCLE_CODE,CM_CIRCLE_NAME FROM TBLCIRCLE ORDER BY CM_CIRCLE_CODE", "--Select--", cmbCircle);
            }

            catch (Exception ex)
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
        }

        /// <summary>
        /// For Displaying Div name in DropDown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbCircle_SelectedIndexChanged(object sender, EventArgs e)
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

        
        /// <summary>
        /// For Getting Office Code
        /// </summary>
        /// <returns></returns>
        private string GetOfficeID()

        /// <summary>
        /// For generating WRGP to AGP the Reoprt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmdGenerate_Click(object sender, EventArgs e)
                if (ValidateForm() == true)
                {
                    objReport.sFromDate = txtFromDate.Text;
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);

        /// <summary>
        /// For text box Field made mandatory
        /// </summary>
        /// <returns></returns>
        private bool ValidateForm()
        {
            bool bValidate = false;

            if (Convert.ToString(txtFromDate.Text).Length == 0)
            {
                ShowMsgBox("Please Select From Date");
                return bValidate;
            }
            if (Convert.ToString(txtToDate.Text).Length == 0)
            {
                ShowMsgBox("Please Select To Date");
                return bValidate;
            }
            
            bValidate = true;
            return bValidate;
        }


        /// <summary>
        /// For reseting the text field Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void cmdReset_Click(object sender, EventArgs e)
        {
            cmbCircle.SelectedIndex = 0;
            cmbDiv.Items.Clear();
            txtFromDate.Text = string.Empty;
            txtToDate.Text = string.Empty;
        }

        /// <summary>
        /// For displaying Msg in pop up
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
                    System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex.StackTrace);
            }
        }

    }
}