
WITH O AS (
    SELECT 
        o.custid,
        c.companyname as CustomerName,
        o.orderdate,
        LAG(o.orderdate) OVER (PARTITION BY o.custid ORDER BY o.orderdate) AS PrevOrderDate
    FROM Sales.Orders o
    JOIN Sales.Customers c ON c.custid = o.custid
)
SELECT
    custid,
    CustomerName,
    MAX(orderdate) AS LastOrderDate,
    DATEADD(
        DAY,
        FLOOR(
            1.0 * SUM(CASE WHEN PrevOrderDate IS NULL
                            THEN 0
                            ELSE DATEDIFF(DAY, PrevOrderDate, orderdate)
                        END) / COUNT(*)
        ),
        MAX(orderdate)
    ) AS NextPredictedOrder
FROM O
GROUP BY custid, CustomerName
HAVING COUNT(*) > 1
ORDER BY CustomerName;