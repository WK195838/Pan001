        <uc2:StyleHeader ID="StyleHeader4" runat="server" />
        <uc3:StyleTitle ID="StyleTitle4" runat="server" Title="專長" />
        <uc4:StyleContentStart ID="StyleContentStart4" runat="server" />
        <table width="100%">
			<TR>
				<TD>
					<uc7:Navigator id="Navigator04" runat="server"></uc7:Navigator>
				</TD>
			</TR>
			<tr>
				<td>
					<asp:Label id="labNoData04" runat="server" Text="尚無專長資料!!" />
				</td>
			</tr>
			<tr>
                <td>
					<asp:GridView ID="GridView04" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Company,EmployeeId,SeqNo"
					OnRowDataBound="GridView04_RowDataBound" OnRowCreated="GridView04_RowCreated"
					OnRowUpdating="GridView04_RowUpdating" OnRowUpdated="GridView04_RowUpdated"
					 DataSourceID="SDS_GridView04" AllowPaging="True" AllowSorting="True">
						<Columns>
							<asp:TemplateField HeaderText="刪除" ShowHeader="False">
								<ItemTemplate>
									<asp:LinkButton ID="btnDelete04" runat="server" CausesValidation="False"  OnClick="btnDelete04_Click" 
										L1PK='<%# DataBinder.Eval(Container, "DataItem.SeqNo")%>'
										OnClientClick='return confirm("確定刪除?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&'quot'; filter:alpha(opacity='50);&quot';  onmouseout=&'quot;this.filters'['alpha'].opacity='50&quot';  onmouseover=&'quot;this.filters'['alpha'].opacity='100&quot'; />">
									</asp:LinkButton>
								</ItemTemplate>
								<HeaderStyle Width="30px" CssClass="paginationRowEdgeL" />
							</asp:TemplateField>

							<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
								<HeaderStyle Width="60px"></HeaderStyle>
								<ItemStyle Width="60px"></ItemStyle>
							</asp:CommandField>							
							<asp:TemplateField FooterText="分類" HeaderText="分類" SortExpression="Category" >
								<ItemTemplate>
									<uc8:CodeList ID="CodeListCategory" runat="server" Enabled="False" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:BoundField DataField="Item" HeaderText="項目" SortExpression="Item"></asp:BoundField>
							<asp:BoundField DataField="Memo" HeaderText="說明" SortExpression="Memo"></asp:BoundField>
							<asp:BoundField DataField="filepath" HeaderText="附檔" SortExpression="filepath"></asp:BoundField>							
						</Columns>
						<EmptyDataTemplate>
							<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
								<tr class="button_bar_cell">
									<td class="paginationRowEdgeLl">新增</td>
									<td>分類</td>
									<td>項目</td>
									<td>說明</td>
									<td class="paginationRowEdgeRI">附檔</td>
								</tr>
								<tr align="center">
									<td class="Grid_GridLine">
										<asp:ImageButton id="btnNew" onclick="btnEmptyNew04_Click" runat="server" SkinID="NewAdd" />
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew01" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew02" />										
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew03" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew04" />
										<asp:ImageButton id="ibOpenFile044" runat="server" SkinID="OpenFile" />
									</td>
								</tr>
							</table>
						</EmptyDataTemplate>
					</asp:GridView>					
                 </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SDS_GridView04" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
			SelectCommand="SELECT * FROM Skill WHERE (Company = @Company And EmployeeId = @EmployeeId )"
			InsertCommand="INSERT INTO Skill([SeqNo]
           ,[Company]
           ,[EmployeeId]
           ,[Category]
           ,[Item]
           ,[Memo]
		   ,[filepath])
			VALUES (@SeqNo,@Company,@EmployeeId,@Category,@Item,@Memo,@filepath)" 
			UpdateCommand="UPDATE Skill SET Item=@Item,Memo=@Memo,filepath=@filepath WHERE (Company=@Company And EmployeeId=@EmployeeId And SeqNo=@SeqNo)"
			>
			<SelectParameters>
				<asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />              
            </SelectParameters>
			<UpdateParameters>
				<asp:Parameter Name="Company" />
				<asp:Parameter Name="EmployeeId" />
				<asp:Parameter Name="SeqNo" />
			</UpdateParameters>
			<InsertParameters>
				<asp:Parameter Name="Company" />
				<asp:Parameter Name="EmployeeId" />
				<asp:Parameter Name="Category" />
			</InsertParameters>			
        </asp:SqlDataSource>
<asp:HiddenField id="hid_IsInsertExit04" runat="server"></asp:HiddenField>
        <uc5:StyleContentEnd ID="StyleContentEnd4" runat="server" />
        <uc6:StyleFooter ID="StyleFooter4" runat="server" />
