CREATE OR ALTER PROC DataReport
		@fromDate VARCHAR(10),
		@toDate VARCHAR(10)
AS
BEGIN
		select
				CAST(b.CreatedDate as Date) as Date,
				COUNT(bd.Id) as TotalSession,
				sum(bd.Quantity * bd.Price) as Revenue,
				sum(bd.Quantity * p.OriginalPrice) as Funds,
				sum((bd.Quantity * bd.Price) -(bd.Quantity * p.OriginalPrice)) as Profit,
				((sum((bd.Quantity * bd.Price) -(bd.Quantity * p.OriginalPrice)) /sum(bd.Quantity * bd.Price))*100) as FlatPercent
				from dbo.Bills b
				inner join dbo.BillDetails bd
				on b.Id = bd.BillId
				inner join Products p
				on bd.ProductId = p.Id
				where b.CreatedDate <= cast(@toDate as date)
				AND b.CreatedDate >= cast(@fromDate as date)
				group by b.CreatedDate
End

EXEC dbo.DataReport      @fromDate = '03/23/2019',
						 @toDate = '03/23/2019'