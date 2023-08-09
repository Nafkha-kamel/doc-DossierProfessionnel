 --- 2.10 ---
select * from membres join EMPRUNTS on membres.numero = EMPRUNTS.MEMBRE
                      join details on emprunts.numero = details.emprunt 
                      join EXEMPLAIRES on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero
                      join ouvrages on ouvrages.isbn = exemplaires.isbn where (round(sysdate - creele) <14);
 --- 2.11 ---
 select genre , count(numero) from ouvrages join exemplaires on ouvrages.isbn = exemplaires.isbn
                                      join genres on genres.code = ouvrages.genre group by genre;
 --- 2.12 ---
 select avg(rendule - creele) from  details inner select avg(rendule - creele) from  details inner join emprunts on emprunts.numero = details.emprunt where rendule is not null;
 where rendule is not null;
 --- 2.13 ---
 select genre ,round(avg(rendule - creele)) from ouvrages join exemplaires on ouvrages.isbn = exemplaires.isbn
                                      join genres on genres.code = ouvrages.genre 
                                      join details on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero
                                      join emprunts on emprunts.numero = details.emprunt                                      
                                      group by genre;
 --- 2.14 ---
  select details.isbn,count(*) from ouvrages join exemplaires on ouvrages.isbn = exemplaires.isbn
                                      join details on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero
                                      join emprunts on emprunts.numero = details.emprunt 
                                      join membres on emprunts.membre = membres.NUMERO
                                      where (months_between(rendule, creele) <12)
                                      group by details.isbn 
                                      having count(*) >10;
--- 2.15 ---
select * from ouvrages full join exemplaires on ouvrages.isbn = exemplaires.isbn order by ouvrages.isbn asc, numero desc;

--- 2.16 ---
create or replace force view VUE1 as
select membres.numero as membre, count(membres.numero) as nombreEmprunts from emprunts   join details on emprunts.membre = details.emprunt
                                     full  join membres on emprunts.membre = membres.numero group by membres.numero, nom, prenom order by membre;
select * from VUE1;

--- 2.17 ---
create or replace view VUE2 as
select ouvrages.isbn, count(*) as nombre from ouvrages full JOIN exemplaires on ouvrages.isbn = exemplaires.isbn group by ouvrages.isbn order by count(*) desc;

select * from VUE2;
create or replace view VUE2 as
select isbn, count(*) as nombre from details group by isbn order by count(*) desc;

--- 2.18 ---
select * from membres order by nom;

--- 2.19 ---
select ouvrages.isbn, count(ouvrages.isbn)
  from exemplaires join details on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero 
                    join ouvrages on ouvrages.isbn = exemplaires.ISBN
  group by ouvrages.isbn; 

 --- 3.1 ---
 create global temporary table tab 
 ( isbn number(10),
    compt number (2)
    ) ON COMMIT PRESERVE ROWS; 
 insert into tab select ouvrages.isbn, count(*)as cmpt  from ouvrages join exemplaires on ouvrages.isbn = exemplaires.isbn
                              join details on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero 
                              group by ouvrages.isbn;
 select ouvrages.isbn, exemplaires.numero, count(exemplaires.numero) from tab  join ouvrages on tab.isbn = ouvrages.ISBN
                    join exemplaires on ouvrages.isbn = exemplaires.isbn
                              join details on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero 
                              group by rollup(ouvrages.isbn, exemplaires.numero);
drop table tab;

--- 3.2 ---

select * from ouvrages join exemplaires on ouvrages.isbn = exemplaires.isbn
                              join details on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero where (rendule is null or (sysdate-rendule)<90) ;
--- 3.3 ---
select * from ouvrages join exemplaires on ouvrages.isbn = exemplaires.isbn where etat != 'NE';

--- 3.4 ---
select * from ouvrages where titre like '%mer%';

--- 3.5 ---
select * from ouvrages where auteur like '%de%';
select auteur from ouvrages where regexp_like(auteur,'^[[:alpha:]]*[[:space:]]de[[:space:]][[:alpha:]]+$');
--- 3.6 ---
select * from genres;
select distinct isbn, titre, case genre when 'BD' then ' Jeunese' 
                               when 'INF' then 'Professionnel'
                               when 'POL' then 'Policier'
                               when 'REC' then 'Tous'
                               when 'ROM' then 'Tous'
                               when 'THE' then 'Tous'
                               end as "Public"
        from ouvrages ;
 --- 3.7 ---
 comment on table membres is 'Descriptifs des membres. Possède le synonyme Abonnes';
 comment on table genres is 'Descriptifs des membres. Possède le synonyme Abonnes';
 select * from user_tab_comments where comments is not null;
 
 --- 4.1 ---
 select * from exemplaires;
 select details.ISBN, exemplaire,etat, count(emprunt) from details join exemplaires on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero group by (details.isbn,exemplaire,etat);
  ------
  set serveroutput on;
  declare 
  cursor neuf is select details.ISBN as isbn, exemplaire,etat, count(emprunt) as compt from details join exemplaires on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero group by (details.isbn,exemplaire,etat);
  cursor_index neuf%rowtype;
  begin
  null;
  for cursor_index in neuf loop
  if cursor_index.compt <10 then 
  update exemplaires set etat ='NE' where  exemplaires.isbn=cursor_index.isbn and exemplaires.numero = cursor_index.exemplaire;
  dbms_output.put_line(cursor_index.compt);
  elsif cursor_index.compt <=25 then
  cursor_index.etat:='BO';
  dbms_output.put_line(cursor_index.compt);
  elsif cursor_index.compt <=40 then
  cursor_index.etat:='ME';
  dbms_output.put_line(cursor_index.compt);  
  else
  cursor_index.etat:='MA';
  dbms_output.put_line(cursor_index.compt);  
  end if;
  end loop;
  
  end;
  select details.ISBN, exemplaire,etat, count(emprunt) from details join exemplaires on details.isbn = exemplaires.isbn and details.EXEMPLAIRE = exemplaires.numero group by (details.isbn,exemplaire,etat);
 --- 4.2 ---
 INSERT INTO membres (numero, nom, prenom, adresse, adhesion, duree) VALUES (seq_membre.NEXTVAL, 'LOMOBO', 'Laurent', '31 rue des lilas',sysdate-1000,1);
 select * from membres where round(sysdate-add_months(adhesion,duree)) >720;
 select * from membres join emprunts on membres.numero = emprunts.membre where round(sysdate-add_months(adhesion,duree)) >720;

 select etat from membres full join emprunts on membres.numero = emprunts.membre;
 rollback; 
 ------
 set serveroutput on;
 declare
  cursor suppression is select * from membres full join emprunts on membres.numero = emprunts.membre;
  cursor_index suppression%rowtype;
  begin
 
  for cursor_index in suppression loop
   if round(sysdate -add_months(cursor_index.adhesion, cursor_index.duree)) >720 then
    if cursor_index.etat =  'RE' then 
          update emprunts set etat = null where emprunts.numero = cursor_index.numero;
          delete from membres ;--where membres.numero = cursor_index.numero and emprunts.etat = cursor_index.etat; 
     dbms_output.put_line(cursor_index.membre);
      dbms_output.put_line(cursor_index.nom);
    end if;
  end if;
  end loop;
 
  end;
  ----
select emprunt,count(*) from membres join emprunts on emprunts.membre = membres.numero join details on emprunts.numero = details.emprunt group by emprunt ;

SELECT e.membre, count(*) nb
    FROM emprunts e INNER JOIN details d ON e.numero=d.emprunt
	 
    GROUP BY e.membre;
 ----------
 set SERVEROUTPUT ON;
 declare
 cursor cursor0 is select isbn, count(*) from details group by isbn order by count(*) desc;
 cpt pls_integer :=0;
 begin
  for cursor_index in cursor0 loop
  SYS.DBMS_OUTPUT.PUT_LINE('#' || cpt || ' : ' || cursor_index.isbn);
  cpt:=cpt+1;
  if cpt =5 then exit; end if;
  end loop;
 
end;
---
select * from membres where (sysdate - add_months(adhesion,duree))>=0 or (sysdate - add_months(adhesion,duree))<=30; 
--
set serveroutput on ;
DECLARE
cursor cursor1 is select * from membres where (sysdate - add_months(adhesion,duree))>=0 or (sysdate - add_months(adhesion,duree))<=30; 
compteur cursor1%rowtype;
ii number :=0 ;
begin
open cursor1;
loop
ii:=ii+1;
exit when ii>5;
fetch cursor1 into compteur;
exit when cursor1%notfound;
dbms_output.put_line('Numero :' || ii || ' numero : ' || compteur.nom);
end loop;
close cursor1;
end;
----
create or replace function FinValidite(nm in number) return date 
is
date_limite date;
begin
 select max(creele) into date_limite from emprunts where membre = nm;
 date_limite := add_months(date_limite, 0.5);
dbms_output.put_line('date limi est : ' || date_limite);
return date_limite;
end;
select FinValidite(1) from dual;
select max(creele) from emprunts where membre = 1;
----
create or replace function adhesionAJour(membre_num in number) return boolean 
is 
BEGIN
  IF (finValidite(membre_num)>=sysdate()) THEN
	RETURN TRUE;
  ELSE
	RETURN FALSE;
  END IF;
END;
SET SERVEROUTPUT ON
BEGIN
  IF (adhesionajour(1)) THEN
    dbms_output.put_line('Membre 1 : adhesion a jour');
  ELSE
    dbms_output.put_line('Membre 1 : adhesion pas a jour');
  END IF;
END;
/
-----
select count(*) from membres; 
create or replace procedure purgeMembres 
is
begin
delete from membres where round(sysdate-add_months(adhesion, duree)) > 1095;
end;

execute purgeMembres;
---
select user from dual;
---
select * from emprunts where membre = 6;
delete from emprunts where numero = 21 ;
Insert into Emprunts values (21, 6,sysdate, 'EC');
--select count(isbn) as cmpt from details where isbn=2070367177  and EXEMPLAIRE = 2 and rendule is not null;
create or replace procedure empruntExpress(membre_num in number, isbn_number in number, exemplaire_num in number) is
cpt PLS_integer :=0;
begin
select count(isbn) into cpt from details where isbn=isbn_number  and EXEMPLAIRE = exemplaire_num and rendule is not null;
dbms_output.put_line(cpt);
if cpt is not null then
Insert into Emprunts values (21,6,sysdate, 'EC');
insert into details values (21, 1, isbn_number,exemplaire_num,null);
end if;
end;

select * from details;
select * from emprunts ; 
insert into details values (21, 1, 2070367177, 2,null);
delete from details where EMPRUNT = 21 ;
delete from emprunts where numero = 21 ;
execute empruntExpress(6, 2070367177, 2);

create or replace trigger trigger01 
before insert on Emprunts
for each row
declare
membreNum number;
ajour number; 
begin
membreNum := :NEW.membre;
select (sysdate - add_months(adhesion, duree)) into ajour from membres where numero = membreNum; 
if ajour >0 then 
raise_application_error(-20001, 'Adhesion pas à jour') ;
end if;  
-- dbms_output.put_line('ajour = ' || ajour);
end;
----

create or replace trigger trigger02 
before update on Emprunts
for each row
begin
if (:NEW.membre <> :Old.membre) then 
raise_application_error(-20300, 'impossible de modifier le membre') ;
end if;  
end;
update EMPRUNTS set membre = 2 where membre=1; 

----
create or replace trigger trigger03
before update on details
for each row
begin
if (:NEW.exemplaire <> :Old.exemplaire) then 
raise_application_error(-20401, 'impossible de changer l''exemplaire') ;
end if;  
end;
select * from details; 
update details set exemplaire = 3 where emprunt = 8 and numero = 1 ;