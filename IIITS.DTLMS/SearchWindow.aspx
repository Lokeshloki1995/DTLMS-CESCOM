﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchWindow.aspx.cs" Inherits="IIITS.DTLMS.SearchWindow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="../Scripts/functions.js" type="text/javascript"></script>
    <link href="../assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="../css/style.css" rel="stylesheet" />

    <script type="text/javascript">
        function characterAndspecialAndNum(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && (charCode < 48 || charCode > 57) && charCode != 32 
                && charCode != 46 && charCode != 47 && charCode != 95 && (charCode < 44 || charCode > 46)) {

                return false;
            }
            return true;
        }
        function nopaste() {

            return false;
        }
        //Remove Special charactes except ._/ and space
        function cleanSpecial(t) {

            t.value = t.value.toString().replace(/[^a-zA-Z 0-9 _ ./\t\n\r\-]+/g, '');
        }
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel ID="pnlControls" runat="server" Height="520px" Width="520px" BackColor="White">
            <div class="modal-body">
                <div class="widget-body">

                    <div class="widget-body form">
                        <div class="form-horizontal" align="center">
                            <asp:Label ID="lblTitle" runat="server" Text="Label" Font-Bold="True"></asp:Label>
                        </div>
                        <!-- BEGIN FORM-->
                        <div class="form-horizontal">
                            <div class="row-fluid">
                                <div class="span1"></div>
                                <div class="span5">

                                    <div class="control-group">
                                        <label class="control-label">Search By</label>
                                        <div class="controls">
                                            <div class="input-append">

                                                <asp:DropDownList ID="cmbFilterType" runat="server" TabIndex="1">
                                                    <asp:ListItem>-- Select Type --</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="control-group">
                                        <label class="control-label">Search Value</label>
                                        <div class="controls">
                                            <div class="input-append">
                                                <asp:TextBox ID="txtSearch" onchange=" return cleanSpecial(this)" runat="server" TabIndex="2" onkeypress="return characterAndspecialAndNum(event);"></asp:TextBox>
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" TabIndex="3" />

                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="span1"></div>
                            </div>


                            <div class="form-horizontal" align="center">

                                <asp:Label ID="lblMsg" runat="server" Font-Size="Small" ForeColor="Red"></asp:Label>
                                <asp:RequiredFieldValidator ID="rfvCombo" runat="server" ErrorMessage="Please Select Type"
                                    ControlToValidate="cmbFilterType" InitialValue="-- Select Type --" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <%--  <asp:RequiredFieldValidator ID="rfvText" runat="server" ErrorMessage="Enter The Value"
                  ControlToValidate="txtSearch"></asp:RequiredFieldValidator>--%>
                            </div>

                            <div class="form-horizontal" align="center">
                                <asp:Table ID="tblResult" runat="server" CssClass="table table-striped table-bordered table-advance table-hover">
                                </asp:Table>
                            </div>
                        </div>
                    </div>



                </div>

            </div>



        </asp:Panel>
    </form>
</body>
</html>
