﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FailurePenddingDetails.aspx.cs"
    Inherits="IIITS.DTLMS.DashboardForm.FailurePenddingDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="/assets/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/assets/bootstrap/css/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="/assets/bootstrap/css/bootstrap-fileupload.css" rel="stylesheet" />
    <link href="/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="/css/style.css" rel="stylesheet" />
    <link href="/css/style-responsive.css" rel="stylesheet" />
    <link href="/css/style-default.css" rel="stylesheet" id="style_color" />
    <link href="/assets/fullcalendar/fullcalendar/bootstrap-fullcalendar.css" rel="stylesheet" />
    <link href="/assets/jquery-easy-pie-chart/jquery.easy-pie-chart.css" rel="stylesheet"
        type="text/css" media="screen" />
    <link href="Styles/calendar.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/functions.js"></script>

    <style type="text/css">
       

         .ascending th a {
        background:url(/img/sort_asc.png) no-repeat;
        display: block;
        padding: 0px 4px 0 20px;
    }

    .descending th a {
        background:url(/img/sort_desc.png) no-repeat;
        display: block;
        padding: 0 4px 0 20px;
    }
     .both th a {
         background: url(/img/sort_both.png) no-repeat;
         display: block;
          padding: 0 4px 0 20px;
     }
    </style>
    <title></title>
    <script type="text/javascript">
        //DTC allow to search chars and nums for 6
        function characterAndnumbers(evt) {
            evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if (charCode > 31 && (charCode < 65 || charCode > 90) &&
            (charCode < 97 || charCode > 122) && charCode > 31 && (charCode < 48 || charCode > 57)) {

                return false;
            }
            return true;
        }

        // Dtc allow Chatractes and Numbers to paste
        function cleanSpecialChars(t) {
            debugger;
            t.value = t.value.toString().replace(/[^a-zA-Z 0-9\t\n\r]+/g, '');
            //alert(" Special charactes are not allowed!");
        }
        //Charactes and space() - . / 0 1 2 3 4 7  to search DTC Name
        function characterAndspecialDtc(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode != 32 && charCode != 40 && charCode != 41 && charCode != 45 && charCode != 46 && charCode != 47 && (charCode < 48 || charCode > 55)) {

                return false;
            }
            return true;
        }
        //Remove Numbers, Special characters except space to search DTC Name
        function cleanSpecialAndNumDtc(t) {

            t.value = t.value.toString().replace(/[^-./()a-zA-Z0-7\t\n\r]+/g, '');


        }
        </script>

    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
                <%--<div class="span12">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">
                        Failure Pending Details
                    </h3>
                    <ul class="breadcrumb" style="display: none;">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                            <div class="input-append search-input-area">
                                <input class="" id="Text1" type="text">
                                <button class="btn" type="button">
                                    <i class="icon-search"></i>
                                </button>
                            </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>--%>
            </div>
            <div style="float: right; margin-top: 2px;">
                <div class="span2">
                    
                </div>
                <asp:HiddenField ID="hdfOffCode" runat="server" />
            </div>
            <div class="space10">
            </div>
            <div class="row-fluid" style="width: 1305px;">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title" >
                            <h4>
                                <i class="icon-reorder"></i>Failure Pending Details</h4>
                            <%--<span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>--%>
                        </div>
                        <div class="widget-body">
                            <div style="float: right">
                            </div>
                            <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="cmbExport_Click" /><br />
                            <!-- END FORM-->
                            <%--                                            <div class="space20"></div>--%>
                            <div id="Gridview1" class="row-fluid" style="width: 1280px; height: 500px; overflow: auto">
                                <asp:GridView ID="grdFailurePending" HeaderStyle-CssClass="both" AutoGenerateColumns="false"
                                    PageSize="9" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true"
                                    CssClass="table table-striped table-bordered table-advance table-hover" AllowPaging="true"
                                    runat="server" OnPageIndexChanging="grdFailurePending_PageIndexChanging" OnRowCreated="grdFailurePending_RowCreated"
                                    OnRowCommand="grdFailurePending_RowCommand" OnSorting="grdFailurePending_Sorting"
                                    AllowSorting="true">
                                    
                                    <%--  <HeaderStyle HorizontalAlign="center" CssClass="FixedHeader" />--%>
                                    <%-- <HeaderStyle CssClass="both" />--%>
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" Visible="true" SortExpression="DT_CODE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>' Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="100px" onpaste="return false"
                                                       MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange ="return cleanSpecialChars(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="DTC Name" Visible="true" SortExpression="DT_NAME">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDtcName" runat="server" Text='<%# Bind("DT_NAME") %>' Style="word-break: break-all;"
                                                    Width="250px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtDTCName" runat="server" placeholder="Enter DTC Name " Width="150px" onpaste="return false"
                                                        onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="OMSECTION" HeaderText="Section Name" Visible="true" SortExpression="OMSECTION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmSectionName" runat="server" Text='<%# Bind("OMSECTION") %>' Style="word-break: break-all;"
                                                    Width="180px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                    CommandName="search" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                      <%--  <asp:TemplateField AccessibleHeaderText="OM_CODE" HeaderText="OM Code" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOmCode" runat="server" Text='<%# Bind("OM_CODE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField AccessibleHeaderText="SUBDIVSION" HeaderText="Sub-Division Name" SortExpression="SUBDIVSION">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSubDivName" runat="server" Text='<%# Bind("SUBDIVSION") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Failure No" SortExpression="DF_ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailureNo" runat="server" Text='<%# Bind("DF_ID") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="DF_DATE" HeaderText="Failure Date" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailureDate" runat="server" Text='<%# Bind("DF_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="EST_NO" HeaderText="Estimation No" SortExpression="EST_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimationNo" runat="server" Text='<%# Bind("EST_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="EST_CRON" HeaderText="Estimation Date" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstimationDate" runat="server" Text='<%# Bind("EST_CRON") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="FL_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFailStatus" runat="server" Text='<%# Bind("FL_STATUS") %>' Style="word-break: break-all;"
                                                    Width="220px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_NO" HeaderText="WO NO/Comm" SortExpression="WO_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONO" runat="server" Text='<%# Bind("WO_NO") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField AccessibleHeaderText="WO_NO/De_Comm" HeaderText="WO No/De-Comm">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONODec" runat="server" Text='<%# Bind("WO_NO_DECOM") %>' Style="word-break: break-all;"
                                                    Width="150px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_DATE" HeaderText="WO Date" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODATE" runat="server" Text='<%# Bind("WO_DATE") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="WO_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOStatus" runat="server" Text='<%# Bind("WO_STATUS") %>' Style="word-break: break-all;"
                                                    Width="235px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_NO" HeaderText="Indent No" SortExpression="TI_INDENT_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentNo" runat="server" Text='<%# Bind("TI_INDENT_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="TI_INDENT_DATE" HeaderText="Indent Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentDATE" runat="server" Text='<%# Bind("TI_INDENT_DATE") %>'
                                                    Style="word-break: break-all;" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="INDT_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndentStatus" runat="server" Text='<%# Bind("INDT_STATUS") %>'
                                                    Style="word-break: break-all;" Width="220px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="IN_INV_NO" HeaderText="Invoice No" SortExpression="IN_INV_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Bind("IN_INV_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="IN_DATE" HeaderText="Commission" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblDATE" runat="server" Text='<%# Bind("IN_DATE") %>' Style="word-break: break-all;"
                                                    Width="90px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="INV_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvStatus" runat="server" Text='<%# Bind("INV_STATUS") %>' Style="word-break: break-all;"
                                                    Width="230px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                      <%--  <asp:TemplateField AccessibleHeaderText="TR_RI_NO" HeaderText="RI No" SortExpression="TR_RI_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblriNo" runat="server" Text='<%# Bind("TR_RI_NO") %>' Style="word-break: break-all;"
                                                    Width="100px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                       <%-- <asp:TemplateField AccessibleHeaderText="TR_RI_DATE" HeaderText="RI Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRiDATE" runat="server" Text='<%# Bind("TR_RI_DATE") %>' Style="word-break: break-all;"
                                                    Width="90px"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    <%--    <asp:TemplateField AccessibleHeaderText="RI_STATUS" HeaderText="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRIStatus" runat="server" Text='<%# Bind("RI_STATUS") %>' Style="word-break: break-all;"
                                                    Width="220px" ForeColor="#77808a"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CR_STATUS" HeaderText="Status" >
            <ItemTemplate>
                <asp:Label ID="lblCRStatus" runat="server" Text='<%# Bind("CR_STATUS") %>' style="word-break: break-all;"  width="200px" ForeColor="#ab8465" ></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>--%>
                                    </Columns>

<%--<HeaderStyle CssClass="Warning" Height="5px" HorizontalAlign="Center"></HeaderStyle>--%>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!-- END SAMPLE FORM PORTLET-->
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
