        <uc2:StyleHeader ID="StyleHeader1" runat="server" />
        <uc3:StyleTitle ID="StyleTitle1" runat="server" Title="背景" />
        <uc4:StyleContentStart ID="StyleContentStart1" runat="server" />
        <table width="100%">
            <tr>
                <td>
					<asp:DetailsView ID="DetailsView01" runat="server"  DataKeyNames="Company,EmployeeId"
											DataSourceID="SqlDataSource1" Width="100%" OnDataBound="DetailsView1_DataBound"
											OnItemUpdating="DetailsView1_ItemUpdating" OnItemUpdated="DetailsView1_ItemUpdated"
											OnItemInserted="DetailsView1_ItemInserted" OnItemInserting="DetailsView1_ItemInserting"
											OnItemCommand="DetailsView1_ItemCommand"
											AutoGenerateRows="False">
						<Fields>
							<asp:TemplateField>
<ItemTemplate>
                        <asp:Panel runat="server" Visible="false" ID="Master">
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>
                        <td class="Grid_GridLine" style="width:10%"><asp:Label ID="lblTitle_Company" runat="server" Text="Company" /></td><td class="Grid_GridLine" style="width:15%"><uc9:CompanyList ID="lbl_CompL_Company" runat="server" /></td>
                        <td class="Grid_GridLine" style="width:10%"><asp:Label ID="lblTitle_EmployeeId" runat="server" Text="EmployeeId" /></td><td class="Grid_GridLine" style="width:15%"><asp:Label ID="lbl_EmployeeId" runat="server" Text='<%# Eval("EmployeeId") %>' /></td>
                        <td class="Grid_GridLine" style="width:10%"><asp:Label ID="lblTitle_DeptId" runat="server" Text="部門" /></td><td class="Grid_GridLine" style="width:15%"><uc8:CodeList ID="lbl_CL_DeptId" runat="server" /></td>
                        </tr>
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EmployeeName" runat="server" Text="EmployeeName" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_EmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EnglishName" runat="server" Text="EnglishName" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_EnglishName" runat="server" Text='<%# Eval("EnglishName") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BirthDate" runat="server" Text="BirthDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_BirthDate" runat="server" Text='<%# Eval("BirthDate") %>' /></td>
                        </tr>
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Sex" runat="server" Text="Sex" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Sex" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_MobilPhone" runat="server" Text="MobilPhone" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_MobilPhone" runat="server" Text='<%# Eval("MobilPhone") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Email" runat="server" Text="Email" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Email" runat="server" Text='<%# Eval("Email") %>' /></td>                        
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EducationCode" runat="server" Text="學歷" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_EducationCode" runat="server" /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_TitleCode" runat="server" Text="職稱" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_TitleCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Rank" runat="server" Text="職級" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Rank" runat="server" /></td>                        
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignCode" runat="server" Text="是否在職" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_ResignCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_HireDate" runat="server" Text="HireDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_HireDate" runat="server" Text='<%# Eval("HireDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignDate" runat="server" Text="ResignDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ResignDate" runat="server" Text='<%# Eval("ResignDate") %>' /></td>                                                
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isBoss" runat="server" Text="是否雇主" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_isBoss" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isSales" runat="server" Text="是否業務" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_isSales" runat="server" /></td>                        
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Year01" runat="server" Text="年齡" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Year01" runat="server" Text='0' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Year02" runat="server" Text="年資" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Year02" runat="server" Text='0' /></td>                        
                        </tr>                                                                                                                                                        
                        </table>
                        </asp:Panel>
                        <asp:Panel runat="server" Visible="true" ID="Other">
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_IDNo" runat="server" Text="IDNo" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_IDNo" runat="server" Text='<%# Eval("IDNo") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_IDType" runat="server" Text="IDType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_IDType" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BloodType" runat="server" Text="BloodType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_BloodType" runat="server" /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_MaritalStatus" runat="server" Text="MaritalStatus" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_MaritalStatus" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DependentsNum" runat="server" Text="DependentsNum" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_DependentsNum" runat="server" Text='<%# Eval("DependentsNum") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Military" runat="server" Text="Military" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Military" runat="server" /></td>                        
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Nationality" runat="server" Text="Nationality" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Nationality" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BornPlace" runat="server" Text="BornPlace" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_BornPlace" runat="server" Text='<%# Eval("BornPlace") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResidenceAddr" runat="server" Text="ResidenceAddr" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ResidenceAddr" runat="server" Text='<%# Eval("ResidenceAddr") %>' /></td>                        
                        </tr>    
                        <tr>                                                
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_TEL" runat="server" Text="TEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_TEL" runat="server" Text='<%# Eval("TEL") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Addr" runat="server" Text="Addr" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Addr" runat="server" Text='<%# Eval("Addr") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LISubsidy" runat="server" Text="LISubsidy" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_LISubsidy" runat="server" /></td>
                        </tr>                                                
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_SpecialSeniority" runat="server" Text="SpecialSeniority" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_SpecialSeniority" runat="server" Text='<%# Eval("SpecialSeniority") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LWC" runat="server" Text="LWC" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_LWC" runat="server" /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Union" runat="server" Text="Union" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Union" runat="server" /></td>
                        </tr>                         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Identify" runat="server" Text="編制別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Identify" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Grade" runat="server" Text="職等" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Grade" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Shift" runat="server" Text="班別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_Shift" runat="server" /></td>                                                
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_PayCode" runat="server" Text="PayCode" /></td><td class="Grid_GridLine"><uc8:CodeList ID="lbl_CL_PayCode" runat="server" /></td>                                                                  
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Introducer" runat="server" Text="Introducer" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Introducer" runat="server" Text='<%# Eval("Introducer") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_IntroducerTEL" runat="server" Text="IntroducerTEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_IntroducerTEL" runat="server" Text='<%# Eval("IntroducerTEL") %>' /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Contact" runat="server" Text="Contact" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Contact" runat="server" Text='<%# Eval("Contact") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1" runat="server" Text="Guarantor1" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor1" runat="server" Text='<%# Eval("Guarantor1") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2" runat="server" Text="Guarantor2" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor2" runat="server" Text='<%# Eval("Guarantor2") %>' /></td>                        
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ContactTEL" runat="server" Text="ContactTEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ContactTEL" runat="server" Text='<%# Eval("ContactTEL") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1TEL" runat="server" Text="Guarantor1TEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor1TEL" runat="server" Text='<%# Eval("Guarantor1TEL") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2TEL" runat="server" Text="Guarantor2TEL" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_Guarantor2TEL" runat="server" Text='<%# Eval("Guarantor2TEL") %>' /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LeaveWithoutPay" runat="server" Text="LeaveWithoutPay" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_LeaveWithoutPay" runat="server" Text='<%# Eval("LeaveWithoutPay") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ObserveExpirationDate" runat="server" Text="ObserveExpirationDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_ObserveExpirationDate" runat="server" Text='<%# Eval("ObserveExpirationDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_CCN" runat="server" Text="CCN" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_CCN" runat="server" Text='<%# Eval("CCN") %>' /></td>
                        </tr> 
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LstPromotionDate" runat="server" Text="LstPromotionDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_LstPromotionDate" runat="server" Text='<%# Eval("LstPromotionDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_LstChangeSalaryDate" runat="server" Text="LstChangeSalaryDate" /></td><td class="Grid_GridLine"><asp:Label ID="lbl_LstChangeSalaryDate" runat="server" Text='<%# Eval("LstChangeSalaryDate") %>' /></td>                        
                        <td></td>
                        </tr>                                              
                        </table>
                        </asp:Panel>
                        </ItemTemplate>
                        <EditItemTemplate> 
                        <asp:Panel runat="server" Visible="false" ID="MasterEdit">                       
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Company" runat="server" Text="Company" /></td><td class="Grid_GridLine"><uc9:CompanyList ID="txt_CompL_Company" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EmployeeId" runat="server" Text="EmployeeId" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_EmployeeId" runat="server" Text='<%# Eval("EmployeeId") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DeptId" runat="server" Text="DeptId" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_DeptId" runat="server" /></td>
                        </tr>
                        <tr>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EmployeeName" runat="server" Text="EmployeeName" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_EmployeeName" runat="server" Text='<%# Eval("EmployeeName") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EnglishName" runat="server" Text="EnglishName" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_EnglishName" runat="server" Text='<%# Eval("EnglishName") %>' /></td>                        
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_BirthDate" runat="server" Text="BirthDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_BirthDate" runat="server" Text='<%# Eval("BirthDate") %>' />                        
                        </td>
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Sex" runat="server" Text="Sex" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Sex" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_MobilPhone" runat="server" Text="MobilPhone" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_MobilPhone" runat="server" Text='<%# Eval("MobilPhone") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Email" runat="server" Text="Email" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Email" runat="server" Text='<%# Eval("Email") %>' MaxLength="100" /></td>
                        </tr>         
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_EducationCode" runat="server" Text="學歷" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_EducationCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_TitleCode" runat="server" Text="職稱" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_TitleCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Rank" runat="server" Text="職級" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Rank" runat="server" /></td>                        
                        </tr>                                             
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignCode" runat="server" Text="是否在職" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_ResignCode" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_HireDate" runat="server" Text="HireDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_HireDate" runat="server" Text='<%# Eval("HireDate") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_ResignDate" runat="server" Text="ResignDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ResignDate" runat="server" Text='<%# Eval("ResignDate") %>' /></td>
                        </tr>
                        <tr>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isBoss" runat="server" Text="是否雇主" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_isBoss" runat="server" /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_isSales" runat="server" Text="是否業務" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_isSales" runat="server" /></td>                        
                        </tr>                          
                        </table>
                        </asp:Panel>
                        <asp:Panel runat="server" Visible="true" ID="OtherEdit">
                        <table width="100%" cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
						<tr>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_IDNo" runat="server" Text="IDNo" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_IDNo" runat="server" Text='<%# Eval("IDNo") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_IDType" runat="server" Text="IDType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_IDType" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_BloodType" runat="server" Text="BloodType" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_BloodType" runat="server" /></td>
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_MaritalStatus" runat="server" Text="MaritalStatus" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_MaritalStatus" runat="server" /></td>								
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_DependentsNum" runat="server" Text="DependentsNum" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_DependentsNum" runat="server" Text='<%# Eval("DependentsNum") %>' /></td>
                        <td class="Grid_GridLine"><asp:Label ID="lblTitle_Military" runat="server" Text="Military" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Military" runat="server" /></td>
                        </tr>
						<tr>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Nationality" runat="server" Text="Nationality" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Nationality" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_BornPlace" runat="server" Text="BornPlace" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_BornPlace" runat="server" Text='<%# Eval("BornPlace") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_ResidenceAddr" runat="server" Text="ResidenceAddr" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ResidenceAddr" runat="server" Text='<%# Eval("ResidenceAddr") %>' MaxLength="200" /></td>
						</tr>
						<tr>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_TEL" runat="server" Text="TEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_TEL" runat="server" Text='<%# Eval("TEL") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Addr" runat="server" Text="Addr" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Addr" runat="server" Text='<%# Eval("Addr") %>' MaxLength="200" /></td>
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_LISubsidy" runat="server" Text="LISubsidy" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_LISubsidy" runat="server" /></td>
            </tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_SpecialSeniority" runat="server" Text="SpecialSeniority" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_SpecialSeniority" runat="server" Text='<%# Eval("SpecialSeniority") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LWC" runat="server" Text="LWC" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_LWC" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Union" runat="server" Text="Union" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Union" runat="server" /></td>
						</tr>						
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Identify" runat="server" Text="編制別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Identify" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Grade" runat="server" Text="職等" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Grade" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Shift" runat="server" Text="班別" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_Shift" runat="server" /></td>						
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_PayCode" runat="server" Text="PayCode" /></td><td class="Grid_GridLine"><uc8:CodeList ID="txt_CL_PayCode" runat="server" /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Introducer" runat="server" Text="Introducer" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Introducer" runat="server" Text='<%# Eval("Introducer") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_IntroducerTEL" runat="server" Text="IntroducerTEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_IntroducerTEL" runat="server" Text='<%# Eval("IntroducerTEL") %>' /></td>						
						</tr>		
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Contact" runat="server" Text="Contact" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Contact" runat="server" Text='<%# Eval("Contact") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1" runat="server" Text="Guarantor1" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor1" runat="server" Text='<%# Eval("Guarantor1") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2" runat="server" Text="Guarantor2" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor2" runat="server" Text='<%# Eval("Guarantor2") %>' /></td>
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_ContactTEL" runat="server" Text="ContactTEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ContactTEL" runat="server" Text='<%# Eval("ContactTEL") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor1TEL" runat="server" Text="Guarantor1TEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor1TEL" runat="server" Text='<%# Eval("Guarantor1TEL") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_Guarantor2TEL" runat="server" Text="Guarantor2TEL" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_Guarantor2TEL" runat="server" Text='<%# Eval("Guarantor2TEL") %>' /></td>
						</tr>					
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LeaveWithoutPay" runat="server" Text="LeaveWithoutPay" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_LeaveWithoutPay" runat="server" Text='<%# Eval("LeaveWithoutPay") %>' /></td>						
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_ObserveExpirationDate" runat="server" Text="ObserveExpirationDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_ObserveExpirationDate" runat="server" Text='<%# Eval("ObserveExpirationDate") %>' /></td>					
            <td class="Grid_GridLine"><asp:Label ID="lblTitle_CCN" runat="server" Text="CCN" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_CCN" runat="server" Text='<%# Eval("CCN") %>' /></td>              
						</tr>
						<tr>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LstPromotionDate" runat="server" Text="LstPromotionDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_LstPromotionDate" runat="server" Text='<%# Eval("LstPromotionDate") %>' /></td>
						<td class="Grid_GridLine"><asp:Label ID="lblTitle_LstChangeSalaryDate" runat="server" Text="LstChangeSalaryDate" /></td><td class="Grid_GridLine"><asp:TextBox ID="txt_LstChangeSalaryDate" runat="server" Text='<%# Eval("LstChangeSalaryDate") %>' /></td>
						</tr>
						</table>                        
                        </asp:Panel>										
									<center>
										<asp:ImageButton ID="btnEdit" runat="server" SkinID="SU1" CommandName="Update" />
										<asp:ImageButton ID="btnCancelEdit" runat="server" SkinID="CC1" CommandName="Cancel" />
									</center>
								</EditItemTemplate>
							</asp:TemplateField>
							<asp:CommandField ShowEditButton="False" ButtonType="Image" EditImageUrl="~/App_Themes/images/edit1.GIF" EditText="編輯"
							UpdateImageUrl="~/App_Themes/images/savegoon1.gif" UpdateText="更新" CancelImageUrl="~/App_Themes/images/cancel1.gif"
							ItemStyle-HorizontalAlign="Center"
                        />
						</Fields>
					</asp:DetailsView>
				</td>
			</tr>
		</table>
		<uc5:StyleContentEnd ID="StyleContentEnd1" runat="server" />
		<uc6:StyleFooter ID="StyleFooter1" runat="server" />
		