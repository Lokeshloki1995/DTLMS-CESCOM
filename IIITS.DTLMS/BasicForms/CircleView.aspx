﻿<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true"
    CodeBehind="CircleView.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.CircleView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <script type="text/javascript">

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });

        function onlyNumbers(event) {
            debugger;
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //Remove Special charactes and Charactes
        function cleanCharAndSpecial(t) {
            t.value = t.value.toString().replace(/[^0-9]+/g, '');
        }
        //Charactes and space &
        function characterAndspecial(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122)  && charCode != 32 && charCode != 38) {

                return false;
            }
            return true;
        }
        //Remove Numbers, Special characters except & and space
        function cleanSpecialAndNum(t) {

            t.value = t.value.toString().replace(/[^a-zA-Z & \t\n\r]+/g, '');


        }
        
    </script>


    <style type="text/css">
        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .ascending th a {
            background: url(/img/sort_asc.png) no-repeat;
            display: block;
            padding: 0px 4px 0 20px;
        }

        .descending th a {
            background: url(/img/sort_desc.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }

        .both th a {
            background: url(/img/sort_both.png) no-repeat;
            display: block;
            padding: 0 4px 0 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div class="container-fluid">
            <%--BEGIN PAGE HEADER--%>
            <div class="row-fluid">
                <div class="span8">
                    <!-- BEGIN THEME CUSTOMIZER-->
                    <!-- END THEME CUSTOMIZER-->
                    <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                    <h3 class="page-title">Circle View
                    </h3>
                    <a style="margin-right:-372px!important;float:right!important"href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size:16px"></i></a>
                    <ul class="breadcrumb" style="display: none">
                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                            </form>
                        </li>
                    </ul>
                    <!-- END PAGE TITLE & BREADCRUMB-->
                </div>
            </div>
            <!-- END PAGE HEADER-->
            <!-- BEGIN PAGE CONTENT-->
            <div class="row-fluid">
                <div class="span12">
                    <!-- BEGIN SAMPLE FORMPORTLET-->
                    <div class="widget blue">
                        <div class="widget-title">
                            <h4>
                                <i class="icon-reorder"></i>Circle View</h4>
                            <span class="tools"><a href="javascript:;" class="icon-chevron-down"></a><a href="javascript:;"
                                class="icon-remove"></a></span>
                        </div>
                        <div class="widget-body">

                            <div style="float: right">
                                <div class="span6">
                                    <asp:Button ID="cmdNewCircle" class="btn btn-primary" Text="New Circle" runat="server"
                                        OnClick="cmdNewCircle_Click" />
                                </div>
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                        OnClick="Export_clicKcircle" /><br />
                                </div>
                            </div>
                            <div class="space20">
                            </div>
                            <div>
                                <asp:GridView ID="grdCirclemaster" AutoGenerateColumns="false" PageSize="5" AllowPaging="true"
                                    ShowFooter="true" EmptyDataText="No Records Found" CssClass="table table-striped table-bordered table-advance table-hover"
                                    runat="server" ShowHeaderWhenEmpty="True" OnPageIndexChanging="grdCirclemaster_PageIndexChanging"
                                    OnRowCommand="grdCirclemaster_RowCommand" OnSorting="grdCirclemaster_Sorting" AllowSorting="true">
                                    <HeaderStyle CssClass="both" />
                                    <Columns>
                                        <asp:TemplateField AccessibleHeaderText="CM_ID" HeaderText="Id" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCirId" runat="server" Text='<%# Bind("CM_CIRCLE_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CM_CIRCLE_CODE" HeaderText="Circle Code"
                                            Visible="true" SortExpression="CM_CIRCLE_CODE">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCircleCode" runat="server" Text='<%# Bind("CM_CIRCLE_CODE") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircleCode" runat="server" Text='<%# Bind("CM_CIRCLE_CODE") %>'
                                                    Style="word-break: break-all;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtCircleCode" runat="server" placeholder="Enter Circle Code" Width="150px" MaxLength="2"
                                                        CssClass="span12" onkeypress="return onlyNumbers(event)" onchange = "cleanCharAndSpecial(this)"></asp:TextBox>
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField AccessibleHeaderText="CM_CIRCLE_NAME" HeaderText="Circle Name"
                                            Visible="true" SortExpression="CM_CIRCLE_NAME">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCircleName" runat="server" Text='<%# Bind("CM_CIRCLE_NAME") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCircleName" runat="server" Text='<%# Bind("CM_CIRCLE_NAME") %>'
                                                    Style="word-break: break-all;" Width="200px"></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                    <asp:TextBox ID="txtCircleName" runat="server" placeholder="Enter Circle Name" Width="150px"
                                                        CssClass="span12"  onkeypress="return characterAndspecial(event)"  
                                        onchange = " return cleanSpecialAndNum(this)" ></asp:TextBox>
                                                    <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png"
                                                        CommandName="search" />
                                                </asp:Panel>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <center>
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" OnClick="imgBtnEdit_Click"
                                                        ImageUrl="~/Styles/images/edit64x64.png" Width="12px" />
                                                </center>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                </div>
            </div>
            <!-- END FORM-->
            <!-- END PAGE CONTENT-->
        </div>
    </div>

    <!-- MODAL-->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title">Help</h4>
                </div>
                <div class="modal-body">
                    <ul><li>
                   This Web Page Can Be Used To View Circle Details and To Add New Circle </li>
              <li>  To Edit Existing Details Click On <u>Edit</u> LiknkButton</li>
                 <li>  To Add New Circle Click On <u>New Circle</u> LiknkButton
                   </li></ul>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

</asp:Content>
