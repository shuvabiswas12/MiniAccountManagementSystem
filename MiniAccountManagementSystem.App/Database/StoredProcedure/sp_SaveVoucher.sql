use [MiniAccountDb]
GO

CREATE PROCEDURE sp_SaveVoucher
    @VoucherType NVARCHAR(20),
    @VoucherDate DATE,
    @ReferenceNo NVARCHAR(50),
    @Entries VoucherEntryType READONLY
AS
BEGIN
    SET NOCOUNT ON;

    -- Insert into Vouchers table
    INSERT INTO Vouchers (VoucherType, VoucherDate, ReferenceNo)
    VALUES (@VoucherType, @VoucherDate, @ReferenceNo);

    DECLARE @VoucherId INT = SCOPE_IDENTITY();

    -- Insert each entry
    INSERT INTO VoucherEntries (VoucherId, AccountId, DebitAmount, CreditAmount)
    SELECT @VoucherId, AccountId, DebitAmount, CreditAmount FROM @Entries;
END
