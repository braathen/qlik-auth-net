<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AuthenticationModuleDemo.Default" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <title>Ticket API Demo</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="Content/bootstrap.min.css">
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
</head>
<body>

    <div class="container">
        <div class="row">
            <div class="col-md-8">
                <h2>Ticket API Demo</h2>
                <form role="form" method="POST" runat="server">

                    <div class="panel panel-default" id="panel1">
                        <div class="panel-heading">
                            <h3 class="panel-title">Step 1</h3>
                        </div>
                        <div class="panel-body">

                            <label>Parameters retrieved from URL:</label>
                            <table class="table table-bordered">
                                <tr>
                                    <td class="active">proxyRestUri</td>
                                    <td id="frmProxyRestUri" runat="server"></td>
                                </tr>
                                <tr>
                                    <td class="active">targetId</td>
                                    <td id="frmTargetId" runat="server"></td>
                                </tr>
                            </table>

                            <div class="form-group">
                                <label>UserDirectory:</label>
                                <input type="text" class="form-control" id="frmUserDirectory" runat="server" />
                            </div>
                            <div class="form-group">
                                <label>Username:</label>
                                <input type="text" class="form-control" id="frmUserId" runat="server" />
                            </div>

                            <div class="row">
                                <div class="col-sm-1">&nbsp;</div>
                            </div>

                            <label class="control-label">Attributes</label>
                            (semicolon separated):
                            <div class="form-group">
                                <div class="col-sm-2" style="padding-left: 0; padding-right: 5px;">
                                    <select id="frmAttrib1" name="frmAttribute" class="form-control" runat="server">
                                        <option value="Group" selected="selected">Group</option>
                                        <option value="Email">Email</option>
                                        <option value="Phone">Phone</option>
                                        <option value="Title">Title</option>
                                        <option value="Company">Company</option>
                                        <option value="Office">Office</option>
                                        <option value="Country">Country</option>
                                    </select>
                                </div>
                                <div class="col-sm-10" style="padding-right: 0; padding-left: 0;">
                                    <input id="frmList1" name="frmList" type="text" class="form-control" style="width: 100%;" runat="server" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-1">&nbsp;</div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-2" style="padding-left: 0; padding-right: 5px;">
                                    <select id="frmAttrib2" name="frmAttribute" class="form-control" runat="server">
                                        <option value="Group">Group</option>
                                        <option value="Email" selected="selected">Email</option>
                                        <option value="Phone">Phone</option>
                                        <option value="Title">Title</option>
                                        <option value="Company">Company</option>
                                        <option value="Office">Office</option>
                                        <option value="Country">Country</option>
                                    </select>
                                </div>
                                <div class="col-sm-10" style="padding-right: 0; padding-left: 0;">
                                    <input id="frmList2" name="frmList" type="text" class="form-control" style="width: 100%;" runat="server" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-1">&nbsp;</div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-2" style="padding-left: 0; padding-right: 5px;">
                                    <select id="frmAttrib3" name="frmAttribute" class="form-control" runat="server">
                                        <option value="Group">Group</option>
                                        <option value="Email">Email</option>
                                        <option value="Phone" selected="selected">Phone</option>
                                        <option value="Title">Title</option>
                                        <option value="Company">Company</option>
                                        <option value="Office">Office</option>
                                        <option value="Country">Country</option>
                                    </select>
                                </div>
                                <div class="col-sm-10" style="padding-right: 0; padding-left: 0;">
                                    <input id="frmList3" name="frmList" type="text" class="form-control" style="width: 100%;" runat="server" />
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-sm-1">&nbsp;</div>
                            </div>

                            <div class="form-group">
                                <button type="submit" class="btn btn-success" name="submit" value="buildrequest" runat="server" id="btnStep1">Build request data</button>
                            </div>
                        </div>
                    </div>

                    <div class="panel panel-default" id="panel2" runat="server" visible="false">
                        <div class="panel-heading">
                            <h3 class="panel-title">Step 2</h3>
                        </div>
                        <div class="panel-body">
                            <div id="step1" runat="server">
                                <div class="form-group">
                                    <div>Using certificate <i>QlikClient</i> from local certificate store to authenticate the ticket request.</div>
                                    <div class="col-sm-16">&nbsp;</div>
                                    <label>Request Data:</label>
                                    <pre id="frmRequestString" runat="server"></pre>
                                </div>
                            </div>
                            <div class="form-group">
                                <button type="submit" class="btn btn-success" name="submit" value="sendrequest" runat="server" id="btnStep2">Send request</button>
                            </div>
                            <div class="form-group" runat="server" visible="false" id="errorpanel">
                                <label>An error occured:</label>
                                <pre id="frmError" runat="server"></pre>
                            </div>
                        </div>
                    </div>

                </form>
            </div>
        </div>

    </div>

</body>
</html>
