        <uc2:StyleHeader ID="StyleHeader6" runat="server" />
        <uc3:StyleTitle ID="StyleTitle6" runat="server" Title="教育訓練" />
        <uc4:StyleContentStart ID="StyleContentStart6" runat="server" />
        <table width="100%">
			<TR>
				<TD>
					<uc7:Navigator id="Navigator06" runat="server"></uc7:Navigator>
				</TD>
			</TR>
			<tr>
				<td>
					<asp:Label id="labNoData06" runat="server" Text="尚無訓練資料!!" />
				</td>
			</tr>
			<tr>
                <td>
					<asp:GridView ID="GridView06" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Company,EmployeeId,Training_datefrom"
					OnRowDataBound="GridView06_RowDataBound" OnRowCreated="GridView06_RowCreated"
					OnRowUpdating="GridView06_RowUpdating" OnRowUpdated="GridView06_RowUpdated"
					 DataSourceID="SDS_GridView06" AllowPaging="True" AllowSorting="True">
						<Columns>
							<asp:TemplateField HeaderText="刪除" ShowHeader="False">
								<ItemTemplate>
									<asp:LinkButton ID="btnDelete06" runat="server" CausesValidation="False"  OnClick="btnDelete06_Click" 
										L1PK='<%# DataBinder.Eval(Container, "DataItem.Training_datefrom")%>'
										OnClientClick='return confirm("確定刪除?");' Text="<img src='../App_Themes/images/delete1.gif' border='0' alt='刪除'  style=&'quot'; filter:alpha(opacity='50);&quot';  onmouseout=&'quot;this.filters'['alpha'].opacity='50&quot';  onmouseover=&'quot;this.filters'['alpha'].opacity='100&quot'; />">
									</asp:LinkButton>
								</ItemTemplate>
								<HeaderStyle Width="30px" CssClass="paginationRowEdgeL" />
							</asp:TemplateField>

							<asp:CommandField CancelImageUrl="~/App_Themes/images/cancel1.gif" EditImageUrl="~/App_Themes/images/edit1.GIF" ShowEditButton="True" UpdateImageUrl="~/App_Themes/images/saveexit1.gif" ButtonType="Image" HeaderText="編輯">
								<HeaderStyle Width="60px"></HeaderStyle>
								<ItemStyle Width="60px"></ItemStyle>
							</asp:CommandField>							
							<asp:BoundField DataField="Training_datefrom" HeaderText="受訓日期（自）" ReadOnly="True" SortExpression="Language"></asp:BoundField>
							<asp:BoundField DataField="Training_dateto" HeaderText="受訓日期（至）"></asp:BoundField>
							<asp:BoundField DataField="Training_courses" HeaderText="訓練課程"></asp:BoundField>
							<asp:BoundField DataField="Hours" HeaderText="時數"></asp:BoundField>
							<asp:BoundField DataField="Amount" HeaderText="金額"></asp:BoundField>
						</Columns>
						<EmptyDataTemplate>
							<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
								<tr class="button_bar_cell">
									<td class="paginationRowEdgeLl">新增</td>
									<td>受訓日期（自）</td>
									<td>受訓日期（至）</td>
									<td>訓練課程</td>
									<td>時數</td>									
									<td class="paginationRowEdgeRI">金額</td>
								</tr>
								<tr align="center">
									<td class="Grid_GridLine">
										<asp:ImageButton id="btnNew" onclick="btnEmptyNew06_Click" runat="server" SkinID="NewAdd" />
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
										<asp:TextBox runat="server" ID="tbAddNew04" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew05" />
									</td>									
								</tr>
							</table>
						</EmptyDataTemplate>
					</asp:GridView>					
                 </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SDS_GridView06" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
			SelectCommand="SELECT * FROM [TrainingData] WHERE (Company = @Company And EmployeeId = @EmployeeId )"
			InsertCommand="INSERT INTO [TrainingData]([Company]
			  ,[EmployeeId]
			  ,[Training_datefrom]
			  ,[Training_dateto]
			  ,[Training_courses]
			  ,[Hours]
			  ,[Amount])
			VALUES (@Company,@EmployeeId,@Training_datefrom,@Training_dateto,@Training_courses,@Hours,@Amount)" 
			UpdateCommand="UPDATE [TrainingData] SET Training_dateto=@Training_dateto,Training_courses=@Training_courses,Hours=@Hours,Amount=@Amount WHERE (Company=@Company And EmployeeId=@EmployeeId And Convert(varchar,Training_datefrom,111)=@Training_datefrom)"
			>
			<SelectParameters>
				<asp:QueryStringParameter Name="Company" QueryStringField="Company" />
                <asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />              
            </SelectParameters>
			<UpdateParameters>
				<asp:Parameter Name="Company" />
				<asp:Parameter Name="EmployeeId" />
				<asp:Parameter Name="Training_datefrom" />
			</UpdateParameters>
			<InsertParameters>
				<asp:Parameter Name="Company" />
				<asp:Parameter Name="EmployeeId" />
				<asp:Parameter Name="Training_datefrom" />
			</InsertParameters>			
        </asp:SqlDataSource>
<asp:HiddenField id="hid_IsInsertExit06" runat="server"></asp:HiddenField>
        <uc5:StyleContentEnd ID="StyleContentEnd6" runat="server" />
        <uc6:StyleFooter ID="StyleFooter6" runat="server" />
