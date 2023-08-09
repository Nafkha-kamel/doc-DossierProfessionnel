use Locations 
go

-- 1) --
select * from lignesFic
(select lignesFic.refart, designation from lignesFic inner join articles on lignesFic.refart =articles.refart)

select  a.refart, designation, cmp=COUNT(l.refart) 
from lignesFic l 
inner join articles a on l.refart =a.refart 
group by a.refart, designation
order by cmp desc; 

SELECT a.refart, designation, nbloc=COUNT(l.refart)
FROM lignesFic l
inner JOIN articles a ON l.refart = a.refart
GROUP BY a.refart, designation
ORDER BY nbloc DESC;

-- 2) --
select nom, prenom from clients left outer join fiches on clients.noCli = fiches.noCli where noFic is null

-- 3) --
 select fiches.noFic, nom, articles.refart, articles.designation from
 fiches inner join lignesFic on fiches.noFic = lignesFic.noFic
 inner join clients on fiches.noCli = clients.noCli
 inner join articles on lignesFic.refart = articles.refart
 where etat = 'SO'

 -- 4) --
 -- 5) --
 select lignesfic.refart, COUNT(distinct nom) from 
	lignesFic inner join fiches on lignesFic.noFic = fiches.noFic 
	inner join clients on fiches.noCli = clients.noCli group by refart  having COUNT(distinct nom) <= 1
