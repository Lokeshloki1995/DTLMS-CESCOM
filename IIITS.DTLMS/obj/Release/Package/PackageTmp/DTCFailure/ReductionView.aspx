﻿<%@ Page Language="C#" MasterPageFile="~/DTLMS.Master"   AutoEventWireup="true" CodeBehind="ReductionView.aspx.cs" Inherits="IIITS.DTLMS.DTCFailure.ReductionView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
        .modalBackground
        {
            background-color: Gray;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

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
          //DTR allow to enter nums for 6 digits
          function onlyNumbers(event) {
              var charCode = (event.which) ? event.which : event.keyCode
              if (charCode > 31 && (charCode < 48 || charCode > 57))
                  return false;

              return true;
          }

          // DTR allow only Numbers to paste
          function cleanSpecialAndChar(t) {
              debugger;
              t.value = t.value.toString().replace(/[^0-9\n\r]+/g, '');
              //alert(" Special charactes and characters are not allowed!");
          }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></ajax:ToolkitScriptManager>
 <div class="container-fluid">
            <!-- BEGIN PAGE HEADER-->
            <div class="row-fluid">
               <div class="span12">
                   <!-- BEGIN THEME CUSTOMIZER-->
                 
                   <!-- END THEME CUSTOMIZER-->
                  <!-- BEGIN PAGE TITLE & BREADCRUMB-->
                   <h3 class="page-title">
                   Capacity Reduction View
                   </h3>
                       <a href="#" data-toggle="modal" data-target="#myModal" title="Click For Help" > <i class="fa fa-exclamation-circle" style="font-size: 36px"></i></a>
                   <ul class="breadcrumb" style="display:none">
                       
                       <li class="pull-right search-wrap">
                           <form action="" class="hidden-phone">
                               <div class="input-append search-input-area">
                                   <input class="" id="appendedInputButton" type="text">
                                   <button class="btn" type="button"><i class="icon-search"></i> </button>
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
                            <h4><i class="icon-reorder"></i>Reduction Details View</h4>
                            <span class="tools">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                            <a href="javascript:;" class="icon-remove"></a>
                            </span>
                        </div>
                        <div class="widget-body">
                         <div class="form-horizontal">
                                    <div class="row-fluid">
                       <%-- <div style="float:left" >--%>
                              <%--  <div class="span8">--%>
                              <div class="span2">
                              <asp:Label ID="lblStatus" runat="server" Text="Filter By :" Font-Bold="true" 
                                        Font-Size="Medium"></asp:Label>
                              </div>
                          <div class="span1">
                            <asp:RadioButton ID="rdbViewAll" runat="server" Text="View All" CssClass="radio" 
                                  GroupName="a" Checked="true" AutoPostBack="true" 
                                  oncheckedchanged="rdbViewAll_CheckedChanged" />
                          </div>
                           <div class="span4">
                              <asp:RadioButton ID="rdbAlready" runat="server"  Text="Already Created" 
                                   CssClass="radio" GroupName="a"  AutoPostBack="true"
                                   oncheckedchanged="rdbAlready_CheckedChanged" />
                            </div>

                             <div style="float:right;">
                                   <div class="span4">
                                 <asp:Button ID="cmdNew" runat="server" Text="New"  
                                       CssClass="btn btn-primary"  onclick="cmdNew_Click"/></div>
                                   <div class="span1">
                         <asp:Button ID="cmbExport" runat="server" Text="Export To Excel" CssClass="btn btn-primary"
                        OnClick="Export_ClickEnhancement" /><br />
                         </div>
                             </div>

                                    <%--<asp:Label ID="lblStatus" runat="server" Text="Status" Font-Bold="False" 
                                        Font-Size="Medium"></asp:Label>

                                    &nbsp;&nbsp;&nbsp;&nbsp;

                                    <asp:DropDownList ID="cmbIndexSelection" runat="server" AutoPostBack="true" 
                                        onselectedindexchanged="cmbIndexSelection_SelectedIndexChanged" >
                                 
                                     <asp:ListItem Text="To Be Created" Value="0"></asp:ListItem>
                                      <asp:ListItem Text="Already Created" Value="1"></asp:ListItem>
                                   </asp:DropDownList>--%>
                                   
                                   
                                <%--   </div>--%>
                      </div>
                        </div>
                        </div>
                                <!-- END FORM-->
                           

                 <asp:GridView ID="grdEnhancement" AutoGenerateColumns="false" PageSize="10" 
                  AllowPaging="true"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" 
                 CssClass="table table-striped table-bordered table-advance table-hover" ShowFooter="true"
                  runat="server" onpageindexchanging="grdEnhancement_PageIndexChanging" 
                            onrowcommand="grdEnhancement_RowCommand"   onrowdatabound="grdEnhancement_RowDataBound"
                             OnSorting="grdEnhancement_Sorting" AllowSorting="true">
                                <HeaderStyle CssClass="both" /> 
    
                    <Columns>

                     <asp:TemplateField AccessibleHeaderText="DF_ID" HeaderText="Id" Visible="false">
           
                            <ItemTemplate>                                       
                                <asp:Label ID="lblEnhanceId" runat="server" Text='<%# Bind("DF_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                    <asp:TemplateField AccessibleHeaderText="DT_ID" HeaderText="Id" Visible="false" >
                            <ItemTemplate>              
                                <asp:Label ID="lblDTCId" runat="server" Text='<%# Bind("DT_ID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="DT_CODE" HeaderText="DTC Code" SortExpression="DT_CODE">
                            <ItemTemplate>
                                <asp:Label ID="lblDtcCode" runat="server" Text='<%# Bind("DT_CODE") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                             <asp:Panel ID="panel1" runat="server" DefaultButton="imgBtnSearch" >
                                 <asp:TextBox ID="txtDtCode" runat="server" placeholder="Enter DTC Code " Width="150px" MaxLength="6" onkeypress="return characterAndnumbers(event)" onchange = "return cleanSpecialChars(this)"  ></asp:TextBox>
                            </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField AccessibleHeaderText="DT_NAME" HeaderText="DTC Name" SortExpression="DT_NAME">
                            <ItemTemplate>
                                <asp:Label ID="lblname" runat="server" Text='<%# Bind("DT_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                            </ItemTemplate>
                             <FooterTemplate>
                               <asp:Panel ID="panel2" runat="server" DefaultButton="imgBtnSearch" >
                                   <asp:TextBox ID="txtDtName" runat="server" placeholder="Enter DTC Name" Width="150px" MaxLength="50" onkeypress="return characterAndspecialDtc(event)"  
                                        onchange = " return cleanSpecialAndNumDtc(this)" ></asp:TextBox>
                              </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>


                           <asp:TemplateField AccessibleHeaderText="TC_CODE" HeaderText="DTR Code">          
                            <ItemTemplate>                                                 
                                <asp:Label ID="lblTcCode" runat="server" Text='<%# Bind("TC_CODE") %>' style="word-break: break-all;" width="100px"></asp:Label>
                            </ItemTemplate>
                              <FooterTemplate>
                                <asp:Panel ID="panel3" runat="server" DefaultButton="imgBtnSearch" >
                                    <asp:TextBox ID="txtDtrCode" runat="server" placeholder="Enter Dtr Code" Width="150px" MaxLength="6" onkeypress="return onlyNumbers(event)" onchange = "return cleanSpecialAndChar(this)" ></asp:TextBox>
                             </asp:Panel>
                             </FooterTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="TC_SLNO" HeaderText="DTr Slno" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblslno" runat="server" Text='<%# Bind("TC_SLNO") %>' style="word-break: break-all;" width="100px"></asp:Label>
                            </ItemTemplate>
                           
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="TM_NAME" HeaderText="Make Name" SortExpression="TM_NAME">
                            <ItemTemplate>
                                <asp:Label ID="lbltmname" runat="server" Text='<%# Bind("TM_NAME") %>' style="word-break: break-all;" width="150px"></asp:Label>
                            </ItemTemplate>
                              <FooterTemplate>
                                   <asp:ImageButton  ID="imgBtnSearch" runat="server"  ImageUrl="~/img/Manual/search.png"  CommandName="search" />
                            </FooterTemplate>
                        </asp:TemplateField>


                        <asp:TemplateField AccessibleHeaderText="TC_CAPACITY" HeaderText="Capacity(in KVA)" >
                           
                            <ItemTemplate>
                                <asp:Label ID="lblcapacity" runat="server" Text='<%# Bind("TC_CAPACITY") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField AccessibleHeaderText="" HeaderText="Enhancement Entry" Visible="false">           
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("STATUS") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

     
                        <asp:TemplateField HeaderText="Action" >
                            <ItemTemplate>
                                <center>
                                   <%-- <asp:ImageButton ID="imgBtnEdit" runat="server" Height="12px" ImageUrl="~/Styles/images/edit64x64.png"
                                        Width="12px" />--%>
                                      <asp:LinkButton runat="server"  CommandName="Create" ID="lnkEdit" >
                                      <img src="../Styles/images/Create.png" style="width:20px" />Declare
                                      </asp:LinkButton>
                                      <asp:LinkButton runat="server"  CommandName="UpdateEnhance" ID="lnkUpdate"  visible="false" >
                                      <img src="../img/Manual/view.png" style="width:20px" />View
                                      </asp:LinkButton>
                                     <asp:LinkButton runat="server"  ID="lnkWaiting"  visible="false" style="border-style:none">
                                      <img src="../img/Manual/Wait.png" style="width:20px" />Waiting for Approval</asp:LinkButton>
                                       <asp:LinkButton runat="server" CommandName="Preview"  ID="lnkEstPrev"  visible="false" >
                                      <img id="Img1" src="../img/Manual/Pdficon.png" style="width:20px" /></asp:LinkButton>

                                </center>
                            </ItemTemplate>
                            <HeaderTemplate>
                                <center>
                                    <asp:Label ID="lblHeader" runat="server" Text="Action" ></asp:Label>
                                </center>
                            </HeaderTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField AccessibleHeaderText="DT_PROJECTTYPE" HeaderText="Project Type" Visible="false">                                       
                            <ItemTemplate>
                                <asp:Label ID="lblProjectType" runat="server" Text='<%# Bind("DT_PROJECTTYPE") %>' style="word-break: break-all;" width="60px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
      
                    </Columns>
                </asp:GridView>

                        <div class="span7"></div>
                                 <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                          </div>
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
                    <h4 class="modal-title">
                        Help</h4>
                </div>
                <div class="modal-body">
                    <p style="color: Black">
                        <i class="fa fa-info-circle"></i>This Web Page Can Be Used To View All Capacity Reduction Details and To Declare Capacity Reduction
                        </p>
                         <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To Declare Capacity Reduction Click On Declare LinkButton Button
                        </p>
                        <p style="color: Black">
                        <i class="fa fa-info-circle"></i>To View Already Created Capacity Reduction Details Click on Already Created Radio Button
                        </p>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- MODAL-->

</asp:Content>
