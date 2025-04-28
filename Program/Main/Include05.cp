        <uc2:StyleHeader ID="StyleHeader5" runat="server" />
        <uc3:StyleTitle ID="StyleTitle5" runat="server" Title="語言" />
        <uc4:StyleContentStart ID="StyleContentStart5" runat="server" />
        <table width="100%">
			<TR>
				<TD>
					<uc7:Navigator id="Navigator05" runat="server"></uc7:Navigator>
				</TD>
			</TR>
			<tr>
				<td>
					<asp:Label id="labNoData05" runat="server" Text="尚無語言資料!!" />
				</td>
			</tr>
			<tr>
                <td>
					<asp:GridView ID="GridView05" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Company,EmployeeId,Language"
					OnRowDataBound="GridView05_RowDataBound" OnRowCreated="GridView05_RowCreated"
					OnRowUpdating="GridView05_RowUpdating" OnRowUpdated="GridView05_RowUpdated"
					 DataSourceID="SDS_GridView05" AllowPaging="True" AllowSorting="True">
						<Columns>
							<asp:TemplateField HeaderText="刪除" ShowHeader="False">
								<ItemTemplate>
									<asp:LinkButton ID="btnDelete05" runat="server" CausesValidation="False"  OnClick="btnDelete05_Click" 
										L1PK='<%# DataBinder.Eval(Container, "DataItem.Language")%>'
										OnClientClick='return confirm("確定刪除?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&'quot'; filter:alpha(opacity='50);&quot';  onmouseout=&'quot;this.filters'['alpha'].opacity='50&quot';  onmouseover=&'quot;this.filters'['alpha'].opacity='100&quot'; />">
									</asp:LinkButton>
								</ItemTemplate>
								<HeaderStyle Width="30px" CssClass="paginationRowEdgeL" />
							</asp:TemplateField>

							<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
								<HeaderStyle Width="60px"></HeaderStyle>
								<ItemStyle Width="60px"></ItemStyle>
							</asp:CommandField>
							<asp:TemplateField FooterText="語言" HeaderText="語言" SortExpression="Language">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListLanguage" runat="server" Enabled="False" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>							
							<asp:TemplateField FooterText="聽" HeaderText="聽" SortExpression="Listen">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListListen" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:TemplateField FooterText="說" HeaderText="說" SortExpression="Speak">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListSpeak" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:TemplateField FooterText="讀" HeaderText="讀" SortExpression="Read">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListRead" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:TemplateField FooterText="寫" HeaderText="寫" SortExpression="Write">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListWrite" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>							
						</Columns>
						<EmptyDataTemplate>
							<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
								<tr class="button_bar_cell">
									<td class="paginationRowEdgeLl">新增</td>
									<td>語言</td>
									<td>聽</td>
									<td>說</td>
									<td>讀</td>									
									<td class="paginationRowEdgeRI">寫</td>
								</tr>
								<tr align="center">
									<td class="Grid_GridLine">
										<asp:ImageButton id="btnNew" onclick="btnEmptyNew05_Click" runat="server" SkinID="NewAdd" />
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew01" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew02" runat="server" />										
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew03" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew04" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew05" runat="server" />
									</td>									
								</tr>
							</table>
						</EmptyDataTemplate>
					</asp:GridView>					
                 </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SDS_GridView05" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
			SelectCommand="SELECT * FROM Language WHERE (Company = @Company And EmployeeId = @EmployeeId )"
			InsertCommand="INSERT INTO Language([SeqNo]
           ,[Company]
           ,[EmployeeId]
           ,[Language]
           ,[Listen]
           ,[Speak]
           ,[Read]
           ,[Write])
			VALUES (@SeqNo,@Company,@EmployeeId,@Language,@Listen,@Speak,@Read,@Write)" 
			UpdateCommand="UPDATE Language SET [Listen]=@Listen,[Speak]=@Speak,[Read]=@Read,[Write]=@Write WHERE (Company=@Company And EmployeeId=@EmployeeId And Language=@Language)"
			>
			<SelectParameters>
				<asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />              
            </SelectParameters>
			<UpdateParameters>
				<asp:Parameter Name="Company" />
				<asp:Parameter Name="EmployeeId" />
				<asp:Parameter Name="Language" />
			</UpdateParameters>
			<InsertParameters>
				<asp:Parameter Name="Company" />
				<asp:Parameter Name="EmployeeId" />
				<asp:Parameter Name="Language" />
			</InsertParameters>			
        </asp:SqlDataSource>
		<asp:HiddenField id="hid_IsInsertExit05" runat="server"></asp:HiddenField>
        <uc5:StyleContentEnd ID="StyleContentEnd5" runat="server" />
        <uc6:StyleFooter ID="StyleFooter5" runat="server" />
