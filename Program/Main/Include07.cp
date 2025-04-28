        <uc2:StyleHeader ID="StyleHeader7" runat="server" />
        <uc3:StyleTitle ID="StyleTitle7" runat="server" Title="全民健保" />
        <uc4:StyleContentStart ID="StyleContentStart7" runat="server" />
        <table width="100%">
			<TR><TD><asp:DetailsView ID="DetailsView07" runat="server"  DataKeyNames="Company,EmployeeId"
					  DataSourceID="SqlDataSource7" Width="100%" OnDataBound="DetailsView07_DataBound"
					  OnItemInserting="DetailsView07_ItemInserting" OnItemInserted="DetailsView07_ItemInserted"
					  OnItemUpdating="DetailsView07_ItemUpdating" OnItemUpdated="DetailsView07_ItemUpdated"
					  AutoGenerateRows="False">
						<Fields>
							<asp:TemplateField>
								<ItemTemplate>
									<asp:Panel runat="server" ID="Master" width="100%">
										<table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
											<tr class="button_bar_cell" align="center">												
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_EffectiveDate" runat="server" Text="EffectiveDate" />
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Insured_amount" runat="server" Text="Insured_amount" />	
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Insured_person" runat="server" Text="Insured_person" />
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Suspends" runat="server" Text="Suspends" />
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Subsidy_code" runat="server" Text="Subsidy_code" />												
												</td>
                        <td class="Grid_GridLine">
                          <asp:Label ID="lblTitle_HI2" runat="server" Text="HI2" />
                        </td>
											</tr>
											<tr>
												<td class="Grid_GridLine" align="right">
													<asp:Label ID="lbl_EffectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>' />
														</td>
												<td class="Grid_GridLine" align="right">
													<asp:Label ID="lbl_Insured_amount" runat="server" Text='<%# Eval("Insured_amount") %>' />
														</td>
												<td class="Grid_GridLine" align="right">
													<asp:Label ID="lbl_Insured_person" runat="server" Text='<%# Eval("Insured_person") %>' />
														</td>
												<td class="Grid_GridLine" align="center">
													<asp:Label ID="lbl_Suspends" runat="server" Text='<%# Eval("Suspends") %>' />
														</td>
												<td class="Grid_GridLine" align="center">
													<asp:Label ID="lbl_Subsidy_code" runat="server" Text='<%# Eval("Subsidy_code") %>
														' />
													<uc8:CodeList ID="lbl_CL_Subsidy_code" runat="server" />
												</td>
												<td class="Grid_GridLine" align="center">
													<asp:Label ID="lbl_HI2" runat="server" Text='<%# Eval("HI2") %>' />													
												</td>                        
											</tr>											
										</table>
									</asp:Panel>
								</ItemTemplate>								
								<EditItemTemplate>
									<asp:Panel runat="server" ID="MasterEdit" width="100%">
										<table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
											<tr class="button_bar_cell" align="center">												
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_EffectiveDate" runat="server" Text="EffectiveDate" />
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Insured_amount" runat="server" Text="Insured_amount" />
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Insured_person" runat="server" Text="Insured_person" />
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Suspends" runat="server" Text="Suspends" />
												</td>
												<td class="Grid_GridLine">
													<asp:Label ID="lblTitle_Subsidy_code" runat="server" Text="Subsidy_code" />
												</td>
                        <td class="Grid_GridLine">
                          <asp:Label ID="lblTitle_HI2" runat="server" Text="HI2" />
                        </td>                        
											</tr>
											<tr align="center">
												<td class="Grid_GridLine">
													<asp:TextBox ID="txt_EffectiveDate" runat="server" Text='<%# Eval("EffectiveDate") %>' />													
														</td>
												<td class="Grid_GridLine">
													<asp:TextBox ID="txt_Insured_amount" runat="server" Text='<%# Eval("Insured_amount") %>' />
														</td>
												<td class="Grid_GridLine">
													<asp:TextBox ID="txt_Insured_person" runat="server" Text='<%# Eval("Insured_person") %>' />
														</td>
												<td class="Grid_GridLine">
													<asp:DropDownList ID="ddl_Suspends" runat="server" >
                            <asp:ListItem Text="投保" Value="N" />
                            <asp:ListItem Text="退保" Value="Y" />
                            <asp:ListItem Text="調整" Value="U" />                        
                          </asp:DropDownList>
												</td>
												<td class="Grid_GridLine">													
													<uc8:CodeList ID="CodeList_Subsidy_code" runat="server" />
												</td>
                        <td class="Grid_GridLine">
                          <asp:DropDownList ID="ddl_HI2" runat="server" >
                            <asp:ListItem Text="是" Value="Y" />
                            <asp:ListItem Text="否" Value="N" />                                                        
                          </asp:DropDownList>
                        </td>
                      </tr>
										</table>
									</asp:Panel>
								</EditItemTemplate>							
							</asp:TemplateField>							
						</Fields>
					</asp:DetailsView>
					<asp:SqlDataSource ID="SqlDataSource7" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString %>
						"
						InsertCommand="INSERT INTO HealthInsurance_Heading
						([Company]
						,[EmployeeId]
						,[EffectiveDate]
						,[TrxType]
						,[Insured_amount]
						,[Insured_person]
						,[Suspends]
						,[Subsidy_code]
            ,[HI2]
            ) VALUES (@Company, @EmployeeId, @EffectiveDate, @TrxType, @Insured_amount, @Insured_person, @Suspends, @Subsidy_code, @HI2
						)"
						SelectCommand="SELECT [Company]
						,[EmployeeId]
						,[EffectiveDate]
						,[TrxType]
						,Convert(varchar,[Insured_amount]) As [Insured_amount]
						,[Insured_person]
						,[Suspends]
						,[Subsidy_code] 
            ,[HI2]
            FROM HealthInsurance_Heading WHERE (Company = @Company And EmployeeId = @EmployeeId)"
						UpdateCommand="UPDATE HealthInsurance_Heading SET EffectiveDate = @EffectiveDate, TrxType = @TrxType, Insured_amount = @Insured_amount
						, Insured_person = @Insured_person, Suspends = @Suspends, Subsidy_code= @Subsidy_code, HI2= @HI2
						WHERE (Company = @Company And EmployeeId = @EmployeeId)">
						<InsertParameters>
								<asp:Parameter Name="Company" />
								<asp:Parameter Name="EmployeeId" />
								<asp:Parameter Name="EffectiveDate" />
								<asp:Parameter Name="TrxType" />
								<asp:Parameter Name="Insured_amount" />
								<asp:Parameter Name="Insured_person" />
								<asp:Parameter Name="Suspends" />
								<asp:Parameter Name="Subsidy_code" />
                <asp:Parameter Name="HI2" />
							</InsertParameters>
							<UpdateParameters>
								<asp:Parameter Name="Company" />
								<asp:Parameter Name="EmployeeId" />
								<asp:Parameter Name="EffectiveDate" />
								<asp:Parameter Name="TrxType" />
								<asp:Parameter Name="Insured_amount" />
								<asp:Parameter Name="Insured_person" />
								<asp:Parameter Name="Suspends" />
								<asp:Parameter Name="Subsidy_code" />
                <asp:Parameter Name="HI2" />
							</UpdateParameters>
							<SelectParameters>
								<asp:QueryStringParameter Name="Company" QueryStringField="Company" />
								<asp:QueryStringParameter Name="EmployeeId" QueryStringField="EmployeeId" />
							</SelectParameters>
						</asp:SqlDataSource>
				</TD>
			</TR>			
			<tr runat="server" id="trInsert07">
				<td align="center">
					<asp:ImageButton ID="btnSave07" runat="server" SkinID="SG2" CommandName="Insert" OnClick="btnSave07_Click" />
					<asp:ImageButton ID="btnEdit07" runat="server" SkinID="SU1" CommandName="Update" OnClick="btnSave07_Click" />
					<asp:ImageButton ID="btnCancel07" runat="server" SkinID="CC1" />
				</td>
			</tr>
			<TR>
				<TD>
					<uc7:Navigator id="Navigator07" runat="server"></uc7:Navigator>
				</TD>
			</TR>
			<tr>
				<td>
					<asp:Label id="labNoData07" runat="server" Text="尚無眷屬加保資料!!" />
				</td>
			</tr>
			<tr>
				<td>
					<asp:GridView ID="GridView07" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="Company,EmployeeId,SeqNo"
					OnRowDataBound="GridView07_RowDataBound" OnRowCreated="GridView07_RowCreated"
					OnRowUpdating="GridView07_RowUpdating" OnRowUpdated="GridView07_RowUpdated"
					 DataSourceID="SDS_GridView07" AllowPaging="True" AllowSorting="True">
						<Columns>
							<asp:TemplateField HeaderText="刪除" ShowHeader="False">
								<ItemTemplate>
									<asp:LinkButton ID="btnDelete07" runat="server" CausesValidation="False"  OnClick="btnDelete07_Click" 
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
							<asp:TemplateField FooterText="停保" HeaderText="停保">
								<ItemTemplate>
									<asp:DropDownList ID="ddlSuspends" runat="server" DataValueField='<%# Eval("Suspends") %>'>
											<asp:ListItem Text="投保" Value="N" />
											<asp:ListItem Text="退保" Value="Y" />
                      <asp:ListItem Text="調整" Value="U" />  
									</asp:DropDownList>
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>							
							<asp:TemplateField FooterText="補助" HeaderText="補助">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListSubsidy_code" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:BoundField DataField="DependentsName" HeaderText="眷屬姓名"></asp:BoundField>
							<asp:BoundField DataField="IDNo" HeaderText="身份証號"></asp:BoundField>
							<asp:BoundField DataField="BirthDate" HeaderText="生日" SortExpression="BirthDate"></asp:BoundField>							
							<asp:TemplateField FooterText="稱謂" HeaderText="稱謂" SortExpression="Dependent_title">
								<ItemTemplate>
									<uc8:CodeList ID="CodeListDependent_title" runat="server" />
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
							<asp:BoundField DataField="EffectiveDate" HeaderText="生效日期"></asp:BoundField>
						</Columns>
						<EmptyDataTemplate>
							<table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;" >
								<tr class="button_bar_cell">
									<td class="paginationRowEdgeLl">新增</td>
									<td>序號</td>
									<td>停保</td>
									<td>補助</td>
									<td>眷屬姓名</td>
									<td>身份証號</td>
									<td>生日</td>
									<td>稱謂</td>
									<td class="paginationRowEdgeRI">生效日期</td>
								</tr>
								<tr align="center">
									<td class="Grid_GridLine">
										<asp:ImageButton id="btnNew" onclick="btnEmptyNew07_Click" runat="server" SkinID="NewAdd" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew01" Width="20px" />										
									</td>
									<td class="Grid_GridLine">
										<asp:DropDownList ID="tbAddNew02" runat="server">
											<asp:ListItem Text="否" Value="N" />
											<asp:ListItem Text="是" Value="Y" />											
										</asp:DropDownList>										
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew03" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew04" Width="100px" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew05" Width="100px" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew06" />										
									</td>
									<td class="Grid_GridLine">
										<uc8:CodeList ID="tbAddNew07" runat="server" />
									</td>
									<td class="Grid_GridLine">
										<asp:TextBox runat="server" ID="tbAddNew08" />										
									</td>
								</tr>
							</table>
						</EmptyDataTemplate>
					</asp:GridView>					
                 </td>
            </tr>
        </table>
        <asp:SqlDataSource ID="SDS_GridView07" runat="server" ConnectionString="<%$ ConnectionStrings:MyConnectionString  %>"
			SelectCommand="SELECT * FROM [HealthInsurance_Detail] WHERE (Company = @Company And EmployeeId = @EmployeeId )"
			InsertCommand="INSERT INTO [HealthInsurance_Detail]
           ([Company]
           ,[EmployeeId]
           ,[EffectiveDate]
           ,[SeqNo]
           ,[Suspends]
           ,[DependentsName]
           ,[IDNo]
           ,[BirthDate]
           ,[Dependent_title]
           ,[Subsidy_code])
			VALUES (@Company,@EmployeeId,@EffectiveDate,@SeqNo,@Suspends,@DependentsName,@IDNo,@BirthDate,@Dependent_title,@Subsidy_code)" 
			UpdateCommand="UPDATE [HealthInsurance_Detail] SET EffectiveDate=@EffectiveDate,Suspends=@Suspends,DependentsName=@DependentsName,IDNo=@IDNo
			,BirthDate=@BirthDate,Dependent_title=@Dependent_title,Subsidy_code=@Subsidy_code
			WHERE (Company=@Company And EmployeeId=@EmployeeId And SeqNo=@SeqNo)"
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
				<asp:Parameter Name="SeqNo" />
			</InsertParameters>			
        </asp:SqlDataSource>
<asp:HiddenField id="hid_IsInsertExit07" runat="server"></asp:HiddenField>
        <uc5:StyleContentEnd ID="StyleContentEnd7" runat="server" />
        <uc6:StyleFooter ID="StyleFooter7" runat="server" />
