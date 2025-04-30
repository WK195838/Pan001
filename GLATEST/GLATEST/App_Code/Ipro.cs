using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public delegate void SubjectsEventHandler(object sender, SubjectsData value);

public delegate void DepartmentEventHandler(object sender, DepartmentData value);

public delegate void CustomerEventHandler(object sender, CustomerData value);

public delegate void EmployeeEventHandler(object sender, EmployeeData value);

public delegate void ProjectEventHandler(object sender, ProjectData value);

public delegate void Supplier01EventHandler(object sender, Supplier01Data value);

public delegate void Supplier20EventHandler(object sender, Supplier20Data value);

public delegate void BankAccount01EventHandler(object sender, BankAccount01Data value);

public delegate void BankAccount20EventHandler(object sender, BankAccount20Data value);

public delegate void SoftWare1EventHandler(object sender, SoftWare1Data value);

public delegate void SoftWare2EventHandler(object sender, SoftWare2Data value);

public delegate void HardwareEventHandler(object sender, HardwareData value);

/// <summary>
/// IproSubjects 的摘要描述
/// </summary>
public interface IproSubjects
{
    event SubjectsEventHandler SubjectsChanged;
}
public class SubjectsData
{
    public string AcctNo { get; set; } //科目編號
    public string AcctDesc1 { get; set; } //科目名稱
    public string Idx01 { get; set; } //相關科目ID 1
    public string Idx01Name { get; set; } //相關科目名稱 1
    public string Idx02 { get; set; } //相關科目ID 2
    public string Idx02Name { get; set; } //相關科目名稱 2
    public string Idx03 { get; set; } //相關科目ID 3
    public string Idx03Name { get; set; } //相關科目名稱 3
    public string Idx04 { get; set; } //相關科目ID 4
    public string Idx04Name { get; set; } //相關科目名稱 4
    public string Idx05 { get; set; } //相關科目ID 5
    public string Idx05Name { get; set; } //相關科目名稱 5
    public string Idx06 { get; set; } //相關科目ID 6
    public string Idx06Name { get; set; } //相關科目名稱 6
    public string Idx07 { get; set; } //相關科目ID 7
    public string Idx07Name { get; set; } //相關科目名稱 7
    /// </summary>
    /// SubjectsData建構式
    /// </summary>
    public SubjectsData()
    {
    }
}

/// <summary>
/// IproDepartment 的摘要描述
/// </summary>
public interface IproDepartment
{
    event DepartmentEventHandler DepartmentChanged;
}
public class DepartmentData
{
    public string DepCode { get; set; } //部門編號
    public string DepName { get; set; } //部門名稱

    /// </summary>
    /// DepartmentData建構式
    /// </summary>
    public DepartmentData()
    {
    }
}

/// <summary>
/// IproCustomer 的摘要描述
/// </summary>
public interface IproCustomer
{
    event CustomerEventHandler CustomerChanged;
}
public class CustomerData
{
    public string CodeCode { get; set; } //客戶編號
    public string CodeName { get; set; } //客戶名稱

    /// </summary>
    /// CustomerData建構式
    /// </summary>
    public CustomerData()
    {
    }
}

/// <summary>
/// IproEmployee 的摘要描述
/// </summary>
public interface IproEmployee
{
    event EmployeeEventHandler EmployeeChanged;
}
public class EmployeeData
{
    public string CodeCode { get; set; } //員工編號
    public string CodeName { get; set; } //員工名稱

    /// </summary>
    /// EmployeeData建構式
    /// </summary>
    public EmployeeData()
    {
    }
}


/// <summary>
/// IproProject 的摘要描述
/// </summary>
public interface IproProject
{
    event ProjectEventHandler ProjectChanged;
}
public class ProjectData
{
    public string CodeCode { get; set; } //專案編號
    public string CodeName { get; set; } //專案名稱

    /// </summary>
    /// ProjectData建構式
    /// </summary>
    public ProjectData()
    {
    }
}

/// <summary>
/// IproSupplier01 的摘要描述
/// </summary>
public interface IproSupplier01
{
    event Supplier01EventHandler Supplier01Changed;
}
public class Supplier01Data
{
    public string SupplierID { get; set; } //供應商編號
    public string SupplierSName { get; set; } //供應商名稱

    /// </summary>
    /// Supplier01Data建構式
    /// </summary>
    public Supplier01Data()
    {
    }
}

/// <summary>
/// IproSupplier20 的摘要描述
/// </summary>
public interface IproSupplier20
{
    event Supplier20EventHandler Supplier20Changed;
}
public class Supplier20Data
{
    public string CodeCode { get; set; } //供應商編號
    public string CodeName { get; set; } //供應商名稱

    /// </summary>
    /// Supplier20Data建構式
    /// </summary>
    public Supplier20Data()
    {
    }
}

/// <summary>
/// IproBankAccount01 的摘要描述
/// </summary>
public interface IproBankAccount01
{
    event BankAccount01EventHandler BankAccount01Changed;
}
public class BankAccount01Data
{
    public string BankNo { get; set; } //銀行編號
    public string BankAccount { get; set; } //銀行帳號

    /// </summary>
    /// BankAccount01Data建構式
    /// </summary>
    public BankAccount01Data()
    {
    }
}

/// <summary>
/// IproBankAccount20 的摘要描述
/// </summary>
public interface IproBankAccount20
{
    event BankAccount20EventHandler BankAccount20Changed;
}
public class BankAccount20Data
{
    public string CodeCode { get; set; } //銀行編號
    public string CodeName { get; set; } //銀行名稱

    /// </summary>
    /// BankAccount20Data建構式
    /// </summary>
    public BankAccount20Data()
    {
    }
}

/// <summary>
/// IproSoftWare1 的摘要描述
/// </summary>
public interface IproSoftWare1
{
    event SoftWare1EventHandler SoftWare1Changed;
}
public class SoftWare1Data
{
    public string CodeCode { get; set; } //軟體編號
    public string CodeName { get; set; } //軟體名稱

    /// </summary>
    /// SoftWare1Data建構式
    /// </summary>
    public SoftWare1Data()
    {
    }
}

/// <summary>
/// IproSoftWare2 的摘要描述
/// </summary>
public interface IproSoftWare2
{
    event SoftWare2EventHandler SoftWare2Changed;
}
public class SoftWare2Data
{
    public string CodeCode { get; set; } //軟體編號
    public string CodeName { get; set; } //軟體名稱

    /// </summary>
    /// SoftWare2Data建構式
    /// </summary>
    public SoftWare2Data()
    {
    }
}

/// <summary>
/// IproHardware 的摘要描述
/// </summary>
public interface IproHardware
{
    event HardwareEventHandler HardwareChanged;
}
public class HardwareData
{
    public string CodeCode { get; set; } //硬體編號
    public string CodeName { get; set; } //硬體名稱

    /// </summary>
    /// HardwareData建構式
    /// </summary>
    public HardwareData()
    {
    }
}