# 🧾 Mini Account Management System

A simple yet functional accounting system built using **ASP.NET Core Razor Pages**, **SQL Server (Stored Procedures Only)**, and **ASP.NET Identity** for authentication and role-based authorization.

---

## 🚀 Features

### 🔐 Authentication & Authorization
- ASP.NET Identity integration
- Role-based access (Admin, Accountant, Viewer)
- Secure login, logout, registration
- Access restrictions per role using `[Authorize]`

### 🧾 Chart of Accounts
- Hierarchical tree structure with parent-child accounts
- CRUD operations using stored procedure `sp_ManageChartOfAccounts`
- Dropdown-based parent selection
- Dynamic account tree view

### 💳 Voucher Entry Module
- Voucher types: Journal, Payment, Receipt
- Multi-line debit/credit entries
- Dynamic add/remove entry rows using JavaScript
- Account dropdowns populated from Chart of Accounts
- Form validation: Total Debit must equal Total Credit
- Voucher storage via stored procedure `sp_SaveVoucher`
- Default today's date in voucher entry
- View voucher details with debit/credit summary

---

## 🛠 Technologies Used

- **ASP.NET Core Razor Pages (.NET 8)**
- **MS SQL Server**
- **Stored Procedures** (no Entity Framework LINQ)
- **ASP.NET Identity** (with custom roles)
- **Bootstrap 5** (for responsive UI)
- **JavaScript** (for dynamic voucher entries)

---

# Admin Credentials (Role- Admin)
- username: admin@admin.com
- password: Admin@1234

# Accountant Credentials (Role- Accountant)
- username: accountant@accountant.com
- password: Accountant@1234

### You can register as a new user (Role- Viewer)

## 📂 Folder Structure (Important Pages)

```
/Pages
└── ChartOfAccounts
├── Index.cshtml (list & tree view)
├── Create.cshtml (add account)
├── Edit.cshtml
└── Delete.cshtml
└── Vouchers
├── Index.cshtml (list vouchers)
├── Create.cshtml (add voucher)
└── Details.cshtml (voucher info)
```
# Table SQL Syntax
```
use [MiniAccountDb]
GO

CREATE TABLE [ChartOfAccounts] (
    [AccountId] INT PRIMARY KEY IDENTITY,
    [AccountName] NVARCHAR(100) NOT NULL,
    [ParentAccountId] INT NULL, -- for hierarchy
    [AccountType] NVARCHAR(50), -- optional: Asset, Liability etc.
    [IsActive] BIT NOT NULL DEFAULT 1
);

CREATE TABLE [Vouchers] (
    VoucherId INT IDENTITY(1,1) PRIMARY KEY,
    VoucherType NVARCHAR(50),
    VoucherDate DATE,
    ReferenceNo NVARCHAR(100)
);

CREATE TABLE [VoucherEntries] (
    EntryId INT IDENTITY(1,1) PRIMARY KEY,
    VoucherId INT FOREIGN KEY REFERENCES Vouchers(VoucherId),
    AccountId INT FOREIGN KEY REFERENCES ChartOfAccounts(AccountId),
    Amount DECIMAL(18, 2),
    EntryType NVARCHAR(10)  -- 'Debit' or 'Credit'
);
```

# Stored Procedures
```
use [MiniAccountDb]
Go

CREATE PROCEDURE sp_GetAllChartOfAccounts
AS
BEGIN
    SELECT AccountId, AccountName, ParentAccoun tId, AccountType
    FROM ChartOfAccounts
    ORDER BY ParentAccountId, AccountName
END

```

```
use [MiniAccountDb]
GO

CREATE PROCEDURE sp_ManageChartOfAccounts
    @Action NVARCHAR(10),        -- 'Insert', 'Update', 'Delete'
    @AccountId INT = NULL,
    @AccountName NVARCHAR(100) = NULL,
    @ParentAccountId INT = NULL,
    @AccountType NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @Action = 'Insert'
    BEGIN
        INSERT INTO ChartOfAccounts (AccountName, ParentAccountId, AccountType)
        VALUES (@AccountName, @ParentAccountId, @AccountType)
    END

    ELSE IF @Action = 'Update'
    BEGIN
        UPDATE ChartOfAccounts
        SET AccountName = @AccountName,
            ParentAccountId = @ParentAccountId,
            AccountType = @AccountType
        WHERE AccountId = @AccountId
    END

    ELSE IF @Action = 'Delete'
    BEGIN
        DELETE FROM ChartOfAccounts
        WHERE AccountId = @AccountId
    END
END

```

```
use [MiniAccountDb]
GO

CREATE TYPE VoucherEntryType AS TABLE (
    AccountId INT,
    Amount DECIMAL(18, 2),
    EntryType NVARCHAR(10)
);
GO

CREATE PROCEDURE sp_SaveVoucher
    @VoucherType NVARCHAR(50),
    @VoucherDate DATE,
    @ReferenceNo NVARCHAR(100),
    @Entries VoucherEntryType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Vouchers (VoucherType, VoucherDate, ReferenceNo)
    VALUES (@VoucherType, @VoucherDate, @ReferenceNo);

    DECLARE @NewVoucherId INT = SCOPE_IDENTITY();

    INSERT INTO VoucherEntries (VoucherId, AccountId, Amount, EntryType)
    SELECT @NewVoucherId, AccountId, Amount, EntryType FROM @Entries;
END

```
