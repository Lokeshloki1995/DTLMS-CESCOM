﻿<%@ Page Title="" Language="C#" MasterPageFile="~/DTLMS.Master" AutoEventWireup="true" CodeBehind="DivisionView.aspx.cs" Inherits="IIITS.DTLMS.BasicForms.DivisionView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip();
        });
        function onlyNumbers(event) {
            var charCode = (event.which) ? event.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        //Remove Special charactes and Charactes
        function cleanCharAndSpecial(t) {
            t.value = t.value.toString().replace(/[^0-9]+/g, '');
        }

        //Charactes and space 
        function characterAndspecial(event) {
            var evt = (evt) ? evt : event;
            var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
            ((evt.which) ? evt.which : 0));
            if ((charCode < 65 || charCode > 90) &&
             (charCode < 97 || charCode > 122) && charCode != 32) {

                return false;
            }
            return true;
        }
        //Remove Numbers, Special characters except space
        function cleanSpecialAndNum(t) {

            t.value = t.value.toString().replace(/[^a-zA-Z \t\n\r]+/g, '');


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
                    <h3 class="page-title">Division View
                    </h3>

                    <a style="margin-right: -372px!important; float: right!important" href="#" data-toggle="modal" data-target="#myModal" title="Click For Help">Help <i class="fa fa-exclamation-circle" style="font-size: 16px"></i></a>

                    <ul class="breadcrumb" style="display: none">

                        <li class="pull-right search-wrap">
                            <form action="" class="hidden-phone">
                                <div class="input-append search-input-area">
                                    <input class="" id="appendedInputButton" type="text">
                                    <button class="btn" type="button"><i class="icon-search"></i></button>
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
                            <h4><i class="icon-reorder"></i>Division View</h4>

                            <span class="tools">
                                <a href="javascript:;" class="icon-chevron-down"></a>
                                <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">



                            <div style="float: right">
                                <div class="span6">
                                    <asp:Button ID="cmdNewDivision" class="btn btn-primary" Text="New Division"
                                        runat="server" OnClick="cmdNewDivision_Click" />

                                    <br />
                                </div>
                                <div class="span1">
                                    <asp:Button ID="cmdexport" runat="server" Text="Export Excel" CssClass="btn btn-primary"
                                        OnClick="Export_clicKDivision" /><br />
                                </div>

                            </div>


                            <div class="space20"></div>
                            <div>

                                <div>

                                    <asp:GridView ID="grdDivisionMaster" AutoGenerateColumns="false" PageSize="5" AllowPaging="true"
                                        ShowFooter="true" EmptyDataText="No Records Found"
                                        CssClass="table table-striped table-bordered table-advance table-hover"
                                        runat="server" ShowHeaderWhenEmpty="True" OnPageIndexChanging="grdDivisionMaster_PageIndexChanging"
                                        OnRowCommand="grdDivisionMaster_RowCommand" OnSorting="grdDivisionMaster_Sorting" AllowSorting="true">
                                        <HeaderStyle CssClass="both" />
                                        <Columns>

                                            <asp:TemplateField AccessibleHeaderText="DIV_ID" HeaderText="Division Id" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivID" runat="server" Text='<%# Bind("DIV_ID") %>' Style="word-break: break-all;"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField AccessibleHeaderText="DIV_CODE" HeaderText="Division Code" Visible="true" SortExpression="DIV_CODE">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDivCode" runat="server" Text='<%# Bind("DIV_CODE") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivCode" runat="server" Text='<%# Bind("DIV_CODE") %>' Style="word-break: break-all;"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch">
                                                        <asp:TextBox ID="txtDivisionCode" runat="server" placeholder="Enter Division Code" Width="150px" CssClass="span12" MaxLength="3" onkeypress="return onlyNumbers(event)" onchange="cleanCharAndSpecial(this)"></asp:TextBox>
                                                    </asp:Panel>
                                                </FooterTemplate>

                                            </asp:TemplateField>


                                            <asp:TemplateField AccessibleHeaderText="DIV_NAME" HeaderText="Division Name"
                                                Visible="true" SortExpression="DIV_NAME">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtDivName" runat="server" Text='<%# Bind("DIV_NAME") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDivName" runat="server" Text='<%# Bind("DIV_NAME") %>' Style="word-break: break-all;"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch">
                                                        <asp:TextBox ID="txtDivisionName" runat="server" placeholder="Enter Division Name" Width="150px" CssClass="span12"  onkeypress="return characterAndspecial(event)"  
                                        onchange = " return cleanSpecialAndNum(this)"></asp:TextBox>
                                                        <asp:ImageButton ID="imgBtnSearch" runat="server" ImageUrl="~/img/Manual/search.png" CommandName="search" />
                                                    </asp:Panel>
                                                </FooterTemplate>
                                            </asp:TemplateField>




                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <center>
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" OnClick="imgBtnEdit_Click" ImageUrl="~/Styles/images/edit64x64.png"
                                                            Width="12px" />
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
                    <ul>
                        <li>This Web Page Can Be Used To View Division Details and To Add New Division </li>
                        <li>To Edit Existing Details Click On <u>Edit</u> LiknkButton</li>
                        <li>To Add New Division Click On <u>New Division</u> LiknkButton
                        </li>
                    </ul>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
