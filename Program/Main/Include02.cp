        <uc2:StyleHeader ID="StyleHeader2" runat="server" />
        <uc3:StyleTitle ID="StyleTitle2" runat="server" Title="學歷" />
        <uc4:StyleContentStart ID="StyleContentStart2" runat="server" />
        <table width="100%">
			<TR>
				<TD>
					<uc7:Navigator id="Navigator02" runat="server"></uc7:Navigator>
				</TD>
			</TR>
			<tr>
				<td>
					<asp:Label id="labNoData02" runat="server" Text="尚無學歷資料!!" />
				</td>
			</tr>
            <tr>
                <td>
					<asp:GridView ID="GridView02" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Company,EmployeeId,SeqNo"
					OnRowDataBound="GridView02_RowDataBound" OnRowCreated="GridView02_RowCreated"
					OnRowUpdating="GridView02_RowUpdating" OnRowUpdated="GridView02_RowUpdated"
					 DataSourceID="SDS_GridView02" AllowPaging="True" AllowSorting="True">
						<Columns>
							<asp:TemplateField HeaderText="刪除" ShowHeader="False">
								<ItemTemplate>
									<asp:LinkButton ID="btnDelete02" runat="server" CausesValidation="False"  OnClick="btnDelete02_Click" 
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
							<asp:BoundField DataField="School1" HeaderText="學校編號" SortExpression="School1"></asp:BoundField>
							<asp:BoundField DataField="Department" HeaderText="科系" SortExpression="Department"></asp:BoundField>							
							<asp:TemplateField FooterText="入校年月" HeaderText="入校年月" SortExpression="BeginYM">
								<ItemTemplate>
									<uc10:SalaryYM ID="ddlYMBeginYM" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:TemplateField FooterText="離校年月" HeaderText="離校年月" SortExpression="EndYM">
								<ItemTemplate>
									<uc10:SalaryYM ID="ddlYMEndYM" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>							
							<asp:TemplateField FooterText="畢／肄" HeaderText="畢／肄" SortExpression="Comp">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListComp" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
						</Columns>
						<EmptyDataTemplate>
							<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
								<tr class="button_bar_cell">
									<td class="paginationRowEdgeLl">新增</td>
									<td>序號</td>
									<td>學校</td>
									<td>科系</td>
									<td>入校年月</td>
									<td>離校年月</td>
									<td class="paginationRowEdgeRI">畢／肄</td>
								</tr>
								<tr align="center" >
									<td class="Grid_GridLine">
										<asp:ImageButton id="btnNew" onclick="btnEmptyNew02_Click" runat="server" SkinID="NewAdd" />
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
										<uc8:CodeList ID="tbAddNew06" runat="server" />
									</td>									
								</tr>
							</table>
						</EmptyDataTemplate>
					</asp:GridView>					
                 </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SDS_GridView02" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
			SelectCommand="SELECT * FROM Education WHERE (Company = @Company And EmployeeId = @EmployeeId) Order By SeqNo"
			InsertCommand="INSERT INTO Education([SeqNo],[Company],[EmployeeId],[School1],[Department],[BeginYM],[EndYM],[Comp])
			VALUES (@SeqNo,@Company,@EmployeeId,@School1,@Department,@BeginYM,@EndYM,@Comp)" 
			UpdateCommand="UPDATE Education SET School1=@School1,Department=@Department,BeginYM=@BeginYM,EndYM=@EndYM,Comp=@Comp WHERE (Company=@Company And EmployeeId=@EmployeeId And SeqNo=@SeqNo)"
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
				<asp:Parameter Name="School1" />
			</InsertParameters>			
        </asp:SqlDataSource>
<asp:HiddenField id="hid_IsInsertExit02" runat="server"></asp:HiddenField>
        <uc5:StyleContentEnd ID="StyleContentEnd2" runat="server" />
        <uc6:StyleFooter ID="StyleFooter2" runat="server" />
