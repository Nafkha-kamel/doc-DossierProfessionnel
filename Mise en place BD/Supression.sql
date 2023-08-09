Use Locations
go
 -- effacer les  contraintes --
  alter table GRILLETARIFS drop
	constraint FK_GRILLETARIFS_codeGam,
	constraint FK_GRILLETARIFS_codeCate,
	constraint FK_GRILLETARIFS_codeTarif;
  
  alter table ARTICLES drop
	constraint FK_ARTICLES_codeGam_codeCate;

alter table LIGNESFIC drop
	constraint FK_LIGNESFICHES_refart;

alter table LIGNESFIC drop
	constraint FK_LIGNESFICHES_noFic;

alter table FICHES drop
	constraint FK_FICHES_noCli;

drop table FICHES;
drop table CLIENTS;
drop table LIGNESFIC;
drop table ARTICLES;
drop table Gammes;
drop table GRILLETARIFS;
drop table TARIFS;
drop table CATEGORIES;