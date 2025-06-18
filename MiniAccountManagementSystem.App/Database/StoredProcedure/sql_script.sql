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



