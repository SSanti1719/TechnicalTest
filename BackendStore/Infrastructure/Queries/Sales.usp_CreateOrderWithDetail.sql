CREATE OR ALTER PROCEDURE Sales.usp_CreateOrderWithDetail
    @CustId INT = NULL,
    @EmpId INT,
    @OrderDate DATETIME,
    @RequiredDate DATETIME,
    @ShippedDate DATETIME = NULL,
    @ShipperId INT,
    @Freight MONEY,
    @ShipName NVARCHAR(40),
    @ShipAddress NVARCHAR(60),
    @ShipCity NVARCHAR(15),
    @ShipRegion NVARCHAR(15) = NULL,
    @ShipPostalCode NVARCHAR(10) = NULL,
    @ShipCountry NVARCHAR(15),
    @ProductId INT,
    @UnitPrice MONEY,
    @Qty SMALLINT,
    @Discount NUMERIC(4,3)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Sales.Orders
        (custid, empid, orderdate, requireddate, shippeddate,
         shipperid, freight, shipname, shipaddress, shipcity,
         shipregion, shippostalcode, shipcountry)
        VALUES
        (@CustId, @EmpId, @OrderDate, @RequiredDate, @ShippedDate,
         @ShipperId, @Freight, @ShipName, @ShipAddress, @ShipCity,
         @ShipRegion, @ShipPostalCode, @ShipCountry);

        DECLARE @NewOrderId INT = SCOPE_IDENTITY();

        INSERT INTO Sales.OrderDetails
        (orderid, productid, unitprice, qty, discount)
        VALUES
        (@NewOrderId, @ProductId, @UnitPrice, @Qty, @Discount);

        COMMIT TRANSACTION;

        SELECT @NewOrderId AS OrderId;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
