<uc2:StyleHeader ID="StyleHeader8" runat="server" />
<uc3:StyleTitle ID="StyleTitle8" runat="server" Title="安全控管" />
<uc4:StyleContentStart ID="StyleContentStart8" runat="server" />
<table width="100%">
	<TR><TD><asp:DetailsView ID="DetailsView08" runat="server"  DataKeyNames="Company,EmployeeId"
			  DataSourceID="SqlDataSource8" Width="100%" OnDataBound="DetailsView08_DataBound"
			  OnItemInserting="DetailsView08_ItemInserting" OnItemInserted="DetailsView08_ItemInserted"
			  OnItemUpdating="DetailsView08_ItemUpdating" OnItemUpdated="DetailsView08_ItemUpdated"
			  AutoGenerateRows="False">
				<Fields>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:Panel runat="server" ID="Master" width="100%">
								<table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
									<tr class="button_bar_cell" align="center">
										<td class="Grid_GridLine">
											<asp:Label ID="lblTitle_ERPID" runat="server" Text="ERPID" />
										</td>
                    <td class="Grid_GridLine">
                      <asp:Label ID="lblTitle_ADAcc" runat="server" Text="AD帳號" />
                    </td>
										<td class="Grid_GridLine">
											<asp:Label ID="lblTitle_PayRollPW" runat="server" Text="PayRollPW" />
										</td>
                    <td class="Grid_GridLine">
                      <asp:Label ID="lblTitle_ErrFrequency" runat="server" Text="ErrFrequency" />
                    </td>                    
									</tr>
									<tr align="center">
										<td class="Grid_GridLine">
											<asp:Label ID="lbl_ERPID" runat="server" Text='<%# Eval("ERPID") %>' /></td>
										<td class="Grid_GridLine">
                      <asp:Label ID="txt_ADAcc" runat="server" Text='<%# Eval("ADAcc") %>' /></td>
										<td class="Grid_GridLine">
											<asp:Label ID="lbl_PayRollPW" runat="server" Text='<%# Eval("PayRollPW") %>' /></td>
                    <td class="Grid_GridLine">
                      <asp:Label ID="lbl_ErrFrequency" runat="server" Text='<%# Eval("ErrFrequency") %>' /></td>                      
									</tr>
								</table>
							</asp:Panel>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:Panel runat="server" ID="MasterEdit" width="100%">
								<table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
									<tr class="button_bar_cell" align="center">
										<td class="Grid_GridLine">
											<asp:Label ID="lblTitle_ERPID" runat="server" Text="ERPID" />
										</td>
                    <td class="Grid_GridLine">
                      <asp:Label ID="lblTitle_ADAcc" runat="server" Text="AD帳號" />
                    </td>
                    <td class="Grid_GridLine">
											<asp:Label ID="lblTitle_PayRollPW" runat="server" Text="PayRollPW" />
										</td>
                    <td class="Grid_GridLine">
                      <asp:Label ID="lblTitle_ErrFrequency" runat="server" Text="ErrFrequency" />
                    </td>
                  </tr>
									<tr align="center">
										<td class="Grid_GridLine">											
											<asp:TextBox ID="txt_ERPID" runat="server" Text='<%# Eval("ERPID") %>' />
										</td>
										<td class="Grid_GridLine">											
											<asp:TextBox ID="txt_ADAcc" runat="server" Text='<%# Eval("ADAcc") %>' />
										</td>                    
										<td class="Grid_GridLine">
											<asp:TextBox ID="txt_PayRollPW" runat="server" Text='<%# Eval("PayRollPW") %>' />
										</td>
                    <td class="Grid_GridLine">
                      <asp:TextBox ID="txt_ErrFrequency" runat="server" Text='<%# Eval("ErrFrequency") %>' />
                    </td>
                  </tr>
								</table>
							</asp:Panel>
						</EditItemTemplate>
					</asp:TemplateField>
				</Fields>
			</asp:DetailsView>
			<asp:SqlDataSource ID="SqlDataSource8" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>
				"
				InsertCommand="INSERT INTO PersonnelSecurity
				([Company]
				,[EmployeeId]
				,[CompanyCode]
				,[ERPID]
				,[PayRollPW]
        ,[ErrFrequency]) VALUES (@Company, @EmployeeId, @CompanyCode, @ERPID, Convert(varbinary(256),@PayRollPW,@ErrFrequency)
				)"
				
        SelectCommand="SELECT [Company]
				,[EmployeeId]
				,[CompanyCode]
				,(select Top 1 ERPID from PersonnelSecurity a where a.CompanyCode!='PAN-PACIFIC' and Company = @Company And EmployeeId = @EmployeeId  ) [ERPID]
				,Convert(varchar,[PayRollPW]) As [PayRollPW]
        ,ErrFrequency
        ,(select Top 1 ERPID from PersonnelSecurity a where a.CompanyCode='PAN-PACIFIC' and Company = @Company And EmployeeId = @EmployeeId  ) ADAcc
				FROM PersonnelSecurity WHERE (Company = @Company And EmployeeId = @EmployeeId)"
        
				UpdateCommand="UPDATE PersonnelSecurity SET [ERPID]=(Case when CompanyCode='PAN-PACIFIC' then IsNull(@ADAcc,'') else IsNull(@ERPID,'') end)
        , [PayRollPW]=Convert(varbinary(256),@PayRollPW),ErrFrequency=@ErrFrequency
				WHERE (Company = @Company And EmployeeId = @EmployeeId)">
				<InsertParameters>
						<asp:Parameter Name="Company" />
						<asp:Parameter Name="EmployeeId" />
						<asp:Parameter Name="CompanyCode" />
						<asp:Parameter Name="ERPID" />
						<asp:Parameter Name="PayRollPW" />
            <asp:Parameter Name="ErrFrequency" />
					</InsertParameters>
					<UpdateParameters>
						<asp:Parameter Name="Company" />
						<asp:Parameter Name="EmployeeId" />
						<asp:Parameter Name="ERPID" />
            <asp:Parameter Name="ADAcc" />
						<asp:Parameter Name="PayRollPW" />
            <asp:Parameter Name="ErrFrequency" />
					</UpdateParameters>
					<SelectParameters>
						<asp:QueryStringParameter Name="Company" QueryStringField="Company" />
						<asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
					</SelectParameters>
				</asp:SqlDataSource>
		</TD>
	</TR>
	<tr runat="server" id="trInsert08">
		<td align="center">
			<asp:ImageButton ID="btnSave08" runat="server" SkinID="SG2" CommandName="Insert" OnClick="btnSave08_Click" />
			<asp:ImageButton ID="btnEdit08" runat="server" SkinID="SU1" CommandName="Update" OnClick="btnSave08_Click" />
			<asp:ImageButton ID="btnCancel08" runat="server" SkinID="CC1" />
		</td>
	</tr>
</table>
<asp:HiddenField id="hid_IsInsertExit08" runat="server"></asp:HiddenField>
<uc5:StyleContentEnd ID="StyleContentEnd8" runat="server" />
<uc6:StyleFooter ID="StyleFooter8" runat="server" />
