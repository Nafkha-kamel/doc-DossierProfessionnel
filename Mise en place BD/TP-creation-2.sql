use ExoLocations
go
-- TP du module 5 --

create table CLIENTS(
noCli numeric(6) not null,
nom varchar(30) not null,
prenom varchar(30),
adresse varchar(120),
cpo char(5) not null constraint CK_CLIENTS_cpo check ((convert(int, cpo) >= 01000) and (convert(int, cpo) =< 95999)),
ville varchar(80) not null constraint DF_CLIENTS_ville default 'Nantes',
constraint PK_CLIENTS_noCli primary key (noCli)
)

create table FICHES(
noFic numeric(6),
noCli numeric(6) not null,
dateCrea date not null constraint DF_FICHES_dateCrea default getdate(),
DatePaye date, 
etat char(2) not null constraint DF_FICHES_etat default 'EC'
					  constraint CK_FICHES_etat check (etat='EC' or etat='RE' or etat='SO'),
	constraint CK_FICHES_DatePaye check ( DatePaye is null or DatePaye >= dateCrea),
	constraint CK_Fiches_DatePaye_etat check ((DatePaye is null and etat <> 'SO') or (DatePaye is not null and etat = 'SO')),

	constraint PK_FICHES_noFic primary key (noFic)
)

create table FICHES1(
noFic char(6)--  constraint PK_01 primary key,
--trip_id numeric(6) not null --, constraint FK_01 foreign key references FICHES(noFic)

)	
drop table FICHES1
 


create table LIGNESFIC(
noFic numeric(6),
noLig numeric(3),
refart char(8) not null,
depart date not null constraint DF_LIGNESFIC_depart default getdate(),
retour date,
	constraint CK_LIGNESFIC_retour check (retour is null or retour >= depart),
	constraint PK_LIGNESFIC_noFic_noLig primary key (noFic, noLig)
)
create table ARTICLES(
refart char(8) not null,
designation varchar(80) not null,
codeGam char(5) not null,
codeCate char(5) not null,
constraint PK_ARTICLES_refart primary key (refart)
)
create table GRILLETARIFS(
codeGam char(5) not null,
codeCate char(5) not null,
codeTarif char(5),
	constraint PK_GRILLETARIFS_codeGam_codecate primary key (codeGam, codeCate)
)

create table GAMMES(
codeGam char(5) not null,
libelle varchar(30) not null constraint UN_GAMMES_libelle unique,
	constraint PK_GAMMES_codeGam primary key (codegam)
)
create table CATEGORIES(
codeCate char(5) not null ,
libelle varchar(30) not null constraint UN_CATEGORIES_libelle unique,
	constraint PK_CATEGORIES_codeCate primary key (codeCate)
)
create table TARIFS
(codeTarif char(5) not null,
libelle varchar(30) not null constraint UN_TARIFS_libelle unique,
prixJour numeric(5,2) not null constraint CK_TARIFS_prixJour check (prixJour >=0),
	constraint PK_TARIS_codeTarif primary key (codeTarif)

)
alter table FICHES
drop constraint  PK_FICHES_noFic ;


alter table FICHES with check
	add constraint FK_FICHES_noCli foreign key (noCli) references CLIENTS(noCli) on delete cascade; 

alter table LIGNESFIC with check
	add constraint FK_LIGNESFIC_noFic foreign key (noFic) references FICHES(noFic) on delete cascade; 

alter  table LIGNESFIC with check
	add constraint FK_LIGNESFIC_refart foreign key (refart) references ARTICLES(refart); 

alter table ARTICLES with check add
	constraint FK_ARTICLES_codeGam_codeCate foreign key (codeGam, codeCate) references GRILLETARIFS(codeGam, codeCate)

alter table GRILLETARIFS with check add
	constraint FK_GRILLESTARIFS_codeGam foreign key (codeGam) references GAMMES(codeGam),
	constraint FK_GRILLESTARIFS_codeCate foreign key (codeCate) references CATEGORIES(codeCate),
	constraint FK_GRILLESTARIFS_codeTarif foreign key (codeTarif) references TARIFS(codeTarif);

create nonclustered index IN_FICHES_CLIENTS_noCli on FICHES(noCli asc);

create nonclustered index IN_LIGNESFIC_FICHES_noFic on LIGNESFIC(noFic asc);
create nonclustered index IN_LIGNESFIC_ARTICLES_noFic on LIGNESFIC(refart asc);

create nonclustered index IN_ARTICLES_GRILLETARIFS_codeGam_codeCate on ARTICLES(codeGam, codecate asc);

create nonclustered index IN_GRILLETARIFS_GAMMES_codeGam on GRILLETARIFS(codeGam asc);
create nonclustered index IN_GRILLETARIFS_CATEGORIES_codeCate on GRILLETARIFS(codeCate asc);
create nonclustered index IN_GRILLETARIFS_TARIFS_codeTarif on GRILLETARIFS(codeTarif asc);

alter table CLIENTS
	alter column nom varchar(20);
select * from CLIENTS ; 

alter table clients
	drop column nom;
	
alter table CLIENTS
	add nom varchar(30) not null; 

alter table LIGNESFIC 
	drop constraint DF_LIGNESFIC_depart;
alter table LIGNESFIC
	add constraint DF_LIGNESFIC_depart default getdate() for depart;
alter table FICHES
	drop constraint FK_FICHES_noCli; 
alter table FICHES with check add
	constraint FK_Fiches_noCli foreign key (noCli) references CLIENTS(noCli) ; 
drop index IN_GRILLETARIFS_TARIFS_codeTarif on GRILLETARIFS;
create index IN_GRILLETARIFS_TARIFS_codeTarif on GRILLETARIFS(codeTarif);
/*
drop table CLIENTS
drop table ARTICLES
drop table CATEGORIES
drop table FICHES
drop table GAMMES
drop table TARIFS
drop table GRILLETARIFS
drop table LIGNESFIC
*/
