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
