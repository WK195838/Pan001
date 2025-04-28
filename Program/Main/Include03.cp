        <uc2:StyleHeader ID="StyleHeader3" runat="server" />
        <uc3:StyleTitle ID="StyleTitle3" runat="server" Title="經歷" />
        <uc4:StyleContentStart ID="StyleContentStart3" runat="server" />
        <table width="100%">
			<TR>
				<TD>
					<uc7:Navigator id="Navigator03" runat="server"></uc7:Navigator>
				</TD>
			</TR>
			<tr>
				<td>
					<asp:Label id="labNoData03" runat="server" Text="尚無經歷資料!!" />
				</td>
			</tr>
			<tr>
                <td>
					<asp:GridView ID="GridView03" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Company,EmployeeId,SeqNo"
					OnRowDataBound="GridView03_RowDataBound" OnRowCreated="GridView03_RowCreated"
					OnRowUpdating="GridView03_RowUpdating" OnRowUpdated="GridView03_RowUpdated"
					 DataSourceID="SDS_GridView03" AllowPaging="True" AllowSorting="True">
						<Columns>
							<asp:TemplateField HeaderText="刪除" ShowHeader="False">
								<ItemTemplate>
									<asp:LinkButton ID="btnDelete03" runat="server" CausesValidation="False"  OnClick="btnDelete03_Click" 
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
							<asp:BoundField DataField="SeqNo" HeaderText="序號" ReadOnly="True" SortExpression="SeqNo"></asp:BoundField>
							<asp:BoundField DataField="Organization" HeaderText="機關名稱" SortExpression="Organization"></asp:BoundField>
							<asp:BoundField DataField="Position" HeaderText="職務" SortExpression="Position"></asp:BoundField>							
							<asp:TemplateField FooterText="任職年月" HeaderText="任職年月" SortExpression="BeginYM">
								<ItemTemplate>
									<uc10:SalaryYM ID="ddlYMBeginYM" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:TemplateField FooterText="離職年月" HeaderText="離職年月" SortExpression="EndYM">
								<ItemTemplate>
									<uc10:SalaryYM ID="ddlYMEndYM" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:BoundField DataField="Memo" HeaderText="備忘" SortExpression="Memo"></asp:BoundField>
						</Columns>
						<EmptyDataTemplate>
							<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
								<tr class="button_bar_cell">
									<td class="paginationRowEdgeLl">新增</td>
									<td>序號</td>
									<td>機關名稱</td>
									<td>職務</td>
									<td>任職年月</td>
									<td>離職年月</td>
									<td class="paginationRowEdgeRI">備忘</td>
								</tr>
								<tr align="center">
									<td class="Grid_GridLine">
										<asp:ImageButton id="btnNew" onclick="btnEmptyNew03_Click" runat="server" SkinID="NewAdd" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew01" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew02" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew03" />										
									</td>
									<td class="Grid_GridLine">
										<uc10:SalaryYM id="tbAddNew04" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<uc10:SalaryYM id="tbAddNew05" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew06" />
									</td>									
								</tr>
							</table>
						</EmptyDataTemplate>
					</asp:GridView>					
                 </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SDS_GridView03" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
			SelectCommand="SELECT * FROM CurriculumVitae WHERE (Company = @Company And EmployeeId = @EmployeeId ) Order By SeqNo"
			InsertCommand="INSERT INTO CurriculumVitae([SeqNo],[Company],[EmployeeId],[Organization],[Position],[BeginYM],[EndYM],[Memo])
			VALUES (@SeqNo,@Company,@EmployeeId,@Organization,@Position,@BeginYM,@EndYM,@Memo)" 
			UpdateCommand="UPDATE CurriculumVitae SET Organization=@Organization,Position=@Position,BeginYM=@BeginYM,EndYM=@EndYM,Memo=@Memo WHERE (Company=@Company And EmployeeId=@EmployeeId And SeqNo=@SeqNo)"
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
				<asp:Parameter Name="Organization" />
			</InsertParameters>			
        </asp:SqlDataSource>
<asp:HiddenField id="hid_IsInsertExit03" runat="server"></asp:HiddenField>
        <uc5:StyleContentEnd ID="StyleContentEnd3" runat="server" />
        <uc6:StyleFooter ID="StyleFooter3" runat="server" />
