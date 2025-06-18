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
    Id INT PRIMARY KEY IDENTITY,
    VoucherType NVARCHAR(20),
    VoucherDate DATE,
    ReferenceNo NVARCHAR(50)
)

CREATE TABLE [VoucherEntries] (
    Id INT PRIMARY KEY IDENTITY,
    VoucherId INT,
    AccountId INT,
    DebitAmount DECIMAL(18,2),
    CreditAmount DECIMAL(18,2),
    FOREIGN KEY (VoucherId) REFERENCES Vouchers(Id),
    FOREIGN KEY (AccountId) REFERENCES ChartOfAccounts([AccountId])
)

CREATE TYPE VoucherEntryType AS TABLE
(
    AccountId INT,
    DebitAmount DECIMAL(18, 2),
    CreditAmount DECIMAL(18, 2)
);


