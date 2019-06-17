CREATE OR ALTER PROC GetRevenueDaily
		@fromDate VARCHAR(10),
		@toDate VARCHAR(10)
AS
BEGIN
		select
				CAST(b.CreatedDate as Date) as Date,
				sum(bd.Quantity * bd.Price) as Revenue,
				sum(bd.Quantity * p.OriginalPrice) as Funds,
				sum((bd.Quantity * bd.Price) -(bd.Quantity * p.OriginalPrice)) as Profit
				from dbo.Bills b
				inner join dbo.BillDetails bd
				on b.Id = bd.BillId
				inner join Products p
				on bd.ProductId = p.Id
				where b.CreatedDate <= cast(@toDate as date)
				AND b.CreatedDate >= cast(@fromDate as date)
				group by b.CreatedDate
End

EXEC dbo.GetRevenueDaily @fromDate = '02/11/2019',
						 @toDate = '04/16/2019'