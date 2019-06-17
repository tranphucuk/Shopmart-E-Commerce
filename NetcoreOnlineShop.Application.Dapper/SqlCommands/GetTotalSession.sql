create or alter Procedure SumSessionPerDay
 @fromdate  varchar(20),
 @todate  varchar(20)
as
begin
select  
			cast (b.CreatedDate	as date) as Date,
			COUNT(bd.Id) as TotalSession
			from dbo.Bills b
			inner join dbo.BillDetails bd
			on b.Id = bd.BillId
			where b.CreatedDate >= cast(@fromdate as Date)
			and b.CreatedDate <= cast(@todate as date)
			group by b.CreatedDate
end

execute SumSessionPerDay '9/27/2018','9/27/2018'