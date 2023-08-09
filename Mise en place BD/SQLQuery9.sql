use Locations
go

-- tp module 9 --

 -- 1) --
 select refart, COUNT(*)  from LIGNESFIC group by refart order by (select COUNT(*)  from LIGNESFIC group by refart)

 select refart, (select refart, COUNT(*)  from LIGNESFIC group by refart) compt from LIGNESFIC