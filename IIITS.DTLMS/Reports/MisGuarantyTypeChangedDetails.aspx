﻿<%@ Page Language="C#"  MasterPageFile="~/DTLMS.Master"   AutoEventWireup="true" CodeBehind="MisGuarantyTypeChangedDetails.aspx.cs" 
    Inherits="IIITS.DTLMS.Reports.MisGuarantyTypeChangedDetails" %>


            if (document.getElementById('<%= txtFromDate.ClientID %>').value.trim() == "") {
                alert('Please select the From date')
                document.getElementById('<%= txtFromDate.ClientID %>').focus()
                return false
            }

            if (document.getElementById('<%= txtToDate.ClientID %>').value.trim() == "") {
                alert('Please select the To date')
                document.getElementById('<%= txtToDate.ClientID %>').focus()
                return false
            }
        }
    </asp:ScriptManager>