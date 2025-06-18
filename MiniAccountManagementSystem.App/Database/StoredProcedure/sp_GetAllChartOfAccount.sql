use [MiniAccountDb]
Go

CREATE PROCEDURE sp_GetAllChartOfAccounts
AS
BEGIN
    SELECT AccountId, AccountName, ParentAccoun tId, AccountType
    FROM ChartOfAccounts
    ORDER BY ParentAccountId, AccountName
END
